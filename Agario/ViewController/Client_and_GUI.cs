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

namespace ViewController
{
    public partial class Client_and_GUI : Form
    {
        private Preserved_Socket_State server;
        private string player_name;
        private string server_name = "localhost";
        private Circle player_circle;
        private Circle world_circle;

        private const int screen_height = 1600;
        private const int screen_width = 900;
        public Client_and_GUI()
        {
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

            player_name = player_name_box?.Text;
            if (player_name is "")
            {
                player_name = $"{new Random()}";
            }
            server_name = server_address_box?.Text;
            // * upon connection the code (in another thread) will execute the "Contact_Established" method.
            this.server = Networking.Connect_to_Server(Contact_Established, server_name);
            connect_button.Visible = false;
            player_name_box.Visible = false;
            player_name_label.Visible = false;
            server_address_box.Visible = false;
            server_label.Visible = false;
            title_label.Visible = false;
            this.ClientSize = new System.Drawing.Size(screen_height, screen_width);

            error_label.Location = new Point(error_label.Location.X * 2, error_label.Location.Y * 2);

            this.Paint += new PaintEventHandler(Draw_Scene);

        }

        private void Contact_Established(Preserved_Socket_State obj)
        {
            Debug.WriteLine("Contact with Server established!");
            obj.on_data_received_handler = Get_Player_Circle;

            Networking.Send(obj.socket, player_name);
            Networking.await_more_data(obj);  // * must "await_more_data" if you want to receive messages.
        }

        private void Get_Player_Circle(Preserved_Socket_State obj)
        {
            player_circle = JsonConvert.DeserializeObject<Circle>(obj.Message);
/*            Networking.await_more_data(obj);
            obj.on_data_received_handler = Get_World_Information;*/
        }

        private void Get_World_Information(Preserved_Socket_State obj)
        {
            world_circle = JsonConvert.DeserializeObject<Circle>(obj.Message);
            Networking.await_more_data(obj);
            obj.on_data_received_handler = Get_World_Information;
        }

        private void Draw_Scene(object sender, PaintEventArgs e)
        {

            this.DoubleBuffered = true;

            Brush brush = new SolidBrush(Color.FromArgb(240, 30, 70));

            if (player_circle != null)
            {
                Rectangle circ_as_rect = new Rectangle(116, -1, (int)player_circle.Radius * 2, (int)player_circle.Radius * 2);
                e.Graphics.FillEllipse(brush, circ_as_rect);
            }

            this.Invalidate();




        }
    }
}
