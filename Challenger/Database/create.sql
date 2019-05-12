CREATE SCHEMA clg AUTHORIZATION dbo
GO

CREATE TABLE clg.[User]
(
	Id INT NOT NULL IDENTITY(1,1)
		CONSTRAINT PK_User PRIMARY KEY CLUSTERED,
	[Name] NVARCHAR(100)
		CONSTRAINT UQ_User_Name UNIQUE
)
GO

CREATE TABLE clg.ChallengeType
(
	Id INT NOT NULL IDENTITY(1,1)
		CONSTRAINT PK_ChallengeType PRIMARY KEY CLUSTERED,
	[Name] NVARCHAR(100)	
)
GO


CREATE TABLE clg.Challenge
(
	Id INT NOT NULL IDENTITY(1,1)
		CONSTRAINT PK_Challenge PRIMARY KEY CLUSTERED,
	UserId INT NOT NULL 
		CONSTRAINT FK_Challenge_User FOREIGN KEY REFERENCES clg.[User] (Id),
	[Name] NVARCHAR(100),
	ChallengeTypeId INT NOT NULL 
		CONSTRAINT FK_Challenge_ChallengeType FOREIGN KEY REFERENCES clg.ChallengeType (Id),
)
GO

CREATE TABLE clg.[Set]
(
	Id INT NOT NULL IDENTITY(1,1)
		CONSTRAINT PK_Set PRIMARY KEY CLUSTERED,
	ChallengeId INT NOT NULL 
		CONSTRAINT FK_Set_Challenge FOREIGN KEY REFERENCES clg.Challenge (Id),
	Repetitions INT NOT NULL,
	[Date] DATE NOT NULL
		CONSTRAINT DF_Set_Date DEFAULT GETDATE(),
	DateTimeCreated DATETIME2 NOT NULL
		CONSTRAINT DF_Set_DateTimeCreated DEFAULT GETDATE()
)
GO

CREATE NONCLUSTERED INDEX IDX_Set_ChallengeId ON clg.[Set] (ChallengeId) INCLUDE (Id)
GO


--drop table clg.[Set]
--drop table clg.Challenge



CREATE PROCEDURE clg.spFixAddOneMoreEachDayChallenge
	@challengeId INT
AS

	;WITH q AS
			(
			SELECT CAST('2019-01-01' AS DATE) AS num
			UNION ALL
			SELECT  DATEADD(day, 1, num) AS num
			FROM    q
			WHERE   num < getdate()-2
			)
	SELECT num AS [date], ROW_NUMBER() OVER (ORDER BY num) AS [cnt]
	INTO #datesUntilYesteday
	FROM    q
	OPTION ( MaxRecursion 400 );

	DELETE FROM clg.[Set] WHERE ChallengeId = @challengeId

	INSERT INTO clg.[Set]
	(ChallengeId, Repetitions, [Date])
	SELECT @challengeId, cnt, [date]
	FROM #datesUntilYesteday

	DROP TABLE #datesUntilYesteday

GO