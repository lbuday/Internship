DROP TABLE AlternativeComparisons;
DROP TABLE Alternatives;
DROP TABLE CriteriaComparisons;
DROP TABLE Criteria;
DROP TABLE Choices;
DROP TABLE Users;

CREATE TABLE Users (
  UserID uniqueidentifier NOT NULL PRIMARY KEY,
  Username varchar(50) NOT NULL UNIQUE,
  Password varchar(255) NOT NULL,
  DateCreated DATETIME NOT NULL,
  DateUpdated DATETIME
);

CREATE TABLE Choices (
  ChoiceID uniqueidentifier NOT NULL PRIMARY KEY,
  ChoiceName varchar(50) NOT NULL,
  UserID uniqueidentifier NOT NULL REFERENCES Users(UserID),
  DateCreated DATETIME NOT NULL,
  DateUpdated DATETIME
);

CREATE TABLE Criteria (
  CriteriaID uniqueidentifier NOT NULL PRIMARY KEY,
  CriteriaName varchar(50) NOT NULL,
  CriteriaScore float,
  ChoiceID uniqueidentifier NOT NULL REFERENCES Choices(ChoiceID),
  DateCreated DATETIME NOT NULL,
  DateUpdated DATETIME
);

CREATE TABLE CriteriaComparisons (
  CriteriaID1 uniqueidentifier NOT NULL REFERENCES Criteria(CriteriaID),
  CriteriaID2 uniqueidentifier NOT NULL REFERENCES Criteria(CriteriaID),
  CriteriaRatio float NOT NULL,
  DateCreated DATETIME NOT NULL,
  DateUpdated DATETIME,
  PRIMARY KEY(CriteriaID1, CriteriaID2)
);

CREATE TABLE Alternatives (
  AlternativeID uniqueidentifier NOT NULL PRIMARY KEY,
  AlternativeName varchar(50) NOT NULL,
  AlternativeScore float,
  ChoiceID uniqueidentifier NOT NULL REFERENCES Choices(ChoiceID),
  DateCreated DATETIME NOT NULL,
  DateUpdated DATETIME
);

CREATE TABLE AlternativeComparisons (
  CriteriaID uniqueidentifier NOT NULL REFERENCES Criteria(CriteriaID),
  AlternativeID1 uniqueidentifier NOT NULL REFERENCES Alternatives(AlternativeID),
  AlternativeID2 uniqueidentifier NOT NULL REFERENCES Alternatives(AlternativeID),
  AlternativeRatio float NOT NULL,
  DateCreated DATETIME NOT NULL,
  DateUpdated DATETIME,
  PRIMARY KEY(CriteriaID, AlternativeID1, AlternativeID2)
);

