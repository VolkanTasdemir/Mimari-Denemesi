CREATE TABLE Categorys (
    ID BIGINT ,                                 
    MainCategoryID BIGINT NULL,                 --Ana Kategori Id
    CategoryCode VARCHAR(10) NOT NULL,          --kategori kodu
    CategoryName VARCHAR(100) NOT NULL,         --kategori adý
    CategoryDesciription LONGTEXT,              --kategori açýklamasý
    CategoryJson LONGTEXT,                      --kategori detay json kaydedilecek sektöre göre deðiþiklþik götere bilir 
    CategoryActive BIT DEFAULT 1,               --Kategori durum 1-Active 2-pasif
    CratedDate DATETIME,                        --oluþturulma tarihi
    ModifiedDate DATETIME,                      --deðiþtirilme tarihi
    DateStatus TINYINT,                         -- 1-Insert, 2-Update
    CONSTRAINT CategorysC01 PRIMARY KEY (Id)
);

CREATE  UNIQUE INDEX CategorysUniqueIndex ON Categorys (CategoryCode,CategoryName) 