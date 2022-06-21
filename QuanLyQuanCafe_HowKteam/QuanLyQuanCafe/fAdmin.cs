using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fAdmin : Form
    {
        BindingSource foodList = new BindingSource();

        BindingSource accountList = new BindingSource();

        BindingSource categoryList = new BindingSource();

        BindingSource tableList = new BindingSource();
        public fAdmin()
        {

            InitializeComponent();
            LoadData();
        }
        public Account LoginAccount; // đặt biến đăng nhập để gán vào trong hàm adminToolStripMenuItem_Click ở form TableManager

        #region method
        void LoadData()
        {
            dtgvFood.DataSource = foodList;
            dtgvAccount.DataSource = accountList;
            dtgvCategory.DataSource = categoryList;
            dtgvTable.DataSource = tableList;

            LoadDateTimePickerBill();
            LoadListBillByDateAndPage(dtpkFromDate.Value, dtpkToDate.Value, Convert.ToInt32(txbPageBill.Text));
            LoadAccount();
            AddAccountBinding();
            LoadListFood();
            LoadCategoryIntoCombobox(cbFoodCategory);
            AddFoodBinding();
            LoadListCategory();
            AddCategoryBinding();
            LoadListTable();
            AddTableBinding();
            LoadResultTotalNumBerBill();
            LoadListUserInfo();
            AddUserInfoBinding();
        }
        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.UtcNow;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1); //
            dtpkToDate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);//công thức lấy ngày cuối cùng của tháng. 
                                                                           //ngày đầu tiên của tháng này + thêm 1 tháng để sang tháng sau -1 ngày để lấy dc ngày cuối cùng của tháng hiện tại.
        }


        #region Account
        void AddAccountBinding()
        {
            txbUserName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "UserName", true, DataSourceUpdateMode.Never));
            txbDisplayName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));

        }

        void LoadAccount()
        {
            accountList.DataSource = AccountDAO.Instance.GetListAccount();
        }

        void AddAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.InsertAccount(userName, displayName, type))
            {
                MessageBox.Show("Thêm tài khoản thành công!");
            }
            else
            {
                MessageBox.Show("Thêm tài khoản thất bại!");
            }
            LoadAccount();
        }

        void EditAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.UpdateAccount(userName, displayName, type))
            {
                MessageBox.Show("Sửa tài khoản thành công!");
            }
            else
            {
                MessageBox.Show("Sửa tài khoản thất bại!");
            }
            LoadAccount();
        }

        void DeleteAccount(string userName)
        {

            if (AccountDAO.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Xóa tài khoản thành công!");
            }
            else
            {
                MessageBox.Show("Xóa tài khoản thất bại!");
            }
            LoadAccount();
        }

        void ResetPassword(string userName)
        {
            if (AccountDAO.Instance.ResetPassword(userName))
            {
                MessageBox.Show("Đặt lại mật khẩu thành công!");
            }
            else
            {
                MessageBox.Show("Đặt lại mật khẩu thất bại!");
            }
        }
        #endregion


        #region Food
        // cho category vào combobox
        void LoadCategoryIntoCombobox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }

        // hiển thị danh sách hóa đơn trong khoản thời gian
        void LoadListBillByDateAndPage(DateTime CheckIn, DateTime CheckOut, int page)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetListBillByDateAndPage(CheckIn, CheckOut, page);
        }

        // Load danh sách món
        void LoadListFood()
        {
            foodList.DataSource = FoodDAO.Instance.GetListFood();
        }


        List<Food> SearchFoodByName(string name)
        {
            List<Food> listFood = new List<Food>();

            listFood = FoodDAO.Instance.SearchFoodByName(name);

            return listFood;
        }
        //End=====================================================
        void AddFoodBinding()
        {
            txbFoodName.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Name", true, DataSourceUpdateMode.Never));//true: tự động ép kiểu

            nmFoodPrice.DataBindings.Add(new Binding("Value", dtgvFood.DataSource, "Price", true, DataSourceUpdateMode.Never));

            txbFoodID.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "ID", true, DataSourceUpdateMode.Never));//Never: ko update dữ liệu, dữ liệu chỉ chạy 1 luồn duy nhất từ source qua và ko update ngược lại

            //cbFoodCategory.DataBindings.Add(new Binding("SelectedValue",dtgvFood.DataSource,"CategoryID",true,DataSourceUpdateMode.Never));

        }
        #endregion


        #region Category

        void LoadListCategory()
        {
            categoryList.DataSource = CategoryDAO.Instance.GetListCategory();
        }
        void AddCategoryBinding()
        {
            txbCategoryID.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "ID", true, DataSourceUpdateMode.Never));
            txbCategoryName.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "Name", true, DataSourceUpdateMode.Never));
        }


        #endregion


        #region TableFood

        void LoadListTable()
        {
            tableList.DataSource = TableDAO.Instance.LoadTableList();
        }

        void AddTableBinding()
        {
            txbTableID.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "ID", true, DataSourceUpdateMode.Never));
            txbTableName.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "Name", true, DataSourceUpdateMode.Never));
        }

        void AddTable(string name)
        {
            if (TableDAO.Instance.InsertTable(name))
            {
                MessageBox.Show("Thêm bàn thành công!", "Xác nhận");
            }
            else
            {
                MessageBox.Show("Thêm bàn thất bại!", "Cảnh báo");
            }
        }

        void EditTable(int id, string name)
        {
            if (TableDAO.Instance.UpdateTable(id, name))
            {
                MessageBox.Show("Cập nhật bàn thành công!", "Xác nhận");
            }
            else
            {
                MessageBox.Show("Cập nhật bàn thất bại!", "Cảnh báo");
            }
        }

        void DeleteTableFood(int id)
        {
            if (TableDAO.Instance.DeleteTable(id))
            {
                MessageBox.Show("Xóa bàn thành công!", "Xác nhận");
            }
            else
            {
                MessageBox.Show("Xóa bàn thất bại!", "Cảnh báo");
            }
        }

        #endregion

        #region Statistical
        void LoadListUserInfo()
        {
            dtgvUserInfo.DataSource = UserInfoDAO.Instance.GetListUserInfo();
        }

        void AddUserInfoBinding()
        {
            txbShowIDUserInfo.DataBindings.Add(new Binding("Text", dtgvUserInfo.DataSource, "ID", true, DataSourceUpdateMode.Never));
            txbShowUserNameInfo.DataBindings.Add(new Binding("Text", dtgvUserInfo.DataSource, "Name", true, DataSourceUpdateMode.Never));
        }
        struct Data
        {
            int first, second;
        };
        


        #endregion
        void LoadResultTotalNumBerBill()
        {
            int totalNumber = BillDAO.Instance.GetNumBillByDate(dtpkFromDate.Value,dtpkToDate.Value);
            lbResultBill.Text = string.Format("Có tổng cộng {0} hóa đơn trong khoản thời gian này.",totalNumber);
        }

        #endregion

        #region event

            #region tab Food
        private void btnShowFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
            LoadCategoryIntoCombobox(cbFoodCategory);
        }

        private void txbFoodID_TextChanged(object sender, EventArgs e)
        {
            //==================== CODE thử ======================== Chạy dc


            //try
            //{
            //    if (dtgvFood.SelectedCells.Count > 0 && dtgvFood.SelectedCells[0].OwningRow.Cells["CategoryID"].Value != null)
            //    {
            //        int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["CategoryID"].Value; 
            //        Category category = CategoryDAO.Instance.GetCategoryByID(id);
            //        cbFoodCategory.SelectedIndex = category.ID;
            //    }
            //}
            //catch (InvalidOperationException) { };



            //==================== CODE gốc ======================== Chạy dc

            try
            {
                if (dtgvFood.SelectedCells.Count > 0 && dtgvFood.SelectedCells[0].OwningRow.Cells["CategoryID"].Value != null)
                {
                    // Lấy dữ liệu có tên là idcategory trên 1 dòng của 1 ô đc chọn trong dtgv
                    int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["CategoryID"].Value;

                    Category category = CategoryDAO.Instance.GetCategoryByID(id);

                    cbFoodCategory.SelectedItem = category;

                    int index = -1;
                    int i = 0;
                    foreach (Category item in cbFoodCategory.Items)
                    {
                        if (item.ID == category.ID)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }
                    cbFoodCategory.SelectedIndex = index;
                }
            }
            catch { };
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int categoryID = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;
            if (FoodDAO.Instance.InsertFood(name, categoryID, price))
            {
                MessageBox.Show("Thêm món thành công!");
                LoadListFood();
                if (insertFood != null)
                    insertFood(this, new EventArgs());
            }
            else
            {
                {
                    MessageBox.Show("Có lỗi khi thêm thức ăn!");
                }
            }
        }
        private void btnEditFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int categoryID = (cbFoodCategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;
            
            if(txbFoodID.Text != "")
            {
                int idFood = Convert.ToInt32(txbFoodID.Text);
                if (FoodDAO.Instance.UpdateFood(idFood, name, categoryID, price))
                {
                    MessageBox.Show("Sửa món thành công!");
                    LoadListFood();
                    if (updateFood != null)
                        updateFood(this, new EventArgs());
                }
                else
                {                  
                        MessageBox.Show("Có lỗi khi sửa thức ăn!");                 
                }
            }
            else
            {
                MessageBox.Show("Hãy chọn món cần sửa");
            }

        }
        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            if(txbFoodID.Text!="")
            {
                int idFood = Convert.ToInt32(txbFoodID.Text);
                if (MessageBox.Show("Bạn có chắc muốn xóa tài khoản nay?", "Cảnh báo!", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    if (FoodDAO.Instance.DeleteFood(idFood))
                    {
                        MessageBox.Show("Xoá món thành công!");
                        LoadListFood();
                        if (deleteFood != null)
                            deleteFood(this, new EventArgs());
                    }
                    else
                    {
                        {
                            MessageBox.Show("Có lỗi khi xóa thức ăn!");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Chưa chọn món cần xóa!");
            }

        }
        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            foodList.DataSource = SearchFoodByName(txbSearchFoodName.Text);
        }
        #endregion

            #region EventHandler
        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }

        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }

        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }

        private event EventHandler insertFoodCategory;
        public event EventHandler InsertFoodCategory
        {
            add { insertFoodCategory += value; }
            remove { insertFoodCategory -= value; }
        }

        private event EventHandler updateFoodCategory;
        public event EventHandler UpdateFoodCategory
        {
            add { updateFoodCategory += value; }
            remove { updateFoodCategory -= value; }
        }

        private event EventHandler deleteFoodCategory;
        public event EventHandler DeleteFoodCategory
        {
            add { deleteFoodCategory += value; }
            remove { deleteFoodCategory -= value; }
        }

        private event EventHandler insertTable;
        public event EventHandler InsertTable
        {
            add { insertTable += value; }
            remove { insertTable -= value; }
        }

        private event EventHandler updateTable;
        public event EventHandler UpdateTable
        {
            add { updateTable += value; }
            remove { updateTable -= value; }
        }

        private event EventHandler deleteTable;
        public event EventHandler DeleteTable
        {
            add { deleteTable += value; }
            remove { deleteTable -= value; }
        }

        #endregion

            #region tab Account
        private void txbUserName_TextChanged(object sender, EventArgs e)
        {
            cbAccountType.Items.Clear();
            cbAccountType.Items.Add("Admin");
            cbAccountType.Items.Add("Staff");
            if (dtgvAccount.SelectedCells.Count > 0 && dtgvAccount.SelectedCells[0].OwningRow.Cells["Type"].Value != null)
            {
                int id = (int)dtgvAccount.SelectedCells[0].OwningRow.Cells["Type"].Value;
                if (id == 1)
                {
                    cbAccountType.SelectedIndex = 0;
                }
                else
                {
                    cbAccountType.SelectedIndex = 1;
                }

            }
        }

        private void btnShowAccount_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            string displayName = txbDisplayName.Text;
            int type;
            if (string.Compare((string)cbAccountType.SelectedItem, "Admin")==0) //kiểm tra acc dc thêm nếu là admin thì gán 1
            {
                type = 1;
            }
            else
            {
                type = 0;
            }
            string str1 = (string)dtgvAccount.SelectedCells[0].OwningRow.Cells["userName"].Value;

            if (str1.Equals(txbUserName.Text))//kiểm tra xem tên tài khoản mới có bị trùng với tài khoản hiện có hay ko
            {
                MessageBox.Show("Tài khoản mới không được trùng tên với tài khoản hiện có!");
            }
            else
            {
                AddAccount(userName, displayName, type);
            }
                
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            if (LoginAccount.UserName.Equals(userName)) // kiểm tra xem tài khoản muốn xóa có trùng với tài khoản đang log in hay ko
            {
                MessageBox.Show("Không thể xóa tài khoản đang đăng nhập!");
                return;
            }
            else
            {
                if (MessageBox.Show("Bạn có chắc muốn xóa tài khoản nay?", "Cảnh báo!", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    DeleteAccount(userName);
                }
            }                  
        }

        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            string displayName = txbDisplayName.Text;
            int type;
            if (string.Compare((string)cbAccountType.SelectedItem, "Admin") == 0)
            {
                type = 1;
            }
            else
            {
                type = 0;
            }
            if (MessageBox.Show("Bạn có chắc muốn cập nhật tài khoản nay?", "Xác nhận!", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                EditAccount(userName, displayName, type);
            }
        }
        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            if (MessageBox.Show("Bạn có chắc muốn cập nhật tài khoản nay?", "Xác nhận!", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                ResetPassword(userName);
            }
        }
        #endregion

            #region tab Category
        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            string nameCategory = txbCategoryName.Text;
            string str1 = (string)dtgvCategory.SelectedCells[0].OwningRow.Cells["Name"].Value;

            if (str1.Equals(nameCategory))//kiểm tra xem tên loại món mới có bị trùng với loại món hiện có hay ko
            {
                MessageBox.Show("Loại món mới không được trùng tên với loại món hiện có!");
            }
            else
            {
                if (CategoryDAO.Instance.InsertCategory(nameCategory))
                {
                    MessageBox.Show("Thêm loại món thành công!", "Thông báo");
                    LoadListCategory();
                    if (insertFoodCategory != null)
                        insertFoodCategory(this, new EventArgs());
                }
                else
                {
                    MessageBox.Show("Thêm loại món Thất bại!", "Thông báo");
                }
            }    
        }

        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn xóa loại món này ?", "Cảnh báo!", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                int idFoodCategory = Convert.ToInt32(txbCategoryID.Text);
                if (CategoryDAO.Instance.DeleteCategory(idFoodCategory))
                {
                    MessageBox.Show("Xóa loại món thành công!", "Thông báo");
                    LoadListCategory();
                    if (deleteFoodCategory != null)
                        deleteFoodCategory(this, new EventArgs());
                }
                else
                {
                    MessageBox.Show("Xóa loại món Thất bại!", "Thông báo");
                }
            }
            
        }

        private void btnEditCategory_Click(object sender, EventArgs e)
        {
            int idFoodCategory = Convert.ToInt32(txbCategoryID.Text);
            string nameCategory = txbCategoryName.Text;
            if (MessageBox.Show("Bạn có chắc muốn cập nhật loại món nay?", "Cảnh báo!", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                if (CategoryDAO.Instance.UpdateCategory(idFoodCategory, nameCategory))
                {
                    MessageBox.Show("Cập nhật loại món thành công!", "Thông báo");
                    LoadListCategory();
                    if (updateFoodCategory != null)
                        updateFoodCategory(this, new EventArgs());
                }
                else
                {
                    MessageBox.Show("Cập nhật loại món Thất bại!", "Thông báo");
                }
            }
            
        }

        private void btnShowCategory_Click(object sender, EventArgs e)
        {
            LoadListCategory();
        }
        #endregion

            #region tab Table
        private void btnShowTable_Click(object sender, EventArgs e)
        {
            LoadListTable();
        }

        private void btnAddTable_Click(object sender, EventArgs e)
        {
            string name = txbTableName.Text;
            string str1 = (string)dtgvAccount.SelectedCells[0].OwningRow.Cells["userName"].Value;

            if (str1.Equals(txbUserName.Text))//kiểm tra xem Tên bàn mới có bị trùng với Tên bàn hiện có hay ko
            {
                MessageBox.Show("Tên bàn mới không được trùng tên với Tên bàn hiện có!");
            }
            else
            {
                AddTable(name);
                if (insertTable != null)
                    insertTable(this, new EventArgs());
                LoadListTable();
            }    
        }

        private void btnEditTable_Click(object sender, EventArgs e)
        {
            int id =Convert.ToInt32(txbTableID.Text);
            string name = txbTableName.Text;
            if ((dtgvTable.SelectedCells[0].OwningRow.Cells["status"].Value).Equals("Trống"))
            {
                if (MessageBox.Show("Bạn có chắc muốn sửa tên bàn này? ", "Xác nhận!", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    EditTable(id, name);
                    if (updateTable != null)
                        updateTable(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Không thể cập nhật bàn đang có người", "Cảnh báo");
            }
            LoadListTable();
        }

        private void btnDeleteTable_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbTableID.Text);
            
            if ((dtgvTable.SelectedCells[0].OwningRow.Cells["status"].Value).Equals("Trống"))
            {
                if (MessageBox.Show("Bạn có chắc muốn xóa bàn này? \n Xóa bàn sẽ xóa luôn hóa đơn của bàn đó.", "Cảnh báo!", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    DeleteTableFood(id);
                    if (deleteTable != null)
                        deleteTable(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Không thể xóa bàn đang có người", "Cảnh báo"); 
            }
            LoadListTable();
        }
        private void txbTableID_TextChanged(object sender, EventArgs e)
        {
            cbTableStatus.Items.Clear();
            cbTableStatus.Items.Add("Trống");
            cbTableStatus.Items.Add("Có người");
            if (dtgvTable.SelectedCells.Count > 0 && dtgvTable.SelectedCells[0].OwningRow.Cells["status"].Value != null)
            {
                string str1 = (string)dtgvTable.SelectedCells[0].OwningRow.Cells["status"].Value;
                if (str1.Equals("Trống"))
                {
                    cbTableStatus.SelectedIndex = 0;
                }
                else
                {
                    cbTableStatus.SelectedIndex = 1;
                }

            }
        }
        #endregion

            #region tab Turnover
        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDateAndPage(dtpkFromDate.Value, dtpkToDate.Value, Convert.ToInt32(txbPageBill.Text));
        }
        private void btnFirstBillPage_Click(object sender, EventArgs e)
        {
            txbPageBill.Text = "1";
        }

        private void btnLastBillPage_Click(object sender, EventArgs e)
        {
            int sumRecord = BillDAO.Instance.GetNumBillByDate(dtpkFromDate.Value, dtpkToDate.Value);

            int lastPage = sumRecord / 10;

            if(sumRecord%10!=0)
            {
                lastPage++;
            }

            txbPageBill.Text = lastPage.ToString();
        }

        private void txbPageBill_TextChanged(object sender, EventArgs e)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetListBillByDateAndPage(dtpkFromDate.Value, dtpkToDate.Value, Convert.ToInt32(txbPageBill.Text));
        }

        private void btnPrevioursBillPage_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txbPageBill.Text);

            if (page > 1)
            {
                page--;
            }
            txbPageBill.Text = page.ToString();
        }

        private void btnNextBillPage_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txbPageBill.Text);
            int sumRecord = BillDAO.Instance.GetNumBillByDate(dtpkFromDate.Value, dtpkToDate.Value);// tổng số dòng
            if (page <= sumRecord)
            {
                page++;
            }    
            txbPageBill.Text = page.ToString();
        }
        #endregion

            #region tab Statistical
        private void btnEditUserInfo_Click(object sender, EventArgs e)
        {
            fEditUserInfo f = new fEditUserInfo();
            f.ShowDialog();
        }



        #endregion
        #endregion


    }
}
