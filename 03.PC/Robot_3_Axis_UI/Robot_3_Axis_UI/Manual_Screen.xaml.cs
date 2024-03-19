using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using System.IO;
using System.Dynamic;
using System.Threading;
using System.Windows.Input;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows.Media;

namespace Robot_3_Axis_UI
{
    /// <summary>
    /// Interaction logic for Manual_Screen.xaml
    /// </summary>
    public partial class Manual_Screen : UserControl
    {
        Mode_1_Cum_Lap mode_1_cumlap = new Mode_1_Cum_Lap();
        Mode_2_Cum_Lap mode_2_cumlap = new Mode_2_Cum_Lap();
        Mode_1_Cum_Do mode_1_cumdo = new Mode_1_Cum_Do();
        Mode_2_Cum_Do mode_2_cumdo = new Mode_2_Cum_Do();
        Mode1_XYZ mode1_XYZ = new Mode1_XYZ();
        Mode2_XYZ mode2_XYZ = new Mode2_XYZ();
        private Thread timerThread;
        Path path = new Path();
        string Ip_Port_Host;
        HttpClient client = new HttpClient();
        Update_Screen update = new Update_Screen();
        dynamic data; 
        float Persent_heater_manual = 0;
        public static bool Flag_Check = false;
        public Manual_Screen()
        {
            InitializeComponent();
        }
        private void Manual_Screen_Loaded(object sender, RoutedEventArgs e)
        {
            string json = File.ReadAllText(path.Setting);
            var data_Setting = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            Ip_Port_Host = data_Setting["Ip_Port_Ser"];
            timerThread = new Thread(TimerThreadFunction);
            timerThread.Start();
            bt_Mode_1_Lap.Background = new SolidColorBrush(Color.FromRgb(100, 149, 237));
            bt_Mode_2_Lap.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            //
            Control_Lap.Children.Clear();
            Control_Lap.Children.Add(mode_1_cumlap);
            Control_Do.Children.Clear();
            Control_Do.Children.Add(mode_1_cumdo);
            Control_XYZ.Children.Clear();
            Control_XYZ.Children.Add(mode1_XYZ);
        }
        private void Manual_Screen_Unloaded(object sender, RoutedEventArgs e)
        {
            if (timerThread != null && timerThread.IsAlive)
            {
                timerThread.Abort();
                timerThread.Join();
            }
            Control_XYZ.Children.Clear();
            Control_Do.Children.Clear();
            Control_Lap.Children.Clear();
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

                            if (Convert.ToString(data.Cum_Check_Check_Man_Done) == "true")
                            {
                                if (!Flag_Check)
                                {
                                    string Date = System.DateTime.Now.ToString("dd/MM/yyyy");
                                    string Time = System.DateTime.Now.ToString("HH:mm:ss");
                                    string json_ = File.ReadAllText(path.History_Check);
                                    string Height = Convert.ToString(data.Cum_Check_Pos_Check_Daily);
                                    double Min = float.Parse(txb_Standar_Check.Text) - 0.2;
                                    double Max = float.Parse(txb_Standar_Check.Text) + 0.2;
                                    if (Max >= float.Parse(Height) && Min <= float.Parse(Height))
                                    {
                                        try
                                        {
                                            string json = File.ReadAllText(path.History_Check);
                                            json = json.Remove(json.Length - 1);
                                            json = json + "," + "{\"Height\": " + "\"" + Height + "\"," + "\"Date\": " + "\"" + Date + "\"," + "\"Time\": " + "\"" + Time + "\"}" + "]";
                                            File.WriteAllText(path.History_Check, json);
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show(ex.Message);
                                        }
                                    }
                                    else
                                    {
                                        var Control = new
                                        {
                                            Machine_Error_Check = true,
                                        };
                                        string jsonData = JsonConvert.SerializeObject(Control);
                                        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                                        Task.Run(() => client.PostAsync("http://" + Ip_Port_Host + "/api/Control_PLC_2", content));
                                    }
                                    try
                                    {
                                        System.DateTime dateTime = System.DateTime.Now;
                                        string formattedDate = dateTime.ToString("dd/MM/yyyy HH:mm:ss");
                                        //
                                        string json = File.ReadAllText(path.History_Maintenancen);
                                        json = json.Remove(json.Length - 1);
                                        if (Max >= float.Parse(Height) && Min <= float.Parse(Height))
                                        {
                                            json = json + "," + "{\"Content\": " + "\"" + "Kiểm Tra Cụm Đo OK - " + "Độ Cao:" + Height + "mm" + "\"," + "\"Time\": " + "\"" + formattedDate + "\"}" + "]";
                                            File.WriteAllText(path.History_Maintenancen, json);
                                            Flag_Check = true;
                                        }
                                        else
                                        {
                                            json = json + "," + "{\"Content\": " + "\"" + "Kiểm Tra Cụm Đo NG - " + "Độ Cao:" + Height + "mm" + "\"," + "\"Time\": " + "\"" + formattedDate + "\"}" + "]";
                                            File.WriteAllText(path.History_Maintenancen, json);
                                            Flag_Check = true;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                    }
                                }
                            }
                            else { Flag_Check = false; }
                        }
                    });
                }
                catch
                {

                }

                Thread.Sleep(100); // Adjust the sleep interval as needed
            }
        }

        public void Update_Screen()
        {
            if (data != null)
            {

                update.bt_Colors_Status_1(bt_Taykep_Kep, Convert.ToString(data.Tay_Kep_Kep));
                update.bt_Colors_Status_0(bt_Taykep_Nha, Convert.ToString(data.Tay_Kep_Kep));
                //
                update.bt_Colors_Status_1(bt_Nang_Cum_Lap, Convert.ToString(data.Cum_Lap_XL_Nang));
                update.bt_Colors_Status_1(bt_Tay_Kep_Lap, Convert.ToString(data.Cum_Lap_XL_Kep));
                update.bt_Colors_Status_1(bt_Blow_Air, Convert.ToString(data.Cum_Lap_Blow_Air));
                //
                update.bt_Colors_Status_1(bt_Jog_BX, Convert.ToString(data.Jog_BX));
                update.bt_Colors_Status_1(bt_Up_Cum_Nung, Convert.ToString(data.XL_Nang));
                update.bt_Colors_Status_0(bt_Down_Cum_Nung, Convert.ToString(data.XL_Nang));
                update.bt_Colors_Status_1(bt_ON_Nung, Convert.ToString(data.ON_Nung));
                update.bt_Colors_Status_0(bt_Off_Nung, Convert.ToString(data.ON_Nung));
                //
                
                update.bt_Colors_Status_1(bt_Check_Manual, Convert.ToString(data.Cum_Check_Check_Man));
                update.bt_Colors_Status_1(bt_Tay_Kep_Check, Convert.ToString(data.Cum_Check_XL_Kep));

                //
                update.bt_Colors_Status_1(bt_RunBT1, Convert.ToString(data.BT1_Man_FWD));
                update.bt_Colors_Status_1(bt_Stopper_BT1, Convert.ToString(data.BT1_Man_Ha));
                update.bt_Colors_Status_1(bt_XL_Kep_Tray_BT1, Convert.ToString(data.BT1_Man_Kep));
                //
                update.bt_Colors_Status_1(bt_Run_BT2, Convert.ToString(data.BT2_Man_FWD));
                update.bt_Colors_Status_1(bt_Stopper_BT2, Convert.ToString(data.BT2_Man_Ha));
                update.bt_Colors_Status_1(bt_XL_KepTray_BT2, Convert.ToString(data.BT2_Man_Kep));
                //
                update.bt_Colors_Status_1(bt_Run_BT_Return, Convert.ToString(data.BT_Return_Man_FWD));
                //update.bt_Colors_Status_1(bt_Stopper1_BT_Return, Convert.ToString(data.BT_Return_Man_Bot_Ha));
                update.bt_Colors_Status_1(bt_Stopper2_BT_Return, Convert.ToString(data.BT_Return_Man_Mid_Ha));
                update.bt_Colors_Status_1(bt_XL_KepTray_BT_Return, Convert.ToString(data.BT_Return_Man_Kep));
                // 


                update.bt_Colors_Status_1(bt_Check_Manual, Convert.ToString(data.Cum_Check_Check_Man));
                txb_Standar_Check.Text = Convert.ToString(data.Standar_Check);
                txb_Pos_Check.Text = Convert.ToString(data.Cum_Check_Pos_Check_Daily);
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
        
        private async void Three_Axis_Kep_Click(object sender, RoutedEventArgs e)
        {
            var data_test = new
            {
                Tay_Kep_Kep = true,
                Tay_Kep_Nha = false,
            };
            string jsonData = JsonConvert.SerializeObject(data_test);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("http://" + Ip_Port_Host + "/api/Control_PLC_1", content);
            var responseContent = await response.Content.ReadAsStringAsync();
        }
        private async void Three_Axis_Nha_Click(object sender, RoutedEventArgs e)
        {
            var data_test = new
            {
                Tay_Kep_Kep = false,
                Tay_Kep_Nha = true,
            };
            string jsonData = JsonConvert.SerializeObject(data_test);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("http://" + Ip_Port_Host + "/api/Control_PLC_1", content);
            var responseContent = await response.Content.ReadAsStringAsync();
        }
        


        private void bt_FWD_Rotate_Table_Click(object sender, RoutedEventArgs e)
        {
            Control("Jog_BX", true, 2);
        }
        private void bt_Cyl_Cum_Nung_Up(object sender, RoutedEventArgs e)
        {
            Control("XL_Nang", true, 2);
        }

        private void bt_Cyl_Cum_Nung_Down(object sender, RoutedEventArgs e)
        {
            Control("XL_Nang", false, 2);
        }
        private void bt_On_Cum_Nung(object sender, RoutedEventArgs e)
        {
            Control("ON_Nung", true, 2);
        }

        private void bt_Off_Cum_Nung(object sender, RoutedEventArgs e)
        {
            Control("ON_Nung", false, 2);
        }
        private void bt_Run_BT1_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToString(data.BT1_Man_FWD) == "false")
            {
                Control("BT1_Man_FWD", true, 1);
            }
            else
            {
                Control("BT1_Man_FWD", false, 1);
            }
        }

        private void bt_Stopper_BT1_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToString(data.BT1_Man_Ha) == "false")
            {
                Control("BT1_Man_Ha", true, 1);
            }
            else
            {
                Control("BT1_Man_Ha", false, 1);
            }
        }

        private void bt_XL_Kep_BT1_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToString(data.BT1_Man_Kep) == "false")
            {
                Control("BT1_Man_Kep", true, 1);
            }
            else
            {
                Control("BT1_Man_Kep", false, 1);
            }
        }

        private void bt_Run_BT2_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToString(data.BT2_Man_FWD) == "false")
            {
                Control("BT2_Man_FWD", true, 1);
            }
            else
            {
                Control("BT2_Man_FWD", false, 1);
            }
        }

        private void bt_Stopper_BT2_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToString(data.BT2_Man_Ha) == "false")
            {
                Control("BT2_Man_Ha", true, 1);
            }
            else
            {
                Control("BT2_Man_Ha", false, 1);
            }
        }

        private void bt_XL_Kep_Tray_BT2_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToString(data.BT2_Man_Kep) == "false")
            {
                Control("BT2_Man_Kep", true, 1);
            }
            else
            {
                Control("BT2_Man_Kep", false, 1);
            }
        }

        private void bt_Run_BT_Return_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToString(data.BT_Return_Man_FWD) == "false")
            {
                Control("BT_Return_Man_FWD", true, 1);
            }
            else
            {
                Control("BT_Return_Man_FWD", false, 1);
            }

        }

        private void bt_XL_Kep_Tray_BT_Return_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToString(data.BT_Return_Man_Kep) == "false")
            {

                Control("BT_Return_Man_Kep", true, 1);
            }
            else
            {
                Control("BT_Return_Man_Kep", false, 1);
            }
        }

        private void bt_Stopper1_BT_Return_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToString(data.BT_Return_Man_Bot_Ha) == "false")
            {

                Control("BT_Return_Man_Bot_Ha", true, 1);
            }
            else
            {
                Control("BT_Return_Man_Bot_Ha", false, 1);
            }
        }

        private void bt_Stopper2_BT_Return_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToString(data.BT_Return_Man_Mid_Ha) == "false")
            {

                Control("BT_Return_Man_Mid_Ha", true, 1);
            }
            else
            {
                Control("BT_Return_Man_Mid_Ha", false, 1);
            }
        }

        private void bt_Blow_Air_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToString(data.Cum_Lap_Blow_Air) == "false")
            {
                Control("Cum_Lap_Blow_Air", true, 2);
            }
            else
            {
                Control("Cum_Lap_Blow_Air", false, 2);
            }
        }

        private void bt_Check_Manual_Click(object sender, RoutedEventArgs e)
        {

            Control("Cum_Check_Check_Man", true, 2);
            Flag_Check = false;
        }



        private void bt_Kep_Cum_Check_Click(object sender, RoutedEventArgs e)
        {

            if (Convert.ToString(data.Cum_Check_XL_Kep) == "false")
            {
                Control("Cum_Check_XL_Kep", true, 2);
            }
            else
            {
                Control("Cum_Check_XL_Kep", false, 2);
            }
        }
        private void bt_Nang_Cum_Lap_Click(object sender, RoutedEventArgs e)
        {

            if (Convert.ToString(data.Cum_Lap_XL_Nang) == "false")
            {
                Control("Cum_Lap_XL_Nang", true, 2);
            }
            else
            {
                Control("Cum_Lap_XL_Nang", false, 2);
            }
        }

        private void bt_Kep_Cum_Lap_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToString(data.Cum_Lap_XL_Kep) == "false")
            {
                Control("Cum_Lap_XL_Kep", true, 2);
            }
            else
            {
                Control("Cum_Lap_XL_Kep", false, 2);
            }
        }
        




        private async void txb_Persen_Heater_TextChanged(object sender, TextChangedEventArgs e)
        {
            string newText = txb_Persen_Heater.Text;
            if (float.TryParse(newText, out float newValue))
            {
                newValue = Math.Max(0, Math.Min(100, newValue));
                txb_Persen_Heater.Text = newValue.ToString();

            }
            try
            {
                if (float.TryParse(txb_Persen_Heater.Text, out float result))
                {
                    Persent_heater_manual = result;
                    var data = new
                    {
                        Machine_Persent_heater_manual = Persent_heater_manual,
                    };
                    string jsonData = JsonConvert.SerializeObject(data);
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("http://" + Ip_Port_Host + "/api/Control_PLC_1", content);
                    var responseContent = await response.Content.ReadAsStringAsync();
                }
            }
            catch
            {
            }
        }
        

        private void bt_Mode_1_Lap_Click(object sender, RoutedEventArgs e)
        {
            bt_Mode_1_Lap.Background = new SolidColorBrush(Color.FromRgb(100, 149, 237));
            
            bt_Mode_2_Lap.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            Control_Lap.Children.Clear();
            Control_Lap.Children.Add(mode_1_cumlap);
            Control_Do.Children.Clear();
            Control_Do.Children.Add(mode_1_cumdo);
            Control_XYZ.Children.Clear();
            Control_XYZ.Children.Add(mode1_XYZ);
        }
        private void bt_Mode_2_Lap_Click(object sender, RoutedEventArgs e)
        {
            bt_Mode_1_Lap.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_Mode_2_Lap.Background = new SolidColorBrush(Color.FromRgb(100, 149, 237));
            Control_Lap.Children.Clear();
            Control_Lap.Children.Add(mode_2_cumlap);
            Control_Do.Children.Clear();
            Control_Do.Children.Add(mode_2_cumdo);
            Control_XYZ.Children.Clear();
            Control_XYZ.Children.Add(mode2_XYZ);
        }
    }
}
