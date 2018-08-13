IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'SelectBookkeepingByTimePeriod') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION SelectBookkeepingByTimePeriod
GO

CREATE function SelectBookkeepingByTimePeriod (@operator INT, @f_name NVARCHAR(10), @pos_name NVARCHAR(50), @f_date_start datetime, @f_date_end datetime)
RETURNS @rtnTable TABLE 
(
    partner int NOT NULL,
    pos int NOT NULL,
	gain decimal  (10,2) not null,
	added decimal (10,2) not null,
	redeemed decimal (10,2)  not null,
	clients int not null
) 
AS  
BEGIN  
	insert into @rtnTable
	   SELECT --distinct 
	   r.partner,
	   pos,
		SUM(gain) as gain
		,sum (added_bonus) as added
		,sum(redeemed_bonus) as redeemed 
		,sum (client_count) as clients
	FROM reganalytics r
	JOIN partner p ON p.id=r.partner 
	JOIN pos pos on pos.id = r.pos
	where 
		r.pos is not null
		and pos.shown = 1
		and r.operator = @operator
		AND p.name LIKE '%'+@f_name+'%'
		AND pos.name LIKE '%'+@f_name+'%'
		AND r.date>=@f_date_start AND r.date<@f_date_end
		group by r.partner,pos
	return
END
go
