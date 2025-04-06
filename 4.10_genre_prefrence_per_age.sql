WITH BorrowerAge AS (
    SELECT BorrowerID, DATEDIFF(YEAR, DateOfBirth, GETDATE()) AS Age
    FROM Borrowers
),
GenreCounts AS (
    SELECT 
        CASE 
            WHEN ba.Age BETWEEN 0 AND 10 THEN '0-10'
            WHEN ba.Age BETWEEN 11 AND 20 THEN '11-20'
            WHEN ba.Age BETWEEN 21 AND 30 THEN '21-30'
            WHEN ba.Age BETWEEN 31 AND 40 THEN '31-40'
            WHEN ba.Age BETWEEN 41 AND 50 THEN '41-50'
            ELSE '51+'
        END AS AgeGroup,
        b.Genre,
        COUNT(*) AS GenreCount
    FROM Loans l
    JOIN Books b ON l.BookID = b.BookID
    JOIN BorrowerAge ba ON l.BorrowerID = ba.BorrowerID
    GROUP BY 
        CASE 
            WHEN ba.Age BETWEEN 0 AND 10 THEN '0-10'
            WHEN ba.Age BETWEEN 11 AND 20 THEN '11-20'
            WHEN ba.Age BETWEEN 21 AND 30 THEN '21-30'
            WHEN ba.Age BETWEEN 31 AND 40 THEN '31-40'
            WHEN ba.Age BETWEEN 41 AND 50 THEN '41-50'
            ELSE '51+'
        END,
        b.Genre
)
SELECT AgeGroup, Genre, GenreCount
FROM (
    SELECT AgeGroup, Genre, GenreCount,
           RANK() OVER (PARTITION BY AgeGroup ORDER BY GenreCount DESC) AS RankPerGroup
    FROM GenreCounts
) AS Ranked
WHERE RankPerGroup = 1;
