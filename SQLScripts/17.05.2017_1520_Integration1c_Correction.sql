
DROP TABLE [dbo].[I1c_primary_Applications];
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[I1c_primary_Applications](
	[Id] [uniqueidentifier] NOT NULL,
	[ExportDatetime] [datetime] NOT NULL,
	[ImportDatetime] [datetime] NULL,
	[ApplicationId] [uniqueidentifier] NULL,
	[ApplicationNumber] [nvarchar](450) NULL,
	[ApplicationDatetime] [datetime] NULL,
	[ApplicationType] [nvarchar](500) NULL,
	[Producer] [nvarchar](4000) NULL,
	[ProducerId] [uniqueidentifier] NULL,
	[Applicant] [nvarchar](4000) NULL,
	[ApplicantId] [uniqueidentifier] NULL,
	[ContractNumber] [nvarchar](450) NULL,
	[ContractStartDate] [date] NULL,
	[ContractEndDate] [date] NULL,
	[ContractId] [uniqueidentifier] NULL,
	[DoverennostNumber] [nvarchar](500) NULL,
	[DoverennostCreatedDate] [date] NULL,
	[DoverennostExpiryDate] [date] NULL,
	[Address] [nvarchar](4000) NULL,
	[Phone] [nvarchar](500) NULL,
	[Payer] [nvarchar](4000) NULL,
	[PayerId] [uniqueidentifier] NULL,
	[PayerAddress] [nvarchar](4000) NULL,
	[PayerBank] [nvarchar](4000) NULL,
	[PayerAccount] [nvarchar](500) NULL,
	[PayerBIK] [nvarchar](500) NULL,
	[PayerSWIFT] [nvarchar](500) NULL,
	[PayerBIN] [nvarchar](500) NULL,
	[PayerIIN] [nvarchar](500) NULL,
	[PayerCurrency] [nvarchar](500) NULL,
	[PayerCurrencyCode] [nvarchar](500) NULL,
	[PayerCurrencyId] [uniqueidentifier] NULL,
	[Country] [nvarchar](500) NULL,
	[CountryId] [uniqueidentifier] NULL,
	[IsResident] [bit] NULL,
	[IsLegal] [bit] NULL,
	[InvoiceNumber1C] [nvarchar](500) NULL,
	[InvoiceDatetime1C] [datetime] NULL,
	[StatementNumber] [nvarchar](500) NULL,
	[DrugTradeName] [nvarchar](500) NULL,
	[DrugTradeNameKz] [nvarchar](500) NULL,
	[DrugPackage] [nvarchar](500) NULL,
	[DrugPackageKz] [nvarchar](500) NULL,
	[TypeId] [int] NULL,
	[Type] [nvarchar](500) NULL,
	[TotalPrice] [decimal](18, 2) NULL,
 CONSTRAINT [PK_I1c_primary_Applications] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[I1c_primary_Applications] ADD  CONSTRAINT [DF_I1c_primary_Applications_Id]  DEFAULT (newid()) FOR [Id]
GO

ALTER TABLE [dbo].[I1c_primary_Applications] ADD  CONSTRAINT [DF_I1c_primary_Applications_ExportDatetime]  DEFAULT (getdate()) FOR [ExportDatetime]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Дата и время выгрузки в промежуточную базу' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'ExportDatetime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Дата и время загрузки в 1С' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'ImportDatetime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ид направления на оплату' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'ApplicationId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'№ направления на оплату' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'ApplicationNumber'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Дата направления на оплату' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'ApplicationDatetime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Тип услуги из справочника I1C_ServiceTypes' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'ApplicationType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Производитель' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'Producer'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ИД производителя в БД Экспертизы' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'ProducerId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Наименование Заявителя' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'Applicant'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ИД заявителя в БД Экспертизы' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'ApplicantId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Номер договора' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'ContractNumber'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Дата начала договора' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'ContractStartDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Дата завершения договора' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'ContractEndDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ИД договора в БД Экспертизы' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'ContractId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'№ доверенности' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'DoverennostNumber'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Дата начала доверенности' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'DoverennostCreatedDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Дата окончания доверенности' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'DoverennostExpiryDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Адрес' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'Address'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Телефон' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'Phone'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Наименование Плательщика' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'Payer'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ИД Плательщика' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'PayerId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Адрес Плательщика' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'PayerAddress'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Банк Плательщика' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'PayerBank'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Расчетный счет Плательщика' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'PayerAccount'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'БИК Плательщика' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'PayerBIK'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SWIFT Плательщика' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'PayerSWIFT'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'БИН Плательщика' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'PayerBIN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ИИН Плательщика' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'PayerIIN'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Валюта Плательщика' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'PayerCurrency'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Код Валюта Плательщика' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'PayerCurrencyCode'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ИД Валюта Плательщика' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'PayerCurrencyId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Страна' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'Country'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ИД страны' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'CountryId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'признак резидент РК или не резидент РК' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'IsResident'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'признак физическое лицо или юридическое лицо' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'IsLegal'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Номер счета на оплату будет передаваться из 1С' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'InvoiceNumber1C'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Дата счета на оплату, возвратившееся из 1С' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'InvoiceDatetime1C'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Номер заявления ЛС' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'StatementNumber'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Торговое название' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'DrugTradeName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Торговое название каз' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'DrugTradeNameKz'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Упаковка' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'DrugPackage'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Упаковка каз' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'DrugPackageKz'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Тип' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'Type'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ОБщая сумма для оплаты' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_Applications', @level2type=N'COLUMN',@level2name=N'TotalPrice'
GO








-- =============================================
-- Author:		Alexandr Zarichanskiy
-- Create date: 2017-05-10
-- Description:	Send data to Db 1C
-- =============================================
ALTER TRIGGER [dbo].[TR_SendToI1cOnUpdate]
   ON  [dbo].[EXP_DirectionToPays]
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @sended1CStatusId  uniqueidentifier, @expireStatusId  uniqueidentifier;--, @agreedStatusId uniqueidentifier;
	
	SELECT TOP 1 @sended1CStatusId = [Id]   
	FROM [dbo].[Dictionaries]
	WHERE [Type] = 'ExpDirectionToPayStatus' AND Code = '5' AND [ExpireDate] IS NULL;

	--SELECT TOP 1 @agreedStatusId = [Id]   
	--FROM [dbo].[Dictionaries]
	--WHERE [Type] = 'ExpDirectionToPayStatus' AND Code = '2' AND [ExpireDate] IS NULL;

	SELECT TOP 1 @expireStatusId = [Id]   
	FROM [dbo].[Dictionaries]
	WHERE [Type] = 'ExpDirectionToPayStatus' AND Code = '7' AND [ExpireDate] IS NULL;

	-- Primary direction to pay
	IF ((select [Type] from inserted) = 1)
	BEGIN
		-- Insert primary direction to pay
		IF (UPDATE(StatusId) AND (select StatusId from inserted) = @sended1CStatusId)
		BEGIN
			INSERT INTO [dbo].[I1c_primary_Applications]
			   ([Id]
			   ,[ExportDatetime]
			   ,ImportDatetime
			   ,[ApplicationId]
			   ,[ApplicationNumber]
			   ,[ApplicationType]
			   ,[ApplicationDatetime]
			   ,[Producer]
			   ,[ProducerId]
			   ,[Applicant]
			   ,[ApplicantId]
			   ,[ContractNumber]
			   ,[ContractStartDate]
			   ,[ContractEndDate]
			   ,[ContractId]
			   ,[DoverennostNumber]
			   ,[DoverennostCreatedDate]
			   ,[DoverennostExpiryDate]
			   ,[Address]
			   ,[Phone]
			   ,[Payer]
			   ,[PayerId]
			   ,[PayerAddress]
			   ,[PayerBank]
			   ,[PayerAccount]
			   ,[PayerBIK]
			   ,[PayerSWIFT]
			   ,[PayerBIN]
			   ,[PayerIIN]
			   ,[PayerCurrency]
			   ,[PayerCurrencyCode]
			   ,[PayerCurrencyId]
			   ,[Country]
			   ,[CountryId]
			   ,[IsResident]
			   ,[IsLegal]
			   ,[InvoiceNumber1C]
			   ,[InvoiceDatetime1C]
			   ,[StatementNumber]
			   ,[DrugTradeName]
			   ,[DrugTradeNameKz]
			   ,[DrugPackage]
			   ,[DrugPackageKz]
			   ,[Type]
			   ,[TypeId]
			   ,[TotalPrice])
			SELECT	NEWID() AS [Id]
					,GETDATE() AS [ExportDatetime]
					,NULL AS [ImportDatetime]
					,Td.[Id] AS [ApplicationId]
					,Td.[Number] AS [ApplicationNumber]
					,'ЛС' AS [ApplicationType]
					,Td.[DirectionDate] [ApplicationDatetime]
					,Tmanuf.[NameRu] AS [Producer]
					,Tmanuf.[Id] AS [ProducerId]
					,TApplicant.[NameRu] AS [Applicant]
					,TApplicant.Id AS [ApplicantId]
					,TContract.Number AS [ContractNumber]
					,TContract.StartDate AS [ContractStartDate]
					,TContract.EndDate AS [ContractStartDate]
					,TContract.Id AS [ContractId]
					,TContract.DoverennostNumber AS [DoverennostNumber]
					,TContract.DoverennostCreatedDate AS [DoverennostCreatedDate]
					,TContract.DoverennostExpiryDate AS [DoverennostExpiryDate]
					,TApplicant.[AddressFact] AS [Address]
					,TApplicant.[Phone] AS [Phone]
					,TPayer.[NameRu] AS [Payer]
					,TPayer.[Id] AS [PayerId]
					,TPayer.[AddressFact] AS [PayerAddress]
					,TPayer.[BankName] AS [PayerBank]
					,TPayer.[PaymentBill] AS [PayerAccount]
					,TPayer.[BankBik] AS [PayerBIK]
					,TPayer.[BankSwift] AS [PayerSWIFT]
					,TPayer.[Bin] AS [PayerBIN]
					,TPayer.[Iin] AS [PayerIIN]
					,TPayer.[BankCurencyDicName] AS [PayerCurrency]
					,Tcurrency.[Code] AS [PayerCurrencyCode]
					,TPayer.[BankCurencyDicId] AS [PayerCurrencyId]
					,TApplicant.[CountryName] AS [Country]
					,TApplicant.[CountryDicId] AS [CountryId]
					,TApplicant.[IsResident] AS [IsResident]
					,(CASE WHEN TOrf.Code = 'fl' THEN 0 ELSE 1 END) AS [IsLegal]
					,NULL AS [InvoiceNumber1C]
					,NULL AS [InvoiceDatetime1C]
					,Tdd.Number AS [StatementNumber]
					,Tdd.NameRu AS [DrugTradeName]
					,Tdd.NameKz AS [DrugTradeNameKz]
					,NULL AS [DrugPackage]
					,NULL AS [DrugPackageKz]
					,Tddtype.NameRu AS [Type]
					,Tdd.TypeId AS [TypeId]
					,Td.TotalPrice AS [TotalPrice]
				FROM [inserted] AS Td
				INNER JOIN [dbo].[EXP_DirectionToPays_DrugDeclaration] AS Tdpdd ON Tdpdd.DirectionToPayId = Td.Id
				INNER JOIN [dbo].[EXP_DrugDeclaration] AS Tdd ON Tdd.Id = Tdpdd.DrugDeclarationId
				INNER JOIN [dbo].[EXP_DIC_Type] AS Tddtype ON Tddtype.Id = Tdd.TypeId
				INNER JOIN [dbo].[Contracts] AS TContract ON TContract.Id = Tdd.ContractId
				INNER JOIN [dbo].[OrganizationsView] AS TApplicant ON TApplicant.Id = TContract.ApplicantOrganizationId
				LEFT OUTER JOIN [dbo].[Dictionaries] AS TOrf ON TOrf.Id = TApplicant.OriginalOrgId
				INNER JOIN [dbo].[OrganizationsView] AS Tmanuf ON Tmanuf.Id = TContract.ManufacturerOrganizationId
				INNER JOIN [dbo].[OrganizationsView] AS TPayer ON TPayer.Id = TContract.PayerOrganizationId  
				INNER JOIN [dbo].[Dictionaries] AS Tcurrency ON Tcurrency.Id = TPayer.[BankCurencyDicId]

				-- insert dosage as elements
				INSERT INTO [dbo].[I1c_primary_ApplicationElements]
						([Id]
						,[Dosage]
						,[MeasureName]
						,[RegNumber]
						,[ConcentrationRu]
						,[ConcentrationKz]
						,[refApplication])
				SELECT NEWID() AS [Id]
					,[Dosage] AS [Dosage]
					,Tmeas.[Name] AS [MeasureName]
					,[RegNumber] AS [RegNumber]		
					,[ConcentrationRu]
					,[ConcentrationKz]
					,Tdp.Id AS [refApplication]
				FROM [dbo].[EXP_DrugDosage] AS Tdd
				INNER JOIN [dbo].[sr_measures] AS Tmeas ON Tmeas.Id = Tdd.DosageMeasureTypeId
				INNER JOIN [dbo].[EXP_DirectionToPays_DrugDeclaration] AS Tdpdd ON Tdpdd.DrugDeclarationId = Tdd.DrugDeclarationId
				INNER JOIN [inserted] AS Tdp ON Tdp.Id = Tdpdd.DirectionToPayId

		END

		-- Add primary refuse direction to pay
		IF (UPDATE(StatusId) AND (select StatusId from inserted) = @expireStatusId)
		BEGIN
			INSERT INTO [dbo].[I1c_primary_ApplicationRefuseState]
			   ([Id]
			   ,[ApplicationNumber]
			   ,[ApplicationDatetime]
			   ,[IsRefuse]
			   ,[RefuseDatetime]
			   ,[refApplication])
			 SELECT
				   NEWID() AS [Id]
				   ,Td.Number AS [ApplicationNumber]
				   ,Td.DirectionDate AS [ApplicationDatetime]
				   ,1 AS [IsRefuse]
				   ,GETDATE()
				   ,Td.Id AS [refApplication]
			 FROM [inserted] AS Td
		END
	END
    ELSE
	BEGIN
		-- Insert translate direction to pay
		IF (UPDATE(StatusId) AND (select StatusId from inserted) = @sended1CStatusId )
		BEGIN
			INSERT INTO [dbo].[I1c_trl_Application]
			   ([Id]
			   ,[ApplicationId]
			   ,[ApplicationNumber]
			   ,[ApplicationDatetime]
			   ,[TranslatorFIO]
			   ,[TranslatorId]
			   ,[PageQuantity]
			   ,[PagePrice]
			   ,[TotalPrice]
			   ,[InvoiceCode_1C]
			   ,[InvoiceNumber_1C]
			   ,[InvoiceDatetime_1C]
			   ,[ExportDatetime]
			   ,[ImportDatetime]
			   ,[refPrimaryApplication])
			 SELECT 
				   NEWID() AS [Id]
				   ,Td.[Id] AS [ApplicationId]
				   ,Td.[Number] AS [ApplicationNumber]
				   ,Td.[DirectionDate] AS ApplicationDatetime
				   ,Te.[FullName] AS [TranslatorFIO]
				   ,Te.[Id] AS [TranslatorId]
				   ,Td.[PageCount] AS [PageQuantity]
				   ,Td.[PriceForPage] AS [PagePrice]
				   ,Td.TotalPrice AS [TotalPrice]
				   ,NULL AS [InvoiceCode_1C]
				   ,NULL AS [InvoiceNumber_1C]
				   ,NULL AS [InvoiceDatetime_1C]
				   ,GETDATE() AS [ExportDatetime]
				   ,NULL AS [ImportDatetime]
				   ,(SELECT TOP 1 DirectionToPayId FROM [dbo].[EXP_DirectionToPays_DrugDeclaration] 
						WHERE DrugDeclarationId = Tdtp.DrugDeclarationId AND DirectionToPayId <> Td.Id) AS [refPrimaryApplication]
			FROM [inserted] AS Td
			INNER JOIN [dbo].[Employees] AS Te ON Te.Id = Td.CreateEmployeeId
			INNER JOIN [dbo].[EXP_DirectionToPays_DrugDeclaration] AS Tdtp ON Tdtp.DirectionToPayId = Td.Id
			WHERE Td.[Type] = 2
		END

		-- Add translate refuse direction to pay 
		IF (UPDATE(StatusId) AND (select StatusId from inserted) = @expireStatusId)
		BEGIN
			INSERT INTO [dbo].[I1c_trl_ApplicationRefuseState]
				   ([Id]
				   ,[ApplicationNumber]
				   ,[ApplicationDatetime]
				   ,[IsRefuse]
				   ,[RefuseDatetime]
				   ,[refApplication])
			 SELECT
				   NEWID() AS [Id]
				   ,Td.Number AS [ApplicationNumber]
				   ,Td.DirectionDate AS [ApplicationDatetime]
				   ,1 AS [IsRefuse]
				   ,GETDATE() AS [RefuseDatetime]
				   ,Td.Id AS [refApplication]
			 FROM [inserted] AS Td
		END
	END
END
GO
