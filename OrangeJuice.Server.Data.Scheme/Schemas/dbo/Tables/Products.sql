CREATE TABLE dbo.[Products]
(
	[ProductId] UniqueIdentifier NOT NULL,
	[Barcode] VarChar(10) NOT NULL,
	[BarcodeType] TinyInt NOT NULL
)