USE [plizcard]

IF NOT EXISTS(SELECT name FROM syscolumns WHERE name = 'id' AND id = (SELECT id FROM sysobjects WHERE name = 'goods'))
BEGIN
alter table goods add [id] [smallint] IDENTITY(1,1) NOT NULL;
alter table goods add constraint PK_goods primary key clustered (id);
END

IF OBJECT_ID('OperatorGetGoods') IS NOT NULL DROP PROCEDURE OperatorGetGoods;
EXEC sp_executesql @statement = N'CREATE PROCEDURE [dbo].[OperatorGetGoods] 
		@operator SMALLINT = NULL,
		@errormessage NVARCHAR(100) OUTPUT
	AS
	BEGIN
		IF NOT EXISTS(SELECT id FROM operator WHERE id = @operator)
		BEGIN
			SET @errormessage = N''”казанный оператор не найден''
			RETURN(1)
		END
		SELECT
			g.code,
			g.name,
			g.id
		FROM
			goods AS g
		WHERE
			partner in (SELECT id FROM partner WHERE operator = @operator)
	END'


IF OBJECT_ID('goodlist') IS NOT NULL DROP Table goodlist;
CREATE TABLE [dbo].[goodlist](
	[id] [smallint] IDENTITY(1,1) NOT NULL,
	[caption] [nvarchar](250) NOT NULL,
	[operator] [smallint] NOT NULL,
 CONSTRAINT [PK_goodlist] PRIMARY KEY CLUSTERED ([id]))
GO

IF OBJECT_ID('goodlist') IS NOT NULL 
BEGIN
	DELETE FROM goodlist;
	INSERT INTO goodlist (caption, operator) VALUES ('Test list goods #1',2), ('Test list goods #2',2); 
END

IF OBJECT_ID('goodlistitems') IS NOT NULL DROP Table goodlistitems;
BEGIN
	CREATE TABLE [dbo].[goodlistitems](
		[id] [smallint] IDENTITY(1,1) NOT NULL,
		[good] [smallint] NOT NULL,
		[goodlist] [smallint] NOT NULL,
	 CONSTRAINT [PK_goodlistitems] PRIMARY KEY CLUSTERED ([id]))

	ALTER TABLE [dbo].[goodlistitems]  WITH NOCHECK ADD  CONSTRAINT [FK_goodlistitems_good] FOREIGN KEY([good])
	REFERENCES [dbo].[goods] ([id])

	ALTER TABLE [dbo].[goodlistitems]  WITH NOCHECK ADD  CONSTRAINT [FK_goodlistitems_goodlist] FOREIGN KEY([goodlist])
	REFERENCES [dbo].[goodlist] ([id])

	ALTER TABLE [dbo].[goodlistitems] NOCHECK CONSTRAINT [FK_goodlistitems_goodlist]
END
