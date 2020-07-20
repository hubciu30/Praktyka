using DatyCzas;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace CzasDoKonca
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ShowTime.Visibility = Visibility.Hidden;
        }

        private Task Process;
        private CDate Date;

        private void Calculate()
        {
            while (Date != null && Date.GetTick() > 0)
            {                        
                Dispatcher.Invoke(() =>
                {
                    Time.Content = Date.GetTimeToEndWork();
                    Percent.Content = Math.Round(Date.GetPercent(),2) + "%";
                    Weekend.Content = Date.GetWeekendPercent();
                   
                });
                Thread.Sleep(500);
            }
            Dispatcher.Invoke(() =>
            {
                if (Date != null && Date.GetTick() <= 0)
                {
                    Time.Content = "00:00:00";
                    Percent.Content = "100%";
                    Weekend.Content = Date.GetWeekendPercent();
                }
            });          
        }

        private void Button_Start(object sender, RoutedEventArgs e)
        {
            if (isGood(StartTime.Text))
            {
                StartTime.BorderBrush = Brushes.Black;
                if (isGood(EndTime.Text))
                {
                    EndTime.BorderBrush = Brushes.Black;
                    switch (MainButton.Content)
                    {
                        case "Start":
                            ShowTime.Visibility = Visibility.Visible;
                            Date = new CDate(StartTime.Text, EndTime.Text);
                            Process = new Task(Calculate);                           
                            Process.Start();
                            MainButton.Content = "Stop";
                            break;
                        case "Stop":
                            Process = null;
                            Date = null;
                            MainButton.Content = "Start";
                            break;
                    }
                }
                else
                {
                    EndTime.BorderBrush = Brushes.Red;
                }
            }
            else
            {
                StartTime.BorderBrush = Brushes.Red;
            }                 
        }

        private bool isGood(string time)
        {
            var tmp = time.Split(':');
            if(tmp.Length == 3)
            {
                for(int i = 0; i < tmp.Length; i++)
                {
                    try
                    {
                        int n = Convert.ToInt16(tmp[i]);
                        switch (i)
                        {
                            case 0:
                                if(n < 0 || n > 23)
                                {
                                    return false;
                                }
                                break;
                            default:
                                if (n < 0 || n > 59)
                                {
                                    return false;
                                }
                                break;
                        }
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
            return true;
        }
    }
}
