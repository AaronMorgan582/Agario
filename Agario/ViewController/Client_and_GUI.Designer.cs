using System.Drawing;
using System.Windows.Forms;

namespace ViewController
{
    partial class Client_and_GUI
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "Agario";

            //Connect Button
            this.connect_button = new Button();
            this.connect_button.Location = new Point(300, 260);
            this.connect_button.Size = new System.Drawing.Size(100, 50);
            this.connect_button.Name = "Connect Button";
            this.connect_button.Text = "Connect";
            //this.Connect_Button.Click += Connect;


            //Input Box for Player Name
            this.player_name_box = new TextBox();
            this.player_name_box.Location = new Point(300, 160);
            this.player_name_box.Name = "Player_Name_Textbox";
            this.player_name_box.Size = new System.Drawing.Size(100, 20);
            this.player_name_box.TabIndex = 2;
            //this.player_name_box.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.player_name_box);


            //Label for Player Name text
            this.player_name_label = new Label();
            this.player_name_label.AutoSize = true;
            this.player_name_label.Visible = true;
            this.player_name_label.Location = new Point(200, 160);
            this.player_name_label.Name = "Player_Name_Label";
            this.player_name_label.Size = new System.Drawing.Size(79, 29);
            this.player_name_label.TabIndex = 3;
            this.player_name_label.Text = "Player Name:";


            //Input Box for Server
            this.server_address_box = new TextBox();
            this.server_address_box.Location = new Point(300, 206);
            this.server_address_box.Name = "Server_Address_Textbox";
            this.server_address_box.Size = new System.Drawing.Size(100, 20);
            this.server_address_box.TabIndex = 2;
            //this.server_address_box.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.server_address_box);


            //Label for Server text
            this.server_label = new Label();
            this.server_label.AutoSize = true;
            this.server_label.Visible = true;
            this.server_label.Location = new Point(200, 206);
            this.server_label.Name = "Server_Label";
            this.server_label.Size = new System.Drawing.Size(79, 29);
            this.server_label.TabIndex = 3;
            this.server_label.Text = "Server:";


            //Label for Error text
            this.error_label = new Label();
            this.error_label.AutoSize = true;
            this.error_label.Visible = true;
            this.error_label.Location = new Point(25, 400);
            this.error_label.Name = "Error_Label";
            this.error_label.Size = new System.Drawing.Size(79, 29);
            this.error_label.TabIndex = 3;
            this.error_label.Text = "Error: ";


            //Label for Title
            this.title_label = new Label();
            this.title_label.AutoSize = true;
            this.title_label.Visible = true;
            this.title_label.Location = new Point(287, 53);
            this.title_label.Name = "Title_Label";
            this.title_label.Size = new System.Drawing.Size(79, 29);
            this.title_label.TabIndex = 3;
            this.title_label.Text = "The Blob";
            this.title_label.Font = new Font("Arial", 16);


            //Adding to Controls
            this.Controls.Add(connect_button);
            this.Controls.Add(player_name_box);
            this.Controls.Add(player_name_label);
            this.Controls.Add(server_address_box);
            this.Controls.Add(server_label);
            this.Controls.Add(error_label);
            this.Controls.Add(title_label);
        }

        #endregion

        private Button connect_button;
        private TextBox player_name_box;
        private Label player_name_label;
        private TextBox server_address_box;
        private Label server_label;
        private Label error_label;
        private Label title_label;

    }
}

