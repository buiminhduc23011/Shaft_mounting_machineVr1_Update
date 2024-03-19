using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot_3_Axis_UI
{
    public class List_Model
    {
        public string Model { get; set; }
        public string Truc { get; set; }
        public string Roto { get; set; }
        public float Temperature { get; set; }
        public List<float> Pos_Roto1 { get; set; }
        public List<float> Pos_Roto2 { get; set; }
        public List<float> Pos_Roto3 { get; set; }
        public List<float> Pos_Roto4 { get; set; }
        public List<float> Pos_Roto5 { get; set; }
        public List<float> Pos_Roto6 { get; set; }
        public List<float> Pos_Roto7 { get; set; }
        public List<float> Pos_Truc1 { get; set; }
        public List<float> Pos_Truc2 { get; set; }
        public List<float> Pos_Truc3 { get; set; }
        public List<float> Pos_Truc4 { get; set; }
        public List<float> Pos_Truc5 { get; set; }
        public List<float> Pos_Truc6 { get; set; }
        public List<float> Pos_Tha_Cum_Ktr { get; set; }
        public List<float> Pos_Nang_Kep_Cum_Ktr { get; set; }
        public List<float> Pos_Gap_Cum_Lap { get; set; }
        public List<float> Pos_Lap_Cum_Lap { get; set; }
        public List<float> Pos_Nhan_Truc { get; set; }
        public List<float> Pos_Nang_Kep_Cum_Lap { get; set; }
        public List<float> Pos_Tha_Ban_Xoay { get; set; }
        public float Offset_Tha_Roto { get; set; }
        public float Cong_Suat_Nung { get; set; }
        public float Time_Blow { get; set; }
        public float Temperature_LamMat { get; set; }
        public float Z_Safety { get; set; }
        public float Pos_LapTruc { get; set; }
        public float LoaiTruc { get; set; }

    }
}
