DECLARE @Month INT = 6, @Year INT = 2024;

SELECT TOP 1 b.Genre, COUNT(*) AS GenreCount,
       RANK() OVER (ORDER BY COUNT(*) DESC) AS GenreRank
FROM Loans l
JOIN Books b ON l.BookID = b.BookID
WHERE MONTH(l.DateBorrowed) = @Month AND YEAR(l.DateBorrowed) = @Year
GROUP BY b.Genre
ORDER BY GenreCount DESC;
