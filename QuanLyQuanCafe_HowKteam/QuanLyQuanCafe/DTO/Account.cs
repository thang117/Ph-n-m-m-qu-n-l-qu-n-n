using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class Account
    {
        public Account(string userName,string displayName, int type, string passWord = null)
        {
            this.UserName = userName;
            this.DisplayName = displayName;
            this.Type = type;
            this.PassWord = passWord;
        }

        public Account(DataRow row)
        {
            this.UserName = (string)row["userName"];
            this.DisplayName = (string)row["displayName"];
            this.Type = (int)row["type"];
            this.PassWord = (string)row["passWord"];
        }

        private string userName;

        private string displayName;

        private string passWord;

        private int type;

        public string UserName 
        {
            get { return userName; }
            set { userName = value; } 
        }

        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }

        public string PassWord
        {
            get { return passWord; }
            set { passWord = value; }
        }

        public int Type
        {
            get { return type; }
            set { type = value; }
        }
    }
}
