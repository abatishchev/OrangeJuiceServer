CREATE TABLE dbo.[User]
(
	[UserId] Int Identity(1,1) NOT NULL,
	[UserGuid] UniqueIdentifier NOT NULL,
	[Email] varchar(25) NOT NULL
)
GO