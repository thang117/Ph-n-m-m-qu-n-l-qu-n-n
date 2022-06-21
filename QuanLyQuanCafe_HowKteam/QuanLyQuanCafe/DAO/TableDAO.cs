using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class TableDAO
    {
        private static TableDAO instance;

        public static TableDAO Instance
        {
            get { if (instance == null) instance = new TableDAO(); return TableDAO.instance; }
            private set { TableDAO.instance = value; }
        }

        private TableDAO() { }

        public void SwitchTable(int id1,int id2)
        {
            DataProvider.Instance.ExecuteQuery("USP_SwitchTable @idTable1 , @idTable2 ",new object[] { id1,id2});
        }

        public List<Table> LoadTableList()
        {
            List<Table> tableList = new List<Table>();

            DataTable data = DataProvider.Instance.ExecuteQuery("USP_GetTableList");

            foreach(DataRow item in data.Rows)
            {
                Table table = new Table(item);
                tableList.Add(table);

            }
            return tableList;
        }
        public bool InsertTable(string name)
        {
            string query = string.Format(" INSERT dbo.TableFood ( name , status ) VALUES( N'{0}' , N'Trống' )", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool UpdateTable(int idTable, string name)
        {
            string query = string.Format(" UPDATE dbo.TableFood SET name = N'{0}'  WHERE id = {1}", name, idTable);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool DeleteTable(int idTable)
        {
            // phải xóa BillInfo và Bill trước rồi mới xóa foodcategory để ko lỗi ràng buộc
            string query = "exec USP_DeleteTable  @idTable ";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { idTable });

            return result > 0;
        }
    }
}
