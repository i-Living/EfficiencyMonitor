using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EfficiencyMonitor.DB
{
    class CategoryCollection : List<AppCategory>
    {
        public void AddCategory(AppCategory AppName)
        {
            this.Add(AppName);
        }
        public void DeleteCategory(AppCategory AppName)
        {
            this.Remove(AppName);
        }
    }
}
