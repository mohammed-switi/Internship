SELECT b.BorrowerID, b.FirstName, b.LastName,
       COUNT(l.LoanID) AS BorrowCount,
       RANK() OVER (ORDER BY COUNT(l.LoanID) DESC) AS BorrowRank
FROM Borrowers b
LEFT JOIN Loans l ON b.BorrowerID = l.BorrowerID
GROUP BY b.BorrowerID, b.FirstName, b.LastName;
