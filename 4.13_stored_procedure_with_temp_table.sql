CREATE PROCEDURE sp_OverdueBorrowers
AS
BEGIN
    CREATE TABLE #OverdueBorrowers (
        BorrowerID INT
    );

    INSERT INTO #OverdueBorrowers (BorrowerID)
    SELECT DISTINCT BorrowerID
    FROM Loans
    WHERE DateReturned IS NULL AND DATEDIFF(DAY, DueDate, GETDATE()) > 0;

    SELECT br.BorrowerID, br.FirstName, br.LastName, 
           l.LoanID, b.Title, l.DueDate,
           DATEDIFF(DAY, l.DueDate, GETDATE()) AS DaysOverdue
    FROM #OverdueBorrowers ob
    JOIN Borrowers br ON ob.BorrowerID = br.BorrowerID
    JOIN Loans l ON ob.BorrowerID = l.BorrowerID
    JOIN Books b ON l.BookID = b.BookID
    WHERE l.DateReturned IS NULL AND DATEDIFF(DAY, l.DueDate, GETDATE()) > 0;

    DROP TABLE #OverdueBorrowers;
END;
