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
            this.Text = "Form1";

            this.Connect_Button = new System.Windows.Forms.Button();
            this.Connect_Button.Location = new Point(50, 50);
            this.Connect_Button.Size = new Size(100, 50);
            this.Connect_Button.Name = "Connect Button";
            this.Connect_Button.Text = "Connect";
            //this.Connect_Button.Click += Connect;
        }

        #endregion

        private Button Connect_Button;


    }
}

