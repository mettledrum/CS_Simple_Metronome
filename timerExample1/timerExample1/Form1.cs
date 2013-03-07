using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//for stopwatch()
using System.Diagnostics;

//fo sounds
using System.Media;

// 3/3/13, 3/4, 3/5
//A first attempt at making a metronome form for windows.
// To use: Stopwatch() for getting the tempo, then a Timer()
// stuffed with the Stopwatch interval for making events
// that show or sound some blips.

//Then, make some time signature allowances and use modulus
// for some simple downbeat blips that are different from
// the filler bleeps.

//numericUpDown - rounds up or down based on decimal entered,
// but the float to int always rounds down, so there's a 
// discrepancy IF a decimal that rounds up is typed.
//just limit it to ReadOnly=true so user can't mess it up.


namespace timerExample1
{
    public partial class Form1 : Form
    {
        //keeps track of the time
        private Timer timer = new Timer();

        //for TAP button, sets BPM
        private Stopwatch s1 = new Stopwatch();

        //sounds to load, upbeats, downbeats
        private SoundPlayer DBsound = new SoundPlayer(Properties.Resources.cl1);
        private SoundPlayer UBsound = new SoundPlayer(Properties.Resources.cl2);

        //meter variables
        private int Tsign;
        private int TcountDown;

        public Form1()
        {
            //some sort of beginning stuff for form I think?
            InitializeComponent();

            //user can only use arrows
            numericUpDown1.ReadOnly = true;

            //can't resize the form
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            //timer init
            timer.Tick += new EventHandler(timer_Tick);     //every time timer ticks, timer_Tick is called
            timer.Interval = 500;                           //default 120 BPM
            
            //show BPM on label1
            int BPM = (int)(60000 / timer.Interval);
            label1.Text = BPM.ToString();

            //default time signature is 1
            Tsign = (int)numericUpDown1.Value;
            TcountDown = Tsign;
        }

        //subscribed to t1.Tick event
        void timer_Tick(object sender, EventArgs e)
        {
            //downbeat or not based in time sig. vars
            if (TcountDown == 1)
            {
                DBsound.Play();
                TcountDown = Tsign;
            }
            else
            {
                UBsound.Play();
                --TcountDown;
            }
        }

        //starts timer
        private void button1_Click(object sender, EventArgs e)
        {
            if(timer.Enabled == false)
            {
                button1.Text = "STOP";
                timer.Enabled = true;
                timer.Start();                              //Start the timer
            }
            else
            {
                button1.Text = "START";
                timer.Enabled = false;
                timer.Stop();
            }
        }

        //TAP button
        private void button3_Click(object sender, EventArgs e)
        {
            //default already set to 120 on constructor
            if(s1.ElapsedMilliseconds != 0)
            {
                //only get interval after second click of TAP
                timer.Interval = (int)(s1.ElapsedMilliseconds);     //cannot take 0
            }

            //starts the stopwatch over again, AFTER getting the interval value
            s1.Restart();

            //show BPM on label1
            int BPM = (int)(60000 / timer.Interval);
            label1.Text = BPM.ToString();
        }

        //time signature value changed
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Tsign = (int)numericUpDown1.Value;
            TcountDown = Tsign;
        }
    }
}
