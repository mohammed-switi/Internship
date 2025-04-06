-- Seed 1000 sample borrowers
INSERT INTO Borrowers (FirstName, LastName, Email, DateOfBirth, MembershipDate)
SELECT
    'FirstName' + CAST(n AS VARCHAR(10)) AS FirstName,
    'LastName' + CAST(n AS VARCHAR(10)) AS LastName,
    'email' + CAST(n AS VARCHAR(10)) + '@example.com' AS Email,
    DATEADD(YEAR, -(20 + (n % 30)), GETDATE()) AS DateOfBirth,  -- Varying ages between ~20 and 50
    DATEADD(DAY, -(n % 365), GETDATE()) AS MembershipDate          -- Varying membership dates within the last year
FROM 
    (SELECT TOP (1000) ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS n
     FROM master..spt_values) AS Numbers;
