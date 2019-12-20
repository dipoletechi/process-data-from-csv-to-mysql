CREATE TABLE filereadstatus (
  [Id] [int] IDENTITY(1,1) NOT NULL,
  [FileName] varchar(1000) DEFAULT NULL,
  [FileStatus] [int] DEFAULT NULL,
  [ProcessedOn] datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY ([Id])
)
