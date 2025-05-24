CREATE PROCEDURE sp_AddNewBorrower
    @FirstName VARCHAR(50),
    @LastName VARCHAR(50),
    @Email VARCHAR(100),
    @DateOfBirth DATE,
    @MembershipDate DATE
AS
BEGIN
    IF EXISTS(SELECT 1 FROM Borrowers WHERE Email = @Email)
    BEGIN
        RAISERROR ('Email already exists', 16, 1);
        RETURN;
    END

    INSERT INTO Borrowers (FirstName, LastName, Email, DateOfBirth, MembershipDate)
    VALUES (@FirstName, @LastName, @Email, @DateOfBirth, @MembershipDate);

    SELECT SCOPE_IDENTITY() AS NewBorrowerID;
END;
