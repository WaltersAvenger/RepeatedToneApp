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
        private System.Windows.Forms.TrackBar trackBarFrequency;
        private System.Windows.Forms.TextBox textBoxFrequency;
        private System.Windows.Forms.TrackBar trackBarAmplitude;
        private System.Windows.Forms.TextBox textBoxAmplitude;
        private System.Windows.Forms.TrackBar trackBarDuration;
        private System.Windows.Forms.TextBox textBoxDuration;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private bool shouldStop;
        private Task tskGenerateBeep;
        private BackgroundBeep backgroundBeep;


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

            // Create the NotifyIcon.
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);

            // The Icon property sets the icon that will appear
            // in the systray for this application.
            if (GetTheme() == 0)

            {
                this.notifyIcon.Icon = RepeatedToneApp.Resource.speakerDarkMode;
                this.Icon = RepeatedToneApp.Resource.speakerDarkMode;
            }
            else
            {
                this.notifyIcon.Icon = RepeatedToneApp.Resource.speakerLightMode;
                this.Icon = RepeatedToneApp.Resource.speakerLightMode;
            }

            // The ContextMenu property sets the menu that will
            // appear when the systray icon is right clicked.
            notifyIcon.ContextMenu = this.contextMenu;

            // The Text property sets the text that will be displayed,
            // in a tooltip, when the mouse hovers over the systray icon.
            notifyIcon.Text = "Repeated Tone";
            notifyIcon.Visible = true;
            this.Resize += new System.EventHandler(this.frmMain_Resize);

            // Handle the DoubleClick event to activate the form.
            notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);

            //Initialize objects
            this.textBoxInterval = new System.Windows.Forms.TextBox();
            this.trackBarInterval = new System.Windows.Forms.TrackBar();
            this.textBoxFrequency = new System.Windows.Forms.TextBox();
            this.trackBarFrequency = new System.Windows.Forms.TrackBar();
            this.textBoxAmplitude = new System.Windows.Forms.TextBox();
            this.trackBarAmplitude = new System.Windows.Forms.TrackBar();
            this.textBoxDuration = new System.Windows.Forms.TextBox();
            this.trackBarDuration = new System.Windows.Forms.TrackBar();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();

            //Add Lables
            System.Windows.Forms.Label labelInterval = new System.Windows.Forms.Label();
            System.Windows.Forms.Label labelFrequency = new System.Windows.Forms.Label();
            System.Windows.Forms.Label labelAmplitude = new System.Windows.Forms.Label();
            System.Windows.Forms.Label labelDuration = new System.Windows.Forms.Label();

            labelInterval.Text = "Interval Between (m)";
            labelInterval.Location = new System.Drawing.Point(8, 4);
            labelFrequency.Text = "Frequency (hZ)";
            labelFrequency.Location = new System.Drawing.Point(8, 60);
            labelAmplitude.Text = "Amplitude (dB)";
            labelAmplitude.Location = new System.Drawing.Point(8, 116);
            labelDuration.Text = "Sound Duration (s)";
            labelDuration.Location = new System.Drawing.Point(8, 172);


            // TextBox for TrackBar.Value update.
            this.textBoxInterval.Location = new System.Drawing.Point(240, 24);
            this.textBoxInterval.Size = new System.Drawing.Size(48, 20);
            this.textBoxInterval.TextChanged += new System.EventHandler(this.textBoxInterval_ValueChanged);
            this.textBoxFrequency.Location = new System.Drawing.Point(240, 80);
            this.textBoxFrequency.Size = new System.Drawing.Size(48, 20);
            this.textBoxFrequency.TextChanged += new System.EventHandler(this.textBoxFrequency_ValueChanged);
            this.textBoxAmplitude.Location = new System.Drawing.Point(240, 136);
            this.textBoxAmplitude.Size = new System.Drawing.Size(48, 20);
            this.textBoxAmplitude.TextChanged += new System.EventHandler(this.textBoxAmplitude_ValueChanged);
            this.textBoxDuration.Location = new System.Drawing.Point(240, 186);
            this.textBoxDuration.Size = new System.Drawing.Size(48, 20);
            this.textBoxDuration.TextChanged += new System.EventHandler(this.textBoxDuration_ValueChanged);

            //Button Value Update
            this.btnStart.Location = new System.Drawing.Point(75, 248);
            this.btnStart.Size = new System.Drawing.Size(48, 20);
            this.btnStop.Location = new System.Drawing.Point(177, 248);
            this.btnStop.Size = new System.Drawing.Size(48, 20);
            this.btnStart.Text = "Start";
            this.btnStart.Click += new System.EventHandler(this.buttonStart_Click);
            this.btnStop.Text = "Stop";
            this.btnStop.Click += new System.EventHandler(this.buttonStop_Click);

            // Set up the TrackBar.
            this.trackBarInterval.Location = new System.Drawing.Point(8, 16);
            this.trackBarInterval.Size = new System.Drawing.Size(224, 45);
            this.trackBarInterval.Scroll += new System.EventHandler(this.trackBarInterval_Scroll);
            this.trackBarInterval.ValueChanged += new System.EventHandler(this.trackBarInterval_ValueChanged);

            this.trackBarFrequency.Location = new System.Drawing.Point(8, 72);
            this.trackBarFrequency.Size = new System.Drawing.Size(224, 45);
            this.trackBarFrequency.Scroll += new System.EventHandler(this.trackBarFrequency_Scroll);
            this.trackBarFrequency.ValueChanged += new System.EventHandler(this.trackBarFrequency_ValueChanged);

            this.trackBarAmplitude.Location = new System.Drawing.Point(8, 128);
            this.trackBarAmplitude.Size = new System.Drawing.Size(224, 45);
            this.trackBarAmplitude.Scroll += new System.EventHandler(this.trackBarAmplitude_Scroll);
            this.trackBarAmplitude.ValueChanged += new System.EventHandler(this.trackBarAmplitude_ValueChanged);

            this.trackBarDuration.Location = new System.Drawing.Point(8, 184);
            this.trackBarDuration.Size = new System.Drawing.Size(224, 45);
            this.trackBarDuration.Scroll += new System.EventHandler(this.trackBarDuration_Scroll);
            this.trackBarDuration.ValueChanged += new System.EventHandler(this.trackBarDuration_ValueChanged);

            // The Minimum property sets the value of the track bar when
            // the slider is all the way to the right.
            trackBarInterval.Minimum = 0;
            trackBarFrequency.Minimum = 10;
            trackBarAmplitude.Minimum = 0;
            trackBarDuration.Minimum = 1;


            // The Maximum property sets the value of the track bar when
            // the slider is all the way to the right.
            trackBarInterval.Maximum = 1800;
            trackBarFrequency.Maximum = 24000;
            trackBarAmplitude.Maximum = 20;
            trackBarDuration.Maximum = 30;


            // The TickFrequency property establishes how many positions
            // are between each tick-mark.
            trackBarInterval.TickFrequency = 1;
            trackBarFrequency.TickFrequency = 10;
            trackBarAmplitude.TickFrequency = 1;
            trackBarDuration.TickFrequency = 1;

            // The LargeChange property sets how many positions to move
            // if the bar is clicked on either side of the slider.
            trackBarInterval.LargeChange = 5;
            trackBarFrequency.LargeChange = 100;
            trackBarAmplitude.LargeChange = 2;
            trackBarDuration.LargeChange = 5;

            // The SmallChange property sets how many positions to move
            // if the keyboard arrows are used to move the slider.
            trackBarInterval.SmallChange = 1;
            trackBarFrequency.SmallChange = 10;
            trackBarAmplitude.SmallChange = 1;
            trackBarDuration.SmallChange = 1;

            // Defaulting the starting value
            trackBarInterval.Value = 5;
            trackBarFrequency.Value = 500;
            trackBarAmplitude.Value = 10;
            trackBarDuration.Value = 5;
            SetAllTextboxes();

            // Set up how the form should be displayed and add the controls to the form.
            this.ClientSize = new System.Drawing.Size(296, 282);
            this.Controls.AddRange(new System.Windows.Forms.Control[] { this.textBoxInterval, this.trackBarInterval, this.textBoxFrequency,
            this.trackBarFrequency, this.textBoxAmplitude, this.trackBarAmplitude, this.textBoxDuration, this.trackBarDuration, this.btnStart, this.btnStop,
            labelInterval, labelFrequency, labelAmplitude, labelDuration});
            this.Text = "Repeated Tone";
        }

        //Main method for generate beep based on gui values
        private void GenerateBeep()
        {
            try
            {
                Int32 delFrequency = 0;
                Int32 delDuration = 0;
                Int32 delInterval = 0;
                Int32 delAmplitude = 0;
                do
                {
                    this.Invoke(new MethodInvoker(delegate ()
                    {
                        delFrequency = trackBarFrequency.Value;
                        delAmplitude = trackBarAmplitude.Value;
                    }));
                    backgroundBeep = new BackgroundBeep(delFrequency, delAmplitude);
                    backgroundBeep.Beep();
                    //VolumeChange.SetApplicationVolume(Application.ProductName, delAmplitude);

                    this.Invoke(new MethodInvoker(delegate ()
                    {
                        delDuration = trackBarDuration.Value;
                    }));
                    tskGenerateBeep.Wait(delDuration * 1000);

                    backgroundBeep.StopBeep();

                    this.Invoke(new MethodInvoker(delegate ()
                    {
                        delInterval = trackBarInterval.Value;
                    }));
                    tskGenerateBeep.Wait(delInterval * 1000);
                } while (!shouldStop);
            }
            catch
            {
                //closing UI before thread is done processing
            }
            finally
            {
                shouldStop = false;
            }
        }
        
        private void SetAllTextboxes()
        {
            // Display the trackbar value in the text box.
            textBoxInterval.Text = "" + trackBarInterval.Value;
            textBoxFrequency.Text = "" + trackBarFrequency.Value;
            textBoxAmplitude.Text = "" + trackBarAmplitude.Value;
            textBoxDuration.Text = "" + trackBarDuration.Value;
        }

        private void trackBarInterval_Scroll(object sender, System.EventArgs e)
        {
            // Display the trackbar value in the text box.
            textBoxInterval.Text = "" + trackBarInterval.Value;
        }
        private void trackBarFrequency_Scroll(object sender, System.EventArgs e)
        {
            // Display the trackbar value in the text box.
            textBoxFrequency.Text = "" + trackBarFrequency.Value;
        }
        private void trackBarAmplitude_Scroll(object sender, System.EventArgs e)
        {
            // Display the trackbar value in the text box.
            textBoxAmplitude.Text = "" + trackBarAmplitude.Value;
        }
        private void trackBarDuration_Scroll(object sender, System.EventArgs e)
        {
            // Display the trackbar value in the text box.
            textBoxDuration.Text = "" + trackBarDuration.Value;
        }
        private void textBoxInterval_ValueChanged(object sender, System.EventArgs e)
        {
            //text block value change, update slider
            try
            {
                Int32 textBoxValue = Int32.Parse(this.textBoxInterval.Text);
                if (textBoxValue > this.trackBarInterval.Maximum)
                {
                    this.trackBarInterval.Value = this.trackBarInterval.Maximum;
                    this.textBoxInterval.Text = "" + this.trackBarInterval.Maximum;
                }
                else if (textBoxValue < this.trackBarInterval.Minimum)
                {
                    this.trackBarInterval.Value = this.trackBarInterval.Minimum;
                    this.textBoxInterval.Text = "" + this.trackBarInterval.Minimum;
                }
                else
                {
                    this.trackBarInterval.Value = textBoxValue;
                }
            }
            catch
            {
                //Only string
            }
        }
        private void textBoxFrequency_ValueChanged(object sender, System.EventArgs e)
        {
            //text box value changes update slider
            try
            {
                Int32 textBoxValue = Int32.Parse(this.textBoxFrequency.Text);
                if (textBoxValue > this.trackBarFrequency.Maximum)
                {
                    this.trackBarFrequency.Value = this.trackBarFrequency.Maximum;
                    this.textBoxFrequency.Text = "" + this.trackBarFrequency.Maximum;
                }
                else if (textBoxValue < this.trackBarFrequency.Minimum)
                {
                    this.trackBarFrequency.Value = this.trackBarFrequency.Minimum;
                    this.textBoxFrequency.Text = "" + this.trackBarFrequency.Minimum;
                }
                else
                {
                    this.trackBarFrequency.Value = textBoxValue;
                }
            }
            catch
            {
                //Only numeric
            }
        }
        private void textBoxAmplitude_ValueChanged(object sender, System.EventArgs e)
        {
            //text box value change, update the slider
            try
            {
                Int32 textBoxValue = Int32.Parse(this.textBoxAmplitude.Text);
                if (textBoxValue > this.trackBarAmplitude.Maximum)
                {
                    this.trackBarAmplitude.Value = this.trackBarAmplitude.Maximum;
                    this.textBoxAmplitude.Text = "" + this.trackBarAmplitude.Maximum;
                }
                else if (textBoxValue < this.trackBarAmplitude.Minimum)
                {
                    this.trackBarAmplitude.Value = this.trackBarAmplitude.Minimum;
                    this.textBoxAmplitude.Text = "" + this.trackBarAmplitude.Minimum;
                }
                else
                {
                    this.trackBarAmplitude.Value = textBoxValue;
                }
            }
            catch
            {
                //Only string
            }
        }
        private void textBoxDuration_ValueChanged(object sender, System.EventArgs e)
        {
            //text box value change, update the slider
            try
            {
                Int32 textBoxValue = Int32.Parse(this.textBoxDuration.Text);
                if (textBoxValue > this.trackBarDuration.Maximum)
                {
                    this.trackBarDuration.Value = this.trackBarDuration.Maximum;
                    this.textBoxDuration.Text = "" + this.trackBarDuration.Maximum;
                }
                else if (textBoxValue < this.trackBarDuration.Minimum)
                {
                    this.trackBarDuration.Value = this.trackBarDuration.Minimum;
                    this.textBoxDuration.Text = "" + this.trackBarDuration.Minimum;
                }
                else
                {
                    this.trackBarDuration.Value = textBoxValue;
                }
            }
            catch
            {
                //Only string
            }
        }
        private void trackBarInterval_ValueChanged(object sender, System.EventArgs e)
        {
        }
        private void trackBarFrequency_ValueChanged(object sender, System.EventArgs e)
        {
        }
        private void trackBarAmplitude_ValueChanged(object sender, System.EventArgs e)
        {
        }
        private void trackBarDuration_ValueChanged(object sender, System.EventArgs e)
        {
        }
        private void buttonStop_Click(object sender, System.EventArgs e)
        {
            if(backgroundBeep != null)
            {
                backgroundBeep.StopBeep();
            }
            shouldStop = true;
            this.btnStart.Enabled = true;
        }
        private void buttonStart_Click(object sender, System.EventArgs e)
        {
            Action a = () => GenerateBeep();
            tskGenerateBeep = Task.Factory.StartNew(a);
            this.btnStart.Enabled = false;
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
            { 
            this.WindowState = FormWindowState.Normal;
            }
            this.Show();
            // Activate the form.
            this.Activate();
        }
        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                this.notifyIcon.Visible = true;
                this.notifyIcon.ShowBalloonTip(500);
                this.Hide();
            }

            else if (FormWindowState.Normal == this.WindowState)
            {
                this.notifyIcon.Visible = false;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.notifyIcon.Visible = true;
                this.Hide();
                e.Cancel = true;
            }
        }

        private void menuItem_Click(object Sender, EventArgs e)
        {
            // Close the form, which closes the application.
            this.Close();
        }
    }
}
