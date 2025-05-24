declare @BorrowerID INT = 1
SELECT b.Title, b.Author, l.DateBorrowed, l.DueDate, l.DateReturned
FROM Books b
JOIN Loans l ON b.BookID = l.BookID
WHERE l.BorrowerID = @BorrowerID;
