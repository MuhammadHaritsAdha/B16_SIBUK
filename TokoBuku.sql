CREATE DATABASE TokoBukuDB;
GO

USE TokoBukuDB;
GO

CREATE TABLE Users (
    userId INT IDENTITY(1,1) PRIMARY KEY,
    nama VARCHAR(100),
    username VARCHAR(50),
    password VARCHAR(100),
    role VARCHAR(20)
);

CREATE TABLE Buku (
    bukuId INT IDENTITY(1,1) PRIMARY KEY,
    judul VARCHAR(150),
    pengarang VARCHAR(100),
    penerbit VARCHAR(100),
    hargaSatuan FLOAT,
    stok INT
);

CREATE TABLE Transaksi (
    transaksiId INT IDENTITY(1,1) PRIMARY KEY,
    tanggal DATE,
    userId INT,
    totalHarga FLOAT,
    statusBayar VARCHAR(20),

    FOREIGN KEY (userId) REFERENCES Users(userId)
);

CREATE TABLE Detail_Transaksi (
    detailId INT IDENTITY(1,1) PRIMARY KEY,
    transaksiId INT,
    bukuId INT,
    jumlah INT,
    subTotal FLOAT,

    FOREIGN KEY (transaksiId) REFERENCES Transaksi(transaksiId),
    FOREIGN KEY (bukuId) REFERENCES Buku(bukuId)
);

INSERT INTO Users (nama, username, password, role)
VALUES 
('Daffa', 'daffatampan', '123', 'admin'),
('Vandi', 'pesonavandi', '123', 'pegawai');

INSERT INTO Buku (judul, pengarang, penerbit, hargaSatuan, stok)
VALUES
('Pulang', 'Tere Liye', 'Sabakgrip', 70000, 10),
('Atomic Habits', 'James Clear', 'Gramedia', 80000, 5);


SELECT * FROM Users

SELECT*FROM Transaksi

SELECT * FROM Buku

-- 1. SP SELECT SEMUA DATA
CREATE PROCEDURE sp_GetBuku
AS
BEGIN
    SET NOCOUNT ON;
    SELECT bukuId, judul, pengarang, penerbit, hargaSatuan, stok FROM Buku
END
GO

-- 2. SP INSERT
CREATE PROCEDURE sp_InsertBuku
    @judul VARCHAR(100),
    @pengarang VARCHAR(50),
    @penerbit VARCHAR(50),
    @harga INT,
    @stok INT
AS
BEGIN
    SET NOCOUNT ON;
    -- Logika tambahan: Cek duplikat sebelum insert
    IF EXISTS (SELECT 1 FROM Buku WHERE judul = @judul AND pengarang = @pengarang)
    BEGIN
        RAISERROR ('Buku dengan judul dan pengarang tersebut sudah ada!', 16, 1);
    END
    ELSE
    BEGIN
        INSERT INTO Buku (judul, pengarang, penerbit, hargaSatuan, stok)
        VALUES (@judul, @pengarang, @penerbit, @harga, @stok);
    END
END
GO

-- 3. SP UPDATE
CREATE PROCEDURE sp_UpdateBuku
    @id INT,
    @judul VARCHAR(100),
    @pengarang VARCHAR(50),
    @penerbit VARCHAR(50),
    @harga INT,
    @stok INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Buku 
    SET judul=@judul, pengarang=@pengarang, penerbit=@penerbit, 
        hargaSatuan=@harga, stok=@stok
    WHERE bukuId=@id;
END
GO

-- 4. SP DELETE
CREATE PROCEDURE sp_DeleteBuku
    @id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Buku WHERE bukuId=@id;
END
GO

-- 5. SP SEARCH
CREATE PROCEDURE sp_SearchBuku
    @keyword VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM Buku 
    WHERE judul LIKE '%' + @keyword + '%' OR pengarang LIKE '%' + @keyword + '%'
END
GO


CREATE PROCEDURE sp_SimpanTransaksi
    @userId INT,
    @total INT,
    @status VARCHAR(20),
    @items XML -- Mengirim data keranjang dalam format XML agar efisien
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        -- 1. Insert ke Tabel Transaksi (Header)
        DECLARE @transaksiId INT;
        INSERT INTO Transaksi (tanggal, userId, totalHarga, statusBayar)
        VALUES (GETDATE(), @userId, @total, @status);
        
        SET @transaksiId = SCOPE_IDENTITY();

        -- 2. Insert ke Detail & Update Stok (Looping via XML)
        -- Ini menggantikan foreach yang berkali-kali buka koneksi di C#
        INSERT INTO Detail_Transaksi (transaksiId, bukuId, jumlah, subTotal)
        SELECT 
            @transaksiId,
            T.Item.value('@bukuId', 'INT'),
            T.Item.value('@jumlah', 'INT'),
            T.Item.value('@subtotal', 'INT')
        FROM @items.nodes('/root/item') AS T(Item);

        -- 3. Update Stok Otomatis
        UPDATE B
        SET B.stok = B.stok - T.jumlah
        FROM Buku B
        INNER JOIN (
            SELECT 
                T.Item.value('@bukuId', 'INT') as id,
                T.Item.value('@jumlah', 'INT') as jumlah
            FROM @items.nodes('/root/item') AS T(Item)
        ) T ON B.bukuId = T.id;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END

CREATE PROCEDURE sp_GetBukuSimple
AS
BEGIN
    SELECT bukuId, judul, hargaSatuan FROM Buku;
END

CREATE VIEW vw_UserLogin AS
SELECT userId, username, password, role 
FROM Users;
GO

CREATE VIEW vw_StokBuku AS
SELECT bukuId, judul, stok 
FROM Buku;
GO



-- B. STORED PROCEDURE untuk menghitung Total dan Jumlah (Output Parameter)
-- Sesuai dengan contoh "COUNT (OUTPUT)" di modul praktikum
CREATE PROCEDURE sp_GetSummaryLaporan
    @awal DATE,
    @akhir DATE,
    @TotalHarga INT OUTPUT,
    @JumlahTransaksi INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    -- Mengambil data dari VIEW vw_TransaksiLaporan, bukan tabel Transaksi
    SELECT 
        @TotalHarga = ISNULL(SUM(totalHarga), 0), 
        @JumlahTransaksi = COUNT(transaksiId)
    FROM vw_TransaksiLaporan 
    WHERE tanggal BETWEEN @awal AND @akhir
END
GO



-- Hapus jika sudah ada, lalu buat ulang
IF EXISTS (SELECT * FROM sys.views WHERE name = 'vw_LaporanDetail')
    DROP VIEW vw_LaporanDetail;
GO

CREATE VIEW vw_LaporanDetail AS
SELECT 
    t.transaksiId,
    t.tanggal,
    b.judul,
    dt.jumlah,
    dt.subTotal, -- Pastikan nama kolom ini 'subTotal'
    t.totalHarga,
    t.statusBayar
FROM Transaksi t
JOIN Detail_Transaksi dt ON t.transaksiId = dt.transaksiId
JOIN Buku b ON dt.bukuId = b.bukuId;
GO

SELECT * FROM sys.views

-- Query untuk membuat VIEW Laporan Transaksi
CREATE VIEW vw_TransaksiLaporan AS
SELECT 
    t.transaksiId,
    t.tanggal,
    u.username,
    t.totalHarga,
    t.statusBayar
FROM Transaksi t
JOIN Users u ON t.userId = u.userId;
GO

-- Membuat VIEW untuk menampilkan data buku secara terbatas
CREATE VIEW vw_BukuPublic AS
SELECT 
    bukuId, 
    judul, 
    pengarang, 
    penerbit, 
    hargaSatuan, 
    stok
FROM Buku;
GO

-- Membuat cadangan data asli (Langkah 9a Modul 9)
SELECT * INTO Buku_Backup FROM Buku;

select * from users

USE TokoBukuDB; -- Sesuaikan nama database lu jika berbeda
GO

CREATE PROCEDURE sp_GetLaporanCrystal
    @awal DATETIME,
    @akhir DATETIME,
    @judul VARCHAR(150)
AS
BEGIN
    SET NOCOUNT ON;

    -- Kolom disamakan persis dengan isi SELECT pada vw_LaporanDetail lu
    SELECT 
        transaksiId,
        tanggal,
        judul,
        jumlah,
        subTotal,     -- Menggunakan nama asli camelCase bawaan view lu
        totalHarga,
        statusBayar
    FROM vw_LaporanDetail 
    WHERE 
        (CAST(tanggal AS DATE) BETWEEN CAST(@awal AS DATE) AND CAST(@akhir AS DATE)) 
        AND (judul LIKE '%' + TRIM(@judul) + '%');
END
GO

CREATE TRIGGER trg_CekStokSebelumPenjualan
ON Detail_Transaksi
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- Validasi T-SQL: Cek apakah ada buku yang jumlah belinya > stok tersedia saat ini
    IF EXISTS (
        SELECT 1 
        FROM Buku B
        INNER JOIN inserted i ON B.bukuId = i.bukuId
        WHERE B.stok < i.jumlah
    )
    BEGIN
        -- Lempar error secara paksa keluar sistem jika stok tidak mencukupi
        RAISERROR('Transaksi dibatalkan otomatis oleh TRIGGER karena stok buku tidak mencukupi!', 16, 1);
        
        -- Gagalkan transaksi induk di Stored Procedure
        IF @@TRANCOUNT > 0 
            ROLLBACK TRANSACTION;
    END
    ELSE
    BEGIN
        -- Jika lolos validasi stok, teruskan perintah INSERT asli ke tabel Detail_Transaksi
        INSERT INTO Detail_Transaksi (transaksiId, bukuId, jumlah, subTotal)
        SELECT transaksiId, bukuId, jumlah, subTotal FROM inserted;
    END
END;
GO

ALTER VIEW vw_LaporanDetail AS
SELECT 
    t.transaksiId,
    t.tanggal,
    b.judul,
    dt.jumlah,
    b.hargaSatuan AS hargaSatuan,
    t.totalHarga,
    t.statusBayar
FROM Transaksi t
JOIN Detail_Transaksi dt ON t.transaksiId = dt.transaksiId
JOIN Buku b ON dt.bukuId = b.bukuId;
GO

ALTER PROCEDURE sp_GetLaporanCrystal
    @awal DATE,
    @akhir DATE,
    @judul VARCHAR(100)
AS
BEGIN
    SELECT 
        transaksiId,
        tanggal,
        judul,
        jumlah,
        hargaSatuan, 
        totalHarga,
        statusBayar
    FROM vw_LaporanDetail
    WHERE (tanggal BETWEEN @awal AND @akhir)
      AND (judul LIKE '%' + @judul + '%');
END
GO

select * from Buku_Backup;
INSERT INTO Buku_Backup(judul, pengarang, penerbit, hargaSatuan, stok)
VALUES
('Bumi', 'Tere Liye', 'Sabakgrip', 700000, 18),
('Judol', 'Rayhan', 'Kyou', 25000, 8);