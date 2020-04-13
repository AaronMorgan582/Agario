using Model;
using NetworkingNS;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using System.Net.Sockets;
using System.Collections;

namespace ViewController
{
    public partial class Client_and_GUI : Form
    {
        private Preserved_Socket_State server;
        private string player_name;
        private string server_name = "localhost";
        private Circle player_circle;
        private Circle world_circle;
        private List<Circle> circle_list = new List<Circle>(); //TODO: May need to remove.

        private const int screen_width = 1600;
        private const int screen_height = 900;

        private bool connected = false;
        private ILogger logger;
        private World game_world;

        private int player_id;

        private float movement_X;
        private float movement_Y;

        private bool can_split = false;

        public Client_and_GUI(ILogger logger)
        {
            this.logger = logger;
            game_world = new World(logger);

            InitializeComponent();
        }

        private void Connect_To_Server(object o, EventArgs e)
        {
            if (this.server != null && this.server.socket.Connected)
            {
                Debug.WriteLine("Shutting down the connection"); //TODO: Write to the screen when the connection has ended.
                this.server.socket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
                return;
            }

            Debug.WriteLine("Asking the network code to connect to the server.");

            player_name = player_name_box.Text;
            if (player_name is "")
            {
                player_name = $"{new Random()}";
            }
            server_name = server_address_box?.Text;
            // * upon connection the code (in another thread) will execute the "Contact_Established" method.
            this.server = Networking.Connect_to_Server(Contact_Established, server_name);

        }

        private void Contact_Established(Preserved_Socket_State obj)
        {
            logger.LogInformation("Contact with Server established!");
            obj.on_data_received_handler = Get_Player_Circle;
            Networking.Send(obj.socket, player_name);

            connected = true;

            Client_and_GUI.ActiveForm.KeyDown += new KeyEventHandler(Space_Down);

            if (!obj.Has_More_Data())
            {
                Networking.await_more_data(obj);  // * must "await_more_data" if you want to receive messages.
            }
        }

        private void Get_Player_Circle(Preserved_Socket_State obj)
        {
            obj.on_data_received_handler = Get_World_Information;

            Circle sent_circle = JsonConvert.DeserializeObject<Circle>(obj.Message);

            if (sent_circle.GetName.Equals(player_name))
            {
                player_circle = sent_circle;
                player_id = player_circle.ID;

                //Debug.WriteLine(player_circle.Location);
            }

            lock (game_world)
            {
                if (!game_world.Contains(player_id))
                {
                    game_world.Add(player_id, player_circle);
                }  
            }

            Networking.await_more_data(obj);
        }

        private void Get_World_Information(Preserved_Socket_State obj)
        {
            try
            {
                world_circle = JsonConvert.DeserializeObject<Circle>(obj.Message);

                if (world_circle.Type.ToString().Equals("heartbeat"))
                {
                    this.Invalidate();
                }

                lock (game_world)
                {
                    if (!game_world.Contains(world_circle.ID))
                    {
                        game_world.Add(world_circle.ID, world_circle);

                    }
                    else
                    {
                        if (!world_circle.Location.Equals(game_world[world_circle.ID])) //If the location has changed from what's already stored
                        {
                            game_world.Remove(world_circle.ID); //Remove the old entry
                            game_world.Add(world_circle.ID, world_circle); //And add in the new one (which is the same "object" via the ID, but in a different location)
                            this.Invalidate();
                        }
                    }

                }

            }
            catch (Exception e)
            {
                //logger.LogError($"{e}");
            }

            Networking.await_more_data(obj);

            Calculate_Movement(out movement_X, out movement_Y);

            Debug.WriteLine($"Sent movement: {movement_X} {movement_Y}");

            if(can_split)
            {
                float destination_X = player_circle.Location.X + 10;
                float destination_Y = player_circle.Location.Y + 10;

                Networking.Send(obj.socket, $"{destination_X},{destination_Y}");

                can_split = false;
            }

            Networking.Send(obj.socket, $"(move,{movement_X},{movement_Y})");

            obj.on_data_received_handler = Get_World_Information;
            
        }

        private void Draw_Scene(object sender, PaintEventArgs e)
        {
            bool location_changed = true;

            if (connected)
            {
                Disable_Login_Menu();
                this.DoubleBuffered = true;
                this.Invalidate();

                lock (game_world)
                {
                    foreach (int ID in game_world.Keys())
                    {
                        Circle circle = game_world[ID];
                        float loc_x = 0;
                        float loc_y = 0;

                        if (circle.ID == player_id)
                        {
                            //circle.Radius = circle.Radius * 5;

                            //loc_x = (screen_width / 2) - (float)circle.Radius;
                            //loc_y = (screen_height / 2) - (float) circle.Radius;

                            loc_x = circle.Location.X / game_world.Width * screen_width;
                            loc_y = circle.Location.Y / game_world.Height * screen_height;

                            float screen_x = (loc_x / game_world.Width) * screen_width; //We don't actually use these, but they are currently just being used to see if the ratio conversion is working (logging.LogInformation)
                            float screen_y = (loc_y / game_world.Height) * screen_height;

                            if (location_changed)
                            {
                                PointF player_point = new PointF(loc_x, loc_y);
                                Brush text_brush = new SolidBrush(Color.Black);
                                e.Graphics.DrawString($"{player_circle.GetName}", new Font("Times New Roman", 12), text_brush, player_point);
                                location_changed = false;
                            }
                           
                            logger.LogInformation($"COORDINATE: {loc_x}, {loc_y} | {screen_x}, {screen_y}");
                            Debug.WriteLine(circle.Location);
                        }

                        else
                        {
                            loc_x = circle.Location.X / game_world.Width * screen_width;
                            loc_y = circle.Location.Y / game_world.Height * screen_height;
                            circle.Radius = 5;
                        }

                        int circle_color = circle.CircleColor;
                        Brush circle_brush = new SolidBrush(Color.FromArgb(circle_color));

                        double diameter = circle.Radius * 2;

                        Rectangle circ_as_rect = new Rectangle((int)loc_x, (int)loc_y, (int)diameter, (int)diameter);
                        e.Graphics.FillEllipse(circle_brush, circ_as_rect);
;                    }
                }
            }
        }

        private void Space_Down(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Space)
            {
                if(player_circle.GetMass > 10)
                {
                    can_split = true;
                }
            }
        }

        public void Calculate_Movement(out float movement_X, out float movement_Y)
        {
            float mouse_X = Cursor.Position.X;
            float mouse_Y = Cursor.Position.Y;

            Debug.WriteLine($"Mouse position: {mouse_X} {mouse_Y}");

            movement_X = (mouse_X / screen_width) * game_world.Width;
            movement_Y = (mouse_Y / screen_height) * game_world.Height;

            Debug.WriteLine($"Calculated location: {movement_X} {movement_Y}");
        }

        private void Disable_Login_Menu()
        {
            connect_button.Visible = false;
            player_name_box.Visible = false;
            player_name_label.Visible = false;
            server_address_box.Visible = false;
            server_label.Visible = false;
            title_label.Visible = false;
            error_label.Visible = false;

            this.ClientSize = new System.Drawing.Size(screen_width, screen_height);
            this.CenterToScreen();          
        }
    }
}
