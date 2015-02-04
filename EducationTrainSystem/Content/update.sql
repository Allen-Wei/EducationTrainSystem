--Date: 2015-02-03
--rename field name
sp_rename 'SchoolSubjects.PricePerSubject', 'PricePerHours'

--drop foreign key constrate
alter table SchoolSubjects drop constraint FK_SS_ST_TrainId
