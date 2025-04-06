CREATE FUNCTION fn_BookBorrowingFrequency(@BookID INT)
RETURNS INT
AS
BEGIN
    DECLARE @Count INT;
    SELECT @Count = COUNT(*) FROM Loans WHERE BookID = @BookID;
    RETURN @Count;
END;
