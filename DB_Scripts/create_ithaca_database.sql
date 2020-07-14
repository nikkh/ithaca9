SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[LuisRegistrations]

CREATE TABLE [dbo].[LuisRegistrations] (
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Email] VARCHAR (320)   NOT     NULL,
    [LuisAccountNumber] VARCHAR (20) NOT NULL,
	[LuisFavoriteColor]       VARCHAR (20) NULL,
	[EnableSelfRegistration] [bit] NOT NULL,
   
);



insert into LuisRegistrations values('nick-9979@outlook.com', 'L000001', 'Red', 1) 
insert into LuisRegistrations values('luis-ithaca20-@outlook.com', 'L000020', 'Cyan', 0)
insert into LuisRegistrations values('luis-ithaca21-@outlook.com', 'L000021', 'Yellow', 0) 
insert into LuisRegistrations values('nick@nikkh.net', 'L000000', 'Magenta', 0) 
update LuisRegistrations set [EnableSelfRegistration] =1 where LuisAccountNumber = 'L000021'

select * from LuisRegistrations

select * from LuisRegistrations  where Email = 'nick-9979@outlook.com'

