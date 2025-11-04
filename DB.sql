====
﻿

CREATE DATABASE DeviceMaintenanceDB;
GO
USE DeviceMaintenanceDB;
GO






-- 1️. BẢNG NGƯỜI DÙNG (USERS)
-- ==========================================
CREATE TABLE dbo.Users (

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
-- 2️. BẢNG TÀI SẢN (ASSETS) - Soft Delete
-- ==========================================
CREATE TABLE dbo.Assets (
    AssetID INT IDENTITY(1,1) PRIMARY KEY,
    AssetName NVARCHAR(200) NOT NULL,
    SerialNumber NVARCHAR(100) UNIQUE NOT NULL,
    Location NVARCHAR(200) NULL,
    PurchaseDate DATE NULL,
    Status NVARCHAR(50) NOT NULL CHECK (Status IN (N'Active', N'Inactive', N'UnderMaintenance', N'Decommissioned')),
    UsageStatus NVARCHAR(50) NOT NULL CONSTRAINT DF_Assets_UsageStatus DEFAULT N'Active'
        CHECK (UsageStatus IN (N'Active', N'Inactive', N'Decommissioned')),
    CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_Assets_CreatedAt DEFAULT SYSUTCDATETIME()
);
GO

-- ==========================================
-- 3️. BẢNG BẢO HÀNH (WARRANTIES) - CASCADE khi xóa Asset
-- ==========================================
CREATE TABLE dbo.Warranties (
    WarrantyID INT IDENTITY(1,1) PRIMARY KEY,
    AssetID INT NOT NULL,
    WarrantyProvider NVARCHAR(100) NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NULL,
    Terms NVARCHAR(255) NULL,
    Status NVARCHAR(50) NOT NULL CONSTRAINT DF_Warranties_Status DEFAULT N'Active'
        CHECK (Status IN (N'Active', N'Expired', N'Cancelled', N'Pending')),
    CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_Warranties_CreatedAt DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Warranties_Assets FOREIGN KEY (AssetID) 
        REFERENCES dbo.Assets(AssetID) ON DELETE CASCADE
);
GO

-- ==========================================
-- 4️. BẢNG LỊCH BẢO TRÌ (SCHEDULES) - Soft Delete + CASCADE
-- ==========================================
CREATE TABLE dbo.Schedules (
    ScheduleID INT IDENTITY(1,1) PRIMARY KEY,
    AssetID INT NOT NULL,
    MaintenanceType NVARCHAR(50) NOT NULL CHECK (MaintenanceType IN (N'Monthly', N'Quarterly', N'Yearly')),
    NextMaintenanceDate DATE NOT NULL,
    LastMaintenanceDate DATE NULL,
    Checklist NVARCHAR(MAX) NULL,
    UsageStatus NVARCHAR(50) NOT NULL CONSTRAINT DF_Schedules_UsageStatus DEFAULT N'Active'
        CHECK (UsageStatus IN (N'Active', N'Inactive', N'Completed')),
    CONSTRAINT FK_Schedules_Assets FOREIGN KEY (AssetID) 
        REFERENCES dbo.Assets(AssetID) ON DELETE CASCADE
);
GO

-- ==========================================
-- 5️. BẢNG SỰ CỐ (TICKETS) - CASCADE khi xóa Asset
-- ==========================================
CREATE TABLE dbo.Tickets (
    TicketID INT IDENTITY(1,1) PRIMARY KEY,
    AssetID INT NOT NULL,
    CreatedBy INT NOT NULL,
    AssignedTo INT NULL,
    Priority NVARCHAR(50) NOT NULL CHECK (Priority IN (N'Low', N'Medium', N'High', N'Critical')),
    SLAHours INT NULL,
    IssueDescription NVARCHAR(MAX) NULL,
    Status NVARCHAR(50) NOT NULL CONSTRAINT DF_Tickets_Status DEFAULT N'Open'
        CHECK (Status IN (N'Open', N'Assigned', N'Resolved', N'Closed')),
    CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_Tickets_CreatedAt DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Tickets_Assets FOREIGN KEY (AssetID) 
        REFERENCES dbo.Assets(AssetID) ON DELETE CASCADE,
    CONSTRAINT FK_Tickets_CreatedBy FOREIGN KEY (CreatedBy) 
        REFERENCES dbo.Users(UserID),
    CONSTRAINT FK_Tickets_AssignedTo FOREIGN KEY (AssignedTo) 
        REFERENCES dbo.Users(UserID)
);
GO

-- ==========================================
-- 6️. BẢNG LỆNH CÔNG VIỆC (WORKORDERS) - Soft Delete + CASCADE
-- ==========================================
CREATE TABLE dbo.WorkOrders (
    WorkOrderID INT IDENTITY(1,1) PRIMARY KEY,
    ScheduleID INT NULL,
    TicketID INT NULL,
    AssetID INT NOT NULL,
    AssignedTo INT NULL,
    WorkType NVARCHAR(50) NOT NULL CHECK (WorkType IN (N'Preventive', N'Corrective')),
    Description NVARCHAR(MAX) NULL,
    Status NVARCHAR(50) NOT NULL CONSTRAINT DF_WorkOrders_Status DEFAULT N'Pending'
        CHECK (Status IN (N'Pending', N'In Progress', N'Completed', N'Cancelled')),
    UsageStatus NVARCHAR(50) NOT NULL CONSTRAINT DF_WorkOrders_UsageStatus DEFAULT N'Active'
        CHECK (UsageStatus IN (N'Active', N'Inactive', N'Cancelled')),
    CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_WorkOrders_CreatedAt DEFAULT SYSUTCDATETIME(),
    CompletedAt DATETIME2 NULL,
    CONSTRAINT FK_WorkOrders_Assets FOREIGN KEY (AssetID) 
        REFERENCES dbo.Assets(AssetID) ON DELETE CASCADE,
    CONSTRAINT FK_WorkOrders_AssignedTo FOREIGN KEY (AssignedTo) 
        REFERENCES dbo.Users(UserID),
    CONSTRAINT FK_WorkOrders_Schedules FOREIGN KEY (ScheduleID) 
        REFERENCES dbo.Schedules(ScheduleID) ON DELETE NO ACTION,
    CONSTRAINT FK_WorkOrders_Tickets FOREIGN KEY (TicketID) 
        REFERENCES dbo.Tickets(TicketID) ON DELETE NO ACTION
);
GO

-- ==========================================
-- 7️. BẢNG LINH KIỆN (PARTS) - Soft Delete
-- ==========================================
CREATE TABLE dbo.Parts (
    PartID INT IDENTITY(1,1) PRIMARY KEY,
    PartName NVARCHAR(100) NOT NULL,
    PartCode NVARCHAR(100) UNIQUE NULL,
    StockQuantity INT NOT NULL CONSTRAINT DF_Parts_StockQuantity DEFAULT 0,
    UnitPrice DECIMAL(10,2) NULL,
    Location NVARCHAR(100) NULL,
    UsageStatus NVARCHAR(50) NOT NULL CONSTRAINT DF_Parts_UsageStatus DEFAULT N'Active'
        CHECK (UsageStatus IN (N'Active', N'Inactive', N'Decommissioned'))
);
GO

-- ==========================================
-- 8️⃣ BẢNG SỬ DỤNG LINH KIỆN (PARTUSAGES) - CASCADE khi xóa WorkOrder
-- ==========================================
CREATE TABLE dbo.PartUsages (
    PartUsageID INT IDENTITY(1,1) PRIMARY KEY,
    WorkOrderID INT NOT NULL,
    PartID INT NOT NULL,
    QuantityUsed INT NOT NULL CHECK (QuantityUsed > 0),
    CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_PartUsages_CreatedAt DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_PartUsages_WorkOrders FOREIGN KEY (WorkOrderID) 
        REFERENCES dbo.WorkOrders(WorkOrderID) ON DELETE CASCADE,
    CONSTRAINT FK_PartUsages_Parts FOREIGN KEY (PartID) 
        REFERENCES dbo.Parts(PartID)
);
GO

-- ==========================================
-- INDEXES
-- ==========================================
CREATE INDEX IX_Users_IsDeleted ON dbo.Users(IsDeleted);
CREATE INDEX IX_Assets_UsageStatus ON dbo.Assets(UsageStatus);
CREATE INDEX IX_Warranties_AssetID ON dbo.Warranties(AssetID);
CREATE INDEX IX_Schedules_AssetID_UsageStatus ON dbo.Schedules(AssetID, UsageStatus);
CREATE INDEX IX_Tickets_AssetID ON dbo.Tickets(AssetID);
CREATE INDEX IX_Tickets_CreatedBy ON dbo.Tickets(CreatedBy);
CREATE INDEX IX_Tickets_AssignedTo ON dbo.Tickets(AssignedTo);
CREATE INDEX IX_WorkOrders_AssetID_UsageStatus ON dbo.WorkOrders(AssetID, UsageStatus);
CREATE INDEX IX_WorkOrders_AssignedTo ON dbo.WorkOrders(AssignedTo);
CREATE INDEX IX_Parts_UsageStatus ON dbo.Parts(UsageStatus);
CREATE INDEX IX_PartUsages_WorkOrderID ON dbo.PartUsages(WorkOrderID);
CREATE INDEX IX_PartUsages_PartID ON dbo.PartUsages(PartID);
GO

-- ==========================================
-- STORED PROCEDURES: USERS (Soft Delete)
-- ==========================================
CREATE PROCEDURE dbo.sp_users_get_by_id @UserID INT
AS BEGIN SET NOCOUNT ON;
    SELECT * FROM dbo.Users WHERE UserID = @UserID;
END
GO

CREATE  PROCEDURE dbo.sp_users_get_all
AS BEGIN SET NOCOUNT ON;
    SELECT * FROM dbo.Users WHERE IsDeleted = 0 ORDER BY FullName;
END
GO

CREATE  PROCEDURE dbo.sp_users_create
    @FullName NVARCHAR(100), @Email NVARCHAR(100), @PasswordHash NVARCHAR(255),
    @Role NVARCHAR(50), @Phone NVARCHAR(20) = NULL, @SkillLevel NVARCHAR(50) = NULL, @Certifications NVARCHAR(255) = NULL
AS BEGIN SET NOCOUNT ON;
    INSERT INTO dbo.Users (FullName, Email, PasswordHash, Role, Phone, SkillLevel, Certifications)
    VALUES (@FullName, @Email, @PasswordHash, @Role, @Phone, @SkillLevel, @Certifications);
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS NewUserID;
END
GO

CREATE  PROCEDURE dbo.sp_users_update
    @UserID INT, @FullName NVARCHAR(100), @Email NVARCHAR(100), @Role NVARCHAR(50),
    @Phone NVARCHAR(20) = NULL, @SkillLevel NVARCHAR(50) = NULL, @Certifications NVARCHAR(255) = NULL
AS BEGIN SET NOCOUNT ON;
    UPDATE dbo.Users SET FullName=@FullName, Email=@Email, Role=@Role, Phone=@Phone, SkillLevel=@SkillLevel, Certifications=@Certifications WHERE UserID=@UserID AND IsDeleted=0;
    SELECT @@ROWCOUNT;
END
GO

CREATE PROCEDURE dbo.sp_users_delete @UserID INT
AS BEGIN SET NOCOUNT ON;
    -- Soft delete
    UPDATE dbo.Users SET IsDeleted = 1 WHERE UserID=@UserID;
    SELECT @@ROWCOUNT;
END
GO

-- ==========================================
-- STORED PROCEDURES: ASSETS (Soft Delete)
-- ==========================================
CREATE PROCEDURE dbo.sp_Assets_get_by_id @AssetID INT
AS BEGIN SET NOCOUNT ON;
    SELECT * FROM dbo.Assets WHERE AssetID = @AssetID;
END
GO

CREATE  PROCEDURE dbo.sp_Assets_get_all
AS BEGIN SET NOCOUNT ON;
    SELECT * FROM dbo.Assets WHERE UsageStatus != N'Decommissioned' ORDER BY CreatedAt DESC;
END
GO

CREATE  PROCEDURE dbo.sp_Assets_create
    @AssetName NVARCHAR(200), 
	@SerialNumber NVARCHAR(100), 
	@Location NVARCHAR(200) = NULL,
    @PurchaseDate DATE = NULL, 
	@Status NVARCHAR(50), 
	@UsageStatus NVARCHAR(50) = N'Active'

AS BEGIN SET NOCOUNT ON;
    INSERT INTO dbo.Assets (AssetName, SerialNumber, Location, PurchaseDate, Status, UsageStatus)
    VALUES (@AssetName, @SerialNumber, @Location, @PurchaseDate, @Status, @UsageStatus);
    SELECT CAST(SCOPE_IDENTITY() AS INT);
END
GO

CREATE  PROCEDURE dbo.sp_Assets_update
    @AssetID INT, @AssetName NVARCHAR(200), @SerialNumber NVARCHAR(100), @Location NVARCHAR(200) = NULL,
    @PurchaseDate DATE = NULL, @Status NVARCHAR(50), @UsageStatus NVARCHAR(50)
AS BEGIN SET NOCOUNT ON;
    UPDATE dbo.Assets SET AssetName=@AssetName, SerialNumber=@SerialNumber, Location=@Location, PurchaseDate=@PurchaseDate, Status=@Status, UsageStatus=@UsageStatus WHERE AssetID=@AssetID;
    SELECT @@ROWCOUNT;
END
GO

CREATE  PROCEDURE dbo.sp_Assets_delete @AssetID INT
AS BEGIN SET NOCOUNT ON;
    -- Soft delete: chuyển sang Decommissioned
    UPDATE dbo.Assets SET UsageStatus = N'Decommissioned' WHERE AssetID=@AssetID;
    SELECT @@ROWCOUNT;
END
GO

-- Proc xóa vĩnh viễn (chỉ Admin gọi)
CREATE  PROCEDURE dbo.sp_Assets_hard_delete @AssetID INT
AS BEGIN SET NOCOUNT ON;
    -- Xóa vật lý, CASCADE sẽ tự xóa Warranties/Schedules/Tickets/WorkOrders/PartUsages
    DELETE FROM dbo.Assets WHERE AssetID=@AssetID AND UsageStatus = N'Decommissioned';
    SELECT @@ROWCOUNT;
END
GO

-- ==========================================
-- STORED PROCEDURES: WARRANTIES (CASCADE)
-- ==========================================
CREATE  PROCEDURE dbo.sp_Warranties_get_by_id @WarrantyID INT
AS BEGIN SET NOCOUNT ON;
    SELECT w.*, a.AssetName FROM dbo.Warranties w LEFT JOIN dbo.Assets a ON a.AssetID = w.AssetID WHERE w.WarrantyID=@WarrantyID;
END
GO

CREATE  PROCEDURE dbo.sp_Warranties_get_all
AS BEGIN SET NOCOUNT ON;
    SELECT w.*, a.AssetName FROM dbo.Warranties w LEFT JOIN dbo.Assets a ON a.AssetID = w.AssetID WHERE a.UsageStatus != N'Decommissioned' ORDER BY w.CreatedAt DESC;
END
GO

CREATE  PROCEDURE dbo.sp_Warranties_create
    @AssetID INT, @WarrantyProvider NVARCHAR(100) = NULL, @StartDate DATE, @EndDate DATE = NULL, @Terms NVARCHAR(255) = NULL, @Status NVARCHAR(50) = N'Active'
AS BEGIN SET NOCOUNT ON;
    INSERT INTO dbo.Warranties (AssetID, WarrantyProvider, StartDate, EndDate, Terms, Status) VALUES (@AssetID, @WarrantyProvider, @StartDate, @EndDate, @Terms, @Status);
    SELECT CAST(SCOPE_IDENTITY() AS INT);
END
GO

CREATE  PROCEDURE dbo.sp_Warranties_update
    @WarrantyID INT, @AssetID INT, @WarrantyProvider NVARCHAR(100) = NULL, @StartDate DATE, @EndDate DATE = NULL, @Terms NVARCHAR(255) = NULL, @Status NVARCHAR(50)
AS BEGIN SET NOCOUNT ON;
    UPDATE dbo.Warranties SET AssetID=@AssetID, WarrantyProvider=@WarrantyProvider, StartDate=@StartDate, EndDate=@EndDate, Terms=@Terms, Status=@Status WHERE WarrantyID=@WarrantyID;
    SELECT @@ROWCOUNT;
END
GO

CREATE  PROCEDURE dbo.sp_Warranties_delete @WarrantyID INT
AS BEGIN SET NOCOUNT ON;
    DELETE FROM dbo.Warranties WHERE WarrantyID=@WarrantyID;
    SELECT @@ROWCOUNT;
END
GO

-- ==========================================
-- STORED PROCEDURES: SCHEDULES (Soft Delete)
-- ==========================================
CREATE  PROCEDURE dbo.sp_schedules_get_by_id @ScheduleID INT
AS BEGIN SET NOCOUNT ON;
    SELECT s.*, a.AssetName FROM dbo.Schedules s LEFT JOIN dbo.Assets a ON a.AssetID = s.AssetID WHERE s.ScheduleID=@ScheduleID;
END
GO

CREATE  PROCEDURE dbo.sp_schedules_get_all
AS BEGIN SET NOCOUNT ON;
    SELECT s.*, a.AssetName FROM dbo.Schedules s LEFT JOIN dbo.Assets a ON a.AssetID = s.AssetID WHERE s.UsageStatus = N'Active' AND a.UsageStatus != N'Decommissioned' ORDER BY s.NextMaintenanceDate ASC;
END
GO

CREATE PROCEDURE dbo.sp_schedules_create
    @AssetID INT, @MaintenanceType NVARCHAR(50), @NextMaintenanceDate DATE, @LastMaintenanceDate DATE = NULL, @Checklist NVARCHAR(MAX) = NULL, @UsageStatus NVARCHAR(50) = N'Active'
AS BEGIN SET NOCOUNT ON;
    INSERT INTO dbo.Schedules (AssetID, MaintenanceType, NextMaintenanceDate, LastMaintenanceDate, Checklist, UsageStatus) VALUES (@AssetID, @MaintenanceType, @NextMaintenanceDate, @LastMaintenanceDate, @Checklist, @UsageStatus);
END
GO

CREATE  PROCEDURE dbo.sp_schedules_update
    @ScheduleID INT, @AssetID INT, @MaintenanceType NVARCHAR(50), @NextMaintenanceDate DATE, @LastMaintenanceDate DATE = NULL, @Checklist NVARCHAR(MAX) = NULL, @UsageStatus NVARCHAR(50)
AS BEGIN SET NOCOUNT ON;
    UPDATE dbo.Schedules SET AssetID=@AssetID, MaintenanceType=@MaintenanceType, NextMaintenanceDate=@NextMaintenanceDate, LastMaintenanceDate=@LastMaintenanceDate, Checklist=@Checklist, UsageStatus=@UsageStatus WHERE ScheduleID=@ScheduleID;
    SELECT @@ROWCOUNT;
END
GO

CREATE  PROCEDURE dbo.sp_schedules_delete @ScheduleID INT
AS BEGIN SET NOCOUNT ON;
    -- Soft delete
    UPDATE dbo.Schedules SET UsageStatus = N'Completed' WHERE ScheduleID=@ScheduleID;
    SELECT @@ROWCOUNT;
END
GO

-- ==========================================
-- STORED PROCEDURES: TICKETS (CASCADE)
-- ==========================================
CREATE PROCEDURE dbo.sp_ticket_get_by_id @TicketID INT
AS BEGIN SET NOCOUNT ON;
    SELECT t.*, a.AssetName, u1.FullName AS CreatedByName, u2.FullName AS AssignedToName
    FROM dbo.Tickets t
    LEFT JOIN dbo.Assets a ON t.AssetID = a.AssetID
    LEFT JOIN dbo.Users u1 ON t.CreatedBy = u1.UserID
    LEFT JOIN dbo.Users u2 ON t.AssignedTo = u2.UserID
    WHERE t.TicketID = @TicketID;
END
GO

CREATE  PROCEDURE dbo.sp_tickets_get_all
AS BEGIN SET NOCOUNT ON;
    SELECT t.*, a.AssetName, u1.FullName AS CreatedByName, u2.FullName AS AssignedToName
    FROM dbo.Tickets t
    LEFT JOIN dbo.Assets a ON t.AssetID = a.AssetID
    LEFT JOIN dbo.Users u1 ON t.CreatedBy = u1.UserID
    LEFT JOIN dbo.Users u2 ON t.AssignedTo = u2.UserID
    WHERE a.UsageStatus != N'Decommissioned'
    ORDER BY t.CreatedAt DESC;
END
GO

CREATE  PROCEDURE dbo.sp_tickets_create
    @AssetID INT, @CreatedBy INT, @AssignedTo INT = NULL, @Priority NVARCHAR(50), @SLAHours INT = NULL, @IssueDescription NVARCHAR(MAX), @Status NVARCHAR(50) = N'Open'
AS BEGIN SET NOCOUNT ON;
    INSERT INTO dbo.Tickets (AssetID, CreatedBy, AssignedTo, Priority, SLAHours, IssueDescription, Status) VALUES (@AssetID, @CreatedBy, @AssignedTo, @Priority, @SLAHours, @IssueDescription, @Status);
    SELECT CAST(SCOPE_IDENTITY() AS INT);
END
GO

CREATE PROCEDURE dbo.sp_tickets_update
    @TicketID INT, @AssetID INT, @AssignedTo INT = NULL, @Priority NVARCHAR(50), @SLAHours INT = NULL, @IssueDescription NVARCHAR(MAX), @Status NVARCHAR(50)
AS BEGIN SET NOCOUNT ON;
    UPDATE dbo.Tickets SET AssetID=@AssetID, AssignedTo=@AssignedTo, Priority=@Priority, SLAHours=@SLAHours, IssueDescription=@IssueDescription, Status=@Status WHERE TicketID=@TicketID;
    SELECT @@ROWCOUNT;
END
GO

CREATE PROCEDURE dbo.sp_tickets_delete @TicketID INT
AS BEGIN SET NOCOUNT ON;
    DELETE FROM dbo.Tickets WHERE TicketID=@TicketID;
    SELECT @@ROWCOUNT;
END
GO

-- ==========================================
-- STORED PROCEDURES: PARTS (Soft Delete)
-- ==========================================
CREATE  PROCEDURE dbo.sp_parts_get_all
AS BEGIN SET NOCOUNT ON;
    SELECT * FROM dbo.Parts WHERE UsageStatus = N'Active' ORDER BY PartName;
END
GO

CREATE  PROCEDURE dbo.sp_parts_delete @PartID INT
AS BEGIN SET NOCOUNT ON;
    UPDATE dbo.Parts SET UsageStatus = N'Decommissioned' WHERE PartID=@PartID;
    SELECT @@ROWCOUNT;
END
GO

-- ==========================================
-- STORED PROCEDURES: PARTUSAGES (CASCADE)
-- ==========================================
CREATE PROCEDURE dbo.sp_partusages_get_by_id @PartUsageID INT
AS BEGIN SET NOCOUNT ON;
    SELECT pu.*, p.PartName FROM dbo.PartUsages pu LEFT JOIN dbo.Parts p ON p.PartID = pu.PartID WHERE pu.PartUsageID = @PartUsageID;
END
GO

CREATE  PROCEDURE dbo.sp_partusages_get_all
AS BEGIN SET NOCOUNT ON;
    SELECT pu.*, p.PartName FROM dbo.PartUsages pu LEFT JOIN dbo.Parts p ON p.PartID = pu.PartID ORDER BY pu.CreatedAt DESC;
END
GO

CREATE  PROCEDURE dbo.sp_partusages_create
    @WorkOrderID INT, @PartID INT, @QuantityUsed INT
AS BEGIN SET NOCOUNT ON;
    INSERT INTO dbo.PartUsages (WorkOrderID, PartID, QuantityUsed) VALUES (@WorkOrderID, @PartID, @QuantityUsed);
END
GO

CREATE  PROCEDURE dbo.sp_partusages_update
    @PartUsageID INT, @WorkOrderID INT, @PartID INT, @QuantityUsed INT
AS BEGIN SET NOCOUNT ON;
    UPDATE dbo.PartUsages SET WorkOrderID=@WorkOrderID, PartID=@PartID, QuantityUsed=@QuantityUsed WHERE PartUsageID=@PartUsageID;
    SELECT @@ROWCOUNT;
END
GO

CREATE  PROCEDURE dbo.sp_partusages_delete @PartUsageID INT
AS BEGIN SET NOCOUNT ON;
    DELETE FROM dbo.PartUsages WHERE PartUsageID=@PartUsageID;
    SELECT @@ROWCOUNT;
END
GO


-- ==========================================
-- 1️⃣ GET ALL WORKORDERS
-- ==========================================
CREATE  PROCEDURE dbo.sp_workorders_get_all
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        wo.WorkOrderID,
        wo.ScheduleID,
        wo.TicketID,
        wo.AssetID,
        wo.AssignedTo,
        wo.WorkType,
        wo.Description,
        wo.Status,
        wo.UsageStatus,
        wo.CreatedAt,
        wo.CompletedAt,
        a.AssetName,
        u.FullName AS AssignedToName
    FROM dbo.WorkOrders wo
    LEFT JOIN dbo.Assets a ON a.AssetID = wo.AssetID
    LEFT JOIN dbo.Users u ON u.UserID = wo.AssignedTo
    WHERE wo.UsageStatus = N'Active'
    ORDER BY wo.CreatedAt DESC;
END
GO

-- ==========================================
-- 2️⃣ GET WORKORDER BY ID
-- ==========================================
CREATE PROCEDURE dbo.sp_workorders_get_by_id
(
    @WorkOrderID INT
)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        wo.WorkOrderID,
        wo.ScheduleID,
        wo.TicketID,
        wo.AssetID,
        wo.AssignedTo,
        wo.WorkType,
        wo.Description,
        wo.Status,
        wo.UsageStatus,
        wo.CreatedAt,
        wo.CompletedAt,
        a.AssetName,
        u.FullName AS AssignedToName
    FROM dbo.WorkOrders wo
    LEFT JOIN dbo.Assets a ON a.AssetID = wo.AssetID
    LEFT JOIN dbo.Users u ON u.UserID = wo.AssignedTo
    WHERE wo.WorkOrderID = @WorkOrderID;
END
GO

-- ==========================================
-- 3️⃣ CREATE WORKORDER
-- ==========================================
CREATE PROCEDURE dbo.sp_workorders_create
(
    @ScheduleID INT = NULL,
    @TicketID INT = NULL,
    @AssetID INT,
    @AssignedTo INT = NULL,
    @WorkType NVARCHAR(50),
    @Description NVARCHAR(MAX) = NULL,
    @Status NVARCHAR(50) = N'Pending',
    @UsageStatus NVARCHAR(50) = N'Active'
)
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO dbo.WorkOrders (
        ScheduleID, 
        TicketID, 
        AssetID, 
        AssignedTo, 
        WorkType, 
        Description, 
        Status, 
        UsageStatus
    )
    VALUES (
        @ScheduleID, 
        @TicketID, 
        @AssetID, 
        @AssignedTo, 
        @WorkType, 
        @Description, 
        @Status, 
        @UsageStatus
    );
    
    -- Không trả ID để phù hợp với Tickets style (repository không mong đợi giá trị trả về)
END
GO

-- ==========================================
-- 4️⃣ UPDATE WORKORDER
-- ==========================================
CREATE  PROCEDURE dbo.sp_workorders_update
(
    @WorkOrderID INT,
    @ScheduleID INT = NULL,
    @TicketID INT = NULL,
    @AssetID INT,
    @AssignedTo INT = NULL,
    @WorkType NVARCHAR(50),
    @Description NVARCHAR(MAX) = NULL,
    @Status NVARCHAR(50),
    @CompletedAt DATETIME2 = NULL,
    @UsageStatus NVARCHAR(50) = N'Active'
)
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE dbo.WorkOrders
    SET 
        ScheduleID = @ScheduleID,
        TicketID = @TicketID,
        AssetID = @AssetID,
        AssignedTo = @AssignedTo,
        WorkType = @WorkType,
        Description = @Description,
        Status = @Status,
        CompletedAt = @CompletedAt,
        UsageStatus = @UsageStatus
    WHERE WorkOrderID = @WorkOrderID;
    
    SELECT @@ROWCOUNT AS RowsAffected;
END
GO

-- ==========================================
-- 5️⃣ DELETE WORKORDER (Soft Delete)
-- ==========================================
CREATE PROCEDURE dbo.sp_workorders_delete
(
    @WorkOrderID INT
)
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Soft delete: chuyển UsageStatus thành 'Cancelled'
    UPDATE dbo.WorkOrders 
    SET UsageStatus = N'Cancelled' 
    WHERE WorkOrderID = @WorkOrderID;
    
    SELECT @@ROWCOUNT AS RowsAffected;
END
GO



CREATE  PROCEDURE dbo.sp_users_authenticate
(
    @Email NVARCHAR(100),
    @PasswordHash NVARCHAR(255)
)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM dbo.Users 
    WHERE Email = @Email 
      AND PasswordHash = @PasswordHash 
      AND IsDeleted = 0;
END
GO



-- Test nhanh
EXEC dbo.sp_workorders_get_all;
GO

-- ==========================================
-- DỮ LIỆU MẪU
-- ==========================================
SET IDENTITY_INSERT dbo.Users ON;
INSERT INTO dbo.Users (UserID, FullName, Email, PasswordHash, Role, Phone, SkillLevel, Certifications, IsDeleted)
VALUES 
(1, N'Nguyễn Văn A', 'admin@system.com', 'hash123', N'Admin', '0901111222', NULL, NULL, 0),
(2, N'Trần Văn B', 'tech1@system.com', 'hash456', N'Technician', '0903333444', N'Cao', N'Chứng chỉ sửa chữa thiết bị điện tử', 0),
(3, N'Lê Thị C', 'staff1@system.com', 'hash789', N'Staff', '0905555666', NULL, NULL, 0);
SET IDENTITY_INSERT dbo.Users OFF;

SET IDENTITY_INSERT dbo.Assets ON;
INSERT INTO dbo.Assets (AssetID, AssetName, SerialNumber, Location, PurchaseDate, Status, UsageStatus)
VALUES
(1, N'Máy in HP LaserJet', 'SN001', N'Phòng Kế Toán', '2023-05-12', N'Active', N'Active'),
(2, N'Máy chiếu Sony', 'SN002', N'Phòng Họp Lớn', '2022-08-20', N'UnderMaintenance', N'Active'),
(3, N'Máy lạnh Daikin', 'SN003', N'Tầng 2 - Văn phòng', '2021-11-03', N'Active', N'Active');
SET IDENTITY_INSERT dbo.Assets OFF;

SET IDENTITY_INSERT dbo.Warranties ON;
INSERT INTO dbo.Warranties (WarrantyID, AssetID, WarrantyProvider, StartDate, EndDate, Terms, Status)
VALUES
(1, 1, N'Công ty HP Việt Nam', '2023-05-12', '2025-05-12', N'Bảo hành 24 tháng, lỗi đổi mới', N'Active'),
(2, 2, N'Sony Việt Nam', '2022-08-20', '2024-08-20', N'Sửa chữa miễn phí trong thời gian bảo hành', N'Expired'),
(3, 3, N'Daikin Việt Nam', '2021-11-03', '2024-11-03', N'Bảo dưỡng định kỳ miễn phí 2 lần/năm', N'Active');
SET IDENTITY_INSERT dbo.Warranties OFF;

SET IDENTITY_INSERT dbo.Schedules ON;
INSERT INTO dbo.Schedules (ScheduleID, AssetID, MaintenanceType, NextMaintenanceDate, LastMaintenanceDate, Checklist, UsageStatus)
VALUES
(1, 1, N'Monthly', '2025-11-01', '2025-10-01', N'Vệ sinh hộp mực, kiểm tra kết nối mạng', N'Active'),
(2, 2, N'Quarterly', '2025-12-15', '2025-09-15', N'Kiểm tra bóng đèn, vệ sinh quạt tản nhiệt', N'Active'),
(3, 3, N'Yearly', '2026-01-10', '2025-01-10', N'Vệ sinh dàn lạnh, kiểm tra gas', N'Active');
SET IDENTITY_INSERT dbo.Schedules OFF;

SET IDENTITY_INSERT dbo.Tickets ON;
INSERT INTO dbo.Tickets (TicketID, AssetID, CreatedBy, AssignedTo, Priority, SLAHours, IssueDescription, Status)
VALUES
(1, 1, 3, 2, N'High', 24, N'Máy in không nhận lệnh in từ mạng nội bộ', N'Assigned'),
(2, 2, 3, 2, N'Medium', 48, N'Máy chiếu bị nhòe hình ảnh', N'Resolved'),
(3, 3, 3, NULL, N'Low', 72, N'Máy lạnh phát ra tiếng ồn nhỏ khi hoạt động', N'Open');
SET IDENTITY_INSERT dbo.Tickets OFF;

SET IDENTITY_INSERT dbo.WorkOrders ON;
INSERT INTO dbo.WorkOrders (WorkOrderID, ScheduleID, TicketID, AssetID, AssignedTo, WorkType, Description, Status, UsageStatus, CompletedAt)
VALUES
(1, NULL, 1, 1, 2, N'Corrective', N'Sửa lỗi kết nối mạng máy in và thay cáp mới', N'Completed', N'Active', '2025-10-10'),
(2, NULL, 2, 2, 2, N'Corrective', N'Lau ống kính, cân chỉnh tiêu cự máy chiếu', N'Completed', N'Active', '2025-09-20'),
(3, 3, NULL, 3, 2, N'Preventive', N'Bảo dưỡng định kỳ máy lạnh tầng 2', N'In Progress', N'Active', NULL);
SET IDENTITY_INSERT dbo.WorkOrders OFF;

SET IDENTITY_INSERT dbo.Parts ON;
INSERT INTO dbo.Parts (PartID, PartName, PartCode, StockQuantity, UnitPrice, Location, UsageStatus)
VALUES
(1, N'Cáp mạng CAT6', 'P001', 50, 15000, N'Kho vật tư A', N'Active'),
(2, N'Bóng đèn máy chiếu Sony', 'P002', 10, 1200000, N'Kho thiết bị quang học', N'Active'),
(3, N'Gas lạnh R32', 'P003', 5, 850000, N'Kho vật tư lạnh', N'Active');
SET IDENTITY_INSERT dbo.Parts OFF;

SET IDENTITY_INSERT dbo.PartUsages ON;
INSERT INTO dbo.PartUsages (PartUsageID, WorkOrderID, PartID, QuantityUsed)
VALUES
(1, 1, 1, 2),
(2, 2, 2, 1),
(3, 3, 3, 1);
SET IDENTITY_INSERT dbo.PartUsages OFF;

GO
PRINT N'✅ Database DeviceMaintenanceDB (HYBRID: Soft Delete + CASCADE) đã hoàn tất!';
GO

USE DeviceMaintenanceDB;
GO

-- Xem tất cả users (kể cả đã xóa)
SELECT UserID, FullName, Email, IsDeleted FROM dbo.Users;



SELECT * FROM dbo.Assets 