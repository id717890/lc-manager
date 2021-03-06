USE [plizcard]
GO
/****** Object:  StoredProcedure [dbo].[OperatorGetPos]    Script Date: 21.03.2018 15:13:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[OperatorGetPos] @operator SMALLINT = NULL,
		@errormessage NVARCHAR(100) OUTPUT
	AS
	BEGIN
		IF NOT EXISTS(SELECT id FROM operator WHERE id = @operator)
		BEGIN
			SET @errormessage = N'Указанный оператор не найден'
			RETURN(1)
		END
		SELECT
			rn.name AS region,
			cy.name AS city,
			ps.address AS posaddress,
			ps.id AS id
		FROM
			pos AS ps
			LEFT JOIN city AS cy ON ps.city = cy.id
			LEFT JOIN region AS rn ON cy.region = rn.id
		WHERE
			partner in (SELECT id FROM partner WHERE operator = @operator)
			AND ps.name <> N'Анкета_офис'
	END