CREATE TRIGGER deleteUserChoices
    ON Users
    INSTEAD OF DELETE
AS
    DELETE FROM Choices
    WHERE UserID IN (SELECT deleted.UserID FROM deleted);
	DELETE FROM Users WHERE UserID IN (SELECT deleted.UserID FROM deleted);
GO

CREATE TRIGGER deleteChoiceAlternativesAndCriteria
    ON Choices
    INSTEAD OF DELETE
AS
    DELETE FROM Alternatives
    WHERE ChoiceID IN (SELECT deleted.ChoiceID FROM deleted);
	DELETE FROM Criteria
    WHERE ChoiceID IN (SELECT deleted.ChoiceID FROM deleted);
	DELETE FROM Choices WHERE ChoiceID IN (SELECT deleted.ChoiceID FROM deleted);
GO

CREATE TRIGGER deleteAlternativesComparisons
    ON Alternatives
    INSTEAD OF DELETE
AS
    DELETE FROM AlternativeComparisons
    WHERE AlternativeID1 IN (SELECT deleted.AlternativeID FROM deleted) OR AlternativeID2 IN (SELECT deleted.AlternativeID FROM deleted);
	DELETE FROM Alternatives WHERE AlternativeID IN (SELECT deleted.AlternativeID FROM deleted);
GO

CREATE TRIGGER deleteCriteriasComparisons
    ON Criteria
    INSTEAD OF DELETE
AS
    DELETE FROM AlternativeComparisons
    WHERE CriteriaID IN (SELECT deleted.CriteriaID FROM deleted);
	DELETE FROM CriteriaComparisons
    WHERE CriteriaID1 IN (SELECT deleted.CriteriaID FROM deleted) OR CriteriaID2 IN (SELECT deleted.CriteriaID FROM deleted);
	DELETE FROM Criteria WHERE CriteriaID IN (SELECT deleted.CriteriaID FROM deleted);
GO
