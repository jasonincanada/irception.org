
drop table FirstChannelVisit

create table ChannelVisit (
	FKUserID int not null,
	FKChannelID int not null,
	DateVisit datetime not null
)

alter table ChannelVisit
add constraint PK_ChannelVisit
primary key clustered (FKUserID, FKChannelID, DateVisit)

alter table ChannelVisit
add constraint FK_ChannelVisit_FKUserID
foreign key (FKUserID)
references [User](UserID)

alter table ChannelVisit
add constraint FK_ChannelVisit_FKChannelID
foreign key (FKChannelID)
references [Channel](ChannelID)


--drop table autonsfw

create table AutoNSFW (
	AutoNSFWID int identity(1,1) not null primary key,
	FKChannelID int not null, 
	DateAdded datetime not null,
	DateDeleted datetime null,
	FKUserIDAddedBy int not null,
	FKUserIDDeletedBy int null,
	Fragment varchar(80) not null	
)

alter table AutoNSFW
add constraint FK_AutoNSFW_FKChannelID
foreign key (FKChannelID)
references Channel(ChannelID)

alter table AutoNSFW
add constraint FK_AutoNSFW_FKUserIDAddedBy
foreign key (FKUserIDAddedBy)
references [User](UserID)

alter table AutoNSFW
add constraint FK_AutoNSFW_FKUserIDDeletedBy
foreign key (FKUserIDDeletedBy)
references [User](UserID)
