--CREATE SCHEMA OsloBysykkel AUTHORIZATION dbo

--drop table OsloBysykkel.StationAvailabilities
--drop table OsloBysykkel.StationBoundaries
--drop table OsloBysykkel.Stations
--drop table OsloBysykkel.Points
--drop SCHEMA OsloBysykkel

CREATE TABLE OsloBysykkel.Points
(
	Id INT NOT NULL CONSTRAINT PK_Points PRIMARY KEY CLUSTERED,
	Latitude DECIMAL(18,15),
	Longitude DECIMAL(18,15)
)

CREATE TABLE OsloBysykkel.Stations
(
	Id INT NOT NULL CONSTRAINT PK_Stations PRIMARY KEY CLUSTERED,
	Title NVARCHAR(255) NULL,
	Subtitle NVARCHAR(500) NULL,
	NumberOfLocks TINYINT NULL,
	CenterPointId INT NOT NULL
		CONSTRAINT FK_Stations_CenterPointId FOREIGN KEY REFERENCES OsloBysykkel.Points(Id),
)

CREATE TABLE OsloBysykkel.StationBoundaries
(
	StationId INT NOT NULL
		CONSTRAINT FK_StationBoundaries_Stations FOREIGN KEY REFERENCES OsloBysykkel.Stations(Id),
	PointId INT NOT NULL
		CONSTRAINT FK_StationBoundaries_Points FOREIGN KEY REFERENCES OsloBysykkel.Points(Id),
)

CREATE TABLE OsloBysykkel.StationAvailabilities
(
	StationId INT NOT NULL
		CONSTRAINT FK_StationAvailabilities_Stations FOREIGN KEY REFERENCES OsloBysykkel.Stations(Id),
	LogDateTime DATETIME NOT NULL,
	Bikes INT NULL,
	Locks INT NULL,
	UpdatedAt DATETIME NULL,
	RefreshRate	DECIMAL(5,2) NULL
)

--INSERT INTO OsloBysykkel.Stations
--(Id, Title, Subtitle, NumberOfLocks, CenterLatitude, CenterLongitude)
--SELECT 210, 'Birkelunden', 'langs Seilduksgata', 10, 59.92559918218687, 10.760778486728668


--{
--      "id": 210,
--      "title": "Birkelunden",
--      "subtitle": "langs Seilduksgata",
--      "number_of_locks": 10,
--      "center": {
--        "latitude": 59.925622,
--        "longitude": 10.760822
--      },
--      "bounds": [
--        {
--          "latitude": 59.92559918218687,
--          "longitude": 10.760778486728668
--        },
--        {
--          "latitude": 59.925603214545724,
--          "longitude": 10.76099306344986
--        },
--        {
--          "latitude": 59.9256529469314,
--          "longitude": 10.760995745658873
--        },
--        {
--          "latitude": 59.9257161203949,
--          "longitude": 10.760791897773741
--        },
--        {
--          "latitude": 59.9256919263167,
--          "longitude": 10.760748982429503
--        },
--        {
--          "latitude": 59.92568117338741,
--          "longitude": 10.76061487197876
--        },
--        {
--          "latitude": 59.92559918218687,
--          "longitude": 10.760778486728668
--        }