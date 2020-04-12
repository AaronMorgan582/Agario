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

namespace ViewController
{
    public partial class Client_and_GUI : Form
    {
        public enum object_type { food, player, heartbeat, admin }
        private Preserved_Socket_State server;
        private string player_name;
        private string server_name = "localhost";
        private Circle player_circle;
        private Circle world_circle;
        private List<Circle> circle_list = new List<Circle>();

        private const int screen_width = 1600;
        private const int screen_height = 900;

        private bool connected = false;
        private ILogger logger;
        private World game_world;

        private int player_id;
        private Socket server_socket;

        private float movement_X;
        private float movement_Y;

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

            if (!obj.Has_More_Data())
            {
                Networking.await_more_data(obj);  // * must "await_more_data" if you want to receive messages.
            }
        }

        private void Get_Player_Circle(Preserved_Socket_State obj)
        {
            obj.on_data_received_handler = Get_World_Information;
            Circle sentCircle = JsonConvert.DeserializeObject<Circle>(obj.Message);

            if (sentCircle.GetName.Equals(player_name))
            {
                player_circle = sentCircle;

                //Debug.WriteLine(player_circle.Location);

            }

            player_id = player_circle.ID;
            lock (circle_list)
            {
                player_circle.Radius = player_circle.Radius * 5;
                circle_list.Add(player_circle);
                
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

                lock (circle_list)
                {
                    circle_list.Add(world_circle);
                }

            }
            catch (Exception e)
            {
                //logger.LogError($"{e}");
            }

            Networking.await_more_data(obj);

            Calculate_Movement(out movement_X, out movement_Y);

            Debug.WriteLine($"Sent movement: {movement_X} {movement_Y}");

            Networking.Send(obj.socket, $"(move,{movement_X},{movement_Y})");

            obj.on_data_received_handler = Get_World_Information;
            
        }

        private void Draw_Scene(object sender, PaintEventArgs e)
        {


            bool location_changed = true;
            if (connected)
            {
                this.Invalidate();

                Disable_Login_Menu();
                this.DoubleBuffered = true;

                lock (circle_list)
                {
                    foreach (Circle circle in circle_list)
                    {
                        float loc_x = 0;
                        float loc_y = 0;

                        if (circle.ID == player_id)
                        {
                            loc_x = (screen_width / 2) - (float)circle.Radius;
                            loc_y = (screen_height / 2) - (float) circle.Radius;

                            float screen_x = (loc_x / 5_000) * 1_600;
                            float screen_y = (loc_y / 5_000) * 900;

                            if (location_changed)
                            {
                                PointF player_point = new PointF(loc_x, loc_y);
                                Brush text_brush = new SolidBrush(Color.Black);
                                e.Graphics.DrawString($"{player_circle.GetName}", new Font("Times New Roman", 12), text_brush, player_point);
                                location_changed = false;
                            }
                           
                            logger.LogInformation($"COORDINATE: {loc_x}, {loc_y} | {screen_x}, {screen_y}");
                            Debug.WriteLine(player_circle.Location);
                        }

                        else
                        {
                            loc_x = circle.Location.X / 5000 * 1600;
                            loc_y = circle.Location.Y / 5000 * 900;
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
        public void Calculate_Movement(out float movement_X, out float movement_Y)
        {
            float mouse_X = Cursor.Position.X;
            float mouse_Y = Cursor.Position.Y;

            Debug.WriteLine($"Mouse position: {mouse_X} {mouse_Y}");

            movement_X = (mouse_X / 5000) * 1600;
            movement_Y = (mouse_Y / 5000) * 900;

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
