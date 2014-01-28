ALTER TABLE dbo.[Users]
	ADD CONSTRAINT [DF_Users_UserId]
	DEFAULT NewId()
	FOR [UserId]
