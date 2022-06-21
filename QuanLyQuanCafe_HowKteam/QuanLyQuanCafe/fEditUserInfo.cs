using QuanLyQuanCafe.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fEditUserInfo : Form
    {
        public fEditUserInfo()
        {
            InitializeComponent();
            LoadData();
        }
        #region Method
      
        void LoadData()
        {
            LoadListUserInfo();
            AddUserInfoBinding();
        }
        void LoadListUserInfo()
        {
            dtgvEditUserInfo.DataSource = UserInfoDAO.Instance.GetListUserInfo();
        }
        void AddUserInfoBinding()
        {
            txbIDUserInfo.DataBindings.Add(new Binding("Text", dtgvEditUserInfo.DataSource, "ID", true, DataSourceUpdateMode.Never));
            txbUserNameInfo.DataBindings.Add(new Binding("Text", dtgvEditUserInfo.DataSource, "Name", true, DataSourceUpdateMode.Never));
        }
        void AddUserInfo()
        {
            string name = txbUserNameInfo.Text;
            float timeCome = (float)Convert.ToDouble(txbTimeCome.Text);
            float timeLeave = (float)Convert.ToDouble(txbTimeLeave.Text); ;
            if(UserInfoDAO.Instance.InsertUserinfo(name,timeCome,timeLeave))
            {
                MessageBox.Show("Thêm thông tin khách thành công!","Xác nhận");
                LoadListUserInfo();
            }
            else
            {
                MessageBox.Show("Thêm thông tin khách thất bại!","Cảnh báo");
            }    
        }
        void EditUserInfo()
        {
            int id = Convert.ToInt32(txbIDUserInfo.Text);
            string name = txbUserNameInfo.Text;
            float timeCome = (float)Convert.ToDouble(txbTimeCome.Text);
            float timeLeave = (float)Convert.ToDouble(txbTimeLeave.Text); ;
            if (txbIDUserInfo.Text!="")
            {
                if (UserInfoDAO.Instance.UpdateUserinfo(id, name, timeCome, timeLeave))
                {
                    MessageBox.Show("Cập nhật thông tin khách thành công!", "Xác nhận");
                    LoadListUserInfo();
                }
                else
                {
                    MessageBox.Show("Cập nhật thông tin khách thất bại!", "Cảnh báo");
                }
            }   
            else
            {
                MessageBox.Show("Hãy chọn thông tin khách cần cập nhật","Cảnh báo");
            } 
        }

        void DeleteUserInfo()
        {
            int id = Convert.ToInt32(txbIDUserInfo.Text);
            if (txbIDUserInfo.Text != "")
            {
                if (MessageBox.Show("Bạn có chắc muốn xóa thông tin của vị khách này?", "Cảnh báo!", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    if (UserInfoDAO.Instance.DeleteUserinfo(id))
                    {
                        MessageBox.Show("Xóa thông tin khách thành công!", "Xác nhận");
                        LoadListUserInfo();
                    }
                    else
                    {
                        MessageBox.Show("Xóa thông tin khách thất bại!", "Cảnh báo");
                    }
                }
            }
            else
            {
                MessageBox.Show("Hãy chọn thông tin khách cần xóa", "Cảnh báo");
            }
        }
        #endregion


        #region Event
      

        private void btnAddUserInfo_Click(object sender, EventArgs e)
        {
            AddUserInfo();
        }
        private void btnShowUserInfo_Click(object sender, EventArgs e)
        {
            LoadListUserInfo();
        }
        private void btnEditUserInfo_Click(object sender, EventArgs e)
        {
            EditUserInfo();
        }

        private void btnDeleteUserInfo_Click(object sender, EventArgs e)
        {
            DeleteUserInfo();
        }


        #endregion

        
    }
}
