using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.IO;

namespace Robot_3_Axis_UI
{
    /// <summary>
    /// Interaction logic for Mode1_XYZ.xaml
    /// </summary>
    public partial class Mode1_XYZ : UserControl
    {
        private Thread timerThread;
        Path path = new Path();
        string Ip_Port_Host;
        HttpClient client = new HttpClient();
        Update_Screen update = new Update_Screen();
        dynamic data;
        float TrucX_Jog_Pos = 0;
        float TrucY_Jog_Pos = 0;
        float TrucZ_Jog_Pos = 0;
        public Mode1_XYZ()
        {
            InitializeComponent();
        }
        private async void Mode_1_Cum_XYZ_Screen_Loaded(object sender, RoutedEventArgs e)
        {
            string json = File.ReadAllText(path.Setting);
            var data_Setting = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            Ip_Port_Host = data_Setting["Ip_Port_Ser"];
            timerThread = new Thread(TimerThreadFunction);
            timerThread.Start();
            txb_Pos_Jog_X.Text = "0";
            txb_Pos_Jog_Y.Text = "0";
            txb_Pos_Jog_Z.Text = "0";
            try
            {
                var data_test = new
                {
                    Truc_X_Pos_Jog = 0,
                    Truc_Y_Pos_Jog = 0,
                    Truc_Z_Pos_Jog = 0,
                };
                string jsonData = JsonConvert.SerializeObject(data_test);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://" + Ip_Port_Host + "/api/Control_PLC_1", content);
                var responseContent = await response.Content.ReadAsStringAsync();
            }
            catch { }
        }
        private void Mode_1_Cum_XYZ_Screen_Unloaded(object sender, RoutedEventArgs e)
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
                update.bt_Colors_Status_1(bt_Jog_Add_X, Convert.ToString(data.Truc_X_Jog_Add));
                update.bt_Colors_Status_1(bt_Jog_Sub_X, Convert.ToString(data.Truc_X_Jog_Sub));
                update.bt_Colors_Status_1(bt_Jog_Add_Y, Convert.ToString(data.Truc_Y_Jog_Add));
                update.bt_Colors_Status_1(bt_Jog_Sub_Y, Convert.ToString(data.Truc_Y_Jog_Sub));
                update.bt_Colors_Status_1(bt_Jog_Add_Z, Convert.ToString(data.Truc_Z_Jog_Add));
                update.bt_Colors_Status_1(bt_Jog_Sub_Z, Convert.ToString(data.Truc_Z_Jog_Sub));
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
        private async void txb_Pos_Jog_X_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (data != null)
            {
                try
                {
                    if (float.TryParse(txb_Pos_Jog_X.Text, out float result))
                    {
                        TrucX_Jog_Pos = result;
                        var data_test = new
                        {
                            Truc_X_Pos_Jog = TrucX_Jog_Pos,
                        };
                        string jsonData = JsonConvert.SerializeObject(data_test);
                        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                        var response = await client.PostAsync("http://" + Ip_Port_Host + "/api/Control_PLC_1", content);
                        var responseContent = await response.Content.ReadAsStringAsync();
                    }
                }
                catch
                {

                }
            }
        }
        private async void txb_Pos_Jog_Y_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (data != null)
            {
                try
                {
                    if (float.TryParse(txb_Pos_Jog_Y.Text, out float result))
                    {
                        TrucY_Jog_Pos = result;
                        var data_test = new
                        {
                            Truc_Y_Pos_Jog = TrucY_Jog_Pos,
                        };
                        string jsonData = JsonConvert.SerializeObject(data_test);
                        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                        var response = await client.PostAsync("http://" + Ip_Port_Host + "/api/Control_PLC_1", content);
                        var responseContent = await response.Content.ReadAsStringAsync();
                    }

                }
                catch
                {
                }
            }

        }
        private async void txb_Pos_Jog_Z_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (data != null)
            {
                try
                {
                    if (float.TryParse(txb_Pos_Jog_Z.Text, out float result))
                    {
                        TrucZ_Jog_Pos = result;
                        var data_test = new
                        {
                            Truc_Z_Pos_Jog = TrucZ_Jog_Pos,
                        };
                        string jsonData = JsonConvert.SerializeObject(data_test);
                        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                        var response = await client.PostAsync("http://" + Ip_Port_Host + "/api/Control_PLC_1", content);
                        var responseContent = await response.Content.ReadAsStringAsync();
                    }
                }
                catch
                {
                }
            }

        }
        private async void X_Axis_Jog_ADD_TouchDown(object sender, RoutedEventArgs e)
        {
            try
            {
                float value_check;
                value_check = float.Parse(Convert.ToString(data.Truc_X_Real_Pos)) + float.Parse(txb_Pos_Jog_X.Text);
                if (value_check <= float.Parse(Convert.ToString(data.Setting_Limit_X_Pos)))
                {
                    var data_test = new
                    {
                        Truc_X_Jog_Add = true,
                    };
                    string jsonData = JsonConvert.SerializeObject(data_test);
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("http://" + Ip_Port_Host + "/api/Control_PLC_1", content);
                    var responseContent = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    MessageBox.Show("Vị trí chạy vượt giới hạn dương trục X");
                }
            }
            catch
            {
                MessageBox.Show("Vui lòng nhập lại giá trị chạy");
            }

        }
        private async void X_Axis_Jog_SUB_TouchDown(object sender, RoutedEventArgs e)
        {
            try
            {
                float value_check;
                value_check = float.Parse(Convert.ToString(data.Truc_X_Real_Pos)) - float.Parse(txb_Pos_Jog_X.Text);
                if (value_check >= float.Parse(Convert.ToString(data.Setting_Limit_X_Neg)))
                {
                    var data_test = new
                    {
                        Truc_X_Jog_Sub = true,
                    };
                    string jsonData = JsonConvert.SerializeObject(data_test);
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("http://" + Ip_Port_Host + "/api/Control_PLC_1", content);
                    var responseContent = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    MessageBox.Show("Vị trí chạy vượt giới hạn âm trục X");
                }
            }
            catch
            {
                MessageBox.Show("Vui lòng nhập lại giá trị chạy");
            }


        }
        private async void Y_Axis_Jog_ADD_TouchDown(object sender, RoutedEventArgs e)
        {
            try
            {
                float value_check;
                value_check = float.Parse(Convert.ToString(data.Truc_Y_Real_Pos)) + float.Parse(txb_Pos_Jog_Y.Text);
                if (value_check <= float.Parse(Convert.ToString(data.Setting_Limit_Y_Pos)))
                {
                    var data_test = new
                    {

                        Truc_Y_Jog_Add = true,
                    };
                    string jsonData = JsonConvert.SerializeObject(data_test);
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("http://" + Ip_Port_Host + "/api/Control_PLC_1", content);
                    var responseContent = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    MessageBox.Show("Vị trí chạy vượt giới hạn dương trục Y");
                }
            }
            catch
            {
                MessageBox.Show("Vui lòng nhập lại giá trị chạy");
            }


        }
        private async void Y_Axis_Jog_SUB_TouchDown(object sender, RoutedEventArgs e)
        {

            try
            {
                float value_check;
                value_check = float.Parse(Convert.ToString(data.Truc_Y_Real_Pos)) - float.Parse(txb_Pos_Jog_Y.Text);
                if (value_check >= float.Parse(Convert.ToString(data.Setting_Limit_Y_Neg)))
                {
                    var data_test = new
                    {
                        Truc_Y_Jog_Sub = true,
                    };
                    string jsonData = JsonConvert.SerializeObject(data_test);
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("http://" + Ip_Port_Host + "/api/Control_PLC_1", content);
                    var responseContent = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    MessageBox.Show("Vị trí chạy vượt giới hạn âm trục Y");
                }
            }
            catch
            {
                MessageBox.Show("Vui lòng nhập lại giá trị chạy");
            }


        }
        private async void Z_Axis_Jog_ADD_TouchDown(object sender, RoutedEventArgs e)
        {
            try
            {
                float value_check;
                value_check = float.Parse(Convert.ToString(data.Truc_Z_Real_Pos)) + float.Parse(txb_Pos_Jog_Z.Text);
                if (value_check <= float.Parse(Convert.ToString(data.Setting_Limit_Z_Pos)))
                {
                    var data_test = new
                    {
                        Truc_Z_Jog_Add = true,
                    };
                    string jsonData = JsonConvert.SerializeObject(data_test);
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("http://" + Ip_Port_Host + "/api/Control_PLC_1", content);
                    var responseContent = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    MessageBox.Show("Vị trí chạy vượt giới hạn dương trục Z");
                }
            }
            catch
            {
                MessageBox.Show("Vui lòng nhập lại giá trị chạy");
            }

        }
        private async void Z_Axis_Jog_SUB_TouchDown(object sender, RoutedEventArgs e)
        {
            try
            {
                float value_check;
                value_check = float.Parse(Convert.ToString(data.Truc_Z_Real_Pos)) - float.Parse(txb_Pos_Jog_Z.Text);
                if (value_check >= float.Parse(Convert.ToString(data.Setting_Limit_Z_Neg)))
                {
                    var data_test = new
                    {
                        Truc_Z_Jog_Sub = true,
                    };
                    string jsonData = JsonConvert.SerializeObject(data_test);
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("http://" + Ip_Port_Host + "/api/Control_PLC_1", content);
                    var responseContent = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    MessageBox.Show("Vị trí chạy vượt giới hạn âm trục Z");
                }
            }
            catch
            {
                MessageBox.Show("Vui lòng nhập lại giá trị chạy");
            }
        }
    }
}
