ALTER TABLE dm.[Requests]
	ADD CONSTRAINT DF_Requests_RequestId
	DEFAULT NewId()
	FOR RequestId
