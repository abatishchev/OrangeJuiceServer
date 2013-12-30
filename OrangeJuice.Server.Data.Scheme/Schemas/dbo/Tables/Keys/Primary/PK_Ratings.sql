ALTER TABLE dbo.[Ratings]
	ADD CONSTRAINT [PK_Ratings]
	PRIMARY KEY (UserId, ProductId)
GO