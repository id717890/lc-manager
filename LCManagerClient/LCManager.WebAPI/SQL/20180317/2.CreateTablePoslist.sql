USE [plizcardtest]
GO

/****** Object:  Table [dbo].[poslist]    Script Date: 17.03.2018 15:53:49 ******/
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


