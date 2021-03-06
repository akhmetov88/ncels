USE [ncels]
GO
/****** Object:  Trigger [dbo].[tmcin_update]    Script Date: 04.04.2017 0:39:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Kairat V. Beysenov
-- Create date: 11-01-2017
-- Description:	-
-- =============================================
ALTER TRIGGER [dbo].[tmcin_update]
   ON  [dbo].[TmcIns]
   AFTER update
AS 
BEGIN

	SELECT  [Number] FROM [dbo].[I1c_lims_Applications] AS Tla
	INNER JOIN inserted AS Ti ON Ti.[Id] = Tla.[Number];
	
	if (UPDATE(StateType) and (select StateType from deleted) = 12 and (select StateType from inserted) = 1
		AND @@ROWCOUNT = 0)
	begin
		INSERT INTO [dbo].[I1c_lims_Applications]
			(Number
			,[ContractNumber]
			,[ContractDate]
			,[LastDeliveryDate]
			,[Provider]
			,[ProviderBin]
			,[FrpersonFio]
			,[FrpersonId]
			,[FullDelivery]
			,[CreateDatetime])
		SELECT
		     [Id]
			,[ContractNumber]
			,[ContractDate]
			,[LastDeliveryDate] 
			,[Provider]
			,[ProviderBin]
			,(select DisplayName from [Employees] where Employees.Id = [OwnerEmployeeId])
			,[OwnerEmployeeId]
			,[IsFullDelivery]
			,[CreatedDate]
		FROM inserted

		if ((select [IsFullDelivery] from inserted) = 0)
		begin
			INSERT INTO [dbo].[I1c_lims_ApplicationElements]
			   (
			    [TmcCode]
			   ,[TmcNumber]
			   ,[Name]
			   ,[Unit]
			   ,[UnitCode]
			   ,[QuntityVolumeStr]
			   ,[QuntityVolume]
			   --,[Producer]
			   --,[ProducerExpirationDate]
			   --,[PartyNumber]
			   ,[Kind]
			   ,[KindCode]
			   --,[WarehouseNumber]
			   --,[CupboardNumber]
			   --,[ShelfNumber]
			   --,[DateOfReceiving]
			   ,[Paking]
			   ,[PakingCode]
			   ,[ExportDatetime]
			   ,[refApplication]
			   )
			SELECT
			   [Id]
			  ,[Code]
			  ,[Name]
			  ,(select Name from Dictionaries where Dictionaries.Id = [MeasureTypeDicId])
			  ,[MeasureTypeDicId]
			  ,[Count]
			  ,[Count]
			  --,[Manufacturer]
			  --,[ExpiryDate]
			  --,[Serial]
			  ,(select Name from Dictionaries where Dictionaries.Id = [TmcTypeDicId])
			  ,[TmcTypeDicId]
			  --,(select Name from Dictionaries where Dictionaries.Id = [StorageDicId])
			  --,[StorageDicId]
			  --,[Safe]
			  --,[Rack]
			  --,ReceivingDate
			  ,(select Name from Dictionaries where Dictionaries.Id = [PackageDicId])
			  ,[PackageDicId]
			  ,GETDATE()
			  ,[TmcInId]
		  FROM [dbo].[Tmcs]
		  where [TmcInId] in (select Id from inserted) and StateType = 0
		end
	end
END


