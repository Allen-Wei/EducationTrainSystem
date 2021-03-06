﻿
--Initial User and Roles
insert into Roles(RoleName, ApplicationName) values('sales', 'alan')
insert into UsersInRoles(UserId, RoleName, ApplicationName) values('sale1', 'sales', 'alan')
insert into Users(Code, Email,Password) values('sale1', 'sale@qq.com', 'saleone')
go

insert into Roles(RoleName, ApplicationName) values('admin', 'alan')
go
insert into Users(Code, Email, Password) values('admin', 'admin@edu.com', '123456')
go
insert into UsersInRoles(UserId, RoleName, ApplicationName) values('admin', 'admin', 'alan')

--Initial CourseCategories
insert into Trains(Name, Description, Category) values('学历教育', '学历教育', 'EduTrains');
insert into Trains(Name, Description, Category) values('资格证培训', '资格证培训', 'CertificationTrains');
insert into Trains(Name, Description, Category) values('中小学辅导', '中小学辅导', 'SchoolTrains');

--Initial Courses
insert into Courses(Name, Description, TrainId) values('研究生','研究生', 1)
insert into Courses(Name, Description, TrainId) values('成人高考','成人高考', 1)
insert into Courses(Name, Description, TrainId) values('自学考试','自学考试', 1)
insert into Courses(Name, Description, TrainId) values('远程教育','远程教育', 1)

insert into Courses(Name, Description, TrainId) values('计算机二级','计算机二级', 2)
insert into Courses(Name, Description, TrainId) values('会计从业资格证','会计从业资格证', 2)
insert into Courses(Name, Description, TrainId) values('教师资格证','教师资格证', 2)

insert into Courses(Name, Description, TrainId) values('极品班','极品班', 3)
insert into Courses(Name, Description, TrainId) values('精品班','精品班', 3)
insert into Courses(Name, Description, TrainId) values('特色班','特色班', 3)
insert into Courses(Name, Description, TrainId) values('寒暑假制胜班','寒暑假制胜班', 3)
insert into Courses(Name, Description, TrainId) values('作业辅导班','作业辅导班', 3)




--Registration
insert into KeyValues(Name, Mark) values('河西', '报名地点')
insert into KeyValues(Name, Mark) values('津南', '报名地点')
insert into KeyValues(Name, Mark) values('红桥', '报名地点')
insert into KeyValues(Name, Mark) values('西青', '报名地点')
insert into KeyValues(Name, Mark) values('河北', '报名地点')
insert into KeyValues(Name, Mark) values('油田', '报名地点')
insert into KeyValues(Name, Mark) values('大港', '报名地点')
insert into KeyValues(Name, Mark) values('塘沽', '报名地点')
insert into KeyValues(Name, Mark) values('南开', '报名地点')
insert into KeyValues(Name, Mark) values('其他', '报名地点')
insert into KeyValueGroups(Name, Description) values('regaddress', '报名地点')
go
declare @gid int 
select @gid = Id from KeyValueGroups where Name = 'regaddress'
print @gid
insert into KeyValueMatches(GroupId, ValueId)
select @gid as GroupId, Id as ValueId from KeyValues where Mark = '报名地点'
go

--Colleges
insert into KeyValues(Name, Mark) values ('南开大学', 'college'), ('天津大学', 'college'), ('外国语学院', 'college'), ('财经大学', 'college'), ('中德职业技术', 'college'), ('电子信息技术', 'college')
insert into KeyValueGroups(Name, Description) values('成人高考', '成人高考')
insert into KeyValueGroups(Name, Description) values('自学考试', '自考')
go
declare @gid int 
select @gid = Id from KeyValueGroups where Name = 'chengkao'
insert into KeyValueMatches(GroupId, ValueId)
select @gid as GroupId, Id as ValueId from KeyValues where Mark = 'college'
go
declare @gid int 
select @gid = Id from KeyValueGroups where Name = 'zikao'
insert into KeyValueMatches(GroupId, ValueId)
select @gid as GroupId, Id as ValueId from KeyValues where Mark = 'college'


--Subjects
insert into KeyValues(Name, Mark) values('语文', 'subject'), ('数学', 'subject'), ('英语', 'subject')
insert into KeyValueGroups(Name, Description) values('subjects', '科目')
go
declare @gid int 
select @gid = Id from KeyValueGroups where Name = 'subjects'
insert into KeyValueMatches(GroupId, ValueId)
select @gid as GroupId, Id as ValueId from KeyValues where Mark = 'subject'
