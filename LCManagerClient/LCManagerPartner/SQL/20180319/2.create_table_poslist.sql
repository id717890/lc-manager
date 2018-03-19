USE [plizcardtest]
GO

/****** Object:  Table [dbo].[poslist]    Script Date: 19.03.2018 11:12:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[poslist](
	[id] [smallint] IDENTITY(1,1) NOT NULL,
	[caption] [nvarchar](250) NOT NULL,
	[operator] [smallint] NOT NULL,
 CONSTRAINT [PK_poslistnew] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[poslist]  WITH CHECK ADD  CONSTRAINT [FK_poslist_operator] FOREIGN KEY([operator])
REFERENCES [dbo].[operator] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[poslist] CHECK CONSTRAINT [FK_poslist_operator]
GO


