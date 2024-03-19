using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Robot_3_Axis_UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Mutex _mutex = null;

        protected override void OnStartup(StartupEventArgs e)
        {
            const string mutexName = "Robot 3 Axis UI Control"; // Đặt tên Mutex duy nhất của bạn

            try
            {
                _mutex = Mutex.OpenExisting(mutexName);
                MessageBox.Show("Ứng dụng đã được khởi động, vui lòng đợi!");
                Application.Current.Shutdown();
            }
            catch
            {
                _mutex = new Mutex(true, mutexName);
            }

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (_mutex != null)
            {
                _mutex.ReleaseMutex();
                _mutex.Close();
            }

            base.OnExit(e);
        }
    }
}
