using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class FoodDAO
    {
        private static FoodDAO instance;

        public static FoodDAO Instance
        {
            get { if (instance == null) instance = new FoodDAO(); return FoodDAO.instance; }
            private set { FoodDAO.instance = value; }
        }

        private FoodDAO() { }

        public List<Food> GetFoodCategoryID(int id)
        {
            List<Food> list = new List<Food>();

            string query = "select * from Food where idCategory = " + id;
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach(DataRow item in data.Rows)
            {
                Food food = new Food(item);
                list.Add(food);

            }
            return list;
        }

        public List<Food> GetListFood()
        {
            List<Food> list = new List<Food>();
            string query = "SELECT * FROM dbo.Food ";
            //string query = " SELECT f.id[STT] , f.name [Tên món] , f.price[Giá] , f.idCategory[Mã loại]  FROM dbo.Food as f ";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Food food = new Food(item);
                list.Add(food);

            }
            return list;
        }

        public List<Food> SearchFoodByName(string name)
        {
            List<Food> list = new List<Food>();
            //like: so sánh gần đúng trong SQL
            // %{0}: vị trí của kí tự là {0} còn % là chuỗi trong trường hợp này thì nghĩa là vị trí cuối trong chuỗi
           /* string query = string.Format("SELECT * FROM dbo.Food WHERE name like N'%{0}%'", name);*///so sánh gần đúng kí tự nhập vào trong 1 chuỗi ở vị trí bất kì
            
            string query = string.Format("SELECT * FROM dbo.Food WHERE dbo.fuConvertToUnsign1(name) LIKE N'%' + dbo.fuConvertToUnsign1(N'{0}') + '%'", name);//tìm kiếm gần đúng bằng cách thay các kí tự có dấu thành ko dấu

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Food food = new Food(item);
                list.Add(food);

            }
            return list;
        }

        public bool InsertFood(string name,int id,float price)
        {
            string query =string.Format( " INSERT dbo.Food (name,idCategory,price) VALUES(N'{0}',{1},{2})",name,id,price);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool UpdateFood(int idFood,string name, int id, float price)
        {
            string query = string.Format(" UPDATE dbo.Food SET name = N'{0}', idCategory = {1} , price = {2} WHERE id = {3}", name, id, price,idFood);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool DeleteFood(int idFood)
        {
            BillInfoDAO.Instance.DeleteBillInfoByFoodID(idFood); // phải xóa billinfo trước rồi mới xóa idFood để ko lỗi ràng buộc
            string query = string.Format("DELETE dbo.Food WHERE id = {0} ",idFood);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool DeleteFoodByCategory(int idCategory)
        {
            string query = string.Format("DELETE dbo.Food WHERE idCategory = {0} ", idCategory);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
    }
}
