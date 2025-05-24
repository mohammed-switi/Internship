CREATE TRIGGER trg_AuditStatusChange
ON Books
AFTER UPDATE
AS
BEGIN
    IF UPDATE(CurrentStatus)
    BEGIN
        INSERT INTO AuditLog(BookID, StatusChange)
        SELECT i.BookID, 
               'Changed from ' + d.CurrentStatus + ' to ' + i.CurrentStatus
        FROM inserted i
        JOIN deleted d ON i.BookID = d.BookID
        WHERE i.CurrentStatus <> d.CurrentStatus;
    END
END;
