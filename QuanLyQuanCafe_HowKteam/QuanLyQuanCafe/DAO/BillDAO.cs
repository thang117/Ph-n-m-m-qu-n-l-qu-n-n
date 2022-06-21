using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class BillDAO
    {
        private static BillDAO instance;

        public static BillDAO Instance
        {
            get { if (instance == null) instance = new BillDAO(); return BillDAO.instance; }
            private set { BillDAO.instance = value; }
        }

        private BillDAO() { }

        /// <summary>
        /// thành công: bill.id
        /// thất bại: -1
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetUnCheckBillByTableID(int id)
        {

            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.Bill WHERE idTable ="+ id +" AND status = 0");
            
            if(data.Rows.Count > 0)
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.ID;

            }

            return -1;
        }

        public void CheckOut(int id,int discount,float totalPrice)
        {
            string query = "UPDATE dbo.Bill SET dateCheckOut = GETDATE(), status = 1," + " discount = " + discount +", totalPrice = " + totalPrice + " WHERE id = " + id; // giảm giá và đổi trạng thái của hóa đon sau khi thanh toán 
            DataProvider.Instance.ExecuteNonQuery(query);
                
        }

        public void InsertBill(int id)
        {
            DataProvider.Instance.ExecuteNonQuery("exec USP_InsertBill @idTable",new object[] {id});

        }

        public DataTable GetListBillByDate(DateTime CheckIn, DateTime CheckOut)
        {
             return DataProvider.Instance.ExecuteQuery("exec USP_GetListBillByDate @checkIn , @checkOut", new object[]{CheckIn, CheckOut});
        }

        public DataTable GetListBillByDateAndPage(DateTime CheckIn, DateTime CheckOut , int page)
        {
            return DataProvider.Instance.ExecuteQuery("exec USP_GetListBillByDateAndPage @checkIn , @checkOut , @page ", new object[] { CheckIn, CheckOut, page });
        }

        public int GetNumBillByDate(DateTime CheckIn, DateTime CheckOut)
        {
            return (int)DataProvider.Instance.ExecuteScalar("exec USP_GetNumBillByDate @checkIn , @checkOut", new object[] { CheckIn, CheckOut }); // lấy tổng số bill đang có
        }

        public int GetMaxIDBill()
        {
            try
            {
                return (int)DataProvider.Instance.ExecuteScalar("SELECT MAX(id) FROM dbo.Bill");
            }
            catch
            {
                return 1;
            }
        }
    }
}
