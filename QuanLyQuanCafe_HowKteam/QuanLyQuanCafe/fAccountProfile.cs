using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using QuanLyQuanCafe.DTO;
using QuanLyQuanCafe.DAO;
using System.Security.Cryptography;

namespace QuanLyQuanCafe
{
    public partial class fAccountProfile : Form
    {
        private Account loginAccount;

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount); }
        }

        public fAccountProfile(Account acc)
        {
            InitializeComponent();

            LoginAccount = acc;
        }
        
        void ChangeAccount(Account acc)
        {
            txbUserName.Text = LoginAccount.UserName;
            txbDisplayName.Text = LoginAccount.DisplayName;

        }

        void UpdateAccountInfo()
        {
            string displayName = txbDisplayName.Text;
            string password = txbPassword.Text;
            string newpass = txbNewPassword.Text;
            string reenterPass = txbReEnterPassword.Text;
            string userName = txbUserName.Text;

            #region Mã hóa pass bằng md5
            byte[] temp = ASCIIEncoding.ASCII.GetBytes(password);                
            byte[] hashData = new MD5CryptoServiceProvider().ComputeHash(temp);  
                                                                                    
            string hashPass = "";

            foreach (byte item in hashData)
            {
                hashPass += item;
            }
            #endregion

            if (!newpass.Equals(reenterPass))
            {
                MessageBox.Show("Nhập lại mật khẩu mới không chính xác!");
            }
            else
            {
                if(AccountDAO.Instance.UpdateAccount(userName,displayName,hashPass,newpass))
                {
                    MessageBox.Show("Cập nhật thành công!");
                    if (updateAccount != null)
                        updateAccount(this,new AccountEvent( AccountDAO.Instance.GetAccountByUserName(userName)));
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập đúng mật khẩu!");
                }    
            }    
        }

        private event EventHandler<AccountEvent> updateAccount;
        public event EventHandler<AccountEvent> UpdateAccount 
        {
            add { updateAccount += value; }
            remove { updateAccount -= value; }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateAccountInfo();
        }

        public class AccountEvent : EventArgs
        {
            private Account acc;

            public Account Acc 
            {
                get { return acc; }
                set { acc = value; } 
            }

            public AccountEvent(Account acc)
            {
                this.Acc = acc;
            }
        }
    }
}
