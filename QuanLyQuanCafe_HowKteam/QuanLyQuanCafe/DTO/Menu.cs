using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class Menu
    {
        public Menu(string foodName,int count,float price,float totalPrice=0)
        {
            this.FoodName = foodName;
            this.Count = count;
            this.Price = price;
            this.TotalPrice = totalPrice;
        }

        public Menu(DataRow row)
        {
            this.FoodName = (string)row["Name"];
            this.Count = (int)row["count"];
            this.Price = (float)Convert.ToDouble(row["price"].ToString());          //đưa giá trị float của SQL sang string của C#
            this.TotalPrice = (float)Convert.ToDouble(row["totalPrice"].ToString());// sau đó chuyển thành double rồi convert thành float
        }


        private string foodName;

        private int count;

        private float price;

        private float totalPrice;

        public string FoodName
        {
            get { return foodName; }
            set { foodName = value; }
        }

        public int Count
        {
            get { return count; }
            set { count = value; }
        }

        public float Price
        {
            get { return price; }
            set { price = value; }
        }

        public float TotalPrice
        {
            get { return totalPrice; }
            set { totalPrice = value; }
        }
    }
}
