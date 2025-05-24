-- Seed 1000 sample loans
-- Assumes BookID and BorrowerID values range from 1 to 1000 (generated in previous scripts)
INSERT INTO Loans (BookID, BorrowerID, DateBorrowed, DueDate, DateReturned)
SELECT 
    ((n * 7) % 1000) + 1 AS BookID,         -- Arbitrary calculation to pick a BookID between 1 and 1000
    ((n * 5) % 1000) + 1 AS BorrowerID,       -- Arbitrary calculation to pick a BorrowerID between 1 and 1000
    DATEADD(DAY, -n, GETDATE()) AS DateBorrowed,
    DATEADD(DAY, -n + 14, GETDATE()) AS DueDate,
    CASE 
        WHEN n % 4 = 0 THEN DATEADD(DAY, -n + 7, GETDATE())  -- For every 4th record, simulate a returned loan
        ELSE NULL                                              -- Otherwise, not returned yet
    END AS DateReturned
FROM 
    (SELECT TOP (1000) ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS n
     FROM master..spt_values) AS Numbers;
