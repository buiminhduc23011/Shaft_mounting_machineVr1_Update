using System;
using System.IO;

namespace Robot_3_Axis_UI
{
    public class Path
    {
        public string Setting = System.IO.Path.Combine("Path", "scnn.ini");
        public string Model = System.IO.Path.Combine("Path", "Model.json");
        public string History = System.IO.Path.Combine("Path", "History.json");
        public string List_Error = System.IO.Path.Combine("Path", "List_Error.ini");
        public string Maintenancen = System.IO.Path.Combine("Path", "Time_Maintenancen.ini");
        public string History_Maintenancen = System.IO.Path.Combine("Path", "History_Maintenancen.json");
        public string History_Check = System.IO.Path.Combine("Path", "History_Check.json");
        public string User_List = System.IO.Path.Combine("Path", "UserCredentials.json");
    }
}
