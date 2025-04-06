-- Insert 1000 sample books
INSERT INTO Books (Title, Author, ISBN, PublishedDate, Genre, ShelfLocation, CurrentStatus)
SELECT 
    'Book ' + CAST(n AS VARCHAR(10)) AS Title,
    'Author ' + CAST(n AS VARCHAR(10)) AS Author,
    LEFT(NEWID(), 13) AS ISBN, -- Generates a random string as ISBN
    DATEADD(DAY, -n, GETDATE()) AS PublishedDate,
    CASE WHEN n % 2 = 0 THEN 'Fiction' ELSE 'Non-Fiction' END AS Genre,
    'Shelf ' + CAST((n % 10) + 1 AS VARCHAR(10)) AS ShelfLocation,
    CASE WHEN n % 3 = 0 THEN 'Borrowed' ELSE 'Available' END AS CurrentStatus
FROM 
    (SELECT TOP (1000) ROW_NUMBER() OVER(ORDER BY (SELECT NULL)) AS n FROM master..spt_values) AS Numbers;
