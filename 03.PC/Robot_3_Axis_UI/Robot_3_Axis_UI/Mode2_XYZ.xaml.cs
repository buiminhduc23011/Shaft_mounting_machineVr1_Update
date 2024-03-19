using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
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

namespace Robot_3_Axis_UI
{
    /// <summary>
    /// Interaction logic for Mode2_XYZ.xaml
    /// </summary>
    public partial class Mode2_XYZ : UserControl
    {
        private Thread timerThread;
        Path path = new Path();
        string Ip_Port_Host;
        HttpClient client = new HttpClient();
        Update_Screen update = new Update_Screen();
        dynamic data;
        public Mode2_XYZ()
        {
            InitializeComponent();
            Touch.FrameReported += Touch_FrameReported;
        }
        private void Mode_2_Cum_XYZ_Screen_Loaded(object sender, RoutedEventArgs e)
        {
            string json = File.ReadAllText(path.Setting);
            var data_Setting = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            Ip_Port_Host = data_Setting["Ip_Port_Ser"];
            timerThread = new Thread(TimerThreadFunction);
            timerThread.Start();
        }
       private void Mode_2_Cum_XYZ_Screen_Unloaded(object sender, RoutedEventArgs e)
        {
            if (timerThread != null && timerThread.IsAlive)
            {
                timerThread.Abort();
                timerThread.Join();
            }
            

        }
        private void Touch_FrameReported(object sender, TouchFrameEventArgs e)
        {
            // Kiểm tra xem có ngón tay nào đang chạm vào màn hình hay không
            if (e.GetTouchPoints(null).Count > 0)
            {
                // Có ngón tay đang chạm vào màn hình
            }
            else
            {
                // Không có ngón tay nào chạm vào màn hình (untouched)
                // Xử lý logic tại đây
                Control("Jog_X_P", false);
                Control("Jog_X_N", false);
                Control("Jog_Y_P", false);
                Control("Jog_Y_N", false);
                Control("Jog_Z_P", false);
                Control("Jog_Z_N", false);
            }
        }
        private void TimerThreadFunction()
        {
            while (true)
            {
                try
                {
                    data = Read_PLC.Data;

                    Dispatcher.Invoke(() =>
                    {
                        Update_Screen();
                        if (data != null)
                        {
                        }
                    });
                }
                catch
                {

                }

                Thread.Sleep(200); // Adjust the sleep interval as needed
            }
        }

        public async void Control(string propertyName, object propertyValue)
        {
            try
            {
                if (data != null)
                {
                        dynamic controlData = new ExpandoObject();
                        var controlDataDict = (IDictionary<string, object>)controlData;
                        controlDataDict[propertyName] = propertyValue;
                        string jsonData = JsonConvert.SerializeObject(controlData);
                        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                        var response = await client.PostAsync("http://" + Ip_Port_Host + "/api/Control_PLC_1", content);
                        var responseContent = await response.Content.ReadAsStringAsync();

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }
        public void Update_Screen()
        {
            if (data != null)
            {
                update.bt_Colors_Status_1(bt_Jog_Add_X, Convert.ToString(data.Jog_X_P));
                update.bt_Colors_Status_1(bt_Jog_Sub_X, Convert.ToString(data.Jog_X_N));
                update.bt_Colors_Status_1(bt_Jog_Add_Y, Convert.ToString(data.Jog_Y_P));
                update.bt_Colors_Status_1(bt_Jog_Sub_Y, Convert.ToString(data.Jog_Y_N));
                update.bt_Colors_Status_1(bt_Jog_Add_Z, Convert.ToString(data.Jog_Z_P));
                update.bt_Colors_Status_1(bt_Jog_Sub_Z, Convert.ToString(data.Jog_Z_N));
                update.bt_Colors_Status_1(Origin_X, Convert.ToString(data.Machine_Run_Home_X));
                update.bt_Colors_Status_1(Origin_Y, Convert.ToString(data.Machine_Run_Home_Y));
                update.bt_Colors_Status_1(Origin_Z, Convert.ToString(data.Machine_Run_Home_Z));
            }
        }
        private async void Origin_X_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var Control = new
                {
                    Machine_Run_Home_Z = true,
                    Machine_Run_Home_X = true,
                };
                string jsonData = JsonConvert.SerializeObject(Control);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://" + Ip_Port_Host + "/api/Control_PLC_1", content);
                var responseContent = await response.Content.ReadAsStringAsync();
            }
            catch
            {

            }
        }

        private async void Origin_Y_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var Control = new
                {
                    Machine_Run_Home_Z = true,
                    Machine_Run_Home_Y = true,
                };
                string jsonData = JsonConvert.SerializeObject(Control);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://" + Ip_Port_Host + "/api/Control_PLC_1", content);
                var responseContent = await response.Content.ReadAsStringAsync();
            }
            catch
            {

            }
        }
        private async void Origin_Z_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var Control = new
                {
                    Machine_Run_Home_Z = true,
                };
                string jsonData = JsonConvert.SerializeObject(Control);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://" + Ip_Port_Host + "/api/Control_PLC_1", content);
                var responseContent = await response.Content.ReadAsStringAsync();
            }
            catch
            {

            }
        }

        private void X_Axis_Jog_ADD_TouchUp(object sender, TouchEventArgs e)
        {
            Control("Jog_X_P", false);
        }

        private void X_Axis_Jog_ADD_TouchDown(object sender, TouchEventArgs e)
        {
            Control("Jog_X_P", true);
        }

        private void X_Axis_Jog_SUB_TouchUp(object sender, TouchEventArgs e)
        {
            Control("Jog_X_N", false);
        }

        private void X_Axis_Jog_SUB_TouchDown(object sender, TouchEventArgs e)
        {
            Control("Jog_X_N", true);
        }

        private void Y_Axis_Jog_ADD_TouchUp(object sender, TouchEventArgs e)
        {
            Control("Jog_Y_P", false);
        }

        private void Y_Axis_Jog_ADD_TouchDown(object sender, TouchEventArgs e)
        {
            Control("Jog_Y_P", true);
        }

        private void Y_Axis_Jog_SUB_TouchUp(object sender, TouchEventArgs e)
        {
            Control("Jog_Y_N", false);
        }

        private void Y_Axis_Jog_SUB_TouchDown(object sender, TouchEventArgs e)
        {
            Control("Jog_Y_N", true);
        }

        private void Z_Axis_Jog_ADD_TouchUp(object sender, TouchEventArgs e)
        {
            Control("Jog_Z_P", false);
        }

        private void Z_Axis_Jog_ADD_TouchDown(object sender, TouchEventArgs e)
        {
            Control("Jog_Z_P",true);
        }

        private void Z_Axis_Jog_SUB_TouchUp(object sender, TouchEventArgs e)
        {
            Control("Jog_Z_N", false);
        }

        private void Z_Axis_Jog_SUB_TouchDown(object sender, TouchEventArgs e)
        {
            Control("Jog_Z_N", true);
        }
    }
}
