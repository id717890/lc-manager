
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('[dbo].[OperatorBookkeepingPaging]') IS NOT NULL
	DROP PROCEDURE [dbo].[OperatorBookkeepingPaging]
GO
 
Create PROCEDURE [dbo].[OperatorBookkeepingPaging]
		@operator SMALLINT = NULL,
		@partner SMALLINT = NULL,
		@pos SMALLINT = NULL,

		@f_date_start DATE = '1900-01-01',
		@f_date_end DATE = '2900-01-01',
		@f_name NVARCHAR(250) = '',
		@pos_name NVARCHAR(250) = '',
		@f_added_more INT = 0,
		@f_added_less INT = 999999999,
		@f_redeemed_more INT = 0,
		@f_redeemed_less INT = 999999999,
		@f_buy_more INT = 0,
		@f_buy_less INT = 999999999,
		@f_clients_more INT = 0,
		@f_clients_less INT = 999999999,

		@start INT = NULL,
		@length INT = NULL,

		@total_rows INT = NULL OUTPUT,
	
		@errormessage NVARCHAR(100) OUTPUT
		AS SET NOCOUNT ON
		DECLARE 
			@current_day DATETIME,
			@m1 DATETIME,
			@m2 DATETIME,
			@m3 DATETIME,
			@m4 DATETIME,
			@m5 DATETIME,
			@m6 DATETIME,
			@m7 DATETIME,
			@m8 DATETIME,
			@m9 DATETIME,
			@m10 DATETIME,
			@m11 DATETIME,
			@m12 DATETIME,
			@m13 DATETIME;

		IF (@operator IS NULL AND @partner IS NULL AND @pos IS NULL) BEGIN SET @errormessage = 'Не задан идентификатор оператора, партнера или торговой точки' RETURN(1) END
	
		IF (@operator IS NOT NULL AND NOT EXISTS (SELECT id FROM operator WHERE id = @operator)) BEGIN SET @errormessage = 'Идентификатор оператора не найден' RETURN(2) END
		IF (@partner IS NOT NULL AND NOT EXISTS (SELECT id FROM partner WHERE id = @partner)) BEGIN SET @errormessage = 'Идентификатор партнера не найден' RETURN(3) END
		IF (@pos IS NOT NULL AND NOT EXISTS (SELECT id FROM pos WHERE id = @pos)) BEGIN SET @errormessage = 'Идентификатор торговой точки не найден' RETURN(4) END

		--Если для пагинации пришли -1, то выбираем все записи
		IF (@start = -1 AND @length = -1)
		BEGIN
			--от записи №1
			SET @start=1;
			--до максимально возможного значения для @length
			SELECT @length = POWER(2.,31)-1;
		END

		SET @current_day = DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0);
		SET @m1 = DATEADD(mm,0,@current_day);
		SET @m2 = DATEADD(mm,1,@current_day);
		SET @m3 = DATEADD(mm,2,@current_day);
		SET @m4 = DATEADD(mm,3,@current_day);
		SET @m5 = DATEADD(mm,4,@current_day);
		SET @m6 = DATEADD(mm,5,@current_day);
		SET @m7 = DATEADD(mm,6,@current_day);
		SET @m8 = DATEADD(mm,7,@current_day);
		SET @m9 = DATEADD(mm,8,@current_day);
		SET @m10 = DATEADD(mm,9,@current_day);
		SET @m11 = DATEADD(mm,10,@current_day);
		SET @m12 = DATEADD(mm,11,@current_day);
		SET @m13 = DATEADD(mm,12,@current_day);

		--print @m1;
		--print @m2;
		--print @m3;
		--print @m4;
		--print @m5;
		--print @m6;
		--print @m7;
		--print @m8;
		--print @m9;
		--print @m10;
		--print @m11;
		--print @m12;
		--print @m13;

		--Если процедура запущена под оператором
		IF (@operator IS NOT NULL AND @partner IS NULL AND @pos IS NULL)
		BEGIN
			--Получаем общее кол-во строк, нужно для пагинации
			SELECT @total_rows = COUNT(*) FROM (
				select * from (
					SELECT distinct 
						r.partner
						,r.pos 
						,SUM(gain) as gain
						,sum (added_bonus) as added
						,sum(redeemed_bonus) as redeemed 
						,sum (client_count) as clients
					FROM reganalytics r
					JOIN partner p ON p.id=r.partner 
					JOIN pos pos on pos.id = r.pos
					where r.operator = @operator 
						AND r.partner IS NOT NULL 
						AND r.pos IS NOT NULL 
						and pos.shown = 1
						AND p.name LIKE '%'+@f_name+'%'
						AND r.date>=@f_date_start AND r.date<@f_date_end
					GROUP BY r.partner,r.pos
			) res
			where 
				CAST(COALESCE(res.gain,0) AS BIGINT)>=@f_buy_more AND CAST(COALESCE(res.gain,0) AS BIGINT)<@f_buy_less
				AND CAST(COALESCE(res.added,0) AS BIGINT)>=@f_added_more AND CAST(COALESCE(res.added,0) AS BIGINT)<@f_added_less 
				AND	CAST(COALESCE(res.redeemed,0) AS BIGINT)>=@f_redeemed_more AND CAST(COALESCE(res.redeemed,0) AS BIGINT)<@f_redeemed_less
				AND CAST(COALESCE(res.clients,0) AS BIGINT)>=@f_clients_more AND CAST(COALESCE(res.clients,0) AS BIGINT)<@f_clients_less

		) t1
		
			--Результирующая выборка
			SELECT * FROM (
				SELECT t1.*, ROW_NUMBER() OVER ( ORDER BY name ) AS RowNum FROM (
					SELECT distinct 
						r.partner, 
						p.name,
						pos.name posName, 
						res.gain as gain, res.added as added, res.redeemed as redeemed, res.clients as clients
					
					,CAST(COALESCE(Jan.gain,0) AS BIGINT) AS g1, CAST(COALESCE(Jan.added,0) AS BIGINT) AS a1, CAST(COALESCE(Jan.redeemed,0) AS BIGINT) AS re1, CAST(COALESCE(Jan.clients,0) AS BIGINT) AS c1
					,CAST(COALESCE(Feb.gain,0) AS BIGINT) AS g2, CAST(COALESCE(Feb.added,0) AS BIGINT) AS a2, CAST(COALESCE(Feb.redeemed,0) AS BIGINT) AS re2, CAST(COALESCE(Feb.clients,0) AS BIGINT) AS c2
					,CAST(COALESCE(Mar.gain,0) AS BIGINT) AS g3, CAST(COALESCE(Mar.added,0) AS BIGINT) AS a3, CAST(COALESCE(Mar.redeemed,0) AS BIGINT) AS re3, CAST(COALESCE(Mar.clients,0) AS BIGINT) AS c3
					,CAST(COALESCE(Apr.gain,0) AS BIGINT) AS g4, CAST(COALESCE(Apr.added,0) AS BIGINT) AS a4, CAST(COALESCE(Apr.redeemed,0) AS BIGINT) AS re4, CAST(COALESCE(Apr.clients,0) AS BIGINT) AS c4
					,CAST(COALESCE(May.gain,0) AS BIGINT) AS g5, CAST(COALESCE(May.added,0) AS BIGINT) AS a5, CAST(COALESCE(May.redeemed,0) AS BIGINT) AS re5, CAST(COALESCE(May.clients,0) AS BIGINT) AS c5
					,CAST(COALESCE(Jun.gain,0) AS BIGINT) AS g6, CAST(COALESCE(Jun.added,0) AS BIGINT) AS a6, CAST(COALESCE(Jun.redeemed,0) AS BIGINT) AS re6, CAST(COALESCE(Jun.clients,0) AS BIGINT) AS c6
					,CAST(COALESCE(Jul.gain,0) AS BIGINT) AS g7, CAST(COALESCE(Jul.added,0) AS BIGINT) AS a7, CAST(COALESCE(Jul.redeemed,0) AS BIGINT) AS re7, CAST(COALESCE(Jul.clients,0) AS BIGINT) AS c7
					,CAST(COALESCE(Aug.gain,0) AS BIGINT) AS g8, CAST(COALESCE(Aug.added,0) AS BIGINT) AS a8, CAST(COALESCE(Aug.redeemed,0) AS BIGINT) AS re8, CAST(COALESCE(Aug.clients,0) AS BIGINT) AS c8
					,CAST(COALESCE(Sep.gain,0) AS BIGINT) AS g9, CAST(COALESCE(Sep.added,0) AS BIGINT) AS a9, CAST(COALESCE(Sep.redeemed,0) AS BIGINT) AS re9, CAST(COALESCE(Sep.clients,0) AS BIGINT) AS c9
					,CAST(COALESCE(Oct.gain,0) AS BIGINT) AS g10, CAST(COALESCE(Oct.added,0) AS BIGINT) AS a10, CAST(COALESCE(Oct.redeemed,0) AS BIGINT) AS re10, CAST(COALESCE(Oct.clients,0) AS BIGINT) AS c10
					,CAST(COALESCE(Nov.gain,0) AS BIGINT) AS g11, CAST(COALESCE(Nov.added,0) AS BIGINT) AS a11, CAST(COALESCE(Nov.redeemed,0) AS BIGINT) AS re11, CAST(COALESCE(Nov.clients,0) AS BIGINT) AS c11
					,CAST(COALESCE(Dec.gain,0) AS BIGINT) AS g12, CAST(COALESCE(Dec.added,0) AS BIGINT) AS a12, CAST(COALESCE(Dec.redeemed,0) AS BIGINT) AS re12, CAST(COALESCE(Dec.clients,0) AS BIGINT) AS c12
					FROM reganalytics r
					JOIN partner p ON p.id=r.partner
					join pos pos on pos.id = r.pos

					left join (select * from SelectBookkeepingByTimePeriod(@operator, @f_name,@pos_name, @f_date_start, @f_date_end)) res 
						on res.partner = r.partner and res.pos = r.pos

					left join (select * from SelectBookkeepingByTimePeriod(@operator, @f_name, @pos_name,@m1, @m2)) 
						Jan on Jan.partner = r.partner and Jan.pos = r.pos
		
					left join (select * from SelectBookkeepingByTimePeriod(@operator, @f_name, @pos_name,@m2, @m3)) Feb 
						on Feb.partner = r.partner and Feb.pos = r.pos

					left join (select * from SelectBookkeepingByTimePeriod(@operator, @f_name,@pos_name, @m3, @m4)) Mar 
						on Mar.partner = r.partner and Mar.pos = r.pos

					left join (select * from SelectBookkeepingByTimePeriod(@operator, @f_name,@pos_name, @m4, @m5)) Apr 
						on Apr.partner = r.partner and Apr.pos = r.pos

					left join (select * from SelectBookkeepingByTimePeriod(@operator, @f_name,@pos_name, @m5, @m6)) May 
						on May.partner = r.partner and May.pos = r.pos

					left join (select * from SelectBookkeepingByTimePeriod(@operator, @f_name,@pos_name, @m6, @m7)) Jun 
						on Jun.partner = r.partner and Jun.pos = r.pos
					
					left join (select * from SelectBookkeepingByTimePeriod(@operator, @f_name,@pos_name, @m7, @m8)) Jul 
						on Jul.partner = r.partner and Jul.pos = r.pos

					left join (select * from SelectBookkeepingByTimePeriod(@operator, @f_name,@pos_name, @m8, @m9)) Aug 
						on Aug.partner = r.partner and Aug.pos = r.pos

					left join (select * from SelectBookkeepingByTimePeriod(@operator, @f_name,@pos_name, @m9, @m10)) Sep 
						on Sep.partner = r.partner and Sep.pos = r.pos

					left join (select * from SelectBookkeepingByTimePeriod(@operator, @f_name,@pos_name, @m10, @m11)) Oct 
						on Oct.partner = r.partner and Oct.pos = r.pos

					left join (select * from SelectBookkeepingByTimePeriod(@operator, @f_name,@pos_name, @m11, @m12)) Nov 
						on Nov.partner = r.partner and Nov.pos = r.pos

					left join (select * from SelectBookkeepingByTimePeriod(@operator, @f_name, @pos_name,@m12, @m13)) Dec 
						on Dec.partner = r.partner and Dec.pos = r.pos

				
					WHERE r.operator = @operator 
						AND r.partner IS NOT NULL 
						AND r.pos IS NOT NULL 
						and pos.shown = 1
						AND p.name LIKE '%'+@f_name+'%'
						AND pos.name LIKE '%'+@pos_name+'%'
						AND CAST(COALESCE(res.gain,0) AS BIGINT)>=@f_buy_more AND CAST(COALESCE(res.gain,0) AS BIGINT)<@f_buy_less
						AND CAST(COALESCE(res.added,0) AS BIGINT)>=@f_added_more AND CAST(COALESCE(res.added,0) AS BIGINT)<@f_added_less 
						AND	CAST(COALESCE(res.redeemed,0) AS BIGINT)>=@f_redeemed_more AND CAST(COALESCE(res.redeemed,0) AS BIGINT)<@f_redeemed_less
						AND CAST(COALESCE(res.clients,0) AS BIGINT)>=@f_clients_more AND CAST(COALESCE(res.clients,0) AS BIGINT)<@f_clients_less
				) AS t1
			) AS RowConstrainedResult WHERE RowNum >= @start AND RowNum < @length ORDER BY RowNum 
		END 

		RETURN(0)