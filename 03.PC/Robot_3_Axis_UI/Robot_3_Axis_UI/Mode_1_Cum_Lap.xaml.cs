﻿using Newtonsoft.Json;
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
using static Robot_3_Axis_UI.LoginWindow;

namespace Robot_3_Axis_UI
{
    /// <summary>
    /// Interaction logic for Mode_1_Cum_Lap.xaml
    /// </summary>
    public partial class Mode_1_Cum_Lap : UserControl
    {
        private Thread timerThread;
        Path path = new Path();
        string Ip_Port_Host;
        HttpClient client = new HttpClient();
        Update_Screen update = new Update_Screen();
        dynamic data;
        float Cum_Lap_Jog_Pos_Up = 0;
        float Cum_Lap_Jog_Pos_Kep = 0;
        public Mode_1_Cum_Lap()
        {
            InitializeComponent();
        }
        private void Mode_1_Cum_Lap_Screen_Loaded(object sender, RoutedEventArgs e)
        {
            string json = File.ReadAllText(path.Setting);
            var data_Setting = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            Ip_Port_Host = data_Setting["Ip_Port_Ser"];
            timerThread = new Thread(TimerThreadFunction);
            timerThread.Start();
            txb_Pos_KepLap.Text = "0";
            txb_Pos_NangLap.Text = "0";
        }
        private void Mode_1_Cum_Lap_Screen_Unloaded(object sender, RoutedEventArgs e)
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
                update.bt_Colors_Status_1(bt_Jog_Add_NangLap, Convert.ToString(data.Cum_Lap_Jog_Up));
                update.bt_Colors_Status_1(bt_Jog_Sub_KepLap, Convert.ToString(data.Cum_Lap_Jog_Kep));
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
        private void Nang_Cum_Lap_Jog_ADD_Click(object sender, RoutedEventArgs e)
        {
            Control("Cum_Lap_Excute_Pos_Up", true, 2);
        }
        private void Kep_Cum_Lap_Jog_SUB_Click(object sender, RoutedEventArgs e)
        {
            Control("Cum_Lap_Excute_Pos_Kep", true, 2);
        }
        private async void txb_Pos_NangLap_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (data != null)
            {
                try
                {
                    if (float.TryParse(txb_Pos_NangLap.Text, out float result))
                    {
                        Cum_Lap_Jog_Pos_Up = result;
                        var data = new
                        {
                            Cum_Lap_Pos_Jog_Up = Cum_Lap_Jog_Pos_Up,
                        };
                        string jsonData = JsonConvert.SerializeObject(data);
                        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                        var response = await client.PostAsync("http://" + Ip_Port_Host + "/api/Control_PLC_2", content);
                        var responseContent = await response.Content.ReadAsStringAsync();
                    }
                }
                catch
                {
                }
            }
        }
        private async void txb_Pos_KepLap_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (data != null)
            {
                try
                {
                    if (float.TryParse(txb_Pos_KepLap.Text, out float result))
                    {
                        Cum_Lap_Jog_Pos_Kep = result;
                        var data = new
                        {
                            Cum_Lap_Pos_Jog_Kep = Cum_Lap_Jog_Pos_Kep,
                        };
                        string jsonData = JsonConvert.SerializeObject(data);
                        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                        var response = await client.PostAsync("http://" + Ip_Port_Host + "/api/Control_PLC_2", content);
                        var responseContent = await response.Content.ReadAsStringAsync();
                    }
                }
                catch
                {
                }
            }

        }
    }
}