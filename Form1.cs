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
using System.Threading;

namespace RepeatedToneApp
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenu contextMenu;
        private System.Windows.Forms.MenuItem menuItem;
        private System.Windows.Forms.TrackBar trackBarInterval;
        private System.Windows.Forms.TextBox textBoxInterval;

        public Form1()
        {
            InitializeComponent();
            this.components = new System.ComponentModel.Container();
            this.contextMenu = new System.Windows.Forms.ContextMenu();
            this.menuItem = new System.Windows.Forms.MenuItem();

            // Initialize contextMenu
            this.contextMenu.MenuItems.AddRange(
                        new System.Windows.Forms.MenuItem[] { this.menuItem });

            // Initialize menuItem
            this.menuItem.Index = 0;
            this.menuItem.Text = "E&xit";
            this.menuItem.Click += new System.EventHandler(this.menuItem_Click);

            // Set up how the form should be displayed.
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Text = "Notify Icon Example";

            // Create the NotifyIcon.
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);

            // The Icon property sets the icon that will appear
            // in the systray for this application.
            if (GetTheme() == 0)
            {
                notifyIcon.Icon = RepeatedToneApp.Resource.speakerDarkMode;
            }
            else
            {
                notifyIcon.Icon = RepeatedToneApp.Resource.speakerLightMode;
            }

            // The ContextMenu property sets the menu that will
            // appear when the systray icon is right clicked.
            notifyIcon.ContextMenu = this.contextMenu;

            // The Text property sets the text that will be displayed,
            // in a tooltip, when the mouse hovers over the systray icon.
            notifyIcon.Text = "Form1 (NotifyIcon example)";
            notifyIcon.Visible = true;

            // Handle the DoubleClick event to activate the form.
            notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);

            this.textBoxInterval = new System.Windows.Forms.TextBox();
            this.trackBarInterval = new System.Windows.Forms.TrackBar();

            // TextBox for TrackBar.Value update.
            this.textBoxInterval.Location = new System.Drawing.Point(240, 16);
            this.textBoxInterval.Size = new System.Drawing.Size(48, 20);

            // Set up how the form should be displayed and add the controls to the form.
            this.ClientSize = new System.Drawing.Size(296, 62);
            this.Controls.AddRange(new System.Windows.Forms.Control[] { this.textBoxInterval, this.trackBarInterval });
            this.Text = "Repeated Tone";

            // Set up the TrackBar.
            this.trackBarInterval.Location = new System.Drawing.Point(8, 8);
            this.trackBarInterval.Size = new System.Drawing.Size(224, 45);
            this.trackBarInterval.Scroll += new System.EventHandler(this.trackBarInterval_Scroll);

            // The Maximum property sets the value of the track bar when
            // the slider is all the way to the right.
            trackBarInterval.Maximum = 30;

            // The TickFrequency property establishes how many positions
            // are between each tick-mark.
            trackBarInterval.TickFrequency = 5;

            // The LargeChange property sets how many positions to move
            // if the bar is clicked on either side of the slider.
            trackBarInterval.LargeChange = 3;

            // The SmallChange property sets how many positions to move
            // if the keyboard arrows are used to move the slider.
            trackBarInterval.SmallChange = 2;
            BackgroundBeep backgroundBeep = new BackgroundBeep(500);
            backgroundBeep.Beep();
            VolumeChange.SetApplicationVolume(Application.ProductName, 50);
            VolumeChange.VolumeChangeStartup(Application.ProductName, 50);
            Thread.Sleep(5000);
            VolumeChange.SetApplicationVolume(Application.ProductName, 100);
            //backgroundBeep.StopBeep();
        }

        private void GenerateBeep()
        {
            BackgroundBeep backgroundBeep = new BackgroundBeep(500);
            backgroundBeep.Beep();
            Thread.Sleep(1000);
            backgroundBeep.StopBeep();
        }

        private void trackBarInterval_Scroll(object sender, System.EventArgs e)
        {
            // Display the trackbar value in the text box.
            textBoxInterval.Text = "" + trackBarInterval.Value;
        }
        public Int32 GetTheme()
        {
            string RegistryKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
            Int32 theme;
            theme = (Int32)Registry.GetValue(RegistryKey, "AppsUseLightTheme", 0);
            return theme;
        }
        private void notifyIcon_DoubleClick(object Sender, EventArgs e)
        {
            // Show the form when the user double clicks on the notify icon.

            // Set the WindowState to normal if the form is minimized.
            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;

            // Activate the form.
            this.Activate();
        }

        private void menuItem_Click(object Sender, EventArgs e)
        {
            // Close the form, which closes the application.
            this.Close();
        }
    }
}
