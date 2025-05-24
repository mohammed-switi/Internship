WITH BorrowerAge AS (
    SELECT 
        BorrowerID,
        CASE 
            WHEN DATEDIFF(YEAR, DateOfBirth, GETDATE()) BETWEEN 0  AND 10 THEN '0-10'
            WHEN DATEDIFF(YEAR, DateOfBirth, GETDATE()) BETWEEN 11 AND 20 THEN '11-20'
            WHEN DATEDIFF(YEAR, DateOfBirth, GETDATE()) BETWEEN 21 AND 30 THEN '21-30'
            WHEN DATEDIFF(YEAR, DateOfBirth, GETDATE()) BETWEEN 31 AND 40 THEN '31-40'
            WHEN DATEDIFF(YEAR, DateOfBirth, GETDATE()) BETWEEN 41 AND 50 THEN '41-50'
            ELSE '51+'
        END AS AgeGroup
    FROM Borrowers
)
SELECT
    ba.AgeGroup,
    b.Genre,
    COUNT(*) AS GenreCount
FROM Loans l
JOIN Books b 
  ON l.BookID = b.BookID
JOIN BorrowerAge ba 
  ON l.BorrowerID = ba.BorrowerID
GROUP BY 
    ba.AgeGroup,
    b.Genre
HAVING 
    COUNT(*) >= ALL (
        SELECT COUNT(*)
        FROM Loans l2
        JOIN Books b2 
          ON l2.BookID = b2.BookID
        JOIN BorrowerAge ba2 
          ON l2.BorrowerID = ba2.BorrowerID
        WHERE ba2.AgeGroup = ba.AgeGroup
        GROUP BY b2.Genre
    );
