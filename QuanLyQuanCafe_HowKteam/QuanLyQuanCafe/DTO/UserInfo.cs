using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class UserInfo
    {
        public UserInfo(string name,float timeCome,float timeLeave)
        {
            this.Name = name;
            this.TimeCome = timeCome;
            this.TimeLeave = timeLeave;
        }
        public UserInfo(DataRow row)
        {
            this.Name = (string)row["name"];
            this.TimeCome = (float)Convert.ToDouble(row["timeCome"].ToString());
            this.TimeLeave = (float)Convert.ToDouble(row["timeLeave"].ToString()); ;
        }

        private string name;
        private float timeCome;
        private float timeLeave;

        public string Name 
        {
            get { return name; }
            set { name = value; } 
        }

        public float TimeCome
        {
            get { return timeCome; }
            set { timeCome = value; }
        }
        public float TimeLeave
        {
            get { return timeLeave; }
            set { timeLeave = value; }
        }
    }
}
