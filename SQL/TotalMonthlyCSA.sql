DECLARE @year int = 2019
DECLARE @month int = 11

SELECT 
		50 AS [CSA Community], 
		200 AS [CSA Service],  
		500 AS [CSA Achievement],
		SUM(Hours) AS [Total Hours]		 
FROM dbo.ServiceRecords 
WHERE 
	date < datefromparts(@year, @month + 1, 1)