using System;
using OfficeOpenXml;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;
using OfficeOpenXml.Table;
using OfficeOpenXml.Style;
using Newtonsoft.Json;
using System.Text.Json;
using System.Windows.Shapes;
using System.Windows;

namespace Robot_3_Axis_UI.Class
{
    
    public class Excel
    {
        public static void Export_Excecl(JArray data_model, string fileName)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                foreach (JObject obj in data_model)
                {
                    JArray Pos_Roto_0 = (JArray)obj["Pos_Roto1"];
                    JArray Pos_Roto_1 = (JArray)obj["Pos_Roto2"];
                    JArray Pos_Roto_2 = (JArray)obj["Pos_Roto3"];
                    JArray Pos_Roto_3 = (JArray)obj["Pos_Roto4"];
                    JArray Pos_Roto_4 = (JArray)obj["Pos_Roto5"];
                    JArray Pos_Roto_5 = (JArray)obj["Pos_Roto6"];
                    JArray Pos_Roto_6 = (JArray)obj["Pos_Roto7"];

                    JArray Pos_Truc_0 = (JArray)obj["Pos_Truc1"];
                    JArray Pos_Truc_1 = (JArray)obj["Pos_Truc2"];
                    JArray Pos_Truc_2 = (JArray)obj["Pos_Truc3"];
                    JArray Pos_Truc_3 = (JArray)obj["Pos_Truc4"];
                    JArray Pos_Truc_4 = (JArray)obj["Pos_Truc5"];
                    JArray Pos_Truc_5 = (JArray)obj["Pos_Truc6"];

                    JArray Pos_Tha_CumKtr = (JArray)obj["Pos_Tha_Cum_Ktr"];
                    JArray Pos_Nang_Kep_CumKtr = (JArray)obj["Pos_Nang_Kep_Cum_Ktr"];


                    JArray Pos_Gap_CumLap = (JArray)obj["Pos_Gap_Cum_Lap"];
                    JArray Pos_Lap_CumLap = (JArray)obj["Pos_Lap_Cum_Lap"];
                    JArray Pos_Nhan_Truc = (JArray)obj["Pos_Nhan_Truc"];
                    JArray Pos_Nang_Kep_CumLap = (JArray)obj["Pos_Nang_Kep_Cum_Lap"];

                    JArray Pos_Tha_BanXoay = (JArray)obj["Pos_Tha_Ban_Xoay"];

                    var worksheet = package.Workbook.Worksheets.Add((string)obj["Roto"] + "_" + (string)obj["Truc"]);
                    worksheet.Cells["A1"].Value = "Model";
                    worksheet.Cells["B1"].Value = (string)obj["Model"];
                    //
                    worksheet.Cells["A2"].Value = "TrucID";
                    worksheet.Cells["B2"].Value = (string)obj["Truc"];
                    //
                    worksheet.Cells["A3"].Value = "RotoID";
                    worksheet.Cells["B3"].Value = (string)obj["Roto"];
                    //
                    worksheet.Cells["B4"].Value = "X";
                    worksheet.Cells["C4"].Value = "Y";
                    worksheet.Cells["D4"].Value = "Z";
                    //
                    worksheet.Cells["A5"].Value = "Roto 1";
                    worksheet.Cells["B5"].Value = Pos_Roto_0[0].ToString();
                    worksheet.Cells["C5"].Value = Pos_Roto_0[1].ToString();
                    worksheet.Cells["D5"].Value = Pos_Roto_0[2].ToString();
                    //
                    worksheet.Cells["A6"].Value = "Roto 2";
                    worksheet.Cells["B6"].Value = Pos_Roto_1[0].ToString();
                    worksheet.Cells["C6"].Value = Pos_Roto_1[1].ToString();
                    worksheet.Cells["D6"].Value = Pos_Roto_1[2].ToString();
                    //
                    worksheet.Cells["A7"].Value = "Roto 3";
                    worksheet.Cells["B7"].Value = Pos_Roto_2[0].ToString();
                    worksheet.Cells["C7"].Value = Pos_Roto_2[1].ToString();
                    worksheet.Cells["D7"].Value = Pos_Roto_2[2].ToString();
                    //
                    worksheet.Cells["A8"].Value = "Roto 4";
                    worksheet.Cells["B8"].Value = Pos_Roto_3[0].ToString();
                    worksheet.Cells["C8"].Value = Pos_Roto_3[1].ToString();
                    worksheet.Cells["D8"].Value = Pos_Roto_3[2].ToString();
                    //
                    worksheet.Cells["A9"].Value = "Roto 5";
                    worksheet.Cells["B9"].Value = Pos_Roto_4[0].ToString();
                    worksheet.Cells["C9"].Value = Pos_Roto_4[1].ToString();
                    worksheet.Cells["D9"].Value = Pos_Roto_4[2].ToString();
                    //
                    worksheet.Cells["A10"].Value = "Trục 1";
                    worksheet.Cells["B10"].Value = Pos_Truc_0[0].ToString();
                    worksheet.Cells["C10"].Value = Pos_Truc_0[1].ToString();
                    worksheet.Cells["D10"].Value = Pos_Truc_0[2].ToString();
                    //
                    worksheet.Cells["A11"].Value = "Trục 2";
                    worksheet.Cells["B11"].Value = Pos_Truc_1[0].ToString();
                    worksheet.Cells["C11"].Value = Pos_Truc_1[1].ToString();
                    worksheet.Cells["D11"].Value = Pos_Truc_1[2].ToString();
                    //
                    worksheet.Cells["A12"].Value = "Trục 3";
                    worksheet.Cells["B12"].Value = Pos_Truc_2[0].ToString();
                    worksheet.Cells["C12"].Value = Pos_Truc_2[1].ToString();
                    worksheet.Cells["D12"].Value = Pos_Truc_2[2].ToString();
                    //
                    worksheet.Cells["A13"].Value = "Trục 4";
                    worksheet.Cells["B13"].Value = Pos_Truc_3[0].ToString();
                    worksheet.Cells["C13"].Value = Pos_Truc_3[1].ToString();
                    worksheet.Cells["D13"].Value = Pos_Truc_3[2].ToString();
                    //
                    worksheet.Cells["A14"].Value = "Trục 5";
                    worksheet.Cells["B14"].Value = Pos_Truc_4[0].ToString();
                    worksheet.Cells["C14"].Value = Pos_Truc_4[1].ToString();
                    worksheet.Cells["D14"].Value = Pos_Truc_4[2].ToString();
                    //
                    worksheet.Cells["A15"].Value = "Roto 6";
                    worksheet.Cells["B15"].Value = Pos_Roto_5[0].ToString();
                    worksheet.Cells["C15"].Value = Pos_Roto_5[1].ToString();
                    worksheet.Cells["D15"].Value = Pos_Roto_5[2].ToString();
                    //
                    worksheet.Cells["A16"].Value = "Roto 7";
                    worksheet.Cells["B16"].Value = Pos_Roto_6[0].ToString();
                    worksheet.Cells["C16"].Value = Pos_Roto_6[1].ToString();
                    worksheet.Cells["D16"].Value = Pos_Roto_6[2].ToString();
                    //
                    worksheet.Cells["A17"].Value = "Trục 6";
                    worksheet.Cells["B17"].Value = Pos_Truc_5[0].ToString();
                    worksheet.Cells["C17"].Value = Pos_Truc_5[1].ToString();
                    worksheet.Cells["D17"].Value = Pos_Truc_5[2].ToString();
                    //
                    worksheet.Cells["A18"].Value = "Cụm Kiểm Tra";
                    //
                    worksheet.Cells["A19"].Value = "Thả";
                    worksheet.Cells["B19"].Value = Pos_Tha_CumKtr[0].ToString();
                    worksheet.Cells["C19"].Value = Pos_Tha_CumKtr[1].ToString();
                    worksheet.Cells["D19"].Value = Pos_Tha_CumKtr[2].ToString();
                    worksheet.Cells["A20"].Value = "Nâng";
                    worksheet.Cells["B20"].Value = Pos_Nang_Kep_CumKtr[0].ToString();
                    worksheet.Cells["C20"].Value = "Tay Kẹp";
                    worksheet.Cells["D20"].Value = Pos_Nang_Kep_CumKtr[1].ToString();
                    //
                    worksheet.Cells["A21"].Value = "Cụm Lắp";
                    worksheet.Cells["A22"].Value = "Gắp";
                    worksheet.Cells["B22"].Value = Pos_Gap_CumLap[0].ToString();
                    worksheet.Cells["C22"].Value = Pos_Gap_CumLap[1].ToString();
                    worksheet.Cells["D22"].Value = Pos_Gap_CumLap[2].ToString();
                    worksheet.Cells["A23"].Value = "Lắp";
                    worksheet.Cells["B23"].Value = Pos_Lap_CumLap[0].ToString();
                    worksheet.Cells["C23"].Value = Pos_Lap_CumLap[1].ToString();
                    worksheet.Cells["D23"].Value = Pos_Lap_CumLap[2].ToString();
                    worksheet.Cells["A24"].Value = "Nhấn";
                    worksheet.Cells["B24"].Value = Pos_Nhan_Truc[0].ToString();
                    worksheet.Cells["C24"].Value = Pos_Nhan_Truc[1].ToString();
                    worksheet.Cells["D24"].Value = Pos_Nhan_Truc[2].ToString();
                    worksheet.Cells["A25"].Value = "Nâng";
                    worksheet.Cells["B25"].Value = Pos_Nang_Kep_CumLap[0].ToString();
                    worksheet.Cells["C25"].Value = "Tay Kẹp";
                    worksheet.Cells["D25"].Value = Pos_Nang_Kep_CumLap[1].ToString();
                    //
                    worksheet.Cells["A26"].Value = "Bàn Nung";
                    worksheet.Cells["A27"].Value = "Thả";
                    worksheet.Cells["B27"].Value = Pos_Tha_BanXoay[0].ToString();
                    worksheet.Cells["C27"].Value = Pos_Tha_BanXoay[1].ToString();
                    worksheet.Cells["D27"].Value = Pos_Tha_BanXoay[2].ToString();
                    //
                    worksheet.Cells["A28"].Value = "Offset Thả Roto";
                    worksheet.Cells["B28"].Value = (string)obj["Offset_Tha_Roto"];
                    //
                    worksheet.Cells["A29"].Value = "Nhiệt Độ Nung";
                    worksheet.Cells["B29"].Value = (string)obj["Temperature"];
                    //
                    worksheet.Cells["A30"].Value = "Công Suất Nung";
                    worksheet.Cells["B30"].Value = (string)obj["Cong_Suat_Nung"];
                    //
                    worksheet.Cells["A31"].Value = "Thời Gian Thổi";
                    worksheet.Cells["B31"].Value = (string)obj["Time_Blow"];
                    //
                    worksheet.Cells["A32"].Value = "Nhiệt Độ Làm Mát";
                    worksheet.Cells["B32"].Value = (string)obj["Temperature_LamMat"];
                    //
                    worksheet.Cells["A33"].Value = "Z An toàn";
                    worksheet.Cells["B33"].Value = (string)obj["Z_Safety"];
                    //
                    worksheet.Cells["A34"].Value = "Vị trí lắp trục";
                    worksheet.Cells["B34"].Value = (string)obj["Pos_LapTruc"];
                    //
                    worksheet.Cells["A35"].Value = "Loại Trục";
                    worksheet.Cells["B35"].Value = (string)obj["LoaiTruc"];
                    //
    }
                package.SaveAs(new FileInfo(fileName));
            }
            using (var package = new ExcelPackage(new FileInfo(fileName)))
            {
                foreach (var worksheet in package.Workbook.Worksheets)
                {

                    worksheet.Column(1).Width = 15;
                    worksheet.Column(1).Style.Font.Bold = true;
                    worksheet.Cells["C20:C20"].Style.Font.Bold = true;
                    worksheet.Cells["C25:C25"].Style.Font.Bold = true;
                    worksheet.Row(4).Style.Font.Bold = true;
                    worksheet.Row(4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Row(4).Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                    worksheet.Cells["B1:D1"].Merge = true;
                    worksheet.Cells["B1:D1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells["B1:D1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    worksheet.Cells["B2:D2"].Merge = true;
                    worksheet.Cells["B2:D2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells["B2:D2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    worksheet.Cells["B3:D3"].Merge = true;
                    worksheet.Cells["B3:D3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells["B3:D3"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    worksheet.Cells["A18:D18"].Merge = true;
                    worksheet.Cells["A18:D18"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells["A18:D18"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    worksheet.Cells["A21:D21"].Merge = true;
                    worksheet.Cells["A21:D21"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells["A21:D21"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    worksheet.Cells["A26:D26"].Merge = true;
                    worksheet.Cells["A26:D26"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells["A26:D26"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    foreach (var cell in worksheet.Cells["B5:D17"])
                    {
                        float floatValue;
                        if (float.TryParse(cell.Text, out floatValue))
                        {
                            cell.Value = floatValue;
                        }
                    }
                    foreach (var cell in worksheet.Cells["B19:D19"])
                    {
                        float floatValue;
                        if (float.TryParse(cell.Text, out floatValue))
                        {
                            cell.Value = floatValue;
                        }
                    }
                    foreach (var cell in worksheet.Cells["B20:B20"])
                    {
                        float floatValue;
                        if (float.TryParse(cell.Text, out floatValue))
                        {
                            cell.Value = floatValue;
                        }
                    }
                    foreach (var cell in worksheet.Cells["D20:D20"])
                    {
                        float floatValue;
                        if (float.TryParse(cell.Text, out floatValue))
                        {
                            cell.Value = floatValue;
                        }
                    }
                    foreach (var cell in worksheet.Cells["B22:D24"])
                    {
                        float floatValue;
                        if (float.TryParse(cell.Text, out floatValue))
                        {
                            cell.Value = floatValue;
                        }
                    }
                    foreach (var cell in worksheet.Cells["B25:B25"])
                    {
                        float floatValue;
                        if (float.TryParse(cell.Text, out floatValue))
                        {
                            cell.Value = floatValue;
                        }
                    }
                    foreach (var cell in worksheet.Cells["D25:D25"])
                    {
                        float floatValue;
                        if (float.TryParse(cell.Text, out floatValue))
                        {
                            cell.Value = floatValue;
                        }
                    }
                    foreach (var cell in worksheet.Cells["B27:D31"])
                    {
                        float floatValue;
                        if (float.TryParse(cell.Text, out floatValue))
                        {
                            cell.Value = floatValue;
                        }
                    }
                    //
                    ExcelRangeBase dataRange = worksheet.Cells[worksheet.Dimension.Address];

                    // Kẻ khung cho phạm vi dữ liệu
                    dataRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    dataRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    dataRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    dataRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }
                package.Save();
            }
        }
        static List<float> ReadFloatListFromExcel(ExcelWorksheet worksheet, string range)
        {
            List<float> result = new List<float>();
            foreach (var cell in worksheet.Cells[range])
            {
                float floatValue;
                if (float.TryParse(cell.Text, out floatValue))
                {
                    result.Add(floatValue);
                }
            }
            return result;
        }
        public static void Import_Excel(string fileimport)
        {
            Path path = new Path();
            string temp = "";
            File.WriteAllText(path.Model, temp);
            FileInfo file = new FileInfo(fileimport);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage package = new ExcelPackage(file))
            {
                // Lặp qua tất cả các trang (worksheets) trong tệp Excel
                foreach (ExcelWorksheet worksheet in package.Workbook.Worksheets)
                {
                    List_Model listModel = new List_Model();

                    // Gán dữ liệu từ Excel vào thuộc tính của đối tượng
                    listModel.Model = worksheet.Cells["B1"].Value.ToString();
                    listModel.Truc = worksheet.Cells["B2"].Value.ToString();
                    listModel.Roto = worksheet.Cells["B3"].Value.ToString();

                    // Gán giá trị cho thuộc tính Temperature
                    float temperature;
                    if (float.TryParse(worksheet.Cells["B29"].Value.ToString(), out temperature))
                    {
                        listModel.Temperature = temperature;
                    }

                    // Gán giá trị cho các danh sách dữ liệu Pos_Roto và Pos_Truc
                    listModel.Pos_Roto1 = ReadFloatListFromExcel(worksheet, "B5:D5");
                    listModel.Pos_Roto2 = ReadFloatListFromExcel(worksheet, "B6:D6");
                    listModel.Pos_Roto3 = ReadFloatListFromExcel(worksheet, "B7:D7");
                    listModel.Pos_Roto4 = ReadFloatListFromExcel(worksheet, "B8:D8");
                    listModel.Pos_Roto5 = ReadFloatListFromExcel(worksheet, "B9:D9");
                    listModel.Pos_Truc1 = ReadFloatListFromExcel(worksheet, "B10:D10");
                    listModel.Pos_Truc2 = ReadFloatListFromExcel(worksheet, "B11:D11");
                    listModel.Pos_Truc3 = ReadFloatListFromExcel(worksheet, "B12:D12");
                    listModel.Pos_Truc4 = ReadFloatListFromExcel(worksheet, "B13:D13");
                    listModel.Pos_Truc5 = ReadFloatListFromExcel(worksheet, "B14:D14");
                    listModel.Pos_Roto6 = ReadFloatListFromExcel(worksheet, "B15:D15");
                    listModel.Pos_Roto7 = ReadFloatListFromExcel(worksheet, "B16:D16");
                    listModel.Pos_Truc6 = ReadFloatListFromExcel(worksheet, "B17:D17");

                    // Gán giá trị cho các danh sách dữ liệu khác
                    listModel.Pos_Tha_Cum_Ktr = ReadFloatListFromExcel(worksheet, "B19:D19");
                    listModel.Pos_Nang_Kep_Cum_Ktr = ReadFloatListFromExcel(worksheet, "B20:D20");
                    listModel.Pos_Gap_Cum_Lap = ReadFloatListFromExcel(worksheet, "B22:D22");
                    listModel.Pos_Lap_Cum_Lap = ReadFloatListFromExcel(worksheet, "B23:D23");
                    listModel.Pos_Nhan_Truc = ReadFloatListFromExcel(worksheet, "B24:D24");
                    listModel.Pos_Nang_Kep_Cum_Lap = ReadFloatListFromExcel(worksheet, "B25:D25");
                    listModel.Pos_Tha_Ban_Xoay = ReadFloatListFromExcel(worksheet, "B27:D27");
                    listModel.Offset_Tha_Roto = float.Parse(worksheet.Cells["B28"].Value.ToString());
                    listModel.Cong_Suat_Nung = float.Parse(worksheet.Cells["B30"].Value.ToString());
                    listModel.Time_Blow = float.Parse(worksheet.Cells["B31"].Value.ToString());
                    listModel.Temperature_LamMat = float.Parse(worksheet.Cells["B32"].Value.ToString());
                    listModel.Z_Safety = float.Parse(worksheet.Cells["B33"].Value.ToString());
                    listModel.Pos_LapTruc = float.Parse(worksheet.Cells["B34"].Value.ToString());
                    listModel.LoaiTruc = float.Parse(worksheet.Cells["B35"].Value.ToString());
                    string list_Model_Json = JsonConvert.SerializeObject(listModel);
                    
                    string json = File.ReadAllText(path.Model);
                    if(json.Length>0)
                    {
                        json = json.Remove(json.Length - 1);
                        json = json + "," + list_Model_Json + "]";
                        File.WriteAllText(path.Model, json);
                    }   
                    else
                    {
                        string json_;
                        json_ = "[" + list_Model_Json + "]";
                        File.WriteAllText(path.Model, json_);
                    }    
                    
                }
            }
        }
    }
}
