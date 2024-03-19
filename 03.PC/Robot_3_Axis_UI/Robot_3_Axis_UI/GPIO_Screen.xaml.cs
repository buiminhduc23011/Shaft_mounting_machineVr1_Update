using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Threading;

namespace Robot_3_Axis_UI
{
    /// <summary>
    /// Interaction logic for GPIO_Screen.xaml
    /// </summary>
    public partial class GPIO_Screen : UserControl
    {
        private DispatcherTimer timer;
        public GPIO_Screen()
        {

            InitializeComponent();
        }
        private void GPIO_Screen_Loaded(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void GPIO_Screen_Unloaded(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            timer.Tick -= Timer_Tick;
            timer = null;
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                });
            }
            catch
            {

            }
        }
    }
}
