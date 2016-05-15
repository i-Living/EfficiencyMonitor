using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EfficiencyMonitor.Monitors;
using System.IO;
using System.Data.SQLite;
using System.Data.Common;
using System.Data;

namespace EfficiencyMonitor
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Timers.Timer t;
        string _appName;
        ulong _time;
        WindowMonitor wm;
        private delegate void StrDel(string str);
        private const string databaseName = @"..\..\DB\DataBase.db";
        private Dictionary<string, ulong> dict;
        private System.Windows.Forms.NotifyIcon TrayIcon = null;
        private System.Windows.Controls.ContextMenu TrayMenu = null;
        private WindowState fCurrentWindowState = WindowState.Normal;

        public MainWindow()
        {
            InitializeComponent();
            dict = new Dictionary<string, ulong>();
            t = new System.Timers.Timer();
            t.Interval = 1000;
            t.Elapsed += OnTimedEvent;
            _appName = string.Empty;
            wm = new WindowMonitor();
            CreateDB();
            _time = 0;
    }

        private void CreateDB()
        {
            if(!File.Exists(databaseName))
            {
                SQLiteConnection.CreateFile(databaseName);
                CreateTables();
            }
        }
        
        private void CreateTables()
        {
            SQLiteFactory factory = (SQLiteFactory)DbProviderFactories.GetFactory("System.Data.SQLite");
            using (SQLiteConnection connection = (SQLiteConnection)factory.CreateConnection())
            {
                connection.ConnectionString = "Data Source = " + databaseName;
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"CREATE TABLE [Applications] (
                    [id] integer PRIMARY KEY AUTOINCREMENT NOT NULL,
                    [name] char(100) NOT NULL,
                    [time] int NOT NULL
                    );";
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
            }
        }

        private void OnTimedEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            _time += 1;
            if(_appName != wm.GetActiveWindowTitle())
            {
                if (_appName != null)
                    UpdateDict(_appName, _time);
                //_appName = wm.GetActiveWindowTitle();
                _appName = wm.GetActiveProcessName();
                _time = 0;
            }
        }

        private void UpdateDict(string appName, ulong time)
        {
            if (!dict.ContainsKey(appName))
            {
                dict.Add(appName, time);
            }
            else
            {
                dict[appName] += time;
            }            
            listBox.Dispatcher.Invoke(new Action(() => { listBox.ItemsSource = dict; }));
            listBox.Dispatcher.Invoke(new Action(() => { listBox.Items.Refresh(); }));
        }

        private void StartBtn_Click(object sender, RoutedEventArgs e)
        {
            t.Enabled = true;
            t.Start();
        }

        private void PauseBtn_Click(object sender, RoutedEventArgs e)
        {
            t.Stop();
        }


        //Minimize Icon Code
        private bool createTrayIcon()
        {
            bool result = false;
            if (TrayIcon == null)
            { // только если мы не создали иконку ранее
                TrayIcon = new System.Windows.Forms.NotifyIcon(); // создаем новую
                TrayIcon.Icon = EfficiencyMonitor.Properties.Resources.eye; // изображение для трея
                                                                             // обратите внимание, за ресурсом с картинкой мы лезем в свойства проекта, а не окна,
                                                                             // поэтому нужно указать полный namespace
                TrayIcon.Text = "Here is tray icon text."; // текст подсказки, всплывающей над иконкой
                TrayMenu = Resources["TrayMenu"] as ContextMenu; // а здесь уже ресурсы окна и тот самый x:Key

                // сразу же опишем поведение при щелчке мыши, о котором мы говорили ранее
                // это будет просто анонимная функция, незачем выносить ее в класс окна
                TrayIcon.Click += delegate (object sender, EventArgs e) {
                    if ((e as System.Windows.Forms.MouseEventArgs).Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        // по левой кнопке показываем или прячем окно
                        ShowHideMainWindow(sender, null);
                    }
                    else
                    {
                        // по правой кнопке (и всем остальным) показываем меню
                        TrayMenu.IsOpen = true;
                        Activate(); // нужно отдать окну фокус, см. ниже
                    }
                };
                result = true;
            }
            else
            { // все переменные были созданы ранее
                result = true;
            }
            TrayIcon.Visible = true; // делаем иконку видимой в трее
            return result;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e); // базовый функционал приложения в момент запуска
            createTrayIcon(); // создание нашей иконки
        }

        private void ShowHideMainWindow(object sender, RoutedEventArgs e)
        {
            TrayMenu.IsOpen = false; // спрячем менюшку, если она вдруг видима
            if (IsVisible)
            {// если окно видно на экране
             // прячем его
                Hide();
                // меняем надпись на пункте меню
                (TrayMenu.Items[0] as MenuItem).Header = "Show";
            }
            else
            { // а если не видно
              // показываем
                Show();
                // меняем надпись на пункте меню
                (TrayMenu.Items[0] as MenuItem).Header = "Hide";
                WindowState = CurrentWindowState;
                Activate(); // обязательно нужно отдать фокус окну,
                            // иначе пользователь сильно удивится, когда увидит окно
                            // но не сможет в него ничего ввести с клавиатуры
            }
        }
        public WindowState CurrentWindowState
        {
            get { return fCurrentWindowState; }
            set { fCurrentWindowState = value; }
        }
        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e); // системная обработка
            if (this.WindowState == System.Windows.WindowState.Minimized)
            {
                // если окно минимизировали, просто спрячем
                Hide();
                // и поменяем надпись на менюшке
                (TrayMenu.Items[0] as MenuItem).Header = "Show";
            }
            else
            {
                // в противном случае запомним текущее состояние
                CurrentWindowState = WindowState;
            }
        }
        private void MenuExitClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
