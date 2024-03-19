using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
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
using System.IO;

namespace Robot_3_Axis_UI
{
    /// <summary>
    /// Interaction logic for Mode_2_Cum_Do.xaml
    /// </summary>
    public partial class Mode_2_Cum_Do : UserControl
    {
        private Thread timerThread;
        Path path = new Path();
        string Ip_Port_Host;
        HttpClient client = new HttpClient();
        Update_Screen update = new Update_Screen();
        dynamic data;
        public Mode_2_Cum_Do()
        {
            InitializeComponent();
        }
        private void Mode_2_Cum_Do_Screen_Loaded(object sender, RoutedEventArgs e)
        {
            string json = File.ReadAllText(path.Setting);
            var data_Setting = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            Ip_Port_Host = data_Setting["Ip_Port_Ser"];
            timerThread = new Thread(TimerThreadFunction);
            timerThread.Start();
        }
        private void Mode_2_Cum_Do_Screen_Unloaded(object sender, RoutedEventArgs e)
        {
            if (timerThread != null && timerThread.IsAlive)
            {
                timerThread.Abort();
                timerThread.Join();
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
                            update.bt_Colors_Status_1(bt_Jog_Up, Convert.ToString(data.Jog_Up_Cum_Do));
                            update.bt_Colors_Status_1(bt_Jog_Down, Convert.ToString(data.Jog_Down_Cum_Do));
                            update.bt_Colors_Status_1(bt_Jog_Mo, Convert.ToString(data.Jog_Mo_Cum_Do));
                            update.bt_Colors_Status_1(bt_Jog_Kep, Convert.ToString(data.Jog_Kep_Cum_Do));
                        }
                    });
                }
                catch
                {

                }

                Thread.Sleep(200); // Adjust the sleep interval as needed
            }
        }


        public void Update_Screen()
        {
            if (data != null)
            {

            }
        }
        public async void Control(string propertyName, object propertyValue, int PLC)
        {
            if (data != null)
            {
                if (PLC == 1)
                {
                    dynamic controlData = new ExpandoObject();
                    var controlDataDict = (IDictionary<string, object>)controlData;
                    controlDataDict[propertyName] = propertyValue;
                    string jsonData = JsonConvert.SerializeObject(controlData);
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("http://" + Ip_Port_Host + "/api/Control_PLC_1", content);
                    var responseContent = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    dynamic controlData = new ExpandoObject();
                    var controlDataDict = (IDictionary<string, object>)controlData;
                    controlDataDict[propertyName] = propertyValue;
                    string jsonData = JsonConvert.SerializeObject(controlData);
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("http://" + Ip_Port_Host + "/api/Control_PLC_2", content);
                    var responseContent = await response.Content.ReadAsStringAsync();
                }
            }
        }
        private void bt_Jog_Up_TouchUp(object sender, TouchEventArgs e)
        {
            Control("Jog_Up_Cum_Do", false, 2);
        }
        private void bt_Jog_Up_TouchLeave(object sender, TouchEventArgs e)
        {
            Control("Jog_Up_Cum_Do", false, 2);
        }
        private void bt_Jog_Up_TouchDown(object sender, TouchEventArgs e)
        {
            Control("Jog_Up_Cum_Do", true, 2);
        }
        //
        private void bt_Jog_Down_TouchUp(object sender, TouchEventArgs e)
        {
            Control("Jog_Down_Cum_Do", false, 2);
        }
        private void bt_Jog_Down_TouchLeave(object sender, TouchEventArgs e)
        {
            Control("Jog_Down_Cum_Do", false, 2);
        }
        private void bt_Jog_Down_TouchDown(object sender, TouchEventArgs e)
        {
            Control("Jog_Down_Cum_Do", true, 2);
        }
        //
        private void bt_Jog_Kep_TouchUp(object sender, TouchEventArgs e)
        {
            Control("Jog_Kep_Cum_Do", false, 2);
        }
        private void bt_Jog_Kep_TouchLeave(object sender, TouchEventArgs e)
        {
            Control("Jog_Kep_Cum_Do", false, 2);
        }
        private void bt_Jog_Kep_TouchDown(object sender, TouchEventArgs e)
        {
            Control("Jog_Kep_Cum_Do", true, 2);
        }
        //

        private void bt_Jog_Mo_TouchUp(object sender, TouchEventArgs e)
        {
            Control("Jog_Mo_Cum_Do", false, 2);
        }
        private void bt_Jog_Mo_TouchLeave(object sender, TouchEventArgs e)
        {
            Control("Jog_Mo_Cum_Do", false, 2);
        }
        private void bt_Jog_Mo_TouchDown(object sender, TouchEventArgs e)
        {
            Control("Jog_Mo_Cum_Do", true, 2);
        }
    }
}
