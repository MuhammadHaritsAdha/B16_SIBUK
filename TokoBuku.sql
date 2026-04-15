CREATE DATABASE TokoBukuDB;
GO

USE TokoBukuDB;
GO

CREATE TABLE Users (
    userId INT IDENTITY(1,1) PRIMARY KEY,
    nama VARCHAR(100),
    username VARCHAR(50),
    password VARCHAR(100),
    noTelp VARCHAR(20),
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

INSERT INTO Users (nama, username, password, noTelp, role)
VALUES 
('Daffa', 'daffatampan', '123', '08123456789', 'admin'),
('Vandi', 'pesonavandi', '123', '08987654321', 'pegawai');

INSERT INTO Buku (judul, pengarang, penerbit, hargaSatuan, stok)
VALUES
('Pulang', 'Tere Liye', 'Sabakgrip', 70000, 10),
('Atomic Habits', 'James Clear', 'Gramedia', 80000, 5);


SELECT * FROM Users

SELECT*FROM Transaksi

SELECT * FROM Buku