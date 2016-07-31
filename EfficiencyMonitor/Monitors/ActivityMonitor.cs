using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

namespace EfficiencyMonitor.Monitors
{
    public class ActivityMonitor
    {
        public int WorkingStatus { get; set; } //0 = не учитывать, 1 = работа, 2 = отдых
        public bool Idle { get; set; }
        public int RelaxTime { get; set; }
        public int WorkingTime { get; set; }
        public int MaxRelaxTime { get; set; }
        public int MaxWorkingTime { get; set; }
        public Timer timer;
        private InputMonitor im;

        public ActivityMonitor(InputMonitor im)
        {
            timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += OnTimedEvent;
            Idle = true;
            WorkingStatus = 0;
            RelaxTime = 0;
            WorkingTime = 0;
            MaxRelaxTime = 15;
            MaxWorkingTime = 4;
            this.im = im;
        }

        public void StartTimer()
        {
            timer.Start();
        }
        public void PauseTimer()
        {
            timer.Stop();
        }
        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            Idle = im.idleState;
            WorkingStatus = GetActivSatus();
            if (!Idle)
            {
                if (WorkingStatus == 1)
                {
                    WorkingTime++;
                }
                if (WorkingStatus == 2)
                {
                    RelaxTime++;
                }
            }
            if(RelaxTime >= MaxRelaxTime)
            {
                MessageWindow mw = new MessageWindow(this);
                mw.Message.Text = "Вы много отдыхаете, пора работать";
                mw.Show();
            }
            if (WorkingTime >= MaxWorkingTime)
            {
                MessageWindow mw = new MessageWindow(this);
                mw.Message.Text = "Вы много работаете, пора отдохнуть";
                mw.Show();
            }
        }
        
        private int GetActivSatus()
        {
            return 0;
        }
    }
}
