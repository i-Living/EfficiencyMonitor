using EfficiencyMonitor.Monitors;
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

namespace EfficiencyMonitor
{
    /// <summary>
    /// Логика взаимодействия для MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow : Window
    {
        public string MyMessage { get; set; }
        private int ExtendTime;
        private ActivityMonitor am;

        public MessageWindow(ActivityMonitor am)
        {
            InitializeComponent();
            MyMessage = string.Empty;
            Message.Text = MyMessage;
            ExtendTime = 0;
            this.am = am;

            comboBox.Items.Add("5 минут");
            comboBox.Items.Add("10 минут");
            comboBox.Items.Add("15 минут");
            comboBox.Items.Add("30 минут");
            comboBox.Items.Add("1 час");
            comboBox.SelectedItem = comboBox.Items[0];
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            if (am.WorkingStatus == 1)
            {
                am.WorkingTime = 0;
            }
            if (am.WorkingStatus == 2)
            {
                am.RelaxTime = 0;
            }
        }

        private void LaterBtn_Click(object sender, RoutedEventArgs e)
        {
            if (am.WorkingStatus == 1)
            {
                am.WorkingTime -= ExtendTime;
            }
            if (am.WorkingStatus == 2)
            {
                am.RelaxTime -= ExtendTime;
            }
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (comboBox.SelectedItem.ToString())
            {
                case "5 минут": ExtendTime = 300;
                    break;
                case "10 минут":
                    ExtendTime = 600;
                    break;
                case "15 минут":
                    ExtendTime = 900;
                    break;
                case "30 минут":
                    ExtendTime = 1800;
                    break;
                case "1 час":
                    ExtendTime = 3600;
                    break;
                default:
                    break;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (am.WorkingStatus == 1)
            {
                am.WorkingTime = 0;
            }
            if (am.WorkingStatus == 2)
            {
                am.RelaxTime = 0;
            }
        }
    }
}
