-- DROP TABLE IF EXISTS TutorialAppSchema.Users;

-- -- IF OBJECT_ID('TutorialAppSchema.Users') IS NOT NULL
-- --     DROP TABLE TutorialAppSchema.Users;

-- CREATE TABLE TutorialAppSchema.Users
-- (
--     UserId INT IDENTITY(1, 1) PRIMARY KEY
--     , FirstName NVARCHAR(50)
--     , LastName NVARCHAR(50)
--     , Email NVARCHAR(50) UNIQUE
--     , Gender NVARCHAR(50)
--     , Active BIT
-- );

-- DROP TABLE IF EXISTS TutorialAppSchema.UserSalary;

-- -- IF OBJECT_ID('TutorialAppSchema.UserSalary') IS NOT NULL
-- --     DROP TABLE TutorialAppSchema.UserSalary;

-- CREATE TABLE TutorialAppSchema.UserSalary
-- (
--     UserId INT UNIQUE
--     , Salary DECIMAL(18, 4)
-- );

-- DROP TABLE IF EXISTS TutorialAppSchema.UserJobInfo;

-- -- IF OBJECT_ID('TutorialAppSchema.UserJobInfo') IS NOT NULL
-- --     DROP TABLE TutorialAppSchema.UserJobInfo;

-- CREATE TABLE TutorialAppSchema.UserJobInfo
-- (
--     UserId INT UNIQUE
--     , JobTitle NVARCHAR(50)
--     , Department NVARCHAR(50),
-- );

USE DotNetCourseDatabase;
GO

SELECT  [UserId]
        , [FirstName]
        , [LastName]
        , [Email]
        , [Gender]
        , [Active]
  FROM  TutorialAppSchema.Users;

SELECT  [UserId]
        , [FirstName]
        , [LastName]
        , [Email]
        , [Gender]
        , [Active]
  FROM  TutorialAppSchema.Users
  WHERE [UserId] = 2;


UPDATE TutorialAppSchema.Users
SET [FirstName] = 'Munib',
    [LastName] = 'Ahmed',
    [Email] = 'munibahmed@gmail.com',
    [Gender] = 'Male',
    [Active] = '1'
WHERE UserId = 1;

INSERT INTO TutorialAppSchema.Users (FirstName, LastName, Email, Gender, Active) VALUES ('', '', '', '', 1);


SELECT * FROM TutorialAppSchema.Users
ORDER BY UserId ASC;

SELECT * FROM TutorialAppSchema.Users WHERE UserId = 500;

SELECT COUNT(*) FROM TutorialAppSchema.Users;

DELETE FROM TutorialAppSchema.Users WHERE UserId = 1003;

SELECT  [UserId]
        , [Salary]
  FROM  TutorialAppSchema.UserSalary;

SELECT  [UserId]
        , [JobTitle]
        , [Department]
  FROM  TutorialAppSchema.UserJobInfo;


INSERT INTO TutorialAppSchema.UserSalary (Salary) VALUES (1);
INSERT INTO TutorialAppSchema.UserJobInfo (JobTitle, Department) VALUES ('', '');


CREATE TABLE TutorialAppSchema.Auth(
  Email VARCHAR(50),
  PasswordHash VARBINARY(MAX),
  PasswordSalt VARBINARY(MAX),
)