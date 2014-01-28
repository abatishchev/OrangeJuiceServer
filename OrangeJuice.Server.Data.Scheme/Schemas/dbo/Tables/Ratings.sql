CREATE TABLE dbo.[Ratings]
(
	[UserId] UniqueIdentifier NOT NULL,
	[ProductId] UniqueIdentifier NOT NULL,
	[Value] TinyInt NOT NULL,
	[Comment] NVarChar(255) NULL
)
