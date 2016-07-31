using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EfficiencyMonitor.DB
{
    class AppCategory
    {
        [PrimaryKey, AutoIncrement]
        public int ProcessId { get; set; }
        public string CategoryName { get; set; }
        public int CategoryStatus { get; set; } //0 = не учитывать, 1 = работа, 2 = отдых
    }
}
