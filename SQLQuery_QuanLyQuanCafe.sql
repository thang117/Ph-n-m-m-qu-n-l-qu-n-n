 CREATE DATABASE QuanLyQuanCafe
 GO

 USE QuanLyQuanCafe
 GO

 --Food
 --Table
 --FoodCategory
 --Account
 --Bill
 --BillInfo

 CREATE TABLE TableFood
 (
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL DEFAULT N'Bàn chưa đặt tên',
	status NVARCHAR(100) NOT NULL DEFAULT N'Trống'--Trống|| có người
 )
 GO

 CREATE TABLE Account
 (
	DisplayName NVARCHAR(100) NOT NULL DEFAULT N'Kter',
	UserName NVARCHAR(100) PRIMARY KEY,
	PassWord NVARCHAR(1000) NOT NULL DEFAULT 0,
	Type INT NOT NULL DEFAULT 0 -- 1:admin || 0: staff
)
 GO

 CREATE TABLE FoodCategory
 (
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL DEFAULT N'Chưa đặt tên',
 )
 GO

 CREATE TABLE Food
 (
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL DEFAULT N'Chưa đặt tên',
	idCategory INT NOT NULL,
	price FLOAT NOT NULL DEFAULT 0

	FOREIGN KEY (idCategory) REFERENCES dbo.FoodCategory(id)
 )
 GO

 CREATE TABLE Bill
 (
	id INT IDENTITY PRIMARY KEY,
	DateCheckIn DATE NOT NULL DEFAULT GETDATE(),
	DateCheckOut DATE ,
	idTable INT NOT NULL,
	status INT NOT NULL  DEFAULT 0, -- 1 đã thanh toán || 0 chưa thanh toán
	discount INT DEFAULT 0,
	totalPrice FLOAT DEFAULT 0
	FOREIGN KEY (idTable) REFERENCES dbo.TableFood(id)
 )
 GO

 CREATE TABLE BillInfo
 (
	id INT IDENTITY PRIMARY KEY,
	idBill INT NOT NULL,
	idFood INT NOT NULL,
	count INT NOT NULL DEFAULT 0

	FOREIGN KEY (idBill) REFERENCES dbo.Bill(id),
	FOREIGN KEY (idFood) REFERENCES dbo.Food(id)
 )
 GO

 CREATE TABLE UserInfo
 (
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL ,
	timeCome FLOAT NOT NULL ,
	timeLeave FLOAT NOT NULL
 )
 GO

 INSERT INTO Account
 (
	DisplayName,
	UserName,
	PassWord,
	Type
 )
 VALUES
 (
	'K9',
	'K9',
	'1962026656160185351301320480154111117132155',
	'1'
 )
 GO

 INSERT INTO Account
 (
	DisplayName,
	UserName,
	PassWord,
	Type
 )
 VALUES
 (
	'staff',
	'staff',
	'1962026656160185351301320480154111117132155',
	'0'
 )
 GO

 CREATE PROC USP_GetAccountByUserName
 @userName nvarchar(100)
 AS
 BEGIN
	SELECT * FROM dbo.Account WHERE UserName = @userName
 END
 GO

 EXEC dbo.USP_GetAccountByUserName @userName = N'K9' -- nvarchar(100)
 GO 

 -- Chống SQL injection
 CREATE PROC USP_Login
 @userName nvarchar(100), @passWord nvarchar(1000)
 AS
 BEGIN
	SELECT * FROM dbo.Account WHERE UserName = @userName AND PassWord = @passWord
 END
 GO

 -- dữ liệu bàn ăn
 DECLARE @1 INT = 0

 WHILE @1 <= 10
 BEGIN
	INSERT dbo.TableFood(name) VALUES (N'Bàn '+ CAST( @1 AS nvarchar(100)))
	SET @1 = @1 + 1
 END
 GO

 CREATE PROC USP_GetTableList
 AS SELECT * FROM dbo.TableFood
 GO

 EXEC dbo.USP_GetTableList
 GO


 --INSERT dbo.TableFood(name,status) VALUES (N'Bàn 1',N'Trống')
 --INSERT dbo.TableFood(name,status) VALUES (N'Bàn 2',N'Trống')
 --INSERT dbo.TableFood(name,status) VALUES (N'Bàn 3',N'Trống')
 --INSERT dbo.TableFood(name,status) VALUES (N'Bàn 4',N'Trống')
 --INSERT dbo.TableFood(name,status) VALUES (N'Bàn 5',N'Trống')
 --INSERT dbo.TableFood(name,status) VALUES (N'Bàn 6',N'Trống')
 --INSERT dbo.TableFood(name,status) VALUES (N'Bàn 7',N'Trống')
 --INSERT dbo.TableFood(name,status) VALUES (N'Bàn 8',N'Trống')
 --INSERT dbo.TableFood(name,status) VALUES (N'Bàn 9',N'Trống')
 --INSERT dbo.TableFood(name,status) VALUES (N'Bàn 10',N'Trống')

 -- Thêm Category
 INSERT dbo.FoodCategory(name) VALUES(N'Hải sản')-- nvarchar(100)
 INSERT dbo.FoodCategory(name) VALUES(N'Gỏi')-- nvarchar(100)
 INSERT dbo.FoodCategory(name) VALUES(N'Đồ Chiên')-- nvarchar(100)
 INSERT dbo.FoodCategory(name) VALUES(N'Đồ luộc,hấp')-- nvarchar(100)
 INSERT dbo.FoodCategory(name) VALUES(N'Nước')-- nvarchar(100)
 
 -- Thêm món

 INSERT dbo.Food(name,idCategory,price ) VALUES(N'Mực xả tế',1,120000) 
 INSERT dbo.Food(name,idCategory,price ) VALUES(N'Nghêu hấp xả',1,50000) 
 INSERT dbo.Food(name,idCategory,price ) VALUES(N'Gỏi cuốn Tây Sơn',2,50000) 
 INSERT dbo.Food(name,idCategory,price ) VALUES(N'Gỏi rau muống',2,40000) 
 INSERT dbo.Food(name,idCategory,price ) VALUES(N'Cơm chiên dương châu',3,35000) 
 INSERT dbo.Food(name,idCategory,price ) VALUES(N'Cánh gà chiên bột',3,75000) 
 INSERT dbo.Food(name,idCategory,price ) VALUES(N'Củ sắn luộc',4,30000)
 INSERT dbo.Food(name,idCategory,price ) VALUES(N'Bánh bao chiều',4,25000) 
 INSERT dbo.Food(name,idCategory,price ) VALUES(N'Nước lọc',5,10000)
 INSERT dbo.Food(name,idCategory,price ) VALUES(N'Sting Dâu',5,12000) 

 -- Thêm Bill

 --INSERT dbo.Bill(DateCheckIn,DateCheckOut,idTable,status) VALUES(GETDATE(),NULL,3,0)
 --INSERT dbo.Bill(DateCheckIn,DateCheckOut,idTable,status) VALUES(GETDATE(),NULL,4,0)
 --INSERT dbo.Bill(DateCheckIn,DateCheckOut,idTable,status) VALUES(GETDATE(),GETDATE(),5,1)

 -- Thêm BillInfo

  --INSERT dbo.BillInfo(idBill,idFood,count) VALUES(1,1,2)
  --INSERT dbo.BillInfo(idBill,idFood,count) VALUES(1,3,4)
  --INSERT dbo.BillInfo(idBill,idFood,count) VALUES(1,5,1)
  --INSERT dbo.BillInfo(idBill,idFood,count) VALUES(2,1,2)
  --INSERT dbo.BillInfo(idBill,idFood,count) VALUES(2,6,2)
  --INSERT dbo.BillInfo(idBill,idFood,count) VALUES(3,5,2)

  SELECT f.name,bi.count,f.price,f.price*bi.count AS totalPrice FROM dbo.BillInfo AS bi,dbo.Bill AS b, dbo.Food AS f
  WHERE bi.idBill = b.id AND bi.idFood = f.id AND b.idTable = 3
  GO

  CREATE PROC USP_InsertBill
  @idTable INT
  AS
  BEGIN
		INSERT dbo.Bill(DateCheckIn,DateCheckOut,idTable,status,discount)
		VALUES (GETDATE(),NULL,@idTable,0,0)
  END
  GO
   
  CREATE PROC USP_InsertBillInfo
  @idBill INT, @idFood INT, @count INT 
  AS
  BEGIN
		
		DECLARE @isExitsBillInfo INT
		DECLARE @foodCount INT = 1

		SELECT @isExitsBillInfo = id, @foodCount = b.count FROM dbo.BillInfo AS b WHERE idBill = @idBill AND idFood = @idFood

		IF(@isExitsBillInfo>0)
		BEGIN
			DECLARE @newCount INT = @foodCount + @count
			IF(@newCount > 0)
				UPDATE dbo.BillInfo	set count = @foodCount + @count	WHERE idFood = @idFood
			ELSE
				DELETE dbo.BillInfo WHERE idBill = @idBill AND idFood = @idFood
		END

		ELSE
		BEGIN
			INSERT dbo.BillInfo(idBill , idFood , count)
			VALUES (@idBill , @idFood , @count)
		END	
  END
  GO

  CREATE TRIGGER UTG_UpdateBillInfo
  ON dbo.BillInfo FOR INSERT, UPDATE
  AS
  BEGIN
		DECLARE @idBill INT

		SELECT @idBill= idBill FROM Inserted

		DECLARE @idTable INT

		SELECT @idTable = idTable FROM dbo.Bill WHERE id = @idBill AND status = 0			

		UPDATE dbo.TableFood SET status = N'Có Người' WHERE id = @idTable
		
  END
  GO

  CREATE TRIGGER UTG_UpdateBill
  ON dbo.Bill FOR UPDATE
  AS
  BEGIN
		DECLARE @idBill INT

		SELECT @idBill = id FROM Inserted

		DECLARE @idTable INT

		SELECT @idTable = idTable FROM dbo.Bill WHERE id = @idBill 

		DECLARE @count INT = 0

		SELECT @count = COUNT(*) FROM dbo.Bill WHERE idTable = @idTable AND status = 0

		IF(@count = 0)
			UPDATE dbo.TableFood SET status = N'Trống' WHERE id = @idTable
  END
  GO

 
 CREATE PROC USP_SwitchTable
 @idTable1 INT,@idTable2 INT
 AS
 BEGIN
	
	DECLARE @idFirstBill int
	DECLARE @idSecondBill int

	SELECT @idSecondBill = id FROM dbo.Bill WHERE idTable = @idTable2 AND status = 0
	SELECT @idFirstBill = id FROM dbo.Bill WHERE idTable = @idTable1 AND status = 0

	IF(@idFirstBill IS NULL)
	BEGIN
		INSERT dbo.Bill(DateCheckIn,DateCheckOut,idTable,status,discount) VALUES(GETDATE(),NULL,@idTable1,0,0)

		SELECT @idFirstBill = MAX(id) FROM dbo.Bill  WHERE idTable = @idTable1 AND status = 0
	END
	
	IF(@idSecondBill IS NULL)
	BEGIN
		INSERT dbo.Bill(DateCheckIn,DateCheckOut,idTable,status) VALUES(GETDATE(),NULL,@idTable2,0)

		SELECT @idSecondBill = MAX(id) FROM dbo.Bill  WHERE idTable = @idTable2 AND status = 0
	END

	SELECT id INTO IDBillInfoTable FROM dbo.BillInfo WHERE idBill = @idSecondBill

	UPDATE dbo.BillInfo SET idBill = @idSecondBill WHERE idBill = @idFirstBill

	UPDATE dbo.TableFood set status = N'Trống' WHERE id = @idTable1 --đổi status mỗi khi chuyển bàn

	UPDATE dbo.BillInfo SET idBill = @idFirstBill WHERE id IN (SELECT * FROM IDBillInfoTable)

	DROP TABLE IDBillInfoTable

 END
 GO

 CREATE PROC USP_GetListBillByDate
 @checkIn date, @checkOut date
 AS
 BEGIN
	SELECT t.name as 'Tên bàn' , b.totalPrice as 'Tổng tiền' , DateCheckIn as 'Ngày tạo' , DateCheckOut as 'Ngày xuất đơn' , discount as 'Giảm giá (%)' 
	FROM dbo.Bill AS b,dbo.TableFood AS t
	WHERE DateCheckIn >= @checkIn AND DateCheckOut <= @checkOut AND b.status = 1 AND t.id = b.idTable
 END
 GO

 CREATE PROC USP_UpdateAccount
 @userName NVARCHAR(100), @displayName NVARCHAR(100), @password NVARCHAR(1000), @newPassword NVARCHAR(100)
 AS
 BEGIN
	DECLARE @isRightPass INT = 0

	SELECT @isRightPass = COUNT(*) FROM dbo.Account WHERE UserName = @userName AND PassWord = @password

	IF(@isRightPass = 1)
	BEGIN
		IF(@newPassword = NULL OR @newPassword = '')
		BEGIN
			UPDATE dbo.Account SET DisplayName = @displayName WHERE UserName = @userName
		END
		ELSE
			UPDATE dbo.Account SET DisplayName = @displayName , PassWord = @newPassword WHERE UserName = @userName
	END
 END
 GO

 CREATE TRIGGER UTG_DeleteBillInfo
 ON dbo.BillInfo FOR DELETE
 AS
 BEGIN
	DECLARE @idBillInfo INT
	DECLARE @idBill INT

	SELECT @idBillInfo = id , @idBill = Deleted.idBill FROM Deleted

	DECLARE @idTable INT
	SELECT @idTable = idTable FROM dbo.Bill WHERE id = @idBill

	DECLARE @count INT = 0
	SELECT @count = COUNT(*) FROM dbo.BillInfo as bi,dbo.Bill as b WHERE b.id = bi.idBill AND b.id = @idBill AND b.status = 0

	IF(@count = 0)
		UPDATE dbo.TableFood SET status = N'Trống' WHERE id = @idTable
 END
 GO

 --INSERT dbo.Food (name,idCategory,price) VALUES(N'',0,0)

 --UPDATE dbo.Food SET name = N'', idCategory = 5 , price = 0 WHERE id = 4

 SELECT * FROM dbo.Bill
 SELECT * FROM dbo.BillInfo	
 SELECT * FROM dbo.Food
 SELECT * FROM dbo.FoodCategory


-- SELECT f.id[STT] , f.name [Tên món] , f.price[Giá] , f.idCategory[Mã loại]  FROM dbo.Food as f 
GO
-- function thay các kí tự có dấu thành ko dấu
CREATE FUNCTION [dbo].[fuConvertToUnsign1] (@strInput NVARCHAR(4000)) 
RETURNS NVARCHAR(4000) 
AS 
BEGIN 
	IF @strInput IS NULL 
		RETURN @strInput 
	IF @strInput = '' 
		RETURN @strInput 
	DECLARE @RT NVARCHAR(4000) 
	DECLARE @SIGN_CHARS NCHAR(136) 
	DECLARE @UNSIGN_CHARS NCHAR (136) 
	SET @SIGN_CHARS = N'ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệế ìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵý ĂÂĐÊÔƠƯÀẢÃẠÁẰẲẴẶẮẦẨẪẬẤÈẺẼẸÉỀỂỄỆẾÌỈĨỊÍ ÒỎÕỌÓỒỔỖỘỐỜỞỠỢỚÙỦŨỤÚỪỬỮỰỨỲỶỸỴÝ' +NCHAR(272)+ NCHAR(208) 
	SET @UNSIGN_CHARS = N'aadeoouaaaaaaaaaaaaaaaeeeeeeeeee iiiiiooooooooooooooouuuuuuuuuuyyyyy AADEOOUAAAAAAAAAAAAAAAEEEEEEEEEEIIIII OOOOOOOOOOOOOOOUUUUUUUUUUYYYYYDD' 
	DECLARE @COUNTER int 
	DECLARE @COUNTER1 int 
	SET @COUNTER = 1 
	WHILE (@COUNTER <=LEN(@strInput)) 
	BEGIN 
		SET @COUNTER1 = 1 
		WHILE (@COUNTER1 <=LEN(@SIGN_CHARS)+1) 
			BEGIN 
			IF UNICODE(SUBSTRING(@SIGN_CHARS, @COUNTER1,1)) = UNICODE(SUBSTRING(@strInput,@COUNTER ,1) ) 
				BEGIN 
				IF @COUNTER=1 SET @strInput = SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)-1) 
				ELSE SET @strInput = SUBSTRING(@strInput, 1, @COUNTER-1) +SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)- @COUNTER) BREAK END 
				SET @COUNTER1 = @COUNTER1 +1 END 
				SET @COUNTER = @COUNTER +1 END 
				SET @strInput = replace(@strInput,' ','-') 
				RETURN @strInput 
END 
GO

--SELECT * FROM dbo.Food WHERE dbo.fuConvertToUnsign1(name) LIKE N'%' + dbo.fuConvertToUnsign1(N'{0}') + '%'", name
CREATE PROC USP_DeleteTable -- xóa bàn
@idTable INT 
AS
BEGIN
	DELETE dbo.BillInfo FROM  dbo.Bill as b, dbo.BillInfo as bi WHERE bi.idBill = b.id AND b.idTable = @idTable
	DELETE dbo.Bill   WHERE  idTable = @idTable
	DELETE dbo.TableFood WHERE id = @idTable
END
GO

-- Proc để lấy bill theo ngày và trang
CREATE PROC USP_GetListBillByDateAndPage
 @checkIn date, @checkOut date , @page int
 AS
 BEGIN
	DECLARE @pageRows INT = 10 -- số dòng trong 1 page
	DECLARE @selectRows INT = @pageRows  -- số dòng lựa chọn  = số dòng trong 1 page * số page
	DECLARE @exceptRows INT = (@page - 1 ) * @pageRows 

	;WITH BillShow AS( SELECT t.name as 'Tên bàn' , b.totalPrice as 'Tổng tiền' , DateCheckIn as 'Ngày tạo' , DateCheckOut as 'Ngày xuất đơn' , discount as 'Giảm giá (%)', b.id as 'ID' 
	FROM dbo.Bill AS b,dbo.TableFood AS t
	WHERE DateCheckIn >= @checkIn AND DateCheckOut <= @checkOut AND b.status = 1 AND t.id = b.idTable)

	SELECT TOP (@selectRows) * FROM BillShow WHERE ID NOT in (SELECT TOP(@exceptRows) ID FROM BillShow)

 END
 GO


 CREATE PROC USP_GetNumBillByDate
 @checkIn date, @checkOut date
 AS
 BEGIN
	SELECT COUNT(*) 
	FROM dbo.Bill AS b,dbo.TableFood AS t
	WHERE DateCheckIn >= @checkIn AND DateCheckOut <= @checkOut AND b.status = 1 AND t.id = b.idTable
 END
 GO
 --exec USP_GetListBillByDate @checkIn = '2022-5-1', @checkOut = '2022-5-30'
 --exec USP_GetListBillByDateAndPage @checkIn = '2022-5-1', @checkOut = '2022-5-30' ,@page = 3
 --SELECT COUNT(id) FROM dbo.UserInfo

 -- gộp bàn
 CREATE PROC USP_MergeTable
 @idTable1 INT,@idTable2 INT
 AS
 BEGIN
	
	DECLARE @idFirstBill int
	DECLARE @idSecondBill int

	SELECT @idSecondBill = id FROM dbo.Bill WHERE idTable = @idTable2 AND status = 0
	SELECT @idFirstBill = id FROM dbo.Bill WHERE idTable = @idTable1 AND status = 0

	IF(@idFirstBill IS NULL)
	BEGIN
		INSERT dbo.Bill(DateCheckIn,DateCheckOut,idTable,status,discount) VALUES(GETDATE(),NULL,@idTable1,0,0)

		SELECT @idFirstBill = MAX(id) FROM dbo.Bill  WHERE idTable = @idTable1 AND status = 0
	END
	
	IF(@idSecondBill IS NULL)
	BEGIN
		INSERT dbo.Bill(DateCheckIn,DateCheckOut,idTable,status) VALUES(GETDATE(),NULL,@idTable2,0)

		SELECT @idSecondBill = MAX(id) FROM dbo.Bill  WHERE idTable = @idTable2 AND status = 0
	END

	SELECT id INTO IDBillInfoTable FROM dbo.BillInfo WHERE idBill = @idSecondBill

	UPDATE dbo.BillInfo SET idBill = @idSecondBill WHERE idBill = @idFirstBill

	

	UPDATE dbo.TableFood set status = N'Trống' WHERE id = @idTable1 --đổi status mỗi khi chuyển bàn

	UPDATE dbo.BillInfo SET idBill = @idFirstBill WHERE id IN (SELECT * FROM IDBillInfoTable)

	DROP TABLE IDBillInfoTable

 END
 GO
