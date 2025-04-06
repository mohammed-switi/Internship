CREATE FUNCTION fn_CalculateOverdueFees(@LoanID INT)
RETURNS DECIMAL(10,2)
AS
BEGIN
    DECLARE @DueDate DATE, @DateReturned DATE, @OverdueDays INT, @Fee DECIMAL(10,2);
    
    SELECT @DueDate = DueDate, @DateReturned = DateReturned
    FROM Loans
    WHERE LoanID = @LoanID;
    
    IF @DateReturned IS NULL OR @DateReturned <= @DueDate
        RETURN 0;

    SET @OverdueDays = DATEDIFF(DAY, @DueDate, @DateReturned);
    
    IF @OverdueDays <= 30
        SET @Fee = @OverdueDays * 1.0;
    ELSE
        SET @Fee = (30 * 1.0) + ((@OverdueDays - 30) * 2.0);
    
    RETURN @Fee;
END;
