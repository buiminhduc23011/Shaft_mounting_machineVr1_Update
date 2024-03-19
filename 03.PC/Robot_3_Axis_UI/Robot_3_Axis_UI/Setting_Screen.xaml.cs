using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Text.Json;
using System.IO;
using System.IO.Ports;
using System.Net.Http;
using System.Windows.Threading;
using Newtonsoft.Json;
using System.Text;
using System.Dynamic;
using System.Windows.Shapes;

namespace Robot_3_Axis_UI
{
    /// <summary>
    /// Interaction logic for Setting_Screen.xaml
    /// </summary>
    public partial class Setting_Screen : UserControl
    {
        Path Path = new Path();
        string Ip_Port_Host;
        string Host_Socket;
        HttpClient client = new HttpClient();
        private DispatcherTimer timer;
        Update_Screen update = new Update_Screen();
        dynamic data;
        private bool Flag_Save_Limit = false;
        private bool Flag_Save_Velocity = false;
        private bool Flag_Save_Of = false;
        public Setting_Screen()
        {
            InitializeComponent();
        }
        private void Setting_Screen_Loaded(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += Timer_Tick;
            timer.Start();
            try
            {
                string json = File.ReadAllText(Path.Setting);
                string json_Maintance = File.ReadAllText(Path.Maintenancen);

                var data_Setting = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                var time_Maintance = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(json_Maintance);

                Ip_Port_Host = data_Setting["Ip_Port_Ser"];
                string[] Server_PLC = Ip_Port_Host.Split(':');
                txb_IPServer.Text = Server_PLC[0];
                txb_Port.Text = Server_PLC[1];
                //
                Host_Socket = data_Setting["Host_Socket"];
                string[] Socket_Server = Host_Socket.Split(':');
                txb_IP_Server.Text = Socket_Server[0];
                txb_Port_Server.Text = Socket_Server[1];

                //
                txb_Time_Maintance.Text = time_Maintance["Time_Maintance"];
                txb_NextTime_Maintance.Text = time_Maintance["Time_Next_Maintance"];

                string[] portNames = SerialPort.GetPortNames();

                cbx_Comport_InTray.Items.Clear();
                cbx_Comport_OutTray.Items.Clear();
                foreach (string portName in portNames)
                {
                    cbx_Comport_InTray.Items.Add(portName);
                    cbx_Comport_OutTray.Items.Add(portName);
                }

                if (cbx_Comport_InTray.Items.Count > 0)
                {
                    cbx_Comport_InTray.SelectedItem = cbx_Comport_InTray.Items[0];
                }
                if (cbx_Comport_OutTray.Items.Count > 0)
                {
                    cbx_Comport_OutTray.SelectedItem = cbx_Comport_OutTray.Items[1];
                }
            }
            catch
            {

            }
            Setting_Limit_X_Pos.IsEnabled = false;
            Setting_Limit_X_Neg.IsEnabled = false;
            Setting_Limit_Y_Pos.IsEnabled = false;
            Setting_Limit_Y_Neg.IsEnabled = false;
            Setting_Limit_Z_Pos.IsEnabled = false;
            Setting_Limit_Z_Neg.IsEnabled = false;
            //
            Setting_Limit_Pos_KepCheck.IsEnabled = false;
            Setting_Limit_Neg_KepCheck.IsEnabled = false;
            setting_Limit_Pos_NangCheck.IsEnabled = false;
            setting_Limit_Neg_NangCheck.IsEnabled = false;
            //
            Setting_Limit_Pos_KepLap.IsEnabled = false;
            Setting_Limit_Neg_KepLap.IsEnabled = false;
            setting_Limit_Pos_NangLap.IsEnabled = false;
            setting_Limit_Neg_NangLap.IsEnabled = false;
            //
            txb_Velocity_X_Man.IsEnabled = false;
            txb_Velocity_Y_Man.IsEnabled = false;
            txb_Velocity_Z_Man.IsEnabled = false;
            txb_Velocity_X_Auto.IsEnabled = false;
            txb_Velocity_Y_Auto.IsEnabled = false;
            txb_Velocity_Z_Auto.IsEnabled = false;
            //
            txb_Velocity_Cum_Check_Nang.IsEnabled = false;
            txb_Velocity_Cum_Check_Kep.IsEnabled = false;
            txb_Velocity_Cum_Lap_Nang.IsEnabled = false;
            txb_Velocity_Cum_Lap_Kep.IsEnabled = false;
            //
            txb_Of_TayKep_CumLap.IsEnabled = false;
            txb_Of_Nang_CumLap.IsEnabled = false;
            txb_Of_TayKep_CumDo.IsEnabled = false;
            txb_Of_Nang_CumDo.IsEnabled = false;

        }
        private void Setting_Screen_Unloaded(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            timer.Tick -= Timer_Tick;
            timer = null;
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                data = Read_PLC.Data;
                Dispatcher.Invoke(() =>
                {
                    if (data != null)
                    {
                        Up_Screen();
                    }

                });
            }
            catch
            {

            }
        }
        private void Up_Screen()
        {
            if (!Flag_Save_Limit)
            {
                Setting_Limit_X_Pos.Text = Convert.ToString(data.Setting_Limit_X_Pos);
                Setting_Limit_X_Neg.Text = Convert.ToString(data.Setting_Limit_X_Neg);
                Setting_Limit_Y_Pos.Text = Convert.ToString(data.Setting_Limit_Y_Pos);
                Setting_Limit_Y_Neg.Text = Convert.ToString(data.Setting_Limit_Y_Neg);
                Setting_Limit_Z_Pos.Text = Convert.ToString(data.Setting_Limit_Z_Pos);
                Setting_Limit_Z_Neg.Text = Convert.ToString(data.Setting_Limit_Z_Neg);
                //
                Setting_Limit_Pos_KepCheck.Text = Convert.ToString(data.Setting_Limit_Pos_KepCheck);
                Setting_Limit_Neg_KepCheck.Text = Convert.ToString(data.Setting_Limit_Neg_KepCheck);
                setting_Limit_Pos_NangCheck.Text = Convert.ToString(data.Setting_Limit_Pos_NangCheck);
                setting_Limit_Neg_NangCheck.Text = Convert.ToString(data.Setting_Limit_Neg_NangCheck);
                //
                Setting_Limit_Pos_KepLap.Text = Convert.ToString(data.Setting_Limit_Pos_KepLap);
                Setting_Limit_Neg_KepLap.Text = Convert.ToString(data.Setting_Limit_Neg_KepLap);
                setting_Limit_Pos_NangLap.Text = Convert.ToString(data.Setting_Limit_Pos_NangLap);
                setting_Limit_Neg_NangLap.Text = Convert.ToString(data.Setting_Limit_Neg_NangLap);
                //
                Setting_Limit_X_Pos.IsEnabled = false;
                Setting_Limit_X_Neg.IsEnabled = false;
                Setting_Limit_Y_Pos.IsEnabled = false;
                Setting_Limit_Y_Neg.IsEnabled = false;
                Setting_Limit_Z_Pos.IsEnabled = false;
                Setting_Limit_Z_Neg.IsEnabled = false;
                //
                Setting_Limit_Pos_KepCheck.IsEnabled = false;
                Setting_Limit_Neg_KepCheck.IsEnabled = false;
                setting_Limit_Pos_NangCheck.IsEnabled = false;
                setting_Limit_Neg_NangCheck.IsEnabled = false;
                //
                Setting_Limit_Pos_KepLap.IsEnabled = false;
                Setting_Limit_Neg_KepLap.IsEnabled = false;
                setting_Limit_Pos_NangLap.IsEnabled = false;
                setting_Limit_Neg_NangLap.IsEnabled = false;
                //
                
            }
            if (!Flag_Save_Velocity)
            {
                txb_Velocity_X_Man.Text = Convert.ToString(data.Setting_Velocity_X_Man);
                txb_Velocity_Y_Man.Text = Convert.ToString(data.Setting_Velocity_Y_Man);
                txb_Velocity_Z_Man.Text = Convert.ToString(data.Setting_Velocity_Z_Man);
                txb_Velocity_X_Auto.Text = Convert.ToString(data.Setting_Velocity_X_Auto);
                txb_Velocity_Y_Auto.Text = Convert.ToString(data.Setting_Velocity_Y_Auto);
                txb_Velocity_Z_Auto.Text = Convert.ToString(data.Setting_Velocity_Z_Auto);
                //
                //txb_Velocity_Cum_Check_Nang.Text = Convert.ToString(data.Setting_Limit_Pos_KepCheck);
                //txb_Velocity_Cum_Check_Kep.Text = Convert.ToString(data.Setting_Limit_Neg_KepCheck);
                //txb_Velocity_Cum_Lap_Nang.Text = Convert.ToString(data.Setting_Limit_Pos_NangCheck);
                //txb_Velocity_Cum_Lap_Kep.Text = Convert.ToString(data.Setting_Limit_Neg_NangCheck);
                //
                txb_Velocity_X_Man.IsEnabled = false;
                txb_Velocity_Y_Man.IsEnabled = false;
                txb_Velocity_Z_Man.IsEnabled = false;
                txb_Velocity_X_Auto.IsEnabled = false;
                txb_Velocity_Y_Auto.IsEnabled = false;
                txb_Velocity_Z_Auto.IsEnabled = false;
                //
                txb_Velocity_Cum_Check_Nang.IsEnabled = false;
                txb_Velocity_Cum_Check_Kep.IsEnabled = false;
                txb_Velocity_Cum_Lap_Nang.IsEnabled = false;
                txb_Velocity_Cum_Lap_Kep.IsEnabled = false;
            }
            if (!Flag_Save_Of)
            {

                txb_Of_TayKep_CumLap.Text = Convert.ToString(data.Of_TayKep_CumLap);
                txb_Of_Nang_CumLap.Text = Convert.ToString(data.Of_Nang_CumLap);
                txb_Of_TayKep_CumDo.Text = Convert.ToString(data.Of_TayKep_CumDo);
                txb_Of_Nang_CumDo.Text = Convert.ToString(data.Of_Nang_CumDo);
                //
                txb_Of_TayKep_CumLap.IsEnabled = false;
                txb_Of_Nang_CumLap.IsEnabled = false;
                txb_Of_TayKep_CumDo.IsEnabled = false;
                txb_Of_Nang_CumDo.IsEnabled = false;
            }
            update.bt_Colors_Status_1(bt_Setting_Off_Buzzer, Convert.ToString(data.Setting_Off_Buzzer));
        }


        private void comPort_OutTray_DropDownOpened(object sender, EventArgs e)
        {
            string[] portNames = SerialPort.GetPortNames();

            cbx_Comport_OutTray.Items.Clear();
            foreach (string portName in portNames)
            {
                cbx_Comport_OutTray.Items.Add(portName);
            }
        }
        private void comPort_InTray_DropDownOpened(object sender, EventArgs e)
        {
            string[] portNames = SerialPort.GetPortNames();

            cbx_Comport_InTray.Items.Clear();
            foreach (string portName in portNames)
            {
                cbx_Comport_InTray.Items.Add(portName);
            }
        }
        private void bt_Save_Click(object sender, RoutedEventArgs e)
        {
            //myPopup.IsOpen = true;
            object data_Setting = new
            {
                Ip_Port_Ser = txb_IPServer.Text + ":" + txb_Port.Text,
                Host_Socket = txb_IP_Server.Text + ":" + txb_Port_Server.Text,
                Comport = cbx_Comport_InTray.Text + ":" + cbx_Comport_OutTray.Text
            };
            string json = System.Text.Json.JsonSerializer.Serialize(data_Setting);
            File.WriteAllText(Path.Setting, json);
            MessageBox.Show("Đã Lưu Thành Công");
        }
        public async void Control(string propertyName, object propertyValue)
        {
            dynamic controlData = new ExpandoObject();
            var controlDataDict = (IDictionary<string, object>)controlData; // Convert to dictionary

            controlDataDict[propertyName] = propertyValue; // Use indexing on the dictionary
            string jsonData = JsonConvert.SerializeObject(controlData);

            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("http://" + Ip_Port_Host + "/api/Control_PLC_1", content);
            var responseContent = await response.Content.ReadAsStringAsync();

        }
        private void bt_Setting_Off_Buzzer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (data != null)
                {
                    if (Convert.ToString(data.Setting_Off_Buzzer) == "false")
                    {
                        Control("Setting_Off_Buzzer", true);
                    }
                    else
                    {
                        Control("Setting_Off_Buzzer", false);
                    }
                }
            }

            catch
            {
                MessageBox.Show("Error");
            }
        }

        private void bt_Edit_Limit_Click(object sender, RoutedEventArgs e)
        {
            Flag_Save_Limit = true;
            Setting_Limit_X_Pos.IsEnabled = true;
            Setting_Limit_X_Neg.IsEnabled = true;
            Setting_Limit_Y_Pos.IsEnabled = true;
            Setting_Limit_Y_Neg.IsEnabled = true;
            Setting_Limit_Z_Pos.IsEnabled = true;
            Setting_Limit_Z_Neg.IsEnabled = true;
            //
            Setting_Limit_Pos_KepCheck.IsEnabled = true;
            Setting_Limit_Neg_KepCheck.IsEnabled = true;
            setting_Limit_Pos_NangCheck.IsEnabled = true;
            setting_Limit_Neg_NangCheck.IsEnabled = true;
            //
            Setting_Limit_Pos_KepLap.IsEnabled = true;
            Setting_Limit_Neg_KepLap.IsEnabled = true;
            setting_Limit_Pos_NangLap.IsEnabled = true;
            setting_Limit_Neg_NangLap.IsEnabled = true;
        }

        private async void bt_Save_Limit_Click(object sender, RoutedEventArgs e)
        {

            var Limit_Pos = new
            {
                Setting_Limit_X_Pos = float.Parse(Setting_Limit_X_Pos.Text),
                Setting_Limit_X_Neg = float.Parse(Setting_Limit_X_Neg.Text),
                Setting_Limit_Y_Pos = float.Parse(Setting_Limit_Y_Pos.Text),
                Setting_Limit_Y_Neg = float.Parse(Setting_Limit_Y_Neg.Text),
                Setting_Limit_Z_Pos = float.Parse(Setting_Limit_Z_Pos.Text),
                Setting_Limit_Z_Neg = float.Parse(Setting_Limit_Z_Neg.Text),
                //
                Setting_Limit_Pos_KepCheck = float.Parse(Setting_Limit_Pos_KepCheck.Text),
                Setting_Limit_Neg_KepCheck = float.Parse(Setting_Limit_Neg_KepCheck.Text),
                Setting_Limit_Pos_NangCheck = float.Parse(setting_Limit_Pos_NangCheck.Text),
                Setting_Limit_Neg_NangCheck = float.Parse(setting_Limit_Neg_NangCheck.Text),
                Setting_Limit_Pos_KepLap = float.Parse(Setting_Limit_Pos_KepLap.Text),
                Setting_Limit_Neg_KepLap = float.Parse(Setting_Limit_Neg_KepLap.Text),
                Setting_Limit_Pos_NangLap = float.Parse(setting_Limit_Pos_NangLap.Text),
                Setting_Limit_Neg_NangLap = float.Parse(setting_Limit_Neg_NangLap.Text)
            };
            string jsonData = JsonConvert.SerializeObject(Limit_Pos);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("http://" + Ip_Port_Host + "/api/Control_PLC_1", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            Flag_Save_Limit = false;
            MessageBox.Show("Đã Lưu Thành Công");

        }

        private void bt_Set_Time_Maintance_Click(object sender, RoutedEventArgs e)
        {
            txb_Time_Maintance.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txb_NextTime_Maintance.Text = DateTime.Now.AddDays(60).ToString("dd/MM/yyyy");
        }

        private void bt_Save_Time_Maintance_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.UserName != "")
            {
                string currentDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                string User = MainWindow.UserName + "_";
                string FileNamebackup = "Path/Log/" + currentDateTime + ".ini";
                File.WriteAllText(FileNamebackup, "Người Bảo Trì:" + MainWindow.UserName + "-" + "Thời Gian Bảo Trì:" + currentDateTime);
                try
                {
                    if (txb_Time_Maintance.Text != "")
                    {
                        object Time_Maintance = new
                        {
                            Time_Maintance = txb_Time_Maintance.Text,
                            Time_Next_Maintance = txb_NextTime_Maintance.Text
                        };
                        string Jsondata = JsonConvert.SerializeObject(Time_Maintance);
                        File.WriteAllText(Path.Maintenancen, Jsondata);
                        MessageBox.Show("Đã Lưu Thành Công");
                    }
                    else
                    {
                        MessageBox.Show("Bạn Chưa Cài Đặt Ngày Bảo Trì");
                    }
                    //
                    System.DateTime dateTime = System.DateTime.Now;

                    string formattedDate = dateTime.ToString("dd/MM/yyyy HH:mm:ss");
                    //
                    string json = File.ReadAllText(Path.History_Maintenancen);
                    json = json.Remove(json.Length - 1);
                    json = json + "," + "{\"Content\": " + "\"" + "Bảo Dưỡng Tổng Thể Máy" + "\"," + "\"Time\": " + "\"" + formattedDate + "\"}" + "]";
                    File.WriteAllText(Path.History_Maintenancen, json);
                }
                catch
                {
                }
            }
            else MessageBox.Show("Vui Lòng Đăng Nhập");
            
        }

        private void bt_Edit_Velocity_Click(object sender, RoutedEventArgs e)
        {
            Flag_Save_Velocity = true;
            //
            txb_Velocity_X_Man.IsEnabled = true;
            txb_Velocity_Y_Man.IsEnabled = true;
            txb_Velocity_Z_Man.IsEnabled = true;
            txb_Velocity_X_Auto.IsEnabled = true;
            txb_Velocity_Y_Auto.IsEnabled = true;
            txb_Velocity_Z_Auto.IsEnabled = true;
            //
            txb_Velocity_Cum_Check_Nang.IsEnabled = true;
            txb_Velocity_Cum_Check_Kep.IsEnabled = true;
            txb_Velocity_Cum_Lap_Nang.IsEnabled = true;
            txb_Velocity_Cum_Lap_Kep.IsEnabled = true;
            //
        }

        private async void bt_Save_Velocity_Click(object sender, RoutedEventArgs e)
        {

            var Velocity = new
            {
                Setting_Velocity_X_Man = float.Parse(txb_Velocity_X_Man.Text),
                Setting_Velocity_Y_Man = float.Parse(txb_Velocity_Y_Man.Text),
                Setting_Velocity_Z_Man = float.Parse(txb_Velocity_Z_Man.Text),
                Setting_Velocity_X_Auto = float.Parse(txb_Velocity_X_Auto.Text),
                Setting_Velocity_Y_Auto = float.Parse(txb_Velocity_Y_Auto.Text),
                Setting_Velocity_Z_Auto = float.Parse(txb_Velocity_Z_Auto.Text),
                //
                Setting_Velocity_Cum_Check_Nang = float.Parse(txb_Velocity_Cum_Check_Nang.Text),
                Setting_Velocity_Cum_Check_Kep = float.Parse(txb_Velocity_Cum_Check_Kep.Text),
                Setting_Velocity_Cum_Lap_Nang = float.Parse(txb_Velocity_Cum_Lap_Nang.Text),
                Setting_Velocity_Cum_Lap_Kep = float.Parse(txb_Velocity_Cum_Lap_Kep.Text),
            };
            string jsonData = JsonConvert.SerializeObject(Velocity);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("http://" + Ip_Port_Host + "/api/Control_PLC_1", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            Flag_Save_Velocity = false;
            MessageBox.Show("Đã Lưu Thành Công");
        }

        private void bt_Edit_Offset(object sender, RoutedEventArgs e)
        {
            Flag_Save_Of = true;
            txb_Of_TayKep_CumLap.IsEnabled = true;
            txb_Of_Nang_CumLap.IsEnabled = true;
            txb_Of_TayKep_CumDo.IsEnabled = true;
            txb_Of_Nang_CumDo.IsEnabled = true;
        }

        private async void bt_Save_Offset(object sender, RoutedEventArgs e)
        {
            var Of = new
            {
                Of_TayKep_CumLap = float.Parse(txb_Of_TayKep_CumLap.Text),
                Of_Nang_CumLap = float.Parse(txb_Of_Nang_CumLap.Text),
                Of_TayKep_CumDo = float.Parse(txb_Of_TayKep_CumDo.Text),
                Of_Nang_CumDo = float.Parse(txb_Of_Nang_CumDo.Text),
                //
            };
            string jsonData = JsonConvert.SerializeObject(Of);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("http://" + Ip_Port_Host + "/api/Control_PLC_2", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            Flag_Save_Of = false;
            MessageBox.Show("Đã Lưu Thành Công");
        }
    }
}
