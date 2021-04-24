using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Forms;

namespace RepeatedToneApp
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.TextBox textBox1;

        public Form1()
        {
            InitializeComponent();
            this.components = new System.ComponentModel.Container();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();

            // Initialize contextMenu1
            this.contextMenu1.MenuItems.AddRange(
                        new System.Windows.Forms.MenuItem[] { this.menuItem1 });

            // Initialize menuItem1
            this.menuItem1.Index = 0;
            this.menuItem1.Text = "E&xit";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);

            // Set up how the form should be displayed.
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Text = "Notify Icon Example";

            // Create the NotifyIcon.
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);

            // The Icon property sets the icon that will appear
            // in the systray for this application.
            if (GetTheme() == 0)
            {
                notifyIcon1.Icon = RepeatedToneApp.Resource.speakerDarkMode;
            }
            else
            {
                notifyIcon1.Icon = RepeatedToneApp.Resource.speakerLightMode;
            }

            // The ContextMenu property sets the menu that will
            // appear when the systray icon is right clicked.
            notifyIcon1.ContextMenu = this.contextMenu1;

            // The Text property sets the text that will be displayed,
            // in a tooltip, when the mouse hovers over the systray icon.
            notifyIcon1.Text = "Form1 (NotifyIcon example)";
            notifyIcon1.Visible = true;

            // Handle the DoubleClick event to activate the form.
            notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);

            this.textBox1 = new System.Windows.Forms.TextBox();
            this.trackBar1 = new System.Windows.Forms.TrackBar();

            // TextBox for TrackBar.Value update.
            this.textBox1.Location = new System.Drawing.Point(240, 16);
            this.textBox1.Size = new System.Drawing.Size(48, 20);

            // Set up how the form should be displayed and add the controls to the form.
            this.ClientSize = new System.Drawing.Size(296, 62);
            this.Controls.AddRange(new System.Windows.Forms.Control[] { this.textBox1, this.trackBar1 });
            this.Text = "TrackBar Example";

            // Set up the TrackBar.
            this.trackBar1.Location = new System.Drawing.Point(8, 8);
            this.trackBar1.Size = new System.Drawing.Size(224, 45);
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);

            // The Maximum property sets the value of the track bar when
            // the slider is all the way to the right.
            trackBar1.Maximum = 30;

            // The TickFrequency property establishes how many positions
            // are between each tick-mark.
            trackBar1.TickFrequency = 5;

            // The LargeChange property sets how many positions to move
            // if the bar is clicked on either side of the slider.
            trackBar1.LargeChange = 3;

            // The SmallChange property sets how many positions to move
            // if the keyboard arrows are used to move the slider.
            trackBar1.SmallChange = 2;
        }

        private void trackBar1_Scroll(object sender, System.EventArgs e)
        {
            // Display the trackbar value in the text box.
            textBox1.Text = "" + trackBar1.Value;
        }
        public Int32 GetTheme()
        {
            string RegistryKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
            Int32 theme;
            theme = (Int32)Registry.GetValue(RegistryKey, "AppsUseLightTheme", 0);
            //theme = theme.Split('\\').Last().Split('.').First().ToString();
            return theme;
        }
        private void notifyIcon1_DoubleClick(object Sender, EventArgs e)
        {
            // Show the form when the user double clicks on the notify icon.

            // Set the WindowState to normal if the form is minimized.
            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;

            // Activate the form.
            this.Activate();
        }

        private void menuItem1_Click(object Sender, EventArgs e)
        {
            // Close the form, which closes the application.
            this.Close();
        }
    }
}
