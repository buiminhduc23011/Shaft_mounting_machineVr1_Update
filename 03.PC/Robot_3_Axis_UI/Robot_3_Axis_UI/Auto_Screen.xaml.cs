using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SocketIOClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;


namespace Robot_3_Axis_UI
{
    public partial class Auto_Screen : UserControl
    {
        Path path = new Path();
        HttpClient client = new HttpClient();
        private DispatcherTimer timer;
        Update_Screen up = new Update_Screen();
        private string Step_Status_Roto = "";
        public dynamic data;
        string Ip_Port_Host;
        //
        private List<string> lampImages = new List<string>
        {
            "/Images/Lamp_Red.png",
            "/Images/Lamp_Yellow.png",
            "/Images/Lamp_Green.png"
        };
        public Auto_Screen()
        {
            InitializeComponent();
        }

        private void Init()
        {
            string json = File.ReadAllText(path.Setting);
            var data_Setting = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            Ip_Port_Host = data_Setting["Ip_Port_Ser"];
        }
        private void Auto_Screen_Loaded(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += Timer_Tick;
            timer.Start();
            Init();


            // Update_Log
            List<Items_Error> items = new List<Items_Error>();
            int index = 1;
            try
            {
                string List_Show = File.ReadAllText(path.History);
                if (List_Show.Length > 0)
                {
                    JArray List_Show_array = JArray.Parse(List_Show);
                    foreach (JObject obj in List_Show_array)

                    {
                        items.Add(new Items_Error { STT = index, Content_ = (string)obj["Content_"], Time = (string)obj["Time"] });
                        index++;
                    }
                    items.Reverse();
                    for (int i = 0; i < items.Count; i++)
                    {
                        items[i].STT = i + 1;
                    }
                    List_Error.ItemsSource = items;
                }
                List<Items_Error> item_Mainten = new List<Items_Error>();
                string List_Mainten_Show = File.ReadAllText(path.History_Maintenancen);
                if (List_Mainten_Show.Length > 0)
                {
                    JArray List_Show_array = JArray.Parse(List_Mainten_Show);
                    foreach (JObject obj in List_Show_array)
                    {
                        item_Mainten.Add(new Items_Error { STT = index, Content_ = (string)obj["Content"], Time = (string)obj["Time"] });
                    }
                    item_Mainten.Reverse();
                    for (int i = 0; i < item_Mainten.Count; i++)
                    {
                        item_Mainten[i].STT = i + 1;
                    }
                    List_Mainten.ItemsSource = item_Mainten;
                }
            }
            catch { }
        }
        private void Auto_Screen_Unloaded(object sender, RoutedEventArgs e)
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
                    Update_Screen();
                });
            }
            catch
            {

            }
        }
        private async void Send_data(string jsonData)
        {
            try
            {
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://" + Ip_Port_Host + "/api/Control_PLC_1", content);
                var responseContent = await response.Content.ReadAsStringAsync();
            }
            catch
            {

            }
        }
        private void Unhide_button(string key, string value, Button button)
        {
            if (key == value)
            {
                button.Visibility = Visibility.Visible;
            }
            else
            {
                button.Visibility = Visibility.Collapsed;
            }
        }

        private void Led_Status(string data)
        {
            int led_index = 0;
            int.TryParse(data, out led_index);
            string imagePath = lampImages[led_index];
            BitmapImage imageSource = new BitmapImage(new Uri(imagePath, UriKind.Relative));
            lampImage.Source = imageSource;
        }
        private void Update_Screen()
        {
            txb_Tray_ID_1.Text = MainWindow.Tray_QR_1;
            txb_Truc_ID_1.Text = MainWindow.Truc_ID_1;
            txb_Roto_ID_1.Text = MainWindow.Roto_ID_1;
            txb_Qty_1.Text = MainWindow.Qty_1;
            txb_Temp_1.Text = MainWindow.Temp_1;
            txb_Height_1.Text = MainWindow.Height_1;
            txbMinMax_1.Text = MainWindow.MinMax_1;
            txb_Diameter_1.Text = MainWindow.Diameter_1;
            txb_Model_ID_1.Text = MainWindow.Model_ID_1;
            //

            txb_Tray_ID_2.Text = MainWindow.Tray_QR_2;
            txb_Truc_ID_2.Text = MainWindow.Truc_ID_2;
            txb_Roto_ID_2.Text = MainWindow.Roto_ID_2;
            txb_Qty_2.Text = MainWindow.Qty_2;
            txb_Temp_2.Text = MainWindow.Temp_2;
            txb_Height_2.Text = MainWindow.Height_2;
            txbMinMax_2.Text = MainWindow.MinMax_2;
            txb_Diameter_2.Text = MainWindow.Diameter_2;
            txb_Model_ID_2.Text = MainWindow.Model_ID_2;
            //
            if (MainWindow.isSerialPort1Initialized) lb_QR_Code_Conv_2.Background = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            else lb_QR_Code_Conv_2.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));

            if (MainWindow.isSerialPort2Initialized) lb_QR_Code_Conv_1.Background = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            else lb_QR_Code_Conv_1.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            if (data != null)
            {
                Unhide_button(Convert.ToString(data.Machine_Alarm_Error), "50", bt_Confirm_LapTruc_OK);
                Unhide_button(Convert.ToString(data.Machine_Alarm_Error), "50", bt_Confirm_LapTruc_NG);
                Unhide_button(Convert.ToString(data.Machine_Noti_NG), "true", bt_Comfirm_Lay_Hang_NG);
                Unhide_button(Convert.ToString(data.ConfirmNG_OK), "true", bt_Confirm_Roto_NG);
                Unhide_button(Convert.ToString(data.ConfirmNG_OK), "true", bt_Confirm_Roto_OK);
                Step_Status_Roto = Convert.ToString(data.Step_Status);
                Led_Status(Convert.ToString(data.Led_Status));
                //
                up.Machine_Status(lb_XYZ_Status, Convert.ToString(data.Robot_3_Truc_Ready));
                up.Machine_Status(lb_BT_Intray_Status, Convert.ToString(data.BT1_Ready));
                up.Machine_Status(lb_BT_Outtray_Status, Convert.ToString(data.BT2_Ready));
                up.Machine_Status(lb_BT_Return_Status, Convert.ToString(data.BT_Return_Ready));
                up.Machine_Status(lb_Cum_Check_Status, Convert.ToString(data.Cum_Check_Ready));
                up.Machine_Status(lb_Cum_Lap_Status, Convert.ToString(data.Cum_Lap_Ready));
                up.Machine_Status(lb_BanXoay_Status, Convert.ToString(data.Ban_Nung_Xoay_Ready));
                //
                up.Roto_Status(lb_Status_11, Convert.ToString(data.Roto_0_Status));
                up.Roto_Status(lb_Status_12, Convert.ToString(data.Roto_1_Status));
                up.Roto_Status(lb_Status_13, Convert.ToString(data.Roto_2_Status));
                up.Roto_Status(lb_Status_14, Convert.ToString(data.Roto_3_Status));
                up.Roto_Status(lb_Status_15, Convert.ToString(data.Roto_4_Status));
                //
                up.Roto_Result(lb_Result_11, Convert.ToString(data.Roto_0_Result));
                up.Roto_Result(lb_Result_12, Convert.ToString(data.Roto_1_Result));
                up.Roto_Result(lb_Result_13, Convert.ToString(data.Roto_2_Result));
                up.Roto_Result(lb_Result_14, Convert.ToString(data.Roto_3_Result));
                up.Roto_Result(lb_Result_15, Convert.ToString(data.Roto_4_Result));
                //
                lb_Height_11.Content = Convert.ToString(data.Roto_0_Height_Truc);
                lb_Height_12.Content = Convert.ToString(data.Roto_1_Height_Truc);
                lb_Height_13.Content = Convert.ToString(data.Roto_2_Height_Truc);
                lb_Height_14.Content = Convert.ToString(data.Roto_3_Height_Truc);
                lb_Height_15.Content = Convert.ToString(data.Roto_4_Height_Truc);
                // Update Log Error
                if (data.Machine_Alarm_Error > 0)
                {
                    LoadItems();
                }
            }
        }
        private void LoadItems()
        {
            List<Items_Error> items = new List<Items_Error>();
            int index = 1;
            try
            {
                string List_Show = File.ReadAllText(path.History);
                if (List_Show.Length > 0)
                {
                    JArray List_Show_array = JArray.Parse(List_Show);
                    items.Clear();
                    foreach (JObject obj in List_Show_array)
                    {
                        items.Add(new Items_Error { STT = index, Content_ = (string)obj["Content_"], Time = (string)obj["Time"] });
                        index++;
                    }
                    items.Reverse();
                    for (int i = 0; i < items.Count; i++)
                    {
                        items[i].STT = i + 1;
                    }
                    List_Error.ItemsSource = items;
                }
            }
            catch { }
        }
        private void txbTrayQR1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string upperCaseText = e.Text.ToUpper();
            TextBox textBox = sender as TextBox;
            int oldCaretIndex = textBox.CaretIndex;
            textBox.Text = textBox.Text.Remove(textBox.SelectionStart, textBox.SelectionLength);
            textBox.Text = textBox.Text.Insert(textBox.CaretIndex, upperCaseText);
            textBox.CaretIndex = oldCaretIndex + 1;
            e.Handled = true;
        }
        private string GetTextBoxValue(TextBox textBox)
        {
            string value = string.Empty;

            Dispatcher.InvokeAsync(() =>
            {
                value = textBox.Text;
            });
            return value;
        }
        private void txbTrayQR2_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string upperCaseText = e.Text.ToUpper();
            TextBox textBox = sender as TextBox;
            int oldCaretIndex = textBox.CaretIndex;
            textBox.Text = textBox.Text.Remove(textBox.SelectionStart, textBox.SelectionLength);
            textBox.Text = textBox.Text.Insert(textBox.CaretIndex, upperCaseText);
            textBox.CaretIndex = oldCaretIndex + 1;
            e.Handled = true;
        }
        private void bt_Confirm_LapTruc_OK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var Control = new
                {
                    Machine_Confirm_Lap_Truc_OK = true,
                    Machine_Reset_All = true,
                };
                string jsonData = JsonConvert.SerializeObject(Control);
                Send_data(jsonData);
            }
            catch { }
        }
        private void bt_Confirm_LapTruc_NG_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var Control = new
                {
                    Machine_Confirm_Lap_Truc_NG = true,
                    Machine_Reset_All = true,
                };
                string jsonData = JsonConvert.SerializeObject(Control);
                Send_data(jsonData);
            }
            catch { }
        }

        private void bt_Confirm_Lay_Hang_NG_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var Control = new
                {
                    Machine_Confirm_NG = true,
                    Machine_Reset_All = true,
                };
                string jsonData = JsonConvert.SerializeObject(Control);
                Send_data(jsonData);
            }
            catch { }
        }

        private void bt_Confirm_Roto_OK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Create and show the input window
                var inputWindow = new ValueInputWindow();
                inputWindow.ShowDialog();

                // Check if the user submitted a value
                if (!string.IsNullOrEmpty(inputWindow.EnteredValue))
                {
                    if (Step_Status_Roto == "0")
                    {
                        var Roto_0 = new
                        {
                            Roto_0_Result = 1,
                            Roto_0_Height_Truc = inputWindow.EnteredValue,
                            bt_OK_NG = true,
                            Machine_Reset_All = true,
                        };
                        string jRoto_0 = JsonConvert.SerializeObject(Roto_0);
                        Send_data(jRoto_0);
                    }
                    if (Step_Status_Roto == "1")
                    {
                        var Roto_1 = new
                        {
                            Roto_1_Result = 1,
                            Roto_1_Height_Truc = inputWindow.EnteredValue,
                            bt_OK_NG = true,
                            Machine_Reset_All = true,
                        };
                        string jRoto_1 = JsonConvert.SerializeObject(Roto_1);
                        Send_data(jRoto_1);
                    }
                    if (Step_Status_Roto == "2")
                    {
                        var Roto_2 = new
                        {
                            Roto_2_Result = 1,
                            Roto_2_Height_Truc = inputWindow.EnteredValue,
                            bt_OK_NG = true,
                            Machine_Reset_All = true,
                        };
                        string jRoto_2 = JsonConvert.SerializeObject(Roto_2);
                        Send_data(jRoto_2);
                    }
                    if (Step_Status_Roto == "3")
                    {
                        var Roto_3 = new
                        {
                            Roto_3_Result = 1,
                            Roto_3_Height_Truc = inputWindow.EnteredValue,
                            bt_OK_NG = true,
                            Machine_Reset_All = true,
                        };
                        string jRoto_3 = JsonConvert.SerializeObject(Roto_3);
                        Send_data(jRoto_3);
                    }
                    if (Step_Status_Roto == "4")
                    {
                        var Roto_4 = new
                        {
                            Roto_4_Result = 1,
                            Roto_4_Height_Truc = inputWindow.EnteredValue,
                            bt_OK_NG = true,
                            Machine_Reset_All = true,
                        };
                        string jRoto_4 = JsonConvert.SerializeObject(Roto_4);
                        Send_data(jRoto_4);
                    }
                }
            }
            catch { }
        }

        private void bt_Confirm_Roto_NG_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                 
                var Control = new
                {
                    bt_OK_NG = true,
                    Machine_Reset_All = true,
                };
                string jsonData = JsonConvert.SerializeObject(Control);
                Send_data(jsonData);
            }
            catch { }
        }
    }
}
