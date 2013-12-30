ALTER TABLE dbo.[Ratings]
	ADD CONSTRAINT [FK_Ratings_Users]
	FOREIGN KEY (UserId)
	REFERENCES dbo.[Users] (UserId)
GO