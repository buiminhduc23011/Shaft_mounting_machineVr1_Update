using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Robot_3_Axis_UI
{
    public class Read_PLC
    {
        Path path = new Path();
        string Ip_Port_Host;
        private DispatcherTimer timer;
        static dynamic data_;
        public static bool flag = false;
        HttpClient client = new HttpClient();
        public void Start()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += Timer_Tick;
            timer.Start();
            string json = File.ReadAllText(path.Setting);
            var data_Setting = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            Ip_Port_Host = data_Setting["Ip_Port_Ser"];

        }
        public void Stop()
        {
            try
            {
                timer.Stop();
                timer.Tick -= Timer_Tick;
                timer = null;
            }
            catch
            {

            }
        }
        public async void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                string response = await client.GetStringAsync("http://" + Ip_Port_Host + "/api/data");
                data_ = JsonConvert.DeserializeObject<dynamic>(response);
                flag = true;
            }
            catch
            {
                flag = false;
            }
        }
        public static dynamic Data
        {
            get { return data_; }
        }
    }
}
