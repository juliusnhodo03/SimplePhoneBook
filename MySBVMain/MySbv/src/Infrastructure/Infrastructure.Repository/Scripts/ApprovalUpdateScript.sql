


USE MYSBV


UPDATE dbo.Account
SET StatusID = 4
WHERE StatusID = 13 OR StatusID = 7



DELETE FROM dbo.Task
WHERE StatusID = 13 OR StatusID = 7



