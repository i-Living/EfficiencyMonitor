using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EfficiencyMonitor.DB
{
    public class ProcessData
    {
        [PrimaryKey, AutoIncrement]
        public int ProcessId { get; set; }
        [MaxLength(250)]
        public string Name { get; set; }
        public int Category { get; set; }
        public ulong Time { get; set; }
        public TimeSpan Time1 { get; set; }

        public ProcessData()
        {
            Name = string.Empty;
            Time = 0;
            Time1 = TimeSpan.Zero;
        }
        public ProcessData(string name, ulong time)
        {
            this.Name = name;
            this.Time = time;
            this.Time1 = TimeSpan.FromSeconds(time);
        }
        public ProcessData(int id, string name, ulong time)
        {
            this.ProcessId = id;
            this.Name = name;
            this.Time = time;
            this.Time1 = TimeSpan.FromSeconds(time);
        }
        public void AddProcess(string name, ulong time)
        {
            this.Name = name;
            this.Time = time;
            this.Time1 = TimeSpan.FromSeconds(time);
        }
        public void UpdateProcess(string name, ulong time)
        {
            this.Time = time;
            this.Time1 = TimeSpan.FromSeconds(time);
        }
        public void UpdateTime()
        {
            this.Time1 = TimeSpan.FromSeconds(this.Time);
        }
    }
}
