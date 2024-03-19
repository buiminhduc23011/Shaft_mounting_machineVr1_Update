using Newtonsoft.Json.Linq;
using Robot_3_Axis_UI.Class;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Robot_3_Axis_UI
{
    /// <summary>
    /// Interaction logic for History_Log_Screen.xaml
    /// </summary>
    public partial class History_Log_Screen : UserControl
    {
        Path path = new Path();
        private DispatcherTimer timer;
        dynamic data;
        public History_Log_Screen()
        {
            InitializeComponent();

        }
        private void History_Screen_Loaded(object sender, RoutedEventArgs e)
        {

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;
            timer.Start();
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
                //
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
        private void History_Screen_Unloaded(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            timer.Tick -= Timer_Tick;
            timer = null;
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            data = Read_PLC.Data;
            if (data != null)
            {
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
                    // Đảo ngược danh sách items
                    items.Reverse();

                    // Cập nhật lại giá trị thuộc tính STT của từng đối tượng
                    for (int i = 0; i < items.Count; i++)
                    {
                        items[i].STT = i + 1;
                    }
                    List_Error.ItemsSource = items;
                }
            }
            catch { }
        }
    }
}
