using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    class AccountDAO
    {
        private static AccountDAO instance;

        public static AccountDAO Instance 
        {
            get { if (instance == null) instance = new AccountDAO(); return instance; }
            private set { instance = value; }
        }

        private AccountDAO() { }


        public bool Login(string userName,string passWord)
        {
            byte[] temp = ASCIIEncoding.ASCII.GetBytes(passWord);
            byte[] hashData = new MD5CryptoServiceProvider().ComputeHash(temp);

            string hashPass = "";

            foreach(byte item in hashData)
            {
                hashPass += item;
            }
            // 1962026656160185351301320480154111117132155 là hash  md5 của 1
            //var list = hashData.ToString();
            //list.Reverse();

            string query = "USP_Login @userName , @passWord";

            DataTable result = DataProvider.Instance.ExecuteQuery(query,new object[] {userName,hashPass/*passWord*/});


            return result.Rows.Count > 0;
        }

        public bool UpdateAccount(string userName , string displayName , string password , string newpass)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("exec USP_UpdateAccount  @userName , @displayName , @password , @newPassword ", new object[]{userName, displayName, password, newpass });

            return result  > 0;
        }
        public DataTable GetListAccount()
        {
            return DataProvider.Instance.ExecuteQuery("SELECT UserName, DisplayName, Type FROM Account ");
        }
        public Account GetAccountByUserName(string userName)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.Account WHERE userName = '" + userName + "'");

            foreach(DataRow item in data.Rows)
            {
                return new Account(item);
            }
            return null;
        }

        public bool InsertAccount(string userName, string displayName, int type)
        {
            string query = string.Format(" INSERT dbo.Account ( userName , displayName , type , password ) VALUES( N'{0}', N'{1}' , {2} , N'{3}') ", userName, displayName, type, "1962026656160185351301320480154111117132155");
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool UpdateAccount( string userName, string displayName, int type)
        {
            string query = string.Format(" UPDATE dbo.Account SET  displayName = N'{0}' , type = {1} WHERE userName = N'{2}'", displayName,type,userName);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool DeleteAccount(string userName)
        {
     
            string query = string.Format("DELETE dbo.Account WHERE userName = N'{0}' ", userName);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool ResetPassword(string userName)
        {
            string query = string.Format("Update dbo.Account SET password = N'1962026656160185351301320480154111117132155' WHERE userName = N'{0}' ", userName);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
    }
}
