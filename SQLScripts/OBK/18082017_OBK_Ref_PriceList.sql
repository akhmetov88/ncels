IF NOT EXISTS (SELECT * FROM sys.objects 
WHERE object_id = OBJECT_ID(N'[dbo].[OBK_Ref_PriceList]') AND type in (N'U'))

BEGIN
	CREATE TABLE [dbo].[OBK_Ref_PriceList](
		[Id] uniqueidentifier NOT NULL,
		[Type] INT NOT NULL,
		[NameRu] NVARCHAR(255) NOT NULL,
		[NameKz] NVARCHAR(255) NOT NULL,
		[Unit] uniqueidentifier NOT NULL,
		[Price] FLOAT NOT NULL,
		[ServiceType] uniqueidentifier NOT NULL,
		[IsDeleted] BIT NOT NULL,
		CONSTRAINT PK_OBK_Ref_PriceList
		PRIMARY KEY ([Id]),
		CONSTRAINT FK_OBK_Ref_PriceList__Type__OBK_Ref_Type__Id
		FOREIGN KEY ([Type])
		REFERENCES [OBK_Ref_Type]([Id]),
		CONSTRAINT FK_OBK_Ref_PriceList__Unit__Dictionaries__Id
		FOREIGN KEY ([Unit])
		REFERENCES [Dictionaries]([Id]),
		CONSTRAINT FK_OBK_Ref_PriceList__ServiceType__OBK_Ref_ServiceType__Id
		FOREIGN KEY ([ServiceType])
		REFERENCES [OBK_Ref_ServiceType]([Id])
	)
END