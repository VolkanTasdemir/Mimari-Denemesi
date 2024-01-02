CREATE TABLE Menus (
    --Id BIGINT AUTO_INCREMENT PRIMARY KEY,             --
    MenuAdi VARCHAR(255) NOT NULL,                      -- Menu Ad� 
    MainMenuId BIGINT,                                  -- �st men� Id
    MenuRoot  VARCHAR(255),                             -- Menu ad�m�n�n a�a�� sayfan�n rootu
    MenuActive BIT DEFAULT 1,                           -- Menu durum 1-Active 2-pasif
    CratedDate DATETIME,                                -- olu�turulma tarihi
    ModifiedDate DATETIME,                              -- de�i�tirilme tarihi
    DateStatus TINYINT,                                 -- 1-Insert, 2-Update
    --UNIQUE INDEX idx_Menu01 (MenuAdi),                --
    INDEX idx_Menu02 (Id, MainMenuId)                   --
);