-- =============================================                                 
-- Author      : Shanu                                   
-- Create date : 2016-01-17                                
-- Description : To Create Database                           
                              
-- =============================================   
--Script to create DB,Table and sample Insert data   
USE MASTER;   
-- 1) Check for the Database Exists .If the database is exist then drop and create new DB   
IF EXISTS (SELECT [name] FROM sys.databases WHERE [name] = 'AttendanceDB' )   
BEGIN   
ALTER DATABASE AttendanceDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE   
DROP DATABASE AttendanceDB ;   
END     
   
CREATE DATABASE AttendanceDB   
GO   
   
USE AttendanceDB 
