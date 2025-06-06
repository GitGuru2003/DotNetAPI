USE DotNetCourseDatabase;
GO

CREATE TABLE TutorialAppSchema.Auth(
  Email VARCHAR(50),
  PasswordHash VARBINARY(MAX),
  PasswordSalt VARBINARY(MAX)
)
GO

SELECT [PasswordHash], [PasswordSalt] FROM TutorialAppSchema.Auth WHERE Email = '';


INSERT INTO TutorialAppSchema.Auth ([Email], [PasswordHash], [PasswordSalt]) VALUES ('', '', '');

SELECT * FROM TutorialAppSchema.Auth;
GO

-- To view the tables and the schemas in T-SQL
SELECT TABLE_SCHEMA, TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE'
  AND TABLE_SCHEMA = 'TutorialAppSchema';
GO

SELECT * FROM TutorialAppSchema.Users;

USE DotNetCourseDatabase;

CREATE TABLE TutorialAppSchema.Posts(
  PostId INT IDENTITY(1,1),
  UserId INT,
  PostTitle NVARCHAR(255),
  PostContent NVARCHAR(MAX),
  PostCreated DATETIME2,
  PostUpdated DATETIME,
);

SELECT [PostId], 
  [UserId], 
  [PostTitle], 
  [PostContent], 
  [PostCreated], 
  [PostUpdated]  FROM TutorialAppSchema.Posts;

-- SELECT * FROM TutorialAppSchema.Posts
--   WHERE PostTitle LIKE %% OR PostContent LIKE %%;

INSERT INTO TutorialAppSchema.Posts([PostId], 
  [UserId], 
  [PostTitle], 
  [PostContent], 
  [PostCreated], 
  [PostUpdated]) VALUES ();

UPDATE TutorialAppSchema.Posts
SET PostContent = '', PostTitle = '', PostUpdated = GETDATE()
WHERE PostId = 4


CREATE CLUSTERED INDEX cix_Posts_UserId_PostId on TutorialAppSchema.Posts(UserId, PostId);


