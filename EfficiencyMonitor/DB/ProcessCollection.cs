using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EfficiencyMonitor.DB
{
    class ProcessCollection : List<ProcessData>
    {
        public void Load(List<ProcessData> listPD)
        {
            foreach (var item in listPD)
            {
                this.Add(item);
            }
        }
        public void AddProcess(ProcessData processName)
        {
            this.Add(processName);
        }
        public void DeleteProcess(ProcessData processName)
        {
            this.Remove(processName);
        }
        public void Update(string name, ulong time)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if(!ContainsName(name))
                {
                    Add(new ProcessData(name, time));
                }
                else
                {
                    if (this[i].Name == name)
                    {
                        this[i].Time += time;
                        this[i].Time1 += TimeSpan.FromSeconds(time);
                    }
                }
            }
        }
        public bool ContainsName(string name)
        {
            foreach (ProcessData process in this)
            {
                if (process.Name == name)
                    return true;
            }
            return false;
        }
        //public IEnumerator<ProcessData> GetEnumerator()
        //{
        //    return this.GetEnumerator();
        //}
        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return this.GetEnumerator();
        //}
    }
}
