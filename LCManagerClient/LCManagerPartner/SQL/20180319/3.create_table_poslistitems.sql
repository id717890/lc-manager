USE [plizcardtest]
GO

/****** Object:  Table [dbo].[poslistitems]    Script Date: 19.03.2018 11:12:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[poslistitems](
	[id] [smallint] IDENTITY(1,1) NOT NULL,
	[pos] [smallint] NOT NULL,
	[poslist] [smallint] NOT NULL,
 CONSTRAINT [PK_poslistitems] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[poslistitems]  WITH CHECK ADD  CONSTRAINT [FK_poslistitems_pos] FOREIGN KEY([pos])
REFERENCES [dbo].[pos] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[poslistitems] CHECK CONSTRAINT [FK_poslistitems_pos]
GO

ALTER TABLE [dbo].[poslistitems]  WITH CHECK ADD  CONSTRAINT [FK_poslistitems_poslistnew] FOREIGN KEY([poslist])
REFERENCES [dbo].[poslist] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[poslistitems] CHECK CONSTRAINT [FK_poslistitems_poslistnew]
GO


