WITH BorrowerLoans AS (
    SELECT 
        BorrowerID,
        COUNT(*) AS TotalLoans,
        SUM(CASE WHEN DateReturned IS NULL THEN 1 ELSE 0 END) AS UnreturnedLoans
    FROM Loans
    GROUP BY BorrowerID
    HAVING 
        COUNT(*) >= 2
        AND SUM(CASE WHEN DateReturned IS NULL THEN 1 ELSE 0 END) = COUNT(*)
)
SELECT b.*
FROM Borrowers b
JOIN BorrowerLoans bl 
  ON b.BorrowerID = bl.BorrowerID;
