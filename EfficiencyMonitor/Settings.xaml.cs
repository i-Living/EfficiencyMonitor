using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using EfficiencyMonitor.Monitors;
using EfficiencyMonitor.DB;
using System.Data;

namespace EfficiencyMonitor
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private MainWindow parent;
        private DbConnector dc;
        private List<Grid> groups;
        private DataTable dt;
        private string[] status = { "Не учитывать", "Учёт работы", "Учёт отдыха" };
        private string[] settingsList = { "Настройки", "Мои группы", "Настройка групп" };

        public Settings(MainWindow parent)
        {
            this.parent = parent;
            dc = new DbConnector(parent.dbPath);
            groups = new List<Grid>();
            dt = new DataTable();
            InitializeComponent();

            cbNewGroup.ItemsSource = status;
            cbNewGroup.SelectedIndex = 0;
            cbSelectedGroup.ItemsSource = status;
            lb.ItemsSource = settingsList;

            groups.Add(settings);
            groups.Add(MyGroups);
            groups.Add(ChangeGroups);
            lb.SelectedIndex = 0;
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            idleTB.Text = Convert.ToString(parent.im.maxIdle);
            GroupList.ItemsSource = LoadAppsFromDB("AppCategory");
            AllAppsLB.ItemsSource = LoadAppsFromDB("KnownApplications");
            SelectedAppsCB.ItemsSource = LoadAppsFromDB("AppCategory");
            SelectedAppsCB.SelectedIndex = 0;
        }

        private List<string> LoadAppsFromDB(string dbName)
        {
            dt = dc.GetTable(dbName, "Name");
            List<string> temp = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                temp.Add(dt.Rows[i].Field<string>("Name"));
            }
            return temp;
        }
        private void SaveidleBtn_Click(object sender, RoutedEventArgs e)
        {
            parent.im.maxIdle = Convert.ToUInt32(idleTB.Text);
        }

        private void AddGroupBtn_Click(object sender, RoutedEventArgs e)
        {
            if(cbNewGroup.SelectedIndex == 0)
            {

            }
            if (cbNewGroup.SelectedIndex == 1)
            {

            }
            if (cbNewGroup.SelectedIndex == 2)
            {

            }
        }

        private void DefaultBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void HideGroups()
        {
            foreach (var item in groups)
            {
                item.Visibility = Visibility.Hidden;
            }
        }

        private void lb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (lb.SelectedItem.ToString())
            {
                case "Настройки":
                    HideGroups();
                    settings.Visibility = Visibility.Visible;
                    break;
                case "Мои группы":
                    HideGroups();
                    MyGroups.Visibility = Visibility.Visible;
                    break;
                case "Настройка групп":
                    HideGroups();
                    ChangeGroups.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        private void GroupList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cbSelectedGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
