CREATE TABLE Menus (
    --Id BIGINT AUTO_INCREMENT PRIMARY KEY,             --
    MenuAdi VARCHAR(255) NOT NULL,                      -- Menu Adý 
    MainMenuId BIGINT,                                  -- Üst menü Id
    MenuRoot  VARCHAR(255),                             -- Menu adýmýnýn açaðý sayfanýn rootu
    MenuActive BIT DEFAULT 1,                           -- Menu durum 1-Active 2-pasif
    CratedDate DATETIME,                                -- oluþturulma tarihi
    ModifiedDate DATETIME,                              -- deðiþtirilme tarihi
    DateStatus TINYINT,                                 -- 1-Insert, 2-Update
    --UNIQUE INDEX idx_Menu01 (MenuAdi),                --
    INDEX idx_Menu02 (Id, MainMenuId)                   --
);