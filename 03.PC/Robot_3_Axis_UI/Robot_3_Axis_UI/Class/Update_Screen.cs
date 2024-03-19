using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Robot_3_Axis_UI
{
    public class Update_Screen
    {
        public void Roto_Status(Label lb, string Status)
        {
            if (Status == "1")
            {
                lb.Content = "Đang Sản Xuất";
            }
            else if (Status == "2")
            {
                lb.Content = "Hoàn Thành";
            }
            else lb.Content = "Chưa Sản Xuất";
        }
        public void Machine_Status(Label lb, string Status)
        {
            if (Status == "true")
            {
                lb.Background = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            }
            else lb.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }
        public void Roto_Result(Label lb, string Status)
        {
            if(Status == "1")
            {
                lb.Content = "OK";
                lb.Foreground = new SolidColorBrush(Color.FromRgb(0,255,0));
            }
            else if (Status == "2")
            {
                lb.Content = "NG";
                lb.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }
            else
            {
                lb.Content = "_____";
                lb.Foreground = new SolidColorBrush(Color.FromRgb(0,0,0));
            }
        }
        public void bt_Colors_Status_1(Button button, string Status)
        {
            if (Status == "true")
            {
                button.Background = new SolidColorBrush(Color.FromRgb(100, 149, 237));
            }
            else
            {
                button.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
        }
        public void bt_Colors_Status_0(Button button, string Status)
        {
            if (Status == "false")
            {
                button.Background = new SolidColorBrush(Color.FromRgb(100, 149, 237));
            }
            else
            {
                button.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
        }
        public void IO_True(Button button, string Status)
        {
            if (Status == "true")
            {
                button.Background = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            }
            else
            {
                button.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
        }
        public void IO_False(Button button, string Status)
        {
            if (Status == "false")
            {
                button.Background = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            }
            else
            {
                button.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
        }
        public void bt_Green_1(Button button, int Status)
        {
            if (Status == 1)
            {
                button.Background = new SolidColorBrush(Color.FromRgb(100, 149, 237));
            }
            else
            {
                button.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
        }
        public void bt_Blue_0(Button button, int Status)
        {
            if (Status == 0)
            {
                button.Background = new SolidColorBrush(Color.FromRgb(100, 149, 237));
            }
            else
            {
                button.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
        }
        public void bt_Blue_1(Button button, int Status)
        {
            if (Status == 1)
            {
                button.Background = new SolidColorBrush(Color.FromRgb(100, 149, 237));
            }
            else
            {
                button.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
        }
        public void bt_Green_2(Button button, int Status)
        {
            if (Status == 2)
            {
                button.Background = new SolidColorBrush(Color.FromRgb(100, 149, 237));
            }
            else
            {
                button.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
        }
        public void Inout(Button button, int Status)
        {
            if (Status == 1)
            {
                button.Background = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            }
            else
            {
                button.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
        }
        public void label(Label label, int Status)
        {
            if (Status == 1)
            {
                label.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            }
            else
            {
                label.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            }
        }

    }
}
