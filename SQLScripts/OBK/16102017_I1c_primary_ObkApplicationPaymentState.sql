USE [ncelsProd]
GO

/****** Object:  Table [dbo].[I1c_primary_ObkApplicationPaymentState]    Script Date: 16.10.2017 18:53:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[I1c_primary_ObkApplicationPaymentState](
	[Id] [uniqueidentifier] NOT NULL,
	[IsPaid] [bit] null,
	[PaymentDatetime] [datetime] NULL,
	[PaymentValue] [decimal](18, 2) NULL,
	[PaymentBill] [decimal](18, 2) NULL,
	[refContractId] [uniqueidentifier] NULL
 CONSTRAINT [PK_I1c_primary_ObkApplicationPaymentState] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[I1c_primary_ObkApplicationPaymentState] ADD  CONSTRAINT [DF_I1c_primary_ObkApplicationPaymentState_Id]  DEFAULT (newid()) FOR [Id]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Признак оплаты' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_ObkApplicationPaymentState', @level2type=N'COLUMN',@level2name=N'IsPaid'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Дата оплаты' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_ObkApplicationPaymentState', @level2type=N'COLUMN',@level2name=N'PaymentDatetime'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Сумма оплаты' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_ObkApplicationPaymentState', @level2type=N'COLUMN',@level2name=N'PaymentValue'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Выставленная сумма по счету' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_ObkApplicationPaymentState', @level2type=N'COLUMN',@level2name=N'PaymentBill'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ссылка на ИД договора' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'I1c_primary_ObkApplicationPaymentState', @level2type=N'COLUMN',@level2name=N'refContractId'
GO
