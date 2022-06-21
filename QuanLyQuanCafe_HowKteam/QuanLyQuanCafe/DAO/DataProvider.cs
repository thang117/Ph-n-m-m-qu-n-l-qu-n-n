using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class DataProvider
    {
        private static DataProvider instance; // ctrl + R + E

        public static DataProvider Instance 
        {
            get { if(instance==null) instance=new DataProvider(); return DataProvider.instance; }
            private set { DataProvider.instance = value; } 
        }

        private DataProvider() { }

        private string connectionSTR = @"Data Source=DESKTOP-J3QR081\SQLEXPRESS;Initial Catalog=QuanLyQuanCafe;Integrated Security=True"; //chuỗi kết nối

       

        public DataTable ExecuteQuery(string query, object[] paramater = null)
        {
            DataTable data = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionSTR)) // tạo kết nối
            {
                connection.Open();// mở kết nối

                SqlCommand command = new SqlCommand(query, connection); // code thực thi câu lệnh truy vấn trên connection đã tạo bên trên       

                if (paramater != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (var item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, paramater[i]);
                            i++;
                        }
                    }
                }

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(data);

                connection.Close();// đóng kết nối 

            }
            return data;
        }
        public int ExecuteNonQuery(string query, object[] paramater = null)
        {
            int data = 0;
            using (SqlConnection connection = new SqlConnection(connectionSTR)) // tạo kết nối
            {
                connection.Open();// mở kết nối

                SqlCommand command = new SqlCommand(query, connection); // code thực thi câu lệnh truy vấn trên connection đã tạo bên trên       

                if (paramater != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (var item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, paramater[i]);
                            i++;
                        }
                    }
                }

                data = command.ExecuteNonQuery();

                connection.Close();// đóng kết nối 

            }
            return data;
        }
        public object ExecuteScalar(string query, object[] paramater = null)
        {
            object data = 0;
            using (SqlConnection connection = new SqlConnection(connectionSTR)) // tạo kết nối
            {
                connection.Open();// mở kết nối

                SqlCommand command = new SqlCommand(query, connection); // code thực thi câu lệnh truy vấn trên connection đã tạo bên trên       

                if (paramater != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (var item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, paramater[i]);
                            i++;
                        }
                    }
                }

                data = command.ExecuteScalar();

                connection.Close();// đóng kết nối 

            }
            return data;
        }
    }
}
