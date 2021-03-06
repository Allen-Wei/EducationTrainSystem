﻿/*
Data Source=(localdb)\v11.0;Initial Catalog=EducationTrainSystem;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False
sqlmetal /code:E:\LINQ2SQL\EducationTrainSystem.cs /server:ozosserver\ozos /user:sa /password:evoss /database:EducationTrainSystem /views /functions /sprocs /context:EducationTrain /pluralize /namespace:EducationTrainSystem.Models
sqlmetal /code:E:\EducationTrainSystem.cs /server:(localdb)\v11.0 /database:EducationTrainSystem /views /functions /sprocs /context:EducationTrain /pluralize /namespace:EducationTrainSystem.Models

		public EducationTrain() :
            base(global::System.Configuration.ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString, mappingSource)
        {
            OnCreated();
        }

*/


/*
Colleges => Professional
*/

--RegistrationAddress, Colleges(成考, 自考), Subjects
if exists (select * from sys.tables where name = 'KeyValueMatches') drop table KeyValueMatches
go
if exists (select * from sys.tables where name = 'KeyValueGroups') drop table KeyValueGroups
go
if exists (select * from sys.tables where name = 'KeyValues') drop table KeyValues
go

create table KeyValueGroups(
	Id int identity(1,1) primary key,
	Name varchar(500) unique not null,
	Category varchar(500),
	Description varchar(500)
)
go
create table KeyValues(
	Id int identity(1,1) primary key,
	Name varchar(500) unique not null,
	Value varchar(max),
	Description varchar(500),
	Mark varchar(500)
)
go
create table KeyValueMatches(
	Id int identity(1,1),
	GroupId int not null,
	ValueId int not null,
	constraint FK_KVM_KVG_GroupId foreign key (GroupId) references KeyValueGroups(Id) on delete cascade on update cascade,
	constraint FK_KVM_KV_ValueId foreign key (ValueId) references KeyValues(Id) on delete cascade on update cascade,
	Constraint PK_KVM_GroupId_ValueId primary key (GroupId, ValueId)
)

go
if exists (select * from sys.tables where name = 'Trains') drop table Trains
go
create table Trains(
	Id int identity(1,1) primary key, 
	Name varchar(500) unique, 
	Description varchar(500) not null,
	Category varchar(500)
)
go
if exists (select * from sys.tables where name = 'Courses') drop table Courses
go
create table Courses(
	Id int identity(1,1) primary key, 
	Name varchar(500) unique, 
	Description varchar(500), 
	TrainId int not null,
	constraint FK_Courses_Trains_TrainId foreign key (TrainId) references Trains(Id)
)
go 
if exists (select * from sys.tables where name = 'RegUsers') drop table RegUsers
go
create table RegUsers(
	Gid uniqueidentifier primary key default newid(),
	RegDate datetime not null default getdate(),			--报名日期
	Name varchar(500) not null,								--名字
	Gender bit not null,									--性别
	Nation varchar(500),									--民族
	Phone varchar(500),										--电话号码
	Phone2 varchar(500),									--电话号码
	CardId varchar(500),									--身份证号
	LiveAddress varchar(500),								--学员住宿地址
	HomeAddress varchar(500)								--户籍
)
go
if exists (select * from sys.tables where name = 'Registrations') drop table Registrations
go
create table Registrations
(
	Id int identity(1,1) primary key,						--记录Id
	Gid Uniqueidentifier  not null default NewID(),		
	GenerateDate datetime not null default getdate(),		--Generate Date

	ReceiptNumber varchar(500) not null,					--票号
	Price money not null,									--费用
	Agent varchar(500) not null,							--代理人
	Payee varchar(500) not null,							--收款人
	Note varchar(max),										--备注
	Confirmed bit not null default 0,
	Address varchar(500),
	
	RegUserId uniqueidentifier null,						
	TrainCategory varchar(500) null,
	TrainId uniqueidentifier null
)

go
if exists (select * from sys.tables where name = 'EduTrains') drop table EduTrains
go
create table EduTrains(
	Gid uniqueidentifier primary key default newid(),
	Course varchar(500) not null,
	RegCollege varchar(500) null,
	RegMajor varchar(500) null,
	EduType varchar(500) null,
	CurrentCollege varchar(500) null,
	CurrentGrade varchar(500) null
)
go
if exists (select * from sys.tables where name = 'CertificationTrains') drop table CertificationTrains
go
create table CertificationTrains(
	Gid uniqueidentifier primary key default newid(),
	Course varchar(500) not null,
	CurrentCollege varchar(500) null,
	CurrentGrade varchar(500) null,
	EduType varchar(500) null
)
go
if exists (select * from sys.tables where name = 'SchoolTrains') drop table SchoolTrains
go
create table SchoolTrains(
	Gid uniqueidentifier primary key default newid(),
	Course varchar(500) not null,
	RegStage varchar(500)
)
go
if exists (select * from sys.tables where name = 'SchoolSubjects') drop table SchoolSubjects
go
create table SchoolSubjects(
	Id int identity(1,1) primary key, 
	TrainId uniqueidentifier not null, 
	Name varchar(500) not null,
	Hours int null,
	PricePerHours money null
)

if exists (select * from sys.tables where name = 'Roles') drop table Roles
go
CREATE TABLE Roles
(
  RoleName varchar(200) NOT NULL,
  ApplicationName varchar(100) NOT NULL,
    CONSTRAINT PKRoles PRIMARY KEY (RoleName, ApplicationName)
)
 go
if exists (select * from sys.tables where name = 'UsersInRoles') drop table UsersInRoles
go
CREATE TABLE UsersInRoles
(
  UserId varchar(200) NOT NULL,
  RoleName varchar(200) NOT NULL,
  ApplicationName varchar(100) NOT NULL,
    CONSTRAINT PKUsersInRoles PRIMARY KEY (UserId, RoleName, ApplicationName)
)
go
if exists (select * from sys.tables where name = 'Users') drop table Users
go
create table Users
(
Id int identity(1,1),
Code varchar(200) primary key,
Email varchar(200) unique null,
Password varchar(500) not null
)

--#endregion


/*
KeyValueGroup(Id, Name, Category, Description): RegistrationAddress, Course(学历教育, 资格证培训, 中小学辅导), College(成考, 自考)
KeyValueMatches(GroupId, ValueId, Description)
KeyValues(Id, Name, Value, Description)


1. Personal Information: 报名时间 姓名 性别 民族 身份证 电话 备用电话 户籍 住址 => RegUser
2. Course Category: 学历教育/资格证培训/中小学辅导 => Course: ...		=> ...Trains
3. Id, Gid, GenerateDate, ReceiptNumber, Agent, Payee, Place, Confirmed, Note	=> Registraion


RegUsers(Gid, ...)

EducationTrains(Gid, Course, RegCollege, RegMajor, EduType, CurrentCollege, CurrentGrade) 学历教育
CertificationTrains(Gid, Course, CurrentCollege, CurrentGrade, EduType) 资格证培训
SchoolTrains(Gid, Course, RegStage) 中小学辅导
	SchoolSubjects(Id, TrainId, Name, Hours, PricePerSubject)

Registraion(Id, Gid, GenerateDate, ReceiptNumber, Agent, Payee, Place, Confirmed, Note, RegUserId, RegTrainName, RegTrainId)
*/