using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class UserInfoDAO
    {
        private static UserInfoDAO instance;

        public static UserInfoDAO Instance
        {
            get { if (instance == null) instance = new UserInfoDAO(); return instance; }
            private set { instance = value; }
        }

        private UserInfoDAO() { }

        public DataTable GetListUserInfo()
        {
            return DataProvider.Instance.ExecuteQuery("SELECT * FROM UserInfo ");
        }

        public int CountUserInfo()
        {
            try
            {
                return (int)DataProvider.Instance.ExecuteScalar("SELECT COUNT(id) FROM dbo.UserInfo");
            }
            catch
            {
                return 0;
            }
        }
        public bool InsertUserinfo(string userName, float timeCome, float timeLeave)
        {
            string query = string.Format(" INSERT dbo.UserInfo ( name , timeCome , timeLeave ) VALUES( N'{0}', {1} , {2} ) ", userName, timeCome ,timeLeave);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool UpdateUserinfo(int id, string userName, float timeCome, float timeLeave)
        {
            string query = string.Format(" UPDATE dbo.UserInfo SET name = N'{0}' , timeCome = '{1}' , timeLeave = '{2}' WHERE id = {3} ", userName, timeCome, timeLeave, id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool DeleteUserinfo(int id)
        {
            string query = string.Format(" DELETE dbo.UserInfo WHERE id = {0} ", id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
    }
}
