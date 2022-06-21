using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QuanLyQuanCafe.fAccountProfile;

namespace QuanLyQuanCafe
{
    public partial class fTableManager : Form
    {
        CultureInfo culture = new CultureInfo("vi-VN");

        private Account loginAccount;

        public Account LoginAccount 
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount.Type); } 
        }

        public fTableManager(Account acc)
        {
            InitializeComponent();

            this.LoginAccount = acc;

            LoadTable();
            LoadCategory();
            LoadComboBoxTable(cbSwitchTable);
        }
        #region Method

        void ChangeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1; //nêu type = 1 thì bật chức năng admin
            thôngTinTàiKhoảnToolStripMenuItem.Text += "(" + LoginAccount.DisplayName + ")"; 
        }

        void LoadCategory()
        {
            List<Category> ListCategory = CategoryDAO.Instance.GetListCategory();
            cbCategory.DataSource = ListCategory;
            cbCategory.DisplayMember = "Name";
        }

        void LoadFoodListCategoryID(int id)
        {
            List<Food> listFood = FoodDAO.Instance.GetFoodCategoryID(id);
            cbFood.DataSource = listFood;
            cbFood.DisplayMember = "Name";
        }

        void LoadTable()
        {
            flpTable.Controls.Clear();

            List<Table> tableList = TableDAO.Instance.LoadTableList(); // cho dữ liệu vào List<Table>

            //xếp dữ liệu vào button
            foreach (Table item in tableList)
            {
                Button btn = new Button() { Width = Table.TableWidth, Height = Table.TableHeight };
                btn.Text = item.Name + Environment.NewLine + item.Status; // cho tên vào button

                btn.Click += btn_Click; // tạo event mỗi lần click vào button

                btn.Tag = item;// lưu thông tin vào button
                //code tạo màu cho button
                switch (item.Status)
                {
                    case "Trống":
                        btn.BackColor = Color.Aqua;
                        break;

                    default:
                        btn.BackColor = Color.Orange;
                        break;
                }

                flpTable.Controls.Add(btn); // thêm button vào flpTable
                
            }

        }

        void ShowBill(int id)
        {
            

            lsvBill.Items.Clear();

            List<Menu> listMenu = MenuDAO.Instance.GetListMenuByTable(id);

            float totalPrice = 0;

            foreach (Menu item in listMenu)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                totalPrice += item.TotalPrice;// cộng toàn bộ thành tiền để có tổng tiền
                lsvBill.Items.Add(lsvItem);
            }

            
            // vi-VN: format VN
            // en-US: format EN
            Thread.CurrentThread.CurrentCulture = culture;// chạy toàn bộ Thread hiện tại là "culture"
            txbTotalPrice.Text = totalPrice.ToString("c");//Chỉ đổi tượng này || In ra  kiểu chuỗi với định dạng tiền theo định dạng "culture"
            
            LoadTable();
            
        }

        void LoadComboBoxTable(ComboBox cb)
        {
            cb.DataSource = TableDAO.Instance.LoadTableList();
            cb.DisplayMember = "Name";
        }

        #endregion
       

       
        #region Event
        private void btnAddFood_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;

            if(table == null)
            {
                MessageBox.Show("Hãy chọn bàn!");
                return;
            }

            int idBill = BillDAO.Instance.GetUnCheckBillByTableID(table.ID);
            int idFood = (cbFood.SelectedItem as Food).ID;
            int count = (int)nmCountFood.Value;

            if(idBill==-1)
            {
                BillDAO.Instance.InsertBill(table.ID); // thêm Bill nếu chưa có Bill
                BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIDBill(), idFood, count); // thêm BillInfo nếu chưa có Bill
            }
            else
            {
                BillInfoDAO.Instance.InsertBillInfo(idBill, idFood, count); //thêm BillInfo nếu đã có Bill
            }
            ShowBill(table.ID); // load lại menu bill
            nmCountFood.Value = 1;
            
        }
        void btn_Click(object sender, EventArgs e)// hàm sự kiện khi click button
        {
            int tableID = ((sender as Button).Tag as Table).ID;// đánh tag cho button = tableID
            string tableName = ((sender as Button).Tag as Table).Name;// đánh tag cho button = tableID
            lsvBill.Tag = (sender as Button).Tag; //đánh tag cho đối tượng trong lsvBill 
            ShowBill(tableID);
            txbShowTableName.Text = tableName;
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAccountProfile f = new fAccountProfile(loginAccount);
            f.UpdateAccount += F_UpdateAccount;
            f.ShowDialog();

        }

        private void F_UpdateAccount(object sender, AccountEvent e)
        {
            thôngTinTàiKhoảnToolStripMenuItem.Text = "Thông tin tài khoản (" + e.Acc.DisplayName + ")";
        }


        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin();
            f.LoginAccount = LoginAccount;// gán thông tin tài khoản đang log in vào biên đăng nhập ở form Admin
            // Gán event chp các trường hợp
            f.InsertFood += F_InsertFood; 
            f.UpdateFood += F_UpdateFood;
            f.DeleteFood += F_DeleteFood;

            f.InsertFoodCategory += F_InsertFoodCategory;
            f.UpdateFoodCategory += F_UpdateFoodCategory;
            f.DeleteFoodCategory += F_DeleteFoodCategory;

            f.InsertTable += F_InsertTable;
            f.UpdateTable += F_UpdateTable;
            f.DeleteTable += F_DeleteTable;
            f.ShowDialog();
        }

        private void F_DeleteTable(object sender, EventArgs e)
        {
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
            LoadTable();
        }

        private void F_UpdateTable(object sender, EventArgs e)
        {
            LoadTable();
        }

        private void F_InsertTable(object sender, EventArgs e)
        {
            LoadTable();
        }

        private void F_DeleteFoodCategory(object sender, EventArgs e)
        {
            LoadCategory();
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
            LoadTable();
        }

        private void F_UpdateFoodCategory(object sender, EventArgs e)
        {
            LoadCategory();
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
        }

        private void F_InsertFoodCategory(object sender, EventArgs e)
        {
            LoadCategory();
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
        }

        private void F_DeleteFood(object sender, EventArgs e)
        {
            LoadFoodListCategoryID((cbCategory.SelectedItem as Category).ID);
            if(lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
            LoadTable();
        }

        private void F_InsertFood(object sender, EventArgs e)
        {
            LoadFoodListCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
        }

        private void F_UpdateFood(object sender, EventArgs e)
        {
            LoadFoodListCategoryID((cbCategory.SelectedItem as Category).ID);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).ID);
        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;
            ComboBox cb = sender as ComboBox;

            if (cb.SelectedItem == null)
                return;

            Category selected = cb.SelectedItem as Category;
            id = selected.ID;

            LoadFoodListCategoryID(id);
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            if(txbShowTableName.Text!="")
            {
                Table table = lsvBill.Tag as Table;

                int idBill = BillDAO.Instance.GetUnCheckBillByTableID(table.ID);
                int discount = (int)nmDiscount.Value;

                double totalPrice = Convert.ToDouble(txbTotalPrice.Text.Split(' ')[0].Replace(".", ""));//cắt bỏ kí tự sau " " và bỏ đi"." để đổi thành dạng số
                double finalTotalPrice = totalPrice - (totalPrice / 100) * discount;

                if (idBill != -1)
                {
                    if (MessageBox.Show(string.Format("Bạn có chắc muốn thanh toán hóa đơn cho [ {0} ] \n || Tổng tiền - (Tổng tiền / 100) x Giảm giá = Thành tiền ||\n => {1} - ({1} / 100) x {2}%  = {3} đ", table.Name, totalPrice, discount, finalTotalPrice), "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                    {
                        BillDAO.Instance.CheckOut(idBill, discount, (float)finalTotalPrice);
                        ShowBill(table.ID);

                        LoadTable();
                    }
                }
                nmDiscount.Value = 0;
            }    
            else
            {
                MessageBox.Show("Chưa chọn bàn và món để thanh toán!","Cảnh báo");
            }    

            
        }

        private void lsvBill_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnSwitchTable_Click(object sender, EventArgs e)
        {        
            if(txbShowTableName.Text!="")
            {
                int id1 = (lsvBill.Tag as Table).ID;

                int id2 = (cbSwitchTable.SelectedItem as Table).ID;
                if (MessageBox.Show(string.Format("Bạn có muốn chuyển từ bàn {0} sang bàng {1}", (lsvBill.Tag as Table).Name, (cbSwitchTable.SelectedItem as Table).Name), "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    TableDAO.Instance.SwitchTable(id1, id2);

                    LoadTable();
                }
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn bàn được chuyển đi!", "Cảnh báo");
            }    

        }

        private void thanhToánToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnCheckOut_Click(this,new EventArgs());
        }

        private void thêmMónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnAddFood_Click(this,new EventArgs());
        }

        #endregion


    }
}
