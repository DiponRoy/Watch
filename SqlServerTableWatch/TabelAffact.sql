SELECT * FROM SmsOutbox;

INSERT INTO SmsOutbox (PhoneNumber ,Sms ,RequestDateTimeUtc) VALUES ('01719289217' ,'Hello, call me!!', GETUTCDATE());
UPDATE SmsOutbox SET PhoneNumber = '01719289218' WHERE Id = 1;
DELETE FROM SmsOutbox WHERE Id = 1;

TRUNCATE TABLE SmsOutbox;