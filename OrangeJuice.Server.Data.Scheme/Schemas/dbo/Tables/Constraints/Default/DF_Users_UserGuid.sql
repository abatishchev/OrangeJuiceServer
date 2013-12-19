ALTER TABLE dbo.[Users]
	ADD CONSTRAINT [DF_Users_UserGuid]
	DEFAULT NewId()
	FOR [UserGuid]
GO