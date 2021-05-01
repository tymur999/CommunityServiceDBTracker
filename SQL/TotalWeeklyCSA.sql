DECLARE @year int = 2019
DECLARE @week int = 11

SELECT 
		50 AS [CSA Community], 
		200 AS [CSA Service],  
		500 AS [CSA Achievement],
		SUM(Hours) AS [Total Hours]  
FROM dbo.ServiceRecords 
WHERE 
	date <= DATEADD(dd,6,DATEADD(wk,DATEDIFF(wk,7,CAST(@year AS NVARCHAR(100))) + (@week -1),6))