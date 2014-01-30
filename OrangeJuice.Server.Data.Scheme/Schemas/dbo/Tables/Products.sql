CREATE TABLE dbo.[Products]
(
	[ProductId] UniqueIdentifier NOT NULL,
	[Barcode] VarChar(13) NOT NULL,
	[BarcodeType] TinyInt NOT NULL
)