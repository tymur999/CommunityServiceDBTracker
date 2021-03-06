DECLARE @year INT = 2019

  SELECT
    StudentId,
	CONVERT(VARCHAR(20),Name),
    ISNULL(CONVERT(VARCHAR(4),[1]),'') AS January,
    ISNULL(CONVERT(VARCHAR(4),[2]),'') AS February,
    ISNULL(CONVERT(VARCHAR(4),[3]),'') AS March,
    ISNULL(CONVERT(VARCHAR(4),[4]),'') AS April,
    ISNULL(CONVERT(VARCHAR(4),[5]),'') AS May,
    ISNULL(CONVERT(VARCHAR(4),[6]),'') AS June,
    ISNULL(CONVERT(VARCHAR(4),[7]),'') AS July,
    ISNULL(CONVERT(VARCHAR(4),[8]),'') AS August,
    ISNULL(CONVERT(VARCHAR(4),[9]),'') AS September,
    ISNULL(CONVERT(VARCHAR(4),[10]),'') AS Oktober,
    ISNULL(CONVERT(VARCHAR(4),[11]),'') AS November,
    ISNULL(CONVERT(VARCHAR(4),[12]),'') AS December,
	ISNULL([1],0) + ISNULL([2],0) +ISNULL([3],0) +ISNULL([4],0) +ISNULL([5],0) +ISNULL([6],0) 
	+ISNULL([7],0) +ISNULL([8],0) +ISNULL([9],0) +ISNULL([10],0) +ISNULL([11],0) +ISNULL([12],0) AS Total
FROM
(Select 
	sr.StudentId,
	s.Name,
	sr.Hours,
	MONTH(sr.Date) as Month
  from
    dbo.ServiceRecords sr
	JOIN dbo.Students s ON s.Id = sr.StudentId
	WHERE YEAR (sr.Date) = @year ) source
PIVOT
(
    SUM(Hours)
    FOR Month
    IN ( [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12] )
) AS pvtMonth 
ORDER BY Name