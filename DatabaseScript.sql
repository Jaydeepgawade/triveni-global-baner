-- Employee Management Database Template
-- This script creates the complete database structure with sample data

-- Create Database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'EmployeeManagementDB')
BEGIN
    CREATE DATABASE EmployeeManagementDB;
END
GO

USE EmployeeManagementDB;
GO

-- Create Departments Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Departments]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Departments](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Name] [nvarchar](100) NOT NULL,
        CONSTRAINT [PK_Departments] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END
GO

-- Create Employees Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Employees]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Employees](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [Name] [nvarchar](100) NOT NULL,
        [Email] [nvarchar](100) NOT NULL,
        [Phone] [nvarchar](20) NOT NULL,
        [Gender] [nvarchar](10) NOT NULL,
        [DOB] [datetime2](7) NOT NULL,
        [DeptId] [int] NOT NULL,
        [ImagePath] [nvarchar](500) NULL,
        CONSTRAINT [PK_Employees] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END
GO

-- Create Salaries Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Salaries]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Salaries](
        [Id] [int] IDENTITY(1,1) NOT NULL,
        [EmpId] [int] NOT NULL,
        [Amount] [decimal](18, 2) NOT NULL,
        [Date] [datetime2](7) NOT NULL,
        CONSTRAINT [PK_Salaries] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END
GO

-- Create Foreign Key Constraints
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Employees_Departments_DeptId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Employees]'))
BEGIN
    ALTER TABLE [dbo].[Employees] WITH CHECK ADD CONSTRAINT [FK_Employees_Departments_DeptId] 
    FOREIGN KEY([DeptId]) REFERENCES [dbo].[Departments] ([Id]);
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Salaries_Employees_EmpId]') AND parent_object_id = OBJECT_ID(N'[dbo].[Salaries]'))
BEGIN
    ALTER TABLE [dbo].[Salaries] WITH CHECK ADD CONSTRAINT [FK_Salaries_Employees_EmpId] 
    FOREIGN KEY([EmpId]) REFERENCES [dbo].[Employees] ([Id]) ON DELETE CASCADE;
END
GO

-- Create Indexes
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Employees]') AND name = N'IX_Employees_Email')
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX [IX_Employees_Email] ON [dbo].[Employees] ([Email] ASC);
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Departments]') AND name = N'IX_Departments_Name')
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX [IX_Departments_Name] ON [dbo].[Departments] ([Name] ASC);
END
GO

-- Insert Sample Data
-- Departments
IF NOT EXISTS (SELECT 1 FROM [dbo].[Departments] WHERE [Name] = 'IT')
BEGIN
    INSERT INTO [dbo].[Departments] ([Name]) VALUES ('IT');
END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[Departments] WHERE [Name] = 'HR')
BEGIN
    INSERT INTO [dbo].[Departments] ([Name]) VALUES ('HR');
END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[Departments] WHERE [Name] = 'Finance')
BEGIN
    INSERT INTO [dbo].[Departments] ([Name]) VALUES ('Finance');
END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[Departments] WHERE [Name] = 'Marketing')
BEGIN
    INSERT INTO [dbo].[Departments] ([Name]) VALUES ('Marketing');
END
GO

-- Sample Employees
IF NOT EXISTS (SELECT 1 FROM [dbo].[Employees] WHERE [Email] = 'john.doe@company.com')
BEGIN
    INSERT INTO [dbo].[Employees] ([Name], [Email], [Phone], [Gender], [DOB], [DeptId]) 
    VALUES ('John Doe', 'john.doe@company.com', '1234567890', 'Male', '1990-05-15', 1);
END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[Employees] WHERE [Email] = 'jane.smith@company.com')
BEGIN
    INSERT INTO [dbo].[Employees] ([Name], [Email], [Phone], [Gender], [DOB], [DeptId]) 
    VALUES ('Jane Smith', 'jane.smith@company.com', '0987654321', 'Female', '1988-12-20', 2);
END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[Employees] WHERE [Email] = 'mike.johnson@company.com')
BEGIN
    INSERT INTO [dbo].[Employees] ([Name], [Email], [Phone], [Gender], [DOB], [DeptId]) 
    VALUES ('Mike Johnson', 'mike.johnson@company.com', '1122334455', 'Male', '1992-08-10', 1);
END
GO

-- Sample Salaries
IF NOT EXISTS (SELECT 1 FROM [dbo].[Salaries] WHERE [EmpId] = 1 AND [Date] = '2024-01-01')
BEGIN
    INSERT INTO [dbo].[Salaries] ([EmpId], [Amount], [Date]) 
    VALUES (1, 75000.00, '2024-01-01');
END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[Salaries] WHERE [EmpId] = 2 AND [Date] = '2024-01-01')
BEGIN
    INSERT INTO [dbo].[Salaries] ([EmpId], [Amount], [Date]) 
    VALUES (2, 65000.00, '2024-01-01');
END
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[Salaries] WHERE [EmpId] = 3 AND [Date] = '2024-01-01')
BEGIN
    INSERT INTO [dbo].[Salaries] ([EmpId], [Amount], [Date]) 
    VALUES (3, 80000.00, '2024-01-01');
END
GO

-- Additional salary records for different months
INSERT INTO [dbo].[Salaries] ([EmpId], [Amount], [Date]) 
SELECT 1, 75000.00, '2024-02-01' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Salaries] WHERE [EmpId] = 1 AND [Date] = '2024-02-01');
GO

INSERT INTO [dbo].[Salaries] ([EmpId], [Amount], [Date]) 
SELECT 2, 65000.00, '2024-02-01' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Salaries] WHERE [EmpId] = 2 AND [Date] = '2024-02-01');
GO

INSERT INTO [dbo].[Salaries] ([EmpId], [Amount], [Date]) 
SELECT 3, 80000.00, '2024-02-01' WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Salaries] WHERE [EmpId] = 3 AND [Date] = '2024-02-01');
GO

PRINT 'Database setup completed successfully!';
PRINT 'Tables created: Departments, Employees, Salaries';
PRINT 'Sample data inserted successfully';
