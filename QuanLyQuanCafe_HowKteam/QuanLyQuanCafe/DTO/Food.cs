using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class Food
    {
        public Food(int id,string name, float price,int categoryID)
        {
            this.ID = id;
            this.Name = name;
            this.CategoryID = categoryID;
            this.Price = price;
        }

        public Food(DataRow row)
        {
            this.ID = (int)row["id"];
            this.Name = (string)row["name"];
            this.Price = (float)Convert.ToDouble(row["price"].ToString());// để chuyển từ float của sql sang float của C#
            this.CategoryID = (int)row["idCategory"];
            //this.ID = (int)row["STT"];
            //this.Name = (string)row["Tên món"];
            //this.CategoryID = (int)row["Mã loại"];
            //this.Price = (float)Convert.ToDouble(row["Giá"].ToString());
        }

        private int iD;

        private string name;

        private int categoryID;

        private float price;
        public int ID
        {
            get { return iD; }
            set { iD = value; } 
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int CategoryID
        {
            get { return categoryID; }
            set { categoryID = value; }
        }

        public float Price
        {
            get { return price; }
            set { price = value; }
        }
    }
}
