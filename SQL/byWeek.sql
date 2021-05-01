DECLARE @year INT = 2019

  SELECT
    StudentId,
	CONVERT(VARCHAR(20),Name),
    ISNULL(CONVERT(VARCHAR(4),[1]),'') AS [1],
    ISNULL(CONVERT(VARCHAR(4),[2]),'') AS [2],
    ISNULL(CONVERT(VARCHAR(4),[3]),'') AS [3],
    ISNULL(CONVERT(VARCHAR(4),[4]),'') AS [4],
    ISNULL(CONVERT(VARCHAR(4),[5]),'') AS [5],
    ISNULL(CONVERT(VARCHAR(4),[6]),'') AS [6],
    ISNULL(CONVERT(VARCHAR(4),[7]),'') AS [7],
    ISNULL(CONVERT(VARCHAR(4),[8]),'') AS [8],
    ISNULL(CONVERT(VARCHAR(4),[9]),'') AS [9],
    ISNULL(CONVERT(VARCHAR(4),[10]),'') AS [10],
    ISNULL(CONVERT(VARCHAR(4),[11]),'') AS [11],
    ISNULL(CONVERT(VARCHAR(4),[12]),'') AS [12],
    ISNULL(CONVERT(VARCHAR(4),[13]),'') AS [13],
    ISNULL(CONVERT(VARCHAR(4),[14]),'') AS [14],
    ISNULL(CONVERT(VARCHAR(4),[15]),'') AS [15],
    ISNULL(CONVERT(VARCHAR(4),[16]),'') AS [16],
    ISNULL(CONVERT(VARCHAR(4),[17]),'') AS [17],
    ISNULL(CONVERT(VARCHAR(4),[18]),'') AS [18],
    ISNULL(CONVERT(VARCHAR(4),[19]),'') AS [19],
    ISNULL(CONVERT(VARCHAR(4),[20]),'') AS [20],
    ISNULL(CONVERT(VARCHAR(4),[21]),'') AS [21],
    ISNULL(CONVERT(VARCHAR(4),[22]),'') AS [22],
    ISNULL(CONVERT(VARCHAR(4),[23]),'') AS [23],
    ISNULL(CONVERT(VARCHAR(4),[24]),'') AS [24],
    ISNULL(CONVERT(VARCHAR(4),[25]),'') AS [25],
    ISNULL(CONVERT(VARCHAR(4),[26]),'') AS [26],
    ISNULL(CONVERT(VARCHAR(4),[27]),'') AS [27],
    ISNULL(CONVERT(VARCHAR(4),[28]),'') AS [28],
    ISNULL(CONVERT(VARCHAR(4),[29]),'') AS [29],
    ISNULL(CONVERT(VARCHAR(4),[30]),'') AS [30],
    ISNULL(CONVERT(VARCHAR(4),[31]),'') AS [31],
    ISNULL(CONVERT(VARCHAR(4),[32]),'') AS [32],
    ISNULL(CONVERT(VARCHAR(4),[33]),'') AS [33],
    ISNULL(CONVERT(VARCHAR(4),[34]),'') AS [34],
    ISNULL(CONVERT(VARCHAR(4),[35]),'') AS [35],
    ISNULL(CONVERT(VARCHAR(4),[36]),'') AS [36],
    ISNULL(CONVERT(VARCHAR(4),[37]),'') AS [37],
    ISNULL(CONVERT(VARCHAR(4),[38]),'') AS [38],
    ISNULL(CONVERT(VARCHAR(4),[39]),'') AS [39],
    ISNULL(CONVERT(VARCHAR(4),[40]),'') AS [40],
    ISNULL(CONVERT(VARCHAR(4),[41]),'') AS [41],
    ISNULL(CONVERT(VARCHAR(4),[42]),'') AS [42],
    ISNULL(CONVERT(VARCHAR(4),[43]),'') AS [43],
    ISNULL(CONVERT(VARCHAR(4),[44]),'') AS [44],
    ISNULL(CONVERT(VARCHAR(4),[45]),'') AS [45],
    ISNULL(CONVERT(VARCHAR(4),[46]),'') AS [46],
    ISNULL(CONVERT(VARCHAR(4),[47]),'') AS [47],
    ISNULL(CONVERT(VARCHAR(4),[48]),'') AS [48],
    ISNULL(CONVERT(VARCHAR(4),[49]),'') AS [49],
    ISNULL(CONVERT(VARCHAR(4),[50]),'') AS [50],
    ISNULL(CONVERT(VARCHAR(4),[51]),'') AS [51],
    ISNULL(CONVERT(VARCHAR(4),[52]),'') AS [52],
	ISNULL([1],0) + ISNULL([2],0) +ISNULL([3],0) +ISNULL([4],0) +ISNULL([5],0) +ISNULL([6],0) +ISNULL([7],0) +ISNULL([8],0) +ISNULL([9],0) +ISNULL([10],0) +
	ISNULL([11],0) + ISNULL([12],0) +ISNULL([13],0) +ISNULL([14],0) +ISNULL([15],0) +ISNULL([16],0) +ISNULL([17],0) +ISNULL([18],0) +ISNULL([19],0) +ISNULL([20],0) +
	ISNULL([21],0) + ISNULL([22],0) +ISNULL([23],0) +ISNULL([24],0) +ISNULL([25],0) +ISNULL([26],0) +ISNULL([27],0) +ISNULL([28],0) +ISNULL([29],0) +ISNULL([30],0) +
	ISNULL([31],0) + ISNULL([32],0) +ISNULL([33],0) +ISNULL([34],0) +ISNULL([35],0) +ISNULL([36],0) +ISNULL([37],0) +ISNULL([38],0) +ISNULL([39],0) +ISNULL([40],0) +
	ISNULL([41],0) + ISNULL([42],0) +ISNULL([43],0) +ISNULL([44],0) +ISNULL([45],0) +ISNULL([46],0) +ISNULL([47],0) +ISNULL([48],0) +ISNULL([49],0) +ISNULL([50],0) +
	ISNULL([51],0) + ISNULL([52],0) AS Total
FROM
(Select 
	sr.StudentId,
	s.Name,
	sr.Hours,
	datepart(wk,sr.Date) as Week
  from
    dbo.ServiceRecords sr
	JOIN dbo.Students s ON s.Id = sr.StudentId
	WHERE YEAR (sr.Date) = @year ) source
PIVOT
(
    SUM(Hours)
    FOR Week
    IN	(  
		  [1],  [2],  [3],  [4],  [5],  [6],  [7],  [8],  [9], [10], [11], [12],[13], [14], [15], [16], [17], [18], [19], [20],
		 [21], [22], [23], [24], [25], [26], [27], [28], [29], [30], [31], [32],[33], [34], [35], [36], [37], [38], [39], [40],
		 [41], [42], [43], [44], [45], [46], [47], [48], [49], [50], [51], [52]
		)
) AS pvtWeek 
ORDER BY Name