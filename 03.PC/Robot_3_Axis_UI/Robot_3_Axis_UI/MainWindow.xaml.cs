using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Windows;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Threading;
using System.Windows.Media;
using Robot_3_Axis_UI.Class;
using Newtonsoft.Json.Linq;
using System.Windows.Controls;
using System.Diagnostics;
using SocketIOClient;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Windows.Input;
using System.Data.SqlTypes;

namespace Robot_3_Axis_UI
{

    public partial class MainWindow : Window
    {
        // Config
        HttpClient client = new HttpClient();
        Auto_Screen auto_Screen = new Auto_Screen();
        Manual_Screen manual_Screen = new Manual_Screen();
        History_Log_Screen History_Log_Screen = new History_Log_Screen();
        Model_Screen Model_Screen = new Model_Screen();
        Setting_Screen Setting_Screen = new Setting_Screen();
        GPIO_Screen Gpio_Screen = new GPIO_Screen();
        Update_Screen update = new Update_Screen();
        BackgroundWorker worker = new BackgroundWorker();
        private bool shouldStop = false; // Flag for cancellation
        Path path = new Path();
        string Ip_Port_Host;
        dynamic data;
        int flag_error = 0;
        Read_PLC PLC = new Read_PLC();
        private bool Sendmac = false;
        public static string Mode = "false";
        private DispatcherTimer Timer_Check_Maintance;
        private DispatcherTimer Timer_UpdateUI;
        //
        private TimeSpan alertTime = new TimeSpan(8, 0, 0);
        private const string JsonFileName = "alert_state.json";
        //
        private bool isMachineDoneTrayProcessed = false;
        PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

        AlertState alertState = new AlertState();
        //
        private CancellationTokenSource _cancellationTokenSource;
        public static SerialPort _serialPort1;
        public static SerialPort _serialPort2;
        public static bool isSerialPort1Initialized = false;
        public static bool isSerialPort2Initialized = false;

        public static string Tray_QR_1 = "";
        public static string Truc_ID_1 = "";
        public static string Roto_ID_1 = "";
        public static string Qty_1 = "";
        public static string Temp_1 = "";
        public static string Height_1 = "";
        public static string MinMax_1 = "";
        public static string Diameter_1 = "";
        public static string Model_ID_1 = "";
        //
        public static string Tray_QR_2 = "";
        public static string Truc_ID_2 = "";
        public static string Roto_ID_2 = "";
        public static string Qty_2 = "";
        public static string Temp_2 = "";
        public static string Height_2 = "";
        public static string MinMax_2 = "";
        public static string Diameter_2 = "";
        public static string Model_ID_2 = "";

        //
        public static string Tray_QR_1_Re = "";
        public static string Tray_QR_2_Re = "";
        string host_socket;
        string[] COM;

        public bool Connected = false;
        SocketIO socketclient;
        private bool areTasksInitialized = false;
        private List<Task> tasks = new List<Task>();
        private string Machine_Request_Tray_In_Re = "";
        private string Machine_Request_Tray_Out_Re = "";
        //
        public static string UserName = "";
        //
        public static string GetMacAddress()
        {
            string macAddress = "";
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up && !nic.Description.ToLower().Contains("virtual") && !nic.Description.ToLower().Contains("pseudo"))
                {
                    if (nic.NetworkInterfaceType == NetworkInterfaceType.Wireless80211) // Check if it's a Wi-Fi interface
                    {
                        byte[] macBytes = nic.GetPhysicalAddress().GetAddressBytes();
                        macAddress = string.Join(":", macBytes.Select(b => b.ToString("X2")));
                        break;
                    }

                }
            }
            return macAddress;
        }

        bool saveResultCalled = false;
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
        //
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Window_Loaded);
            AddHandler(TouchDownEvent, new RoutedEventHandler(TouchDownHandler));
        }
        private void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }
        private void StopBackgroundWork()
        {
            shouldStop = true; // Set the cancellation flag
        }
        public void CancelAllTasks()
        {
            _cancellationTokenSource.Cancel();
            foreach (var task in tasks)
            {
                task.Wait();
            }
        }
        public void CancelCOM()
        {
            if (_serialPort1 != null && _serialPort1.IsOpen)
            {
                _serialPort1.Close();
            }

            if (_serialPort2 != null && _serialPort2.IsOpen)
            {
                _serialPort2.Close();
            }
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            PLC.Stop();
            CancelAllTasks();
            CancelCOM();
            StopBackgroundWork();
            Pannel_Monitor.Children.Clear();
            Stop_Timer_Check_Maintance();


        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string json = File.ReadAllText(path.Setting);
            var data_Setting = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            Ip_Port_Host = data_Setting["Ip_Port_Ser"];
            host_socket = "http://" + data_Setting["Host_Socket"];
            //
            PLC.Start();
            LanguageComboBox.SelectedIndex = 1;
            worker.DoWork += new DoWorkEventHandler(DoWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerCompleted);
            worker.RunWorkerAsync();
            //
            bt_Auto.Background = new SolidColorBrush(Color.FromRgb(100, 149, 237));
            bt_Origin.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_Manu.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_Reset.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_History.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_GPIO.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_Model.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_Setting.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            Pannel_Monitor.Children.Add(auto_Screen);
            //
            Timer_UpdateUI = new DispatcherTimer();
            Timer_UpdateUI.Interval = TimeSpan.FromMilliseconds(1000);
            Timer_UpdateUI.Tick += Timer_UpdateUI_Tick;
            Timer_UpdateUI.Start();
            //
            Timer_Check_Maintance = new DispatcherTimer();
            Timer_Check_Maintance.Interval = TimeSpan.FromHours(1);
            Timer_Check_Maintance.Tick += Timer_Check_Maintance_Tick;
            Timer_Check_Maintance.Start();
            //
            COM = data_Setting["Comport"].Split(':');
            if (!isSerialPort1Initialized)
            {
                try
                {
                    _serialPort1 = new SerialPort(COM[1], 9600, Parity.None, 8, StopBits.One);
                    _serialPort1.Open();
                    isSerialPort1Initialized = true;
                }
                catch
                {
                    isSerialPort1Initialized = false;
                }
            }

            if (!isSerialPort2Initialized)
            {
                try
                {
                    _serialPort2 = new SerialPort(COM[0], 9600, Parity.None, 8, StopBits.One);
                    _serialPort2.Open();
                    isSerialPort2Initialized = true;
                }
                catch
                {
                    isSerialPort2Initialized = false;
                }
            }
            if (!areTasksInitialized)
            {
                _cancellationTokenSource = new CancellationTokenSource();
                tasks.Add(Task.Run(() => MyBackgroundTask(_cancellationTokenSource.Token)));
                tasks.Add(Task.Run(() => ProcessComPort1(_cancellationTokenSource.Token)));
                tasks.Add(Task.Run(() => ProcessComPort2(_cancellationTokenSource.Token)));
                areTasksInitialized = true;
            }
        }

        private void Clear_Data_Tray()
        {
            if (data != null)
            {
                if (Convert.ToString(data.I_SS_Detect_Tray_In2) == "false" || Mode == "false")
                {
                    Tray_QR_1 = "";
                    Truc_ID_1 = "";
                    Roto_ID_1 = "";
                    Qty_1 = "";
                    Temp_1 = "";
                    Height_1 = "";
                    MinMax_1 = "";
                    Diameter_1 = "";
                    Model_ID_1 = "";
                    Tray_QR_1_Re = "";
                }
                if (Convert.ToString(data.I_SS_Detect_Tray_In1) == "false" || Mode == "false")
                {
                    Tray_QR_2 = "";
                    Truc_ID_2 = "";
                    Roto_ID_2 = "";
                    Qty_2 = "";
                    Temp_2 = "";
                    Height_2 = "";
                    MinMax_2 = "";
                    Diameter_2 = "";
                    Model_ID_2 = "";
                    Tray_QR_2_Re = "";
                }
            }
            else
            {
                Tray_QR_1_Re = "";
                Tray_QR_2_Re = "";
                Tray_QR_1 = "";
                Truc_ID_1 = "";
                Roto_ID_1 = "";
                Qty_1 = "";
                Temp_1 = "";
                Height_1 = "";
                MinMax_1 = "";
                Diameter_1 = "";
                Model_ID_1 = "";
                Tray_QR_2 = "";
                Truc_ID_2 = "";
                Roto_ID_2 = "";
                Qty_2 = "";
                Temp_2 = "";
                Height_2 = "";
                MinMax_2 = "";
                Diameter_2 = "";
                Model_ID_2 = "";
            }
        }
        private int[] ConvertBytesToIntArray(byte[] byteArray)
        {
            int[] intArray = new int[byteArray.Length / 4];

            for (int i = 0; i < byteArray.Length; i += 4)
            {
                int value = BitConverter.ToInt32(byteArray, i);
                intArray[i / 4] = value;
            }

            return intArray;
        }
        private async void Data_Tray_1(string Truc, string Roto, int qty_)
        {
            try
            {
                string json = File.ReadAllText(path.Model);
                if (json.Length > 0)
                {
                    JArray jsonArray = JArray.Parse(json);
                    bool Is_check_data = false;
                    foreach (JObject obj in jsonArray)
                    {
                        if ((string)obj["Truc"] == Truc && (string)obj["Roto"] == Roto)
                        {
                            string jsonStr = obj.ToString();
                            Temp_1 = (string)obj["Temperature"];
                            JArray pos_Nang_Kep_Cum_Ktr = (JArray)obj["Pos_Nang_Kep_Cum_Ktr"];
                            Height_1 = pos_Nang_Kep_Cum_Ktr[0].ToString();
                            double Min = float.Parse(pos_Nang_Kep_Cum_Ktr[0].ToString()) - 0.5;
                            double Max = float.Parse(pos_Nang_Kep_Cum_Ktr[0].ToString()) + 0.5;
                            MinMax_1 = Min.ToString() + "-" + Max.ToString();
                            Diameter_1 = pos_Nang_Kep_Cum_Ktr[1].ToString();
                            Model_ID_1 = (string)obj["Model"];

                            //
                            JArray Pos_Roto_0 = (JArray)obj["Pos_Roto1"];
                            JArray Pos_Roto_1 = (JArray)obj["Pos_Roto2"];
                            JArray Pos_Roto_2 = (JArray)obj["Pos_Roto3"];
                            JArray Pos_Roto_3 = (JArray)obj["Pos_Roto4"];
                            JArray Pos_Roto_4 = (JArray)obj["Pos_Roto5"];

                            JArray Pos_Truc_0 = (JArray)obj["Pos_Truc1"];
                            JArray Pos_Truc_1 = (JArray)obj["Pos_Truc2"];
                            JArray Pos_Truc_2 = (JArray)obj["Pos_Truc3"];
                            JArray Pos_Truc_3 = (JArray)obj["Pos_Truc4"];
                            JArray Pos_Truc_4 = (JArray)obj["Pos_Truc5"];

                            JArray Pos_Tha_CumKtr = (JArray)obj["Pos_Tha_Cum_Ktr"];
                            JArray Pos_Nang_Kep_CumKtr = (JArray)obj["Pos_Nang_Kep_Cum_Ktr"];


                            JArray Pos_Gap_CumLap = (JArray)obj["Pos_Gap_Cum_Lap"];
                            JArray Pos_Lap_CumLap = (JArray)obj["Pos_Lap_Cum_Lap"];
                            JArray Pos_Nhan_Truc = (JArray)obj["Pos_Nhan_Truc"];
                            JArray Pos_Nang_Kep_CumLap = (JArray)obj["Pos_Nang_Kep_Cum_Lap"];

                            JArray Pos_Tha_BanXoay = (JArray)obj["Pos_Tha_Ban_Xoay"];
                            var DB_Run = new
                            {
                                Temperature_TrayID1 = int.Parse((string)obj["Temperature"]),
                                Pos_Z_Offset_Tha_Roto = float.Parse((string)obj["Offset_Tha_Roto"]),
                                Persent_Power_Heater_TrayID1 = float.Parse((string)obj["Cong_Suat_Nung"]),
                                Time_Blow_TrayID1 = int.Parse((string)obj["Time_Blow"]),
                                Type_Roto = int.Parse((string)obj["LoaiTruc"]),
                                Z_Safety_ID1 = float.Parse((string)obj["Z_Safety"]),
                                T_LamMat_ID1 = float.Parse((string)obj["Temperature_LamMat"]),
                                Pos_Roto_0_X = float.Parse(Pos_Roto_0[0].ToString()),
                                Pos_Roto_0_Y = float.Parse(Pos_Roto_0[1].ToString()),
                                Pos_Roto_0_Z = float.Parse(Pos_Roto_0[2].ToString()),
                                Pos_Roto_1_X = float.Parse(Pos_Roto_1[0].ToString()),
                                Pos_Roto_1_Y = float.Parse(Pos_Roto_1[1].ToString()),
                                Pos_Roto_1_Z = float.Parse(Pos_Roto_1[2].ToString()),
                                Pos_Roto_2_X = float.Parse(Pos_Roto_2[0].ToString()),
                                Pos_Roto_2_Y = float.Parse(Pos_Roto_2[1].ToString()),
                                Pos_Roto_2_Z = float.Parse(Pos_Roto_2[2].ToString()),
                                Pos_Roto_3_X = float.Parse(Pos_Roto_3[0].ToString()),
                                Pos_Roto_3_Y = float.Parse(Pos_Roto_3[1].ToString()),
                                Pos_Roto_3_Z = float.Parse(Pos_Roto_3[2].ToString()),
                                Pos_Roto_4_X = float.Parse(Pos_Roto_4[0].ToString()),
                                Pos_Roto_4_Y = float.Parse(Pos_Roto_4[1].ToString()),
                                Pos_Roto_4_Z = float.Parse(Pos_Roto_4[2].ToString()),
                                Pos_Truc_0_X = float.Parse(Pos_Truc_0[0].ToString()),
                                Pos_Truc_0_Y = float.Parse(Pos_Truc_0[1].ToString()),
                                Pos_Truc_0_Z = float.Parse(Pos_Truc_0[2].ToString()),
                                Pos_Truc_1_X = float.Parse(Pos_Truc_1[0].ToString()),
                                Pos_Truc_1_Y = float.Parse(Pos_Truc_1[1].ToString()),
                                Pos_Truc_1_Z = float.Parse(Pos_Truc_1[2].ToString()),
                                Pos_Truc_2_X = float.Parse(Pos_Truc_2[0].ToString()),
                                Pos_Truc_2_Y = float.Parse(Pos_Truc_2[1].ToString()),
                                Pos_Truc_2_Z = float.Parse(Pos_Truc_2[2].ToString()),
                                Pos_Truc_3_X = float.Parse(Pos_Truc_3[0].ToString()),
                                Pos_Truc_3_Y = float.Parse(Pos_Truc_3[1].ToString()),
                                Pos_Truc_3_Z = float.Parse(Pos_Truc_3[2].ToString()),
                                Pos_Truc_4_X = float.Parse(Pos_Truc_4[0].ToString()),
                                Pos_Truc_4_Y = float.Parse(Pos_Truc_4[1].ToString()),
                                Pos_Truc_4_Z = float.Parse(Pos_Truc_4[2].ToString()),
                                Pos_Cum_Kiem_Tra_Tha_X = float.Parse(Pos_Tha_CumKtr[0].ToString()),
                                Pos_Cum_Kiem_Tra_Tha_Y = float.Parse(Pos_Tha_CumKtr[1].ToString()),
                                Pos_Cum_Kiem_Tra_Tha_Z = float.Parse(Pos_Tha_CumKtr[2].ToString()),
                                Pos_Ktr_Up = float.Parse(Pos_Nang_Kep_CumKtr[0].ToString()),
                                Pos_Ktr_Kep = float.Parse(Pos_Nang_Kep_CumKtr[1].ToString()),
                                //
                                Pos_Cum_Lap_Gap_X_TrayID1 = float.Parse(Pos_Gap_CumLap[0].ToString()),
                                Pos_Cum_Lap_Gap_Y_TrayID1 = float.Parse(Pos_Gap_CumLap[1].ToString()),
                                Pos_Cum_Lap_Gap_Z_TrayID1 = float.Parse(Pos_Gap_CumLap[2].ToString()),
                                // Còn Data Nhấn
                                Pos_Cum_Lap_Nhan_X_TrayID1 = float.Parse(Pos_Nhan_Truc[0].ToString()),
                                Pos_Cum_Lap_Nhan_Y_TrayID1 = float.Parse(Pos_Nhan_Truc[1].ToString()),
                                Pos_Cum_Lap_Nhan_Z_TrayID1 = float.Parse(Pos_Nhan_Truc[2].ToString()),

                                //
                                Pos_Cum_Lap_Lap_X_TrayID1 = float.Parse(Pos_Lap_CumLap[0].ToString()),
                                Pos_Cum_Lap_Lap_Y_TrayID1 = float.Parse(Pos_Lap_CumLap[1].ToString()),
                                Pos_Cum_Lap_Lap_Z_TrayID1 = float.Parse(Pos_Lap_CumLap[2].ToString()),
                                //
                                Pos_Nang_Kep_Up_TrayID1 = float.Parse(Pos_Nang_Kep_CumLap[0].ToString()),
                                Pos_Nang_Kep_Kep_TrayID1 = float.Parse(Pos_Nang_Kep_CumLap[1].ToString()),
                                //
                                Pos_Ban_XoayX_TrayID1 = float.Parse(Pos_Tha_BanXoay[0].ToString()),
                                Pos_Ban_XoayY_TrayID1 = float.Parse(Pos_Tha_BanXoay[1].ToString()),
                                Pos_Ban_XoayZ_TrayID1 = float.Parse(Pos_Tha_BanXoay[2].ToString()),
                                //
                                SL_In_Tray_1 = qty_,
                                Check_QR1_Done = true
                            };

                            string jsonData = JsonConvert.SerializeObject(DB_Run);
                            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                            var response = await client.PostAsync("http://" + Ip_Port_Host + "/api/Control_PLC_1", content);
                            var responseContent = await response.Content.ReadAsStringAsync();

                            Is_check_data = true;
                        }
                    }
                    if (!Is_check_data) MessageBox.Show("Không tìm thấy data Trục ID: " + Truc + ", Roto ID: " + Roto + " trong cơ sở dữ liệu cài đặt!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private async void Data_Tray_2(string Truc, string Roto, int qty_)
        {
            try
            {
                string json = File.ReadAllText(path.Model);
                if (json.Length > 0)
                {
                    JArray jsonArray = JArray.Parse(json);
                    foreach (JObject obj in jsonArray)
                    {
                        if ((string)obj["Truc"] == Truc && (string)obj["Roto"] == Roto)
                        {
                            string jsonStr = obj.ToString();
                            Temp_2 = (string)obj["Temperature"];
                            JArray pos_Nang_Kep_Cum_Ktr = (JArray)obj["Pos_Nang_Kep_Cum_Ktr"];
                            Height_2 = pos_Nang_Kep_Cum_Ktr[0].ToString();
                            Diameter_2 = pos_Nang_Kep_Cum_Ktr[1].ToString();
                            double Min = float.Parse(pos_Nang_Kep_Cum_Ktr[0].ToString()) - 0.5;
                            double Max = float.Parse(pos_Nang_Kep_Cum_Ktr[0].ToString()) + 0.5;
                            MinMax_2 = Min.ToString() + "-" + Max.ToString();
                            Model_ID_2 = (string)obj["Model"];
                            //
                            JArray Pos_Roto_5 = (JArray)obj["Pos_Roto6"];
                            JArray Pos_Roto_6 = (JArray)obj["Pos_Roto7"];
                            JArray Pos_Truc_5 = (JArray)obj["Pos_Truc6"];
                            //
                            JArray Pos_Gap_CumLap = (JArray)obj["Pos_Gap_Cum_Lap"];
                            JArray Pos_Lap_CumLap = (JArray)obj["Pos_Lap_Cum_Lap"];
                            JArray Pos_Nhan_Truc = (JArray)obj["Pos_Nhan_Truc"];
                            JArray Pos_Nang_Kep_CumLap = (JArray)obj["Pos_Nang_Kep_Cum_Lap"];

                            JArray Pos_Tha_BanXoay = (JArray)obj["Pos_Tha_Ban_Xoay"];
                            var DB_Run = new
                            {
                                Temperature_TrayID2 = int.Parse((string)obj["Temperature"]),
                                Persent_Power_Heater_TrayID2 = float.Parse((string)obj["Cong_Suat_Nung"]),
                                Time_Blow_TrayID2 = int.Parse((string)obj["Time_Blow"]),
                                Z_Safety_ID2 = float.Parse((string)obj["Z_Safety"]),
                                T_LamMat_ID2 = float.Parse((string)obj["Temperature_LamMat"]),
                                Pos_Roto_5_X = float.Parse(Pos_Roto_5[0].ToString()),
                                Pos_Roto_5_Y = float.Parse(Pos_Roto_5[1].ToString()),
                                Pos_Roto_5_Z = float.Parse(Pos_Roto_5[2].ToString()),
                                Pos_Roto_6_X = float.Parse(Pos_Roto_6[0].ToString()),
                                Pos_Roto_6_Y = float.Parse(Pos_Roto_6[1].ToString()),
                                Pos_Roto_6_Z = float.Parse(Pos_Roto_6[2].ToString()),
                                Pos_Truc_5_X = float.Parse(Pos_Truc_5[0].ToString()),
                                Pos_Truc_5_Y = float.Parse(Pos_Truc_5[1].ToString()),
                                Pos_Truc_5_Z = float.Parse(Pos_Truc_5[2].ToString()),
                                //
                                Pos_Cum_Lap_Gap_X_TrayID2 = float.Parse(Pos_Gap_CumLap[0].ToString()),
                                Pos_Cum_Lap_Gap_Y_TrayID2 = float.Parse(Pos_Gap_CumLap[1].ToString()),
                                Pos_Cum_Lap_Gap_Z_TrayID2 = float.Parse(Pos_Gap_CumLap[2].ToString()),
                                // Còn Data Nhấn
                                //
                                Pos_Cum_Lap_Nhan_X_TrayID2 = float.Parse(Pos_Nhan_Truc[0].ToString()),
                                Pos_Cum_Lap_Nhan_Y_TrayID2 = float.Parse(Pos_Nhan_Truc[1].ToString()),
                                Pos_Cum_Lap_Nhan_Z_TrayID2 = float.Parse(Pos_Nhan_Truc[2].ToString()),
                                //
                                Pos_Cum_Lap_Lap_X_TrayID2 = float.Parse(Pos_Lap_CumLap[0].ToString()),
                                Pos_Cum_Lap_Lap_Y_TrayID2 = float.Parse(Pos_Lap_CumLap[1].ToString()),
                                Pos_Cum_Lap_Lap_Z_TrayID2 = float.Parse(Pos_Lap_CumLap[2].ToString()),
                                //
                                Pos_Nang_Kep_Up_TrayID2 = float.Parse(Pos_Nang_Kep_CumLap[0].ToString()),
                                Pos_Nang_Kep_Kep_TrayID2 = float.Parse(Pos_Nang_Kep_CumLap[1].ToString()),
                                //
                                Pos_Ban_XoayX_TrayID2 = float.Parse(Pos_Tha_BanXoay[0].ToString()), //
                                Pos_Ban_XoayY_TrayID2 = float.Parse(Pos_Tha_BanXoay[1].ToString()), //
                                Pos_Ban_XoayZ_TrayID2 = float.Parse(Pos_Tha_BanXoay[2].ToString()), //
                                //
                                SL_In_Tray_2 = qty_,
                                Check_QR2_Done = true
                            };
                            string jsonData = JsonConvert.SerializeObject(DB_Run);
                            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                            var response = await client.PostAsync("http://" + Ip_Port_Host + "/api/Control_PLC_1", content);
                            var responseContent = await response.Content.ReadAsStringAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Tray_Fail_Detect()
        {
            var data = new
            {
                Machine_Error_Detect_Tray = true,
            };
            string jsonData = JsonConvert.SerializeObject(data);
            Send_data(jsonData);
        }
        private async void ProcessComPort1(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (_serialPort1 != null && _serialPort1.IsOpen && data != null)
                {
                    if (_serialPort1.BytesToRead > 0 && MainWindow.Mode == "true")
                    {
                        var receivedData = _serialPort1.ReadExisting();
                        receivedData = receivedData.Replace("\r", "").Replace("\n", "");
                        Tray_QR_1 = receivedData.ToUpper();
                    }
                }
                await Task.Delay(10);
            }
        }
        private async void ProcessComPort2(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (_serialPort2 != null && _serialPort2.IsOpen && data != null)
                {
                    if (_serialPort2.BytesToRead > 0 && MainWindow.Mode == "true")
                    {
                        var receivedData = _serialPort2.ReadExisting();
                        receivedData = receivedData.Replace("\r", "").Replace("\n", "");
                        Tray_QR_2 = receivedData.ToUpper();
                    }
                }
                await Task.Delay(10);
            }
        }
        private async void MyBackgroundTask(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    if (!Connected)
                    {
                        socketclient = new SocketIO(host_socket);
                        await socketclient.ConnectAsync();
                        Connected = true;
                    }
                    if (Sendmac == false && Connected == true)
                    {
                        var mac = new
                        {
                            mac = GetMacAddress(),
                        };
                        await socketclient.EmitAsync("connect-machine", mac);
                        Sendmac = true;
                    }
                    socketclient.OnConnected += (eventSender, eventArgs) =>
                    {
                        Connected = true;
                    };

                    socketclient.OnDisconnected += (eventSender, eventArgs) =>
                    {
                        Connected = false;
                        Sendmac = false;
                        Machine_Request_Tray_Out_Re = "";
                        Machine_Request_Tray_In_Re = "";
                    };
                    if (Connected && Tray_QR_1 != "" && Tray_QR_1 != Tray_QR_1_Re)
                    {
                        if (Tray_QR_1.Length > 0 && Tray_QR_1.Length < 8)
                        {
                            try
                            {
                                var Tray_ID = new
                                {
                                    mac = GetMacAddress(),
                                    tray_id = Tray_QR_1,
                                };
                                await socketclient.EmitAsync("get-data-rotor-machine-with-machine", Tray_ID);
                                Tray_QR_1_Re = Tray_QR_1;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                    }
                    if (Connected && Tray_QR_2 != "" && Tray_QR_2_Re != Tray_QR_2)
                    {
                        try
                        {
                            var Tray_ID = new
                            {
                                mac = GetMacAddress(),
                                tray_id = Tray_QR_2,
                            };
                            await socketclient.EmitAsync("get-data-rotor-machine-with-machine-waiting", Tray_ID);
                            Tray_QR_2_Re = Tray_QR_2;
                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message); }
                    }
                    if (data != null)
                    {
                        if (Convert.ToString(data.Machine_Done_Tray) == "true" && !saveResultCalled)
                        {
                            var Tray_Status = new
                            {
                                mac = GetMacAddress(),
                                tray_id = Tray_QR_1,
                                qty = Qty_1,
                                height_setting = Height_1,
                                shaft_diameter_setting = Diameter_1,
                                temperature_setting = Temp_1,
                                Roto_1 = Convert.ToString(data.Roto_0_Height_Truc) + "^" + Convert.ToString(data.Roto_0_Result),
                                Roto_2 = Convert.ToString(data.Roto_1_Height_Truc) + "^" + Convert.ToString(data.Roto_1_Result),
                                Roto_3 = Convert.ToString(data.Roto_2_Height_Truc) + "^" + Convert.ToString(data.Roto_2_Result),
                                Roto_4 = Convert.ToString(data.Roto_3_Height_Truc) + "^" + Convert.ToString(data.Roto_3_Result),
                                Roto_5 = Convert.ToString(data.Roto_4_Height_Truc) + "^" + Convert.ToString(data.Roto_4_Result),
                            };
                            if (Connected)
                            {
                                await socketclient.EmitAsync("push-data-production", Tray_Status);
                            }
                            var Control = new
                            {
                                Machine_Save_Result_Done = true,
                            };
                            string jsonData = JsonConvert.SerializeObject(Control);
                            Send_data(jsonData);
                            saveResultCalled = true;
                        }

                        if (Convert.ToString(data.Machine_Done_Tray) == "false")
                        {
                            saveResultCalled = false;
                        }
                        if (Machine_Request_Tray_In_Re != Convert.ToString(data.Machine_Request_Tray_In))
                        {
                            var Input_Tray = new
                            {
                                mac = GetMacAddress(),
                                status = Convert.ToString(data.Machine_Request_Tray_In),
                            };

                            if (Connected)
                            {
                                await socketclient.EmitAsync("input-tray", Input_Tray);
                                Machine_Request_Tray_In_Re = Convert.ToString(data.Machine_Request_Tray_In);
                            }
                        }
                        if (Machine_Request_Tray_Out_Re != Convert.ToString(data.Machine_Request_Tray_Out))
                        {
                            var output_Tray = new
                            {
                                mac = GetMacAddress(),
                                status = Convert.ToString(data.Machine_Request_Tray_Out),
                            };

                            if (Connected)
                            {
                                await socketclient.EmitAsync("output-tray", output_Tray);
                                Machine_Request_Tray_Out_Re = Convert.ToString(data.Machine_Request_Tray_Out);
                            }
                        }
                    }
                    socketclient.On("get-data-rotor-machine-with-machine", response =>
                    {
                        string res = response.ToString();
                        Dictionary<string, object>[] objects = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>[]>(res);
                        if (objects[0]["status"].ToString() == "False")
                        {
                            Tray_Fail_Detect();
                        }
                        else
                        {
                            JArray jsonArray = JArray.Parse(res);
                            string trucId = jsonArray[0]["data"]["truc_id"].ToString();
                            string rotoId = jsonArray[0]["data"]["rotor_id"].ToString();
                            string qty = jsonArray[0]["data"]["qty"].ToString();
                            Truc_ID_1 = trucId;
                            Roto_ID_1 = rotoId;
                            Qty_1 = qty;
                            Data_Tray_1(trucId, rotoId, int.Parse(qty));
                        }
                    });
                    socketclient.On("get-data-rotor-machine-with-machine-waiting", response =>
                    {
                        string res = response.ToString();
                        Dictionary<string, object>[] objects = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>[]>(res);
                        if (objects[0]["status"].ToString() == "False")
                        {
                            //Tray_Fail_Detect();
                        }
                        else
                        {
                            JArray jsonArray = JArray.Parse(res);
                            string trucId = jsonArray[0]["data"]["truc_id"].ToString();
                            string rotoId = jsonArray[0]["data"]["rotor_id"].ToString();
                            string qty = jsonArray[0]["data"]["qty"].ToString();
                            Truc_ID_2 = trucId;
                            Roto_ID_2 = rotoId;
                            Qty_2 = qty;
                            Data_Tray_2(trucId, rotoId, int.Parse(qty));

                        }
                    });
                    socketclient.On("sync-model-machine", async response =>
                    {
                        string json = File.ReadAllText(path.Model);
                        try
                        {
                            var Data_Sync = new
                            {
                                mac = GetMacAddress(),
                                data = json
                            };
                            await socketclient.EmitAsync("sync-model-machine", Data_Sync);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    });
                }
                catch
                {

                }
                await Task.Delay(100);
            }
        }
        private void Timer_UpdateUI_Tick(object sender, EventArgs e)
        {
            float cpuUsage = cpuCounter.NextValue();
            Dispatcher.Invoke(() =>
            {
                string formattedCpuUsage = cpuUsage.ToString("F2") + "%";
                Per_CPU.Content = formattedCpuUsage;

            });
        }

        private async void Timer_Check_Maintance_Tick(object sender, EventArgs e)
        {
            try
            {
                string json_Maintance = File.ReadAllText(path.Maintenancen);

                var time_Maintance = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(json_Maintance);
                if (data != null)
                {
                    if (time_Maintance["Time_Next_Maintance"] != DateTime.Now.ToString("dd/MM/yyyy"))
                    {
                        var Control = new
                        {
                            Machine_Flag_Maintance = false,
                        };
                        string jsonData = JsonConvert.SerializeObject(Control);
                        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                        var response = await client.PostAsync("http://" + Ip_Port_Host + "/api/Control_PLC_1", content);
                        var responseContent = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        var Control = new
                        {
                            Machine_Flag_Maintance = true,
                        };
                        string jsonData = JsonConvert.SerializeObject(Control);
                        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                        var response = await client.PostAsync("http://" + Ip_Port_Host + "/api/Control_PLC_1", content);
                        var responseContent = await response.Content.ReadAsStringAsync();
                    }
                }
                Dispatcher.Invoke(() =>
                { });
            }
            catch
            { }
            try
            {
                string json_Check = File.ReadAllText(path.History_Check);
                if (json_Check.Length > 0)
                {
                    JArray List_Check = JArray.Parse(json_Check);
                    bool isDateFound = false;
                    foreach (JObject obj in List_Check)
                    {
                        if (System.DateTime.Now.ToString("dd/MM/yyyy") == (string)obj["Date"])
                        {
                            isDateFound = true;
                            break;
                        }
                    }
                    if (!isDateFound)
                    {
                        var Control = new
                        {
                            Setting_Warring_Check = true,
                        };
                        string jsonData = JsonConvert.SerializeObject(Control);
                        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                        var response = await client.PostAsync("http://" + Ip_Port_Host + "/api/Control_PLC_1", content);
                        var responseContent = await response.Content.ReadAsStringAsync();
                    }
                }
                else
                {
                    var Control = new
                    {
                        Setting_Warring_Check = true,
                    };
                    string jsonData = JsonConvert.SerializeObject(Control);
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("http://" + Ip_Port_Host + "/api/Control_PLC_1", content);
                    var responseContent = await response.Content.ReadAsStringAsync();
                }
            }
            catch { }
        }
        private void Stop_Timer_Check_Maintance()
        {
            Timer_Check_Maintance.Stop();
            Timer_Check_Maintance.Tick -= Timer_Check_Maintance_Tick;
            Timer_Check_Maintance = null;
        }
        private void DoWork(object sender, DoWorkEventArgs e)
        {

            while (!shouldStop)
            {
                try
                {
                    data = Read_PLC.Data;
                    Dispatcher.Invoke(() =>
                    {

                        System.DateTime dateTime = System.DateTime.Now;
                        string formattedDate = dateTime.ToString("dd/MM/yyyy");
                        lb_Day.Content = formattedDate;
                        string formattedtime = dateTime.ToString("HH:mm:ss");
                        lb_Time.Content = formattedtime;

                        if (Connected)
                        {
                            lb_Server.Foreground = System.Windows.Media.Brushes.Green;
                        }
                        else
                        {
                            lb_Server.Foreground = System.Windows.Media.Brushes.Red;
                        }
                        if (Read_PLC.flag == true)
                        {
                            if (data != null)
                            {
                                Clear_Data_Tray();
                                Mode = data.I_A_M;
                                txb_Real_Pos_X.Text = String.Format("{0:F2}", data.Truc_X_Real_Pos);
                                txb_Real_Pos_Y.Text = String.Format("{0:F2}", data.Truc_Y_Real_Pos);
                                txb_Real_Pos_Z.Text = String.Format("{0:F2}", data.Truc_Z_Real_Pos);
                                txb_Real_Pos_NangLap.Text = String.Format("{0:F2}", data.Cum_Lap_Real_Pos_UpDown);
                                txb_Real_Pos_KepLap.Text = String.Format("{0:F2}", data.Cum_Lap_Real_Pos_KepMo);
                                txb_Real_Pos_NangCheck.Text = String.Format("{0:F2}", data.Cum_Check_Real_Pos_UpDown);
                                txb_Real_Pos_KepCheck.Text = String.Format("{0:F2}", data.Cum_Check_Real_Pos_KepMo);
                                txb_Temper_Heater.Text = String.Format("{0:F2}", data.Machine_Temperate_Heater);
                                txb_NhietDo_LamMat.Text = String.Format("{0:F2}", data.Machine_T_LamMat);
                                if (data.I_A_M != "BAD 255")
                                {
                                    lb_Connect.Foreground = System.Windows.Media.Brushes.Green;
                                }
                                else
                                {
                                    lb_Connect.Foreground = System.Windows.Media.Brushes.Red;
                                }
                                update.bt_Colors_Status_1(bt_Origin, Convert.ToString(data.Machine_Home_Status));
                                //
                                if (Mode == "true") lb_Mode.Content = "Tự Động";
                                else lb_Mode.Content = "Bằng Tay";
                                if (data.Machine_Alarm_Error > 0)
                                {
                                    flag_error++;
                                    if (flag_error == 1)
                                    {
                                        string formattedDate_ = dateTime.ToString("dd/MM/yyyy HH:mm:ss");
                                        string json_ = File.ReadAllText(path.List_Error);
                                        string[] errorArray = json_.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                                        try
                                        {
                                            string json = File.ReadAllText(path.History);
                                            json = json.Remove(json.Length - 1);
                                            json = json + "," + "{\"Content_\": " + "\"" + errorArray[data.Machine_Alarm_Error - 1].Replace("\r", "").Replace("\n", "") + "\"," + "\"Time\": " + "\"" + formattedDate_ + "\"}" + "]";
                                            File.WriteAllText(path.History, json);
                                        }
                                        catch (Exception ex)
                                        {
                                            string json = File.ReadAllText(path.History);
                                            json = json.Remove(json.Length - 1);
                                            json = json + "," + "{\"Content_\": " + "\"" + "Lỗi Không Xác Định" + "\"," + "\"Time\": " + "\"" + formattedDate_ + "\"}" + "]";
                                            File.WriteAllText(path.History, json);
                                            MessageBox.Show(ex.Message);
                                        }
                                    }
                                }
                                else
                                {
                                    flag_error = 0;
                                }
                                if (Convert.ToString(data.Machine_Done_Tray) == "true" && !isMachineDoneTrayProcessed)
                                {
                                    bt_Auto.Background = new SolidColorBrush(Color.FromRgb(100, 149, 237));
                                    bt_Origin.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                                    bt_Manu.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                                    bt_Reset.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                                    bt_History.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                                    bt_GPIO.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                                    bt_Model.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                                    bt_Setting.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                                    Pannel_Monitor.Children.Clear();
                                    Pannel_Monitor.Children.Add(auto_Screen);
                                    isMachineDoneTrayProcessed = true;
                                }
                                else
                                {
                                    isMachineDoneTrayProcessed = false;
                                }
                            }
                            else
                            {
                                lb_Connect.Foreground = System.Windows.Media.Brushes.Red;
                            }
                        }

                    });
                }
                catch
                {
                }
                Thread.Sleep(100);
            }
        }
        private void Auto_Click(object sender, RoutedEventArgs e)
        {
            bt_Auto.Background = new SolidColorBrush(Color.FromRgb(100, 149, 237));
            bt_Origin.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_Manu.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_Reset.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_History.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_GPIO.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_Model.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_Setting.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            Pannel_Monitor.Children.Clear();
            Pannel_Monitor.Children.Add(auto_Screen);
        }
        private async void Origin_Click(object sender, RoutedEventArgs e)
        {
            if (isSerialPort1Initialized && isSerialPort2Initialized)
            {
                if (data != null)
                {
                    if (Convert.ToString(data.I_A_M) == "true")
                    {
                        MessageBox.Show("Vui Lòng Chuyển Sang Chế Độ Bằng Tay");
                    }
                    else
                    {
                        try
                        {
                            var Control = new
                            {
                                Machine_Run_Home_All = true,
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
                }
                else
                {
                    MessageBox.Show("QR Code chưa sẵn sàng");
                }
            }

        }
        private void Manual_Click(object sender, RoutedEventArgs e)
        {
            bt_Auto.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_Origin.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_Manu.Background = new SolidColorBrush(Color.FromRgb(100, 149, 237));
            bt_Reset.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_History.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_GPIO.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_Model.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_Setting.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            Pannel_Monitor.Children.Clear();
            Pannel_Monitor.Children.Add(manual_Screen);
        }
        private async void bt_Reset_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                flag_error = 0;
                var Control = new
                {
                    Machine_Reset_All = true,
                };
                string jsonData = JsonConvert.SerializeObject(Control);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://" + Ip_Port_Host + "/api/Control_PLC_1", content);
                var responseContent = await response.Content.ReadAsStringAsync();
            }
            catch { }
        }
        private void History_Click(object sender, RoutedEventArgs e)
        {
            bt_Auto.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_Origin.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_Manu.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_Reset.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_History.Background = new SolidColorBrush(Color.FromRgb(100, 149, 237));
            bt_GPIO.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_Model.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_Setting.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            Pannel_Monitor.Children.Clear();
            Pannel_Monitor.Children.Add(History_Log_Screen);
        }
        private void Model_Click(object sender, RoutedEventArgs e)
        {

            bt_Auto.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_Origin.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_Manu.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_Reset.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_History.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_GPIO.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_Model.Background = new SolidColorBrush(Color.FromRgb(100, 149, 237));
            bt_Setting.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            Pannel_Monitor.Children.Clear();
            Pannel_Monitor.Children.Add(Model_Screen);

        }
        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            if (UserName == "STI-Service")
            {
                bt_Auto.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                bt_Origin.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                bt_Manu.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                bt_Reset.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                bt_History.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                bt_GPIO.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                bt_Model.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                bt_Setting.Background = new SolidColorBrush(Color.FromRgb(100, 149, 237));
                Pannel_Monitor.Children.Clear();
                Pannel_Monitor.Children.Add(Setting_Screen);
            }
            else MessageBox.Show("Vui Lòng Gọi STI_Service");
        }
        private void bt_GPIO_Click(object sender, RoutedEventArgs e)
        {
            bt_Auto.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_Origin.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_Manu.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_Reset.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_History.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_GPIO.Background = new SolidColorBrush(Color.FromRgb(100, 149, 237));
            bt_Model.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            bt_Setting.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            Pannel_Monitor.Children.Clear();
            Pannel_Monitor.Children.Add(Gpio_Screen);
        }
        private void bt_Login(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.LoginSuccessful += LoginWindow_LoginSuccessful;
            loginWindow.ShowDialog();
        }
        private DispatcherTimer logoutTimer;
        private DateTime lastActivityTime;
        private void LoginWindow_LoginSuccessful(object sender, EventArgs e)
        {
            lb_Name.Content = UserName;
            logoutTimer = new DispatcherTimer();
            logoutTimer.Interval = TimeSpan.FromMinutes(10);
            logoutTimer.Tick += LogoutTimer_Tick;
            Login();
        }
        private void LogoutTimer_Tick(object sender, EventArgs e)
        {
            // Kiểm tra thời gian không hoạt động và thực hiện đăng xuất tự động.
            TimeSpan idleTime = DateTime.Now - lastActivityTime;
            if (idleTime >= TimeSpan.FromMinutes(10))
            {
                Logout();
            }
        }
        private void Login()
        {
            lastActivityTime = DateTime.Now;
            logoutTimer.Start();
        }
        private void Logout()
        {
            UserName = "";
            lb_Name.Content = UserName;
        }
        private void TouchDownHandler(object sender, RoutedEventArgs e)
        {
            TouchEventArgs touchArgs = e as TouchEventArgs;
            if (touchArgs != null)
            {
                lastActivityTime = DateTime.Now;
            }
        }
        private void LanguageComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (LanguageComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string languageTag = selectedItem.Tag.ToString();
                ChangeAppLanguage(languageTag);
            }
        }
        private void ChangeAppLanguage(string languageTag)
        {
            if (LanguageComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedLanguage = selectedItem.Tag.ToString();
                Uri resourceUri = new Uri($"Resources/{selectedLanguage}", UriKind.Relative);
                ResourceDictionary newDictionary = new ResourceDictionary();
                newDictionary.Source = resourceUri;
                Application.Current.Resources.MergedDictionaries[0] = newDictionary;
            }
        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            Logout();
        }
    }
}
