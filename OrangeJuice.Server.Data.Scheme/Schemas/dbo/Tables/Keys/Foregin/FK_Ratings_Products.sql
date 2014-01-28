ALTER TABLE dbo.[Ratings]
	ADD CONSTRAINT [FK_Ratings_Products]
	FOREIGN KEY ([ProductId])
	REFERENCES dbo.[Products] ([ProductId])
