-- ==========================================
-- TẠO DATABASE
-- ==========================================
CREATE DATABASE DeviceMaintenanceDB;
GO
USE DeviceMaintenanceDB;
GO

-- ==========================================
-- BẢNG NGƯỜI DÙNG (USERS)
-- ==========================================
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    Role NVARCHAR(50) CHECK (Role IN ('Admin', 'Technician', 'Staff')),
    Phone NVARCHAR(20),
    SkillLevel NVARCHAR(50),         -- Nếu là kỹ thuật viên
    Certifications NVARCHAR(255),    -- Nếu là kỹ thuật viên
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- ==========================================
-- BẢNG TÀI SẢN (ASSETS)
-- ==========================================
CREATE TABLE Assets (
    AssetID INT IDENTITY(1,1) PRIMARY KEY,
    AssetName NVARCHAR(100) NOT NULL,
    SerialNumber NVARCHAR(100) UNIQUE NOT NULL,
    Location NVARCHAR(255),
    PurchaseDate DATE,
    Status NVARCHAR(50) CHECK (Status IN ('Active', 'Inactive', 'UnderMaintenance', 'Decommissioned')),
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- ==========================================
-- BẢNG BẢO HÀNH (WARRANTIES)
-- ==========================================
CREATE TABLE Warranties (
    WarrantyID INT IDENTITY(1,1) PRIMARY KEY,
    AssetID INT NOT NULL,
    WarrantyProvider NVARCHAR(100),
    StartDate DATE,
    EndDate DATE,
    Terms NVARCHAR(255),
    FOREIGN KEY (AssetID) REFERENCES Assets(AssetID)
);

-- ==========================================
-- BẢNG LỊCH BẢO TRÌ (SCHEDULES)
-- ==========================================
CREATE TABLE Schedules (
    ScheduleID INT IDENTITY(1,1) PRIMARY KEY,
    AssetID INT NOT NULL,
    MaintenanceType NVARCHAR(50) CHECK (MaintenanceType IN ('Monthly', 'Quarterly', 'Yearly')),
    NextMaintenanceDate DATE NOT NULL,
    LastMaintenanceDate DATE,
    Checklist NVARCHAR(MAX),
    FOREIGN KEY (AssetID) REFERENCES Assets(AssetID)
);

-- ==========================================
-- BẢNG SỰ CỐ (TICKETS)
-- ==========================================
CREATE TABLE Tickets (
    TicketID INT IDENTITY(1,1) PRIMARY KEY,
    AssetID INT NOT NULL,
    CreatedBy INT NOT NULL,             -- Người tạo (user nội bộ)
    AssignedTo INT NULL,                -- Người được phân công (kỹ thuật viên)
    Priority NVARCHAR(50) CHECK (Priority IN ('Low', 'Medium', 'High', 'Critical')),
    SLAHours INT,
    IssueDescription NVARCHAR(MAX),
    Status NVARCHAR(50) CHECK (Status IN ('Open', 'Assigned', 'Resolved', 'Closed')),
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (AssetID) REFERENCES Assets(AssetID),
    FOREIGN KEY (CreatedBy) REFERENCES Users(UserID),
    FOREIGN KEY (AssignedTo) REFERENCES Users(UserID)
);

-- ==========================================
-- BẢNG WORK ORDERS (LỆNH CÔNG VIỆC)
-- ==========================================
CREATE TABLE WorkOrders (
    WorkOrderID INT IDENTITY(1,1) PRIMARY KEY,
    ScheduleID INT NULL,                -- PM
    TicketID INT NULL,                  -- CM
    AssetID INT NOT NULL,
    AssignedTo INT NULL,                -- Người phụ trách (kỹ thuật viên)
    WorkType NVARCHAR(50) CHECK (WorkType IN ('Preventive', 'Corrective')),
    Description NVARCHAR(MAX),
    Status NVARCHAR(50) CHECK (Status IN ('Pending', 'In Progress', 'Completed', 'Cancelled')),
    CreatedAt DATETIME DEFAULT GETDATE(),
    CompletedAt DATETIME NULL,
    FOREIGN KEY (AssetID) REFERENCES Assets(AssetID),
    FOREIGN KEY (AssignedTo) REFERENCES Users(UserID),
    FOREIGN KEY (ScheduleID) REFERENCES Schedules(ScheduleID),
    FOREIGN KEY (TicketID) REFERENCES Tickets(TicketID)
);

-- ==========================================
-- BẢNG LINH KIỆN (PARTS)
-- ==========================================
CREATE TABLE Parts (
    PartID INT IDENTITY(1,1) PRIMARY KEY,
    PartName NVARCHAR(100) NOT NULL,
    PartCode NVARCHAR(100) UNIQUE,
    StockQuantity INT DEFAULT 0,
    UnitPrice DECIMAL(10,2),
    Location NVARCHAR(100)
);

-- ==========================================
-- BẢNG SỬ DỤNG LINH KIỆN (PART USAGES)
-- ==========================================
CREATE TABLE PartUsages (
    PartUsageID INT IDENTITY(1,1) PRIMARY KEY,
    WorkOrderID INT NOT NULL,
    PartID INT NOT NULL,
    QuantityUsed INT NOT NULL,
    UsedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (WorkOrderID) REFERENCES WorkOrders(WorkOrderID),
    FOREIGN KEY (PartID) REFERENCES Parts(PartID)
);
