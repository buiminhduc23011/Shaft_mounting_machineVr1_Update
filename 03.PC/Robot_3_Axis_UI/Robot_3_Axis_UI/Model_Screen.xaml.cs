using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

using System.Text.Json;
using System.Net.Http;
using System.Text;
using System.Windows.Threading;
using System.Dynamic;
using Robot_3_Axis_UI.Class;
using OfficeOpenXml;

namespace Robot_3_Axis_UI
{
    /// <summary>
    /// floateraction logic for Model_Screen.xaml
    /// </summary>
    public partial class Model_Screen : UserControl
    {
        Path path = new Path();
        string Ip_Port_Host;
        Update_Screen update = new Update_Screen();
        HttpClient client = new HttpClient();
        private DispatcherTimer timer;
        dynamic data;
        public string realPosX;
        public string realPosY;
        public string realPosZ;
        public string realPosNangLap;
        public string realPosKepLap;
        public string realPosNangCheck;
        public string realPosKepCheck;
        float TrucX_Jog_Pos = 0;
        float TrucY_Jog_Pos = 0;
        float TrucZ_Jog_Pos = 0;
        public Model_Screen()
        {

            InitializeComponent();
        }

        private async void Model_Screen_Loaded(object sender, RoutedEventArgs e)
        {
            YourMethod();
            //
            string json = File.ReadAllText(path.Setting);
            var data_Setting = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            Ip_Port_Host = data_Setting["Ip_Port_Ser"];
            //
            List<Items> items = new List<Items>();
            int index = 1;
            try
            {
                string List_Show = File.ReadAllText(path.Model);
                if (List_Show.Length > 0)
                {
                    JArray List_Show_array = JArray.Parse(List_Show);
                    foreach (JObject obj in List_Show_array)
                    {
                        items.Add(new Items { STT = index, Model = (string)obj["Model"], Truc = (string)obj["Truc"], Roto = (string)obj["Roto"] });
                        index++;
                    }
                    List_Models.ItemsSource = items;
                }
            }
            catch { }

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
        private void DS_Model_Click(object sender, RoutedEventArgs e)
        {

        }
        private void YourMethod()
        {
            List_Models.AddHandler(DataGrid.SelectionChangedEvent, new SelectionChangedEventHandler(Model_SelectionChanged));
        }

        private void Model_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedRow = List_Models.SelectedItem as Items;
            if (selectedRow != null)
            {
                // Lấy dữ liệu từ hàng được chọn
                var data = selectedRow.Truc;
                txb_Truc.Text = data.ToString();
                var data_ = selectedRow.Roto;
                txb_Roto.Text = data_.ToString();
            }
        }
        private void bt_SetRotor1_Click(object sender, EventArgs e)
        {

            txb_Roto1_PosX.Text = realPosX;
            txb_Roto1_PosY.Text = realPosY;
            txb_Roto1_PosZ.Text = realPosZ;
        }
        private void bt_SetRotor2_Click(object sender, EventArgs e)
        {
            txb_Roto2_PosX.Text = realPosX;
            txb_Roto2_PosY.Text = realPosY;
            txb_Roto2_PosZ.Text = realPosZ;
        }
        private void bt_SetRotor3_Click(object sender, EventArgs e)
        {
            txb_Roto3_PosX.Text = realPosX;
            txb_Roto3_PosY.Text = realPosY;
            txb_Roto3_PosZ.Text = realPosZ;
        }
        private void bt_SetRotor4_Click(object sender, EventArgs e)
        {
            txb_Roto4_PosX.Text = realPosX;
            txb_Roto4_PosY.Text = realPosY;
            txb_Roto4_PosZ.Text = realPosZ;
        }
        private void bt_SetRotor5_Click(object sender, EventArgs e)
        {
            txb_Roto5_PosX.Text = realPosX;
            txb_Roto5_PosY.Text = realPosY;
            txb_Roto5_PosZ.Text = realPosZ;
        }
        private void bt_SetRotor6_Click(object sender, EventArgs e)
        {
            txb_Roto6_PosX.Text = realPosX;
            txb_Roto6_PosY.Text = realPosY;
            txb_Roto6_PosZ.Text = realPosZ;
        }
        private void bt_SetRotor7_Click(object sender, EventArgs e)
        {
            txb_Roto7_PosX.Text = realPosX;
            txb_Roto7_PosY.Text = realPosY;
            txb_Roto7_PosZ.Text = realPosZ;
        }
        private void bt_SetTruc1_Click(object sender, EventArgs e)
        {
            txb_Truc1_PosX.Text = realPosX;
            txb_Truc1_PosY.Text = realPosY;
            txb_Truc1_PosZ.Text = realPosZ;
        }
        private void bt_SetTruc2_Click(object sender, EventArgs e)
        {
            txb_Truc2_PosX.Text = realPosX;
            txb_Truc2_PosY.Text = realPosY;
            txb_Truc2_PosZ.Text = realPosZ;
        }
        private void bt_SetTruc3_Click(object sender, EventArgs e)
        {
            txb_Truc3_PosX.Text = realPosX;
            txb_Truc3_PosY.Text = realPosY;
            txb_Truc3_PosZ.Text = realPosZ;
        }
        private void bt_SetTruc4_Click(object sender, EventArgs e)
        {
            txb_Truc4_PosX.Text = realPosX;
            txb_Truc4_PosY.Text = realPosY;
            txb_Truc4_PosZ.Text = realPosZ;
        }
        private void bt_SetTruc5_Click(object sender, EventArgs e)
        {
            txb_Truc5_PosX.Text = realPosX;
            txb_Truc5_PosY.Text = realPosY;
            txb_Truc5_PosZ.Text = realPosZ;
        }
        private void bt_SetTruc6_Click(object sender, EventArgs e)
        {
            txb_Truc6_PosX.Text = realPosX;
            txb_Truc6_PosY.Text = realPosY;
            txb_Truc6_PosZ.Text = realPosZ;
        }
        private void bt_Set_Cum_Ktr_Tha(object sender, EventArgs e)
        {
            txb_Cum_Ktr_Tha_PosX.Text = realPosX;
            txb_Cum_Ktr_Tha_PosY.Text = realPosY;
            txb_Cum_Ktr_Tha_PosZ.Text = realPosZ;
        }
        private void bt_Set_Cum_Lap_Gap(object sender, EventArgs e)
        {
            txb_Cum_Lap_Gap_PosX.Text = realPosX;
            txb_Cum_Lap_Gap_PosY.Text = realPosY;
            txb_Cum_Lap_Gap_PosZ.Text = realPosZ;
        }
        private void bt_Set_Cum_Lap_Lap(object sender, EventArgs e)
        {
            txb_Cum_Lap_Lap_PosX.Text = realPosX;
            txb_Cum_Lap_Lap_PosY.Text = realPosY;
            txb_Cum_Lap_Lap_PosZ.Text = realPosZ;
        }
        private void bt_Set_Cum_Lap_Nhan(object sender, RoutedEventArgs e)
        {
            txb_Cum_Lap_Nhan_PosX.Text = realPosX;
            txb_Cum_Lap_Nhan_PosY.Text = realPosY;
            txb_Cum_Lap_Nhan_PosZ.Text = realPosZ;
        }
        private void bt_Set_Cum_Ktr_NangKep(object sender, EventArgs e)
        {
            txb_Cum_Ktr_Pos_Nang.Text = realPosNangCheck;
            txb_Cum_Ktr_Pos_Taykep.Text = realPosKepCheck;
        }
        private void bt_Set_Cum_Lap_NangKep(object sender, EventArgs e)
        {
            txb_Cum_Lap_Pos_Nang.Text = realPosNangLap;
            txb_Cum_Lap_Pos_TayKep.Text = realPosKepLap;
        }
        private void bt_Set_Ban_Nung_Tha(object sender, RoutedEventArgs e)
        {
            txb_Ban_Nung_Tha_PosX.Text = realPosX;
            txb_Ban_Nung_Tha_PosY.Text = realPosY;
            txb_Ban_Nung_Tha_PosZ.Text = realPosZ;
        }
        private void SetEmptyTextBoxToZero()
        {
            foreach (var textBox in FindVisualChildren<TextBox>(this))
            {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    textBox.Text = "0";
                }
            }
        }
        private void SetTextBoxToZero()
        {
            foreach (var textBox in FindVisualChildren<TextBox>(this))
            {
                    textBox.Text = "0";
            }
        }
        private IEnumerable<T> FindVisualChildren<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is T)
                {
                    yield return (T)child;
                }
                else
                {
                    var result = FindVisualChildren<T>(child);
                    if (result != null)
                    {
                        foreach (var item in result)
                        {
                            yield return item;
                        }
                    }
                }
            }
        }
        private void Backup_Data()
        {
            string currentDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            string User = MainWindow.UserName + "_";
            string FileNamebackup = "Path/Backup/" + "Backup_List_model_"+ User + currentDateTime + ".json";
            string data_backup = File.ReadAllText(path.Model);
            File.WriteAllText(FileNamebackup, data_backup);
        }
        private void bt_Save_Model_Click(object sender, EventArgs e)
        {
            if (MainWindow.UserName != "")
            {
                Backup_Data();
                try
                {
                    if (txb_Model.Text.Length > 0)
                    {
                        if (txb_Roto.Text.Length > 0)
                        {
                            if (txb_Truc.Text.Length > 0)
                            {
                                SetEmptyTextBoxToZero();
                                List_Model List_Model = new List_Model();
                                List_Model.Model = txb_Model.Text;
                                List_Model.Truc = txb_Truc.Text;
                                List_Model.Roto = txb_Roto.Text;
                                List_Model.Temperature = float.Parse(txb_Temperature.Text);
                                List_Model.Pos_Roto1 = new List<float> { float.Parse(txb_Roto1_PosX.Text), float.Parse(txb_Roto1_PosY.Text), float.Parse(txb_Roto1_PosZ.Text) };
                                List_Model.Pos_Roto2 = new List<float> { float.Parse(txb_Roto2_PosX.Text), float.Parse(txb_Roto2_PosY.Text), float.Parse(txb_Roto2_PosZ.Text) };
                                List_Model.Pos_Roto3 = new List<float> { float.Parse(txb_Roto3_PosX.Text), float.Parse(txb_Roto3_PosY.Text), float.Parse(txb_Roto3_PosZ.Text) };
                                List_Model.Pos_Roto4 = new List<float> { float.Parse(txb_Roto4_PosX.Text), float.Parse(txb_Roto4_PosY.Text), float.Parse(txb_Roto4_PosZ.Text) };
                                List_Model.Pos_Roto5 = new List<float> { float.Parse(txb_Roto5_PosX.Text), float.Parse(txb_Roto5_PosY.Text), float.Parse(txb_Roto5_PosZ.Text) };
                                List_Model.Pos_Roto6 = new List<float> { float.Parse(txb_Roto6_PosX.Text), float.Parse(txb_Roto6_PosY.Text), float.Parse(txb_Roto6_PosZ.Text) };
                                List_Model.Pos_Roto7 = new List<float> { float.Parse(txb_Roto7_PosX.Text), float.Parse(txb_Roto7_PosY.Text), float.Parse(txb_Roto7_PosZ.Text) };
                                List_Model.Pos_Truc1 = new List<float> { float.Parse(txb_Truc1_PosX.Text), float.Parse(txb_Truc1_PosY.Text), float.Parse(txb_Truc1_PosZ.Text) };
                                List_Model.Pos_Truc2 = new List<float> { float.Parse(txb_Truc2_PosX.Text), float.Parse(txb_Truc2_PosY.Text), float.Parse(txb_Truc2_PosZ.Text) };
                                List_Model.Pos_Truc3 = new List<float> { float.Parse(txb_Truc3_PosX.Text), float.Parse(txb_Truc3_PosY.Text), float.Parse(txb_Truc3_PosZ.Text) };
                                List_Model.Pos_Truc4 = new List<float> { float.Parse(txb_Truc4_PosX.Text), float.Parse(txb_Truc4_PosY.Text), float.Parse(txb_Truc4_PosZ.Text) };
                                List_Model.Pos_Truc5 = new List<float> { float.Parse(txb_Truc5_PosX.Text), float.Parse(txb_Truc5_PosY.Text), float.Parse(txb_Truc5_PosZ.Text) };
                                List_Model.Pos_Truc6 = new List<float> { float.Parse(txb_Truc6_PosX.Text), float.Parse(txb_Truc6_PosY.Text), float.Parse(txb_Truc6_PosZ.Text) };
                                List_Model.Pos_Tha_Cum_Ktr = new List<float> { float.Parse(txb_Cum_Ktr_Tha_PosX.Text), float.Parse(txb_Cum_Ktr_Tha_PosY.Text), float.Parse(txb_Cum_Ktr_Tha_PosZ.Text) };
                                List_Model.Pos_Nang_Kep_Cum_Ktr = new List<float> { float.Parse(txb_Cum_Ktr_Pos_Nang.Text), float.Parse(txb_Cum_Ktr_Pos_Taykep.Text) };
                                List_Model.Pos_Gap_Cum_Lap = new List<float> { float.Parse(txb_Cum_Lap_Gap_PosX.Text), float.Parse(txb_Cum_Lap_Gap_PosY.Text), float.Parse(txb_Cum_Lap_Gap_PosZ.Text) };
                                List_Model.Pos_Lap_Cum_Lap = new List<float> { float.Parse(txb_Cum_Lap_Lap_PosX.Text), float.Parse(txb_Cum_Lap_Lap_PosY.Text), float.Parse(txb_Cum_Lap_Lap_PosZ.Text) };
                                List_Model.Pos_Nhan_Truc = new List<float> { float.Parse(txb_Cum_Lap_Nhan_PosX.Text), float.Parse(txb_Cum_Lap_Nhan_PosY.Text), float.Parse(txb_Cum_Lap_Nhan_PosZ.Text) };
                                List_Model.Pos_Nang_Kep_Cum_Lap = new List<float> { float.Parse(txb_Cum_Lap_Pos_Nang.Text), float.Parse(txb_Cum_Lap_Pos_TayKep.Text) };
                                List_Model.Pos_Tha_Ban_Xoay = new List<float> { float.Parse(txb_Ban_Nung_Tha_PosX.Text), float.Parse(txb_Ban_Nung_Tha_PosY.Text), float.Parse(txb_Ban_Nung_Tha_PosZ.Text) };
                                List_Model.Offset_Tha_Roto = float.Parse(txb_Offset_Tha_Roto.Text);
                                List_Model.Cong_Suat_Nung = float.Parse(txb_PersenHeater.Text);
                                List_Model.Time_Blow = float.Parse(txb_Time_Blow.Text);
                                List_Model.Temperature_LamMat = float.Parse(txb_NhietDo_LamMat.Text);
                                List_Model.Z_Safety = float.Parse(txb_Z_Safety.Text);
                                List_Model.Pos_LapTruc = int.Parse(txb_Vtri_Lap_Truc.Text);
                                List_Model.LoaiTruc = int.Parse(txb_LoaiTruc.Text);
                                string list_Model_Json = JsonConvert.SerializeObject(List_Model);
                                //
                                try
                                {
                                    string json = File.ReadAllText(path.Model);
                                    var options = new JsonSerializerOptions { ReadCommentHandling = JsonCommentHandling.Skip };
                                    var data = System.Text.Json.JsonSerializer.Deserialize<List_Model_Temp[]>(json, options);
                                    float flag = 0;
                                    foreach (var item in data)
                                    {
                                        if (item.Truc == txb_Truc.Text & item.Roto == txb_Roto.Text)
                                        {
                                            item.Model = txb_Model.Text;
                                            item.Temperature = float.Parse(txb_Temperature.Text);
                                            item.Pos_Roto1 = new List<float> { float.Parse(txb_Roto1_PosX.Text), float.Parse(txb_Roto1_PosY.Text), float.Parse(txb_Roto1_PosZ.Text) };
                                            item.Pos_Roto2 = new List<float> { float.Parse(txb_Roto2_PosX.Text), float.Parse(txb_Roto2_PosY.Text), float.Parse(txb_Roto2_PosZ.Text) };
                                            item.Pos_Roto3 = new List<float> { float.Parse(txb_Roto3_PosX.Text), float.Parse(txb_Roto3_PosY.Text), float.Parse(txb_Roto3_PosZ.Text) };
                                            item.Pos_Roto4 = new List<float> { float.Parse(txb_Roto4_PosX.Text), float.Parse(txb_Roto4_PosY.Text), float.Parse(txb_Roto4_PosZ.Text) };
                                            item.Pos_Roto5 = new List<float> { float.Parse(txb_Roto5_PosX.Text), float.Parse(txb_Roto5_PosY.Text), float.Parse(txb_Roto5_PosZ.Text) };
                                            item.Pos_Roto6 = new List<float> { float.Parse(txb_Roto6_PosX.Text), float.Parse(txb_Roto6_PosY.Text), float.Parse(txb_Roto6_PosZ.Text) };
                                            item.Pos_Roto7 = new List<float> { float.Parse(txb_Roto7_PosX.Text), float.Parse(txb_Roto7_PosY.Text), float.Parse(txb_Roto7_PosZ.Text) };
                                            item.Pos_Truc1 = new List<float> { float.Parse(txb_Truc1_PosX.Text), float.Parse(txb_Truc1_PosY.Text), float.Parse(txb_Truc1_PosZ.Text) };
                                            item.Pos_Truc2 = new List<float> { float.Parse(txb_Truc2_PosX.Text), float.Parse(txb_Truc2_PosY.Text), float.Parse(txb_Truc2_PosZ.Text) };
                                            item.Pos_Truc3 = new List<float> { float.Parse(txb_Truc3_PosX.Text), float.Parse(txb_Truc3_PosY.Text), float.Parse(txb_Truc3_PosZ.Text) };
                                            item.Pos_Truc4 = new List<float> { float.Parse(txb_Truc4_PosX.Text), float.Parse(txb_Truc4_PosY.Text), float.Parse(txb_Truc4_PosZ.Text) };
                                            item.Pos_Truc5 = new List<float> { float.Parse(txb_Truc5_PosX.Text), float.Parse(txb_Truc5_PosY.Text), float.Parse(txb_Truc5_PosZ.Text) };
                                            item.Pos_Truc6 = new List<float> { float.Parse(txb_Truc6_PosX.Text), float.Parse(txb_Truc6_PosY.Text), float.Parse(txb_Truc6_PosZ.Text) };
                                            item.Pos_Tha_Cum_Ktr = new List<float> { float.Parse(txb_Cum_Ktr_Tha_PosX.Text), float.Parse(txb_Cum_Ktr_Tha_PosY.Text), float.Parse(txb_Cum_Ktr_Tha_PosZ.Text) };
                                            item.Pos_Nang_Kep_Cum_Ktr = new List<float> { float.Parse(txb_Cum_Ktr_Pos_Nang.Text), float.Parse(txb_Cum_Ktr_Pos_Taykep.Text) };
                                            item.Pos_Gap_Cum_Lap = new List<float> { float.Parse(txb_Cum_Lap_Gap_PosX.Text), float.Parse(txb_Cum_Lap_Gap_PosY.Text), float.Parse(txb_Cum_Lap_Gap_PosZ.Text) };
                                            item.Pos_Lap_Cum_Lap = new List<float> { float.Parse(txb_Cum_Lap_Lap_PosX.Text), float.Parse(txb_Cum_Lap_Lap_PosY.Text), float.Parse(txb_Cum_Lap_Lap_PosZ.Text) };
                                            item.Pos_Nhan_Truc = new List<float> { float.Parse(txb_Cum_Lap_Nhan_PosX.Text), float.Parse(txb_Cum_Lap_Nhan_PosY.Text), float.Parse(txb_Cum_Lap_Nhan_PosZ.Text) };
                                            item.Pos_Nang_Kep_Cum_Lap = new List<float> { float.Parse(txb_Cum_Lap_Pos_Nang.Text), float.Parse(txb_Cum_Lap_Pos_TayKep.Text) };
                                            item.Pos_Tha_Ban_Xoay = new List<float> { float.Parse(txb_Ban_Nung_Tha_PosX.Text), float.Parse(txb_Ban_Nung_Tha_PosY.Text), float.Parse(txb_Ban_Nung_Tha_PosZ.Text) };
                                            item.Offset_Tha_Roto = float.Parse(txb_Offset_Tha_Roto.Text);
                                            item.Cong_Suat_Nung = float.Parse(txb_PersenHeater.Text);
                                            item.Time_Blow = float.Parse(txb_Time_Blow.Text);
                                            item.Temperature_LamMat = float.Parse(txb_NhietDo_LamMat.Text);
                                            item.Z_Safety = float.Parse(txb_Z_Safety.Text);
                                            item.Pos_LapTruc = int.Parse(txb_Vtri_Lap_Truc.Text);
                                            item.LoaiTruc = int.Parse(txb_LoaiTruc.Text);
                                            var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
                                            string newJsonString = System.Text.Json.JsonSerializer.Serialize(data, jsonOptions);
                                            // Write back to file
                                            File.WriteAllText(path.Model, newJsonString);
                                            MessageBox.Show("Đã Lưu Thành Công");
                                            flag = 1;
                                            break;
                                        }
                                    }
                                    if (flag == 0)
                                    {
                                        json = json.Remove(json.Length - 1);
                                        json = json + "," + list_Model_Json + "]";
                                        File.WriteAllText(path.Model, json);
                                        MessageBox.Show("Đã Lưu Và Tạo Model Mới Thành Công");
                                    }
                                }
                                catch
                                {
                                    string json_;
                                    json_ = "[" + list_Model_Json + "]";
                                    File.WriteAllText(path.Model, json_);
                                    MessageBox.Show("Đã Lưu Và Tạo Model Mới Thành Công");
                                }
                                List<Items> items = new List<Items>();
                                int index = 1;
                                string List_Show = File.ReadAllText(path.Model);
                                if (List_Show.Length > 0)
                                {
                                    JArray List_Show_array = JArray.Parse(List_Show);
                                    foreach (JObject obj in List_Show_array)
                                    {
                                        items.Add(new Items { STT = index, Model = (string)obj["Model"], Truc = (string)obj["Truc"], Roto = (string)obj["Roto"] });
                                        index++;
                                    }
                                    List_Models.ItemsSource = items;
                                }
                                
                            }
                            else MessageBox.Show("Vui Lòng Khai Báo Trục ID");
                        }
                        else MessageBox.Show("Vui Lòng Khai Báo Roto ID");
                    }

                    else MessageBox.Show("Vui Lòng Khai Báo Model");
                }
                catch
                {
                    MessageBox.Show("Vui Lòng Khai Báo Lại");
                }
            }
            else MessageBox.Show("Vui Lòng Đăng Nhập");
        }
        private void txb_Truc_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string json = File.ReadAllText(path.Model);
                if (json.Length > 0)
                {
                    JArray jsonArray = JArray.Parse(json);
                    foreach (JObject obj in jsonArray)
                    {
                        if ((string)obj["Truc"] == txb_Truc.Text & (string)obj["Roto"] == txb_Roto.Text)
                        {
                            txb_Roto.Text = (string)obj["Roto"];
                            txb_Model.Text = (string)obj["Model"];
                            txb_Temperature.Text = (string)obj["Temperature"];
                            JArray pos_Roto1 = (JArray)obj["Pos_Roto1"];
                            txb_Roto1_PosX.Text = pos_Roto1[0].ToString();
                            txb_Roto1_PosY.Text = pos_Roto1[1].ToString();
                            txb_Roto1_PosZ.Text = pos_Roto1[2].ToString();
                            JArray pos_Roto2 = (JArray)obj["Pos_Roto2"];
                            txb_Roto2_PosX.Text = pos_Roto2[0].ToString();
                            txb_Roto2_PosY.Text = pos_Roto2[1].ToString();
                            txb_Roto2_PosZ.Text = pos_Roto2[2].ToString();
                            JArray pos_Roto3 = (JArray)obj["Pos_Roto3"];
                            txb_Roto3_PosX.Text = pos_Roto3[0].ToString();
                            txb_Roto3_PosY.Text = pos_Roto3[1].ToString();
                            txb_Roto3_PosZ.Text = pos_Roto3[2].ToString();
                            JArray pos_Roto4 = (JArray)obj["Pos_Roto4"];
                            txb_Roto4_PosX.Text = pos_Roto4[0].ToString();
                            txb_Roto4_PosY.Text = pos_Roto4[1].ToString();
                            txb_Roto4_PosZ.Text = pos_Roto4[2].ToString();
                            JArray pos_Roto5 = (JArray)obj["Pos_Roto5"];
                            txb_Roto5_PosX.Text = pos_Roto5[0].ToString();
                            txb_Roto5_PosY.Text = pos_Roto5[1].ToString();
                            txb_Roto5_PosZ.Text = pos_Roto5[2].ToString();
                            JArray pos_Roto6 = (JArray)obj["Pos_Roto6"];
                            txb_Roto6_PosX.Text = pos_Roto6[0].ToString();
                            txb_Roto6_PosY.Text = pos_Roto6[1].ToString();
                            txb_Roto6_PosZ.Text = pos_Roto6[2].ToString();
                            JArray pos_Roto7 = (JArray)obj["Pos_Roto7"];
                            txb_Roto7_PosX.Text = pos_Roto7[0].ToString();
                            txb_Roto7_PosY.Text = pos_Roto7[1].ToString();
                            txb_Roto7_PosZ.Text = pos_Roto7[2].ToString();
                            JArray pos_Truc1 = (JArray)obj["Pos_Truc1"];
                            txb_Truc1_PosX.Text = pos_Truc1[0].ToString();
                            txb_Truc1_PosY.Text = pos_Truc1[1].ToString();
                            txb_Truc1_PosZ.Text = pos_Truc1[2].ToString();
                            JArray pos_Truc2 = (JArray)obj["Pos_Truc2"];
                            txb_Truc2_PosX.Text = pos_Truc2[0].ToString();
                            txb_Truc2_PosY.Text = pos_Truc2[1].ToString();
                            txb_Truc2_PosZ.Text = pos_Truc2[2].ToString();
                            JArray pos_Truc3 = (JArray)obj["Pos_Truc3"];
                            txb_Truc3_PosX.Text = pos_Truc3[0].ToString();
                            txb_Truc3_PosY.Text = pos_Truc3[1].ToString();
                            txb_Truc3_PosZ.Text = pos_Truc3[2].ToString();
                            JArray pos_Truc4 = (JArray)obj["Pos_Truc4"];
                            txb_Truc4_PosX.Text = pos_Truc4[0].ToString();
                            txb_Truc4_PosY.Text = pos_Truc4[1].ToString();
                            txb_Truc4_PosZ.Text = pos_Truc4[2].ToString();
                            JArray pos_Truc5 = (JArray)obj["Pos_Truc5"];
                            txb_Truc5_PosX.Text = pos_Truc5[0].ToString();
                            txb_Truc5_PosY.Text = pos_Truc5[1].ToString();
                            txb_Truc5_PosZ.Text = pos_Truc5[2].ToString();
                            JArray pos_Truc6 = (JArray)obj["Pos_Truc6"];
                            txb_Truc6_PosX.Text = pos_Truc6[0].ToString();
                            txb_Truc6_PosY.Text = pos_Truc6[1].ToString();
                            txb_Truc6_PosZ.Text = pos_Truc6[2].ToString();
                            JArray pos_Tha_Cum_Ktr = (JArray)obj["Pos_Tha_Cum_Ktr"];
                            txb_Cum_Ktr_Tha_PosX.Text = pos_Tha_Cum_Ktr[0].ToString();
                            txb_Cum_Ktr_Tha_PosY.Text = pos_Tha_Cum_Ktr[1].ToString();
                            txb_Cum_Ktr_Tha_PosZ.Text = pos_Tha_Cum_Ktr[2].ToString();
                            JArray pos_Nang_Kep_Cum_Ktr = (JArray)obj["Pos_Nang_Kep_Cum_Ktr"];
                            txb_Cum_Ktr_Pos_Nang.Text = pos_Nang_Kep_Cum_Ktr[0].ToString();
                            txb_Cum_Ktr_Pos_Taykep.Text = pos_Nang_Kep_Cum_Ktr[1].ToString();
                            JArray pos_Gap_Cum_Lap = (JArray)obj["Pos_Gap_Cum_Lap"];
                            txb_Cum_Lap_Gap_PosX.Text = pos_Gap_Cum_Lap[0].ToString();
                            txb_Cum_Lap_Gap_PosY.Text = pos_Gap_Cum_Lap[1].ToString();
                            txb_Cum_Lap_Gap_PosZ.Text = pos_Gap_Cum_Lap[2].ToString();
                            JArray pos_Lap_Cum_Lap = (JArray)obj["Pos_Lap_Cum_Lap"];
                            txb_Cum_Lap_Lap_PosX.Text = pos_Lap_Cum_Lap[0].ToString();
                            txb_Cum_Lap_Lap_PosY.Text = pos_Lap_Cum_Lap[1].ToString();
                            txb_Cum_Lap_Lap_PosZ.Text = pos_Lap_Cum_Lap[2].ToString();
                            JArray pos_Nhan_Truc = (JArray)obj["Pos_Nhan_Truc"];
                            txb_Cum_Lap_Nhan_PosX.Text = pos_Nhan_Truc[0].ToString();
                            txb_Cum_Lap_Nhan_PosY.Text = pos_Nhan_Truc[1].ToString();
                            txb_Cum_Lap_Nhan_PosZ.Text = pos_Nhan_Truc[2].ToString();
                            JArray pos_Nang_Kep_Cum_Lap = (JArray)obj["Pos_Nang_Kep_Cum_Lap"];
                            txb_Cum_Lap_Pos_Nang.Text = pos_Nang_Kep_Cum_Lap[0].ToString();
                            txb_Cum_Lap_Pos_TayKep.Text = pos_Nang_Kep_Cum_Lap[1].ToString();
                            JArray pos_Tha_Ban_Xoay = (JArray)obj["Pos_Tha_Ban_Xoay"];
                            txb_Ban_Nung_Tha_PosX.Text = pos_Tha_Ban_Xoay[0].ToString();
                            txb_Ban_Nung_Tha_PosY.Text = pos_Tha_Ban_Xoay[1].ToString();
                            txb_Ban_Nung_Tha_PosZ.Text = pos_Tha_Ban_Xoay[2].ToString();
                            //
                            txb_Offset_Tha_Roto.Text = (string)obj["Offset_Tha_Roto"];
                            txb_PersenHeater.Text = (string)obj["Cong_Suat_Nung"];
                            txb_Time_Blow.Text = (string)obj["Time_Blow"];
                           txb_NhietDo_LamMat.Text = (string)obj["Temperature_LamMat"];
                            txb_Z_Safety.Text=(string)obj["Z_Safety"];
                            txb_Vtri_Lap_Truc.Text=(string)obj["Pos_LapTruc"];
                            txb_LoaiTruc.Text = (string)obj["LoaiTruc"];
    }
                    }
                }
            }
            catch
            {

            }

        }
        private void txbModel_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Đổi đầu vào thành chữ hoa
            string upperCaseText = e.Text.ToUpper();

            // Xóa đoạn văn bản cũ
            TextBox textBox = sender as TextBox;
            int oldCaretIndex = textBox.CaretIndex;
            textBox.Text = textBox.Text.Remove(textBox.SelectionStart, textBox.SelectionLength);

            // Chèn đoạn văn bản mới vào TextBox
            textBox.Text = textBox.Text.Insert(textBox.CaretIndex, upperCaseText);

            // Đặt lại vị trí con trỏ hiện tại
            textBox.CaretIndex = oldCaretIndex + 1;

            // Hủy sự kiện nhập vào TextBox
            e.Handled = true;
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
        private void X_Axis_Jog_ADD_Click(object sender, RoutedEventArgs e)
        {
            Control("Truc_X_Jog_Add", true);
        }
        private void X_Axis_Jog_SUB_Click(object sender, RoutedEventArgs e)
        {
            Control("Truc_X_Jog_Sub", true);
        }
        private void Y_Axis_Jog_ADD_Click(object sender, RoutedEventArgs e)
        {
            Control("Truc_Y_Jog_Add", true);
        }

        private void Y_Axis_Jog_SUB_Click(object sender, RoutedEventArgs e)
        {
            Control("Truc_Y_Jog_Sub", true);
        }

        private void Z_Axis_Jog_ADD_Click(object sender, RoutedEventArgs e)
        {
            Control("Truc_Z_Jog_Add", true);
        }

        private void Z_Axis_Jog_SUB_Click(object sender, RoutedEventArgs e)
        {
            Control("Truc_Z_Jog_Sub", true);
        }

        private void Nang_Cum_Lap_Jog_ADD_Click(object sender, RoutedEventArgs e)
        {
            Control("Cum_Lap_Jog_Up", true);
        }

        private void Nang_Cum_Lap_Jog_SUB_Click(object sender, RoutedEventArgs e)
        {
            Control("Cum_Lap_Jog_Down", true);
        }

        private void Kep_Cum_Lap_Jog_ADD_Click(object sender, RoutedEventArgs e)
        {
            Control("Cum_Lap_Jog_Mo", true);
        }

        private void Kep_Cum_Lap_Jog_SUB_Click(object sender, RoutedEventArgs e)
        {
            Control("Cum_Lap_Jog_Kep", true);
        }

        private void Nang_Cum_Check_Jog_ADD_Click(object sender, RoutedEventArgs e)
        {
            Control("Cum_Check_Jog_Up", true);
        }

        private void Nang_Cum_Check_Jog_SUB_Click(object sender, RoutedEventArgs e)
        {
            Control("Cum_Check_Jog_Down", true);
        }

        private void Kep_Cum_Check_Jog_ADD_Click(object sender, RoutedEventArgs e)
        {
            Control("Cum_Check_Jog_Mo", true);
        }

        private void Kep_Cum_Check_Jog_SUB_Click(object sender, RoutedEventArgs e)
        {
            Control("Cum_Check_Jog_Kep", true);
        }  
        private void bt_Delete_Model_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.UserName != "")
            {
                
                MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa mã Truc: " + txb_Truc.Text + " và mã Roto: " + txb_Roto.Text + " ?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes & txb_Roto.Text.Length > 0 & txb_Truc.Text.Length > 0)
                {
                    Backup_Data();
                    string json = File.ReadAllText(path.Model);
                    var options = new JsonSerializerOptions { ReadCommentHandling = JsonCommentHandling.Skip };
                    var data = System.Text.Json.JsonSerializer.Deserialize<List_Model_Temp[]>(json, options);

                    var newData = new List<List_Model_Temp>();

                    foreach (var item in data)
                    {
                        if (item.Truc != txb_Truc.Text || item.Roto != txb_Roto.Text)
                        {
                            newData.Add(item);
                        }
                    }
                    var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
                    string newJsonString = System.Text.Json.JsonSerializer.Serialize(newData, jsonOptions);
                    // Write back to file
                    File.WriteAllText(path.Model, newJsonString);
                    // Fill lại giá trị vào bAng
                    List<Items> items = new List<Items>();
                    int index = 1;
                    string List_Show = File.ReadAllText(path.Model);
                    if (List_Show.Length > 0)
                    {
                        JArray List_Show_array = JArray.Parse(List_Show);
                        foreach (JObject obj in List_Show_array)
                        {
                            items.Add(new Items { STT = index, Model = (string)obj["Model"], Truc = (string)obj["Truc"], Roto = (string)obj["Roto"] });
                            index++;
                        }
                        List_Models.ItemsSource = items;
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy mã Truc: " + txb_Truc.Text + " và mã Roto: " + txb_Roto.Text + " cần xóa");
                }
            }
            else MessageBox.Show("Vui Lòng Đăng Nhập");
            
        }
        private async void Send_Data_Test(TextBox txb1, TextBox txb2, TextBox txb3)
        {
            var data_test = new
            {
                Pos_X = float.Parse(txb1.Text),
                Pos_Y = float.Parse(txb2.Text),
                Pos_Z = float.Parse("0"),
                Run_Test = true
            };
            string jsonData = JsonConvert.SerializeObject(data_test);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("http://" + Ip_Port_Host + "/api/Run_Test", content);
            var responseContent = await response.Content.ReadAsStringAsync();
        }
        private void bt_Roto1_Click(object sender, RoutedEventArgs e)
        {
            Send_Data_Test(txb_Roto1_PosX, txb_Roto1_PosY, txb_Roto1_PosZ);
        }
        private void bt_Roto2_Click(object sender, RoutedEventArgs e)
        {
            Send_Data_Test(txb_Roto2_PosX, txb_Roto2_PosY, txb_Roto2_PosZ);
        }
        private void bt_Roto3_Click(object sender, RoutedEventArgs e)
        {
            Send_Data_Test(txb_Roto3_PosX, txb_Roto3_PosY, txb_Roto3_PosZ);
        }
        private void bt_Roto4_Click(object sender, RoutedEventArgs e)
        {
            Send_Data_Test(txb_Roto4_PosX, txb_Roto4_PosY, txb_Roto4_PosZ);
        }

        private void bt_Roto5_Click(object sender, RoutedEventArgs e)
        {
            Send_Data_Test(txb_Roto5_PosX, txb_Roto5_PosY, txb_Roto5_PosZ);
        }

        private void bt_Truc1_Click(object sender, RoutedEventArgs e)
        {
            Send_Data_Test(txb_Truc1_PosX, txb_Truc1_PosY, txb_Truc1_PosZ);
        }

        private void bt_Truc2_Click(object sender, RoutedEventArgs e)
        {
            Send_Data_Test(txb_Truc2_PosX, txb_Truc2_PosY, txb_Truc2_PosZ);
        }

        private void bt_Truc3_Click(object sender, RoutedEventArgs e)
        {
            Send_Data_Test(txb_Truc3_PosX, txb_Truc3_PosY, txb_Truc3_PosZ);
        }

        private void bt_Truc4_Click(object sender, RoutedEventArgs e)
        {
            Send_Data_Test(txb_Truc4_PosX, txb_Truc4_PosY, txb_Truc4_PosZ);
        }

        private void bt_Truc5_Click(object sender, RoutedEventArgs e)
        {
            Send_Data_Test(txb_Truc5_PosX, txb_Truc5_PosY, txb_Truc5_PosZ);
        }

        private void bt_Roto6_Click(object sender, RoutedEventArgs e)
        {
            Send_Data_Test(txb_Roto6_PosX, txb_Roto6_PosY, txb_Roto6_PosZ);
        }

        private void bt_Roto7_Click(object sender, RoutedEventArgs e)
        {
            Send_Data_Test(txb_Roto7_PosX, txb_Roto7_PosY, txb_Roto7_PosZ);
        }

        private void bt_Truc6_Click(object sender, RoutedEventArgs e)
        {
            Send_Data_Test(txb_Truc6_PosX, txb_Truc6_PosY, txb_Truc6_PosZ);
        }

        private void bt_Tha_Cum_Ktr_Click(object sender, RoutedEventArgs e)
        {
            Send_Data_Test(txb_Cum_Ktr_Tha_PosX, txb_Cum_Ktr_Tha_PosY, txb_Cum_Ktr_Tha_PosZ);
        }

        private void bt_Gap_CumLap_Click(object sender, RoutedEventArgs e)
        {
            Send_Data_Test(txb_Cum_Lap_Gap_PosX, txb_Cum_Lap_Gap_PosY, txb_Cum_Lap_Gap_PosZ);
        }

        private void bt_Lap_CumLap_Click(object sender, RoutedEventArgs e)
        {
            Send_Data_Test(txb_Cum_Lap_Lap_PosX, txb_Cum_Lap_Lap_PosY, txb_Cum_Lap_Lap_PosZ);
        }
        private void bt_Nhan_CumLap_Click(object sender, RoutedEventArgs e)
        {
            Send_Data_Test(txb_Cum_Lap_Nhan_PosX, txb_Cum_Lap_Nhan_PosY, txb_Cum_Lap_Nhan_PosZ);
        }

        private void bt_Tha_BanXoay_Click(object sender, RoutedEventArgs e)
        {
            Send_Data_Test(txb_Ban_Nung_Tha_PosX, txb_Ban_Nung_Tha_PosY, txb_Ban_Nung_Tha_PosZ);
        }

        private void bt_Export_File_Click(object sender, RoutedEventArgs e)
        {
            string currentDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            string defaultFileName = "List_model_" + currentDateTime + ".xlsx";
            string filePath = "";

            System.Windows.Forms.SaveFileDialog dialog = new System.Windows.Forms.SaveFileDialog();
            dialog.Filter = "Excel | *.xlsx | Excel 2003 | *.xls";
            dialog.FileName = defaultFileName;
            dialog.CheckPathExists = true;

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filePath = dialog.FileName;
                string json = File.ReadAllText(path.Model);
                if (json.Length > 0)
                {
                    JArray jsonArray = JArray.Parse(json);
                    Excel.Export_Excecl(jsonArray, filePath);
                }
                MessageBox.Show("Hoàn Thành Xuất List Model");
            }

            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("Đường dẫn báo cáo không hợp lệ");
                return;
            }


        }

        private void bt_import_list_model_click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.UserName != "")
            {
                try
                {
                    Backup_Data();
                    string currentDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string defaultFileName = "BackUp_Models" + currentDateTime + ".xlsx";
                    string filePath = "";
                    System.Windows.Forms.SaveFileDialog dialog = new System.Windows.Forms.SaveFileDialog();
                    dialog.Filter = "Excel | *.xlsx | Excel 2003 | *.xls";
                    dialog.FileName = defaultFileName;
                    dialog.CheckPathExists = true;

                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        filePath = dialog.FileName;
                        string json = File.ReadAllText(path.Model);
                        if (json.Length > 10)
                        {
                            JArray jsonArray = JArray.Parse(json);
                            Excel.Export_Excecl(jsonArray, filePath);
                        }
                    }
                    if (string.IsNullOrEmpty(filePath))
                    {
                        MessageBox.Show("Đường dẫn xuất file không hợp lệ");
                        return;
                    }
                    //
                    string file_import = "";
                    System.Windows.Forms.OpenFileDialog import = new System.Windows.Forms.OpenFileDialog();
                    import.Title = "Chọn tệp Excel";
                    import.Filter = "Tệp Excel (*.xlsx)|*.xlsx|Tất cả các tệp (*.*)|*.*";
                    import.FilterIndex = 1;
                    import.RestoreDirectory = true;

                    if (import.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        file_import = import.FileName;
                        Excel.Import_Excel(file_import);
                    }
                    List<Items> items = new List<Items>();
                    int index = 1;
                    string List_Show = File.ReadAllText(path.Model);
                    if (List_Show.Length > 0)
                    {
                        JArray List_Show_array = JArray.Parse(List_Show);
                        foreach (JObject obj in List_Show_array)
                        {
                            items.Add(new Items { STT = index, Model = (string)obj["Model"], Truc = (string)obj["Truc"], Roto = (string)obj["Roto"] });
                            index++;
                        }
                        List_Models.ItemsSource = items;
                    }
                    SetTextBoxToZero();
                    txb_Truc.Text = "";
                    txb_Roto.Text = "";
                    txb_Model.Text = "";
                    MessageBox.Show("Hoàn Thành Nhập Data Model");
                }
                catch
                {
                    MessageBox.Show("Lỗi khi nhập file dữ liệu model, vui lòng kiểm tra lại data");
                }
            }
            else MessageBox.Show("Vui Lòng Đăng Nhập");
        }

        
    }
}
