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
using System.Drawing.Drawing2D;

namespace ViewController
{
    public partial class Client_and_GUI : Form
    {
        
        private Preserved_Socket_State server;
        private string player_name;
        private string server_name = "localhost";
        private Circle player_circle;
        private Circle world_circle;

        private const int screen_width = 1600;
        private const int screen_height = 900;

        private bool connected = false;
        private bool isDead = false;
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
                logger.LogInformation("Shutting down the connection"); //TODO: Write to the screen when the connection has ended.
                this.server.socket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
                return;
            }

            logger.LogInformation("Asking the network code to connect to the server.");

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
                Calculate_Movement(out movement_X, out movement_Y);
                Networking.Send(obj.socket, $"(move,{movement_X},{movement_Y})");
                logger.LogDebug($"Sent movement: {movement_X} {movement_Y}");

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
                        if (!world_circle.Location.Equals(game_world[world_circle.ID])) //If the player location has changed from what's already stored
                        {
                            game_world.Remove(world_circle.ID); //Remove the old entry
                            game_world.Add(world_circle.ID, world_circle); //And add in the new one (which is the same "object" via the ID, but in a different location)
                        }
                        if (world_circle.GetMass <= 0 && world_circle.Type.ToString().Equals("food"))
                        {
                            game_world.Remove(world_circle.ID); //Remove the old entry
                        }
                    }

                }

            }
            catch (Exception e)
            {
                logger.LogError($"Json convert error.");
            }
            try
            {
                Networking.await_more_data(obj);
            }
            catch(SocketException e)
            {
                // If a player has died, stop drawing the game scene and display game over screen
                logger.LogInformation($"{player_circle.GetName} has been killed.");
                connected = false;
                isDead = true;
            }

            // If the user wishes to split, send the coordinates for split
            if(can_split)
            {
                //float destination_X = player_circle.Location.X + 10;
                //float destination_Y = player_circle.Location.Y + 10;
                float destination_X = 1000;
                float destination_Y = 500;

                Networking.Send(obj.socket, $"split,{destination_X},{destination_Y}");

                can_split = false;
            }

            obj.on_data_received_handler = Get_World_Information;
            
        }

        /// <summary>
        /// The event for drawing the game and game over scene
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Draw_Scene(object sender, PaintEventArgs e)
        {
            bool location_changed = true;

            if (connected)
            {
                // When a user is connected, disable the login menu and begin drawing the game scene
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

                        // If one of the circles is the user, center the camera around them
                        if (circle.ID == player_id)
                        {
                            loc_x = circle.Location.X / game_world.Width * screen_width;
                            loc_y = circle.Location.Y / game_world.Height * screen_height;
                            e.Graphics.TranslateTransform(-loc_x + screen_width / 2, -loc_y + screen_height / 2);
                            //Zooming
                            //float scale_x =circle.GetMass / 8;
                            //float scale_y =circle.GetMass / 8;
                            //float squared_scale_x = scale_x * scale_x;
                            //float squared_scale_y = scale_y * scale_y;
                            //e.Graphics.ScaleTransform(scale_x,scale_y);
                            //e.Graphics.TranslateTransform(-(loc_x) + (loc_x * scale_x) / squared_scale_x, -(loc_y) + (loc_y * scale_y) / squared_scale_y);
                           
                            logger.LogDebug($"COORDINATE: {loc_x}, {loc_y}");
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

                        if (location_changed)
                        {
                            PointF player_point = new PointF(loc_x, loc_y);
                            Brush text_brush = new SolidBrush(Color.Black);
                            e.Graphics.DrawString($"{player_circle.GetName}", new Font("Times New Roman", 12), text_brush, player_point);
                            location_changed = false;
                        }
;                    }
                }
            }
            else if(isDead)
            {
                Display_Game_Over_Screen(e);
            }
        }

        /// <summary>
        /// Handler for when a player wants to split
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Space_Down(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Space)
            {
                if (player_circle.GetMass > 10)
                {
                    can_split = true;
                }
            }
        }

        /// <summary>
        /// This helper method determines the cursor's position, which is used to calculate
        /// how far to move the player
        /// </summary>
        /// <param name="movement_X">Reference to player's x position</param>
        /// <param name="movement_Y">Reference to player's y position</param>
        public void Calculate_Movement(out float movement_X, out float movement_Y)
        {
            float mouse_X = Cursor.Position.X;
            float mouse_Y = Cursor.Position.Y;
            float monitor_X = (mouse_X / 1920) * screen_width;
            float monitor_Y = (mouse_Y / 1080) * screen_height;

            logger.LogDebug($"Mouse position: {mouse_X} {mouse_Y}");

            movement_X = (monitor_X / screen_width) * game_world.Width;
            movement_Y = (monitor_Y / screen_width) * game_world.Height;

            logger.LogDebug($"Calculated location: {movement_X} {movement_Y}");
        }

        /// <summary>
        /// This helper method displays a game over screen when the user dies
        /// </summary>
        /// <param name="e"></param>
        private void Display_Game_Over_Screen(PaintEventArgs e)
        {
            Brush text_brush = new SolidBrush(Color.Black);
            Point game_over_point = new Point(260, 86);
            Point mass_point = new Point(287, 107);

            Width = 700;
            Height = 450;
            CenterToScreen();
            
            player_name_label.Show();
            player_name_box.Show();
            title_label.Show();
            server_label.Show();
            server_address_box.Show();
            connect_button.Text = "Try Again";
            connect_button.Show();
            e.Graphics.DrawString($"Game Over...Try Again?", new Font("Times New Roman", 12), text_brush, game_over_point);
            e.Graphics.DrawString($"Total Mass: {player_circle.GetMass}", new Font("Times New Roman", 12), text_brush, mass_point);
        }
        /// <summary>
        /// This helper method disables the GUI controls found on the login screen, and adjusts the screen size
        /// </summary>
        private void Disable_Login_Menu()
        {
            connect_button.Visible = false;
            player_name_box.Visible = false;
            player_name_label.Visible = false;
            server_address_box.Visible = false;
            server_label.Visible = false;
            title_label.Visible = false;
            error_label.Visible = false;

            this.Width = 1600;
            this.Height = 900;
            this.CenterToScreen();          
        }
    }
}
