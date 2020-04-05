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
            // Do we deserialize here?
            //player_circle = JsonConvert.SerializeObject(new Circle());
        }

        private void Draw_Scene(object sender, PaintEventArgs e)
        {
            Brush brush = new SolidBrush(Color.FromArgb(240,30,70));
            e.Graphics.FillEllipse(brush, new Rectangle(116,-1, 450,450));

        }
    }
}
