SELECT TOP 3 
    DATENAME(WEEKDAY, DateBorrowed) AS DayOfWeek,
    CAST(100.0 * COUNT(*) / (SELECT COUNT(*) FROM Loans) AS DECIMAL(5,2)) AS LoanPercentage
FROM Loans
GROUP BY DATENAME(WEEKDAY, DateBorrowed)
ORDER BY LoanPercentage DESC;
