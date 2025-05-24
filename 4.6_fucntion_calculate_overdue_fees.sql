CREATE FUNCTION fn_CalculateOverdueFees(@LoanID INT)
RETURNS DECIMAL(10,2)
AS
BEGIN
    DECLARE
        @DueDate        DATE,
        @DateReturned   DATE,
        @OverdueDays    INT,
        @Fee            DECIMAL(10,2),

        @GracePeriod    INT             = 30,    
        @RateBefore30   DECIMAL(10,2)   = 1.00,  
        @RateAfter30    DECIMAL(10,2)   = 2.00;  

    SELECT 
        @DueDate      = DueDate,
        @DateReturned = DateReturned
    FROM Loans
    WHERE LoanID = @LoanID;

    IF @DateReturned IS NULL OR @DateReturned <= @DueDate
        RETURN 0;

    SET @OverdueDays = DATEDIFF(DAY, @DueDate, @DateReturned);

    IF @OverdueDays <= @GracePeriod
        SET @Fee = @OverdueDays * @RateBefore30;
    ELSE
        SET @Fee = (@GracePeriod * @RateBefore30)
                 + ((@OverdueDays - @GracePeriod) * @RateAfter30);

    RETURN @Fee;
END;
GO
