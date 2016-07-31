using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Timers;

namespace EfficiencyMonitor.Monitors
{
    public class InputMonitor
    {
        private IKeyboardMouseEvents m_GlobalHook;
        public ulong MouseCount { get; set; }
        public ulong KeyboardCount { get; set; }
        public uint maxIdle { get; set; }
        public bool idleState { get; set; }
        private System.Windows.Controls.Label MouseL;
        private System.Windows.Controls.Label KeyboardL;
        private System.Timers.Timer timer;
        public uint time;

        public InputMonitor(System.Windows.Controls.Label mouse, System.Windows.Controls.Label keyboard)
        {
            this.MouseCount = 0;
            this.KeyboardCount = 0;
            this.MouseL = mouse;
            this.KeyboardL = keyboard;
            timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += OnTimedEvent;
            time = 0;
            maxIdle = 300;
            idleState = false;
            timer.Start();
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            time++;
            if (time >= maxIdle)
            {
                idleState = true;
            }
            if (time >= 1000)
                time = maxIdle + 1;
        }

        public void Subscribe()
        {
            // Note: for the application hook, use the Hook.AppEvents() instead
            m_GlobalHook = Hook.GlobalEvents();

            m_GlobalHook.MouseDownExt += GlobalHookMouseDown;
            m_GlobalHook.KeyPress += GlobalHookKeyPress;
        }

        private void GlobalHookKeyPress(object sender, KeyPressEventArgs e)
        {
            //Console.WriteLine("KeyPress: \t{0}", e.KeyChar);
            KeyboardCount++;
            KeyboardL.Dispatcher.Invoke(new Action(() => { KeyboardL.Content = KeyboardCount.ToString(); }));
            time = 0;
            idleState = false;
        }

        private void GlobalHookMouseDown(object sender, MouseEventExtArgs e)
        {
            //Console.WriteLine("MouseDown: \t{0}; \t System Timestamp: \t{1}", e.Button, e.Timestamp);
            // uncommenting the following line will suppress the middle mouse button click
            //if (e.Buttons == MouseButtons.Middle) { e.Handled = true; }
            MouseCount++;
            KeyboardL.Dispatcher.Invoke(new Action(() => { MouseL.Content = MouseCount.ToString(); }));
            time = 0;
            idleState = false;
        }

        public void Unsubscribe()
        {
            m_GlobalHook.MouseDownExt -= GlobalHookMouseDown;
            m_GlobalHook.KeyPress -= GlobalHookKeyPress;

            //It is recommened to dispose it
            m_GlobalHook.Dispose();
        }
    }
}
