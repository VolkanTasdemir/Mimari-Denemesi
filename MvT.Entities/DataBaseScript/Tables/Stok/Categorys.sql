CREATE TABLE Categorys (
    ID BIGINT ,                                 
    MainCategoryID BIGINT NULL,                 --Ana Kategori Id
    CategoryCode VARCHAR(10) NOT NULL,          --kategori kodu
    CategoryName VARCHAR(100) NOT NULL,         --kategori ad�
    CategoryDesciription LONGTEXT,              --kategori a��klamas�
    CategoryJson LONGTEXT,                      --kategori detay json kaydedilecek sekt�re g�re de�i�ikl�ik g�tere bilir 
    CategoryActive BIT DEFAULT 1,               --Kategori durum 1-Active 2-pasif
    CratedDate DATETIME,                        --olu�turulma tarihi
    ModifiedDate DATETIME,                      --de�i�tirilme tarihi
    DateStatus TINYINT,                         -- 1-Insert, 2-Update
    CONSTRAINT CategorysC01 PRIMARY KEY (Id)
);

CREATE  UNIQUE INDEX CategorysUniqueIndex ON Categorys (CategoryCode,CategoryName) 