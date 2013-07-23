CREATE TABLE dbo.[Users]
(
	[UserId] Int Identity(1,1) NOT NULL,
	[UserGuid] UniqueIdentifier NOT NULL,
	[Email] varchar(254) NOT NULL
)
GO