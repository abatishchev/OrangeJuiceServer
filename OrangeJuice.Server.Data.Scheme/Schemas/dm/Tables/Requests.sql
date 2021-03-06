﻿CREATE TABLE dm.[Requests]
(
	[RequestId] UniqueIdentifier NOT NULL,
	[Timestamp] DateTime2(2) NOT NULL,
	[Url] VarChar(255) NOT NULL,
	[HttpMethod] VarChar(4) NOT NULL,
	[IpAddress] VarChar(15) NOT NULL,
	[UserAgent] VarChar(255) NOT NULL
)