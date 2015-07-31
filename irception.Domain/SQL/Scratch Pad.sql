﻿

create table Vote (
	FKURLID int not null,
	FKUserID int not null,
	Value	int not null,
	DateVoted datetime not null
)

alter table Vote add constraint PK_Vote primary key clustered (FKURLID, FKUserID)

alter table Vote
add constraint FK_Vote_FKUserID
foreign key (FKUserID)
references [User](UserID)

alter table Vote
add constraint FK_Vote_FKURLID
foreign key (FKURLID)
references [URL](URLID)

/*
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
*/

/*
Lambda syntax: 

public List<FirstChannelVisit> GetFirstChannelVisits(int userID)
{
    return _context
        .ChannelVisits
        .Where(cv => cv.FKUserID == userID)
        .GroupBy(g => new
        {
            ChannelSlug = g.Channel.Slug,
            NetworkSlug = g.Channel.Network.Slug
        })
        .Select(cv => new FirstChannelVisit
        {
            ChannelSlug = cv.Key.ChannelSlug,
            NetworkSlug = cv.Key.NetworkSlug,
            DateVisit = cv.Min(c => c.DateVisit)
        })
        .ToList();
}

T-SQL:

SELECT
	chan.Slug AS ChannelSlug,
	net.Slug AS NetworkSlug,
	MIN(cv.DateVisit) AS DateVisit
FROM ChannelVisit cv
	JOIN Channel chan ON chan.ChannelID = cv.FKChannelID
	JOIN Network net ON net.NetworkID = chan.FKNetworkID
WHERE cv.FKUserID = @userID
GROUP BY
	chan.Slug,
	net.Slug
*/