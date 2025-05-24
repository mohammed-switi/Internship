CREATE PROCEDURE sp_BorrowedBooksReport
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SELECT b.Title, b.Author, br.FirstName, br.LastName,
           l.DateBorrowed, l.DueDate, l.DateReturned
    FROM Loans l
    JOIN Books b ON l.BookID = b.BookID
    JOIN Borrowers br ON l.BorrowerID = br.BorrowerID
    WHERE l.DateBorrowed BETWEEN @StartDate AND @EndDate;
END;
