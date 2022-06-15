# Program-cafe-manager

Phần mềm được tạo ra dựa trên video hướng dẫn tạo chương trình quản lý quán ăn bằng C# winform của HowKteam. 

Admin 
- Account: k9 
- pass: 1

Staff
- Account: staff 
- pass: 1

*Note: mật khẩu sẽ được mã khóa vì vậy nếu bạn muốn thêm mật khẩu thông qua SQL hãy chắc rằng bạn mã hóa mật khẩu theo đoạn code dưới:

 #region Mã hóa pass bằng md5
 
            byte[] temp = ASCIIEncoding.ASCII.GetBytes(password);                
            byte[] hashData = new MD5CryptoServiceProvider().ComputeHash(temp);  
                                                                                    
            string hashPass = "";

            foreach (byte item in hashData)
            {
                hashPass += item;
            }
#endregion

*Password ở đây là mật khẩu được nhập vào từ textbox.

*Phần mềm hiện tại chưa hoàn thiện.
