USE DotNetCourseDatabase;
GO

-- Passing parameters to the stored procedure
-- ALTER PROCEDURE TutorialAppSchema.spUsers_Get @UserId INT
-- /* EXEC TutorialAppSchema.spUsers_Get @UserId = 3*/
-- AS
-- BEGIN
--     SELECT * FROM TutorialAppSchema.Users AS Users WHERE Users.UserId = @UserId
-- END


-- ALTER PROCEDURE TutorialAppSchema.spUsers_Get @UserId INT = NULL
-- AS
-- BEGIN
--     SELECT * FROM TutorialAppSchema.Users AS Users WHERE Users.UserId = ISNULL(@UserId, UserId)
-- END

-- EXEC TutorialAppSchema.spUsers_Get @UserId = 5


ALTER PROCEDURE TutorialAppSchema.spUsers_Get 
    @UserId INT = NULL
AS
BEGIN

    DROP TABLE IF EXISTS #AverageDeptSalary

    SELECT UserJobInfo.Department AS Department, AVG(UserSalary.Salary) AS AvgSalary
    INTO #AverageDeptSalary
    FROM TutorialAppSchema.Users AS Users
    LEFT JOIN TutorialAppSchema.UserSalary AS UserSalary
    ON Users.UserId = UserSalary.UserId
    LEFT JOIN TutorialAppSchema.UserJobInfo AS UserJobInfo
    ON Users.UserId = UserJobInfo.UserId
    GROUP BY Department

    CREATE CLUSTERED INDEX cix_AverageDeptSalary_Department ON #AverageDeptSalary(Department)

    SELECT Users.UserId,
        Users.FirstName,
        Users.LastName,
        Users.Email,
        Users.Gender,
        Users.Active,
        UserSalary.Salary,
        UserJobInfo.JobTitle,
        UserJobInfo.Department,
        AvgSalary.AvgSalary
    FROM TutorialAppSchema.Users AS Users
    LEFT JOIN TutorialAppSchema.UserSalary AS UserSalary
    ON Users.UserId = UserSalary.UserId
    LEFT JOIN TutorialAppSchema.UserJobInfo AS UserJobInfo
    ON Users.UserId = UserJobInfo.UserId
    LEFT JOIN #AverageDeptSalary AS AvgSalary
    ON UserJobInfo.Department = AvgSalary.Department
    -- OUTER APPLY(
    --         SELECT 
    --             UserJobInfo2.Department,
    --             AVG(UserSalary2.Salary) AvgSalary
    --         FROM TutorialAppSchema.Users AS Users
    --         LEFT JOIN TutorialAppSchema.UserSalary AS UserSalary2
    --         ON Users.UserId = UserSalary2.UserId
    --         LEFT JOIN TutorialAppSchema.UserJobInfo AS UserJobInfo2
    --         ON Users.UserId = UserJobInfo2.UserId
    --         WHERE UserJobInfo2.Department = UserJobInfo.Department
    --         GROUP BY UserJobInfo2.Department
    -- ) AS AvgSalary
    WHERE Users.UserId = ISNULL(@UserId, Users.UserId)
END

EXEC TutorialAppSchema.spUsers_Get @UserId = 3

-- SELECT * FROM TutorialAppSchema.UserSalary
SELECT 
       UserJobInfo.Department,
       AVG(UserSalary.Salary)
FROM TutorialAppSchema.Users AS Users
LEFT JOIN TutorialAppSchema.UserSalary AS UserSalary
ON Users.UserId = UserSalary.UserId
LEFT JOIN TutorialAppSchema.UserJobInfo AS UserJobInfo
ON Users.UserId = UserJobInfo.UserId
GROUP BY UserJobInfo.Department



