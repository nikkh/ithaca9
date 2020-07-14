SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- DROP TABLE dbo.Readings


CREATE TABLE [dbo].[Readings] (
	[ReadingType]          NTEXT        NULL,
    [DeviceId]       VARCHAR (20) NULL,
	[SensorId]       VARCHAR (20) NULL,
    [ReadingTime]          DATETIME   NULL,
	[Units]          NTEXT        NULL,
	[Reading] float null,
    [Pier]			NTEXT        NULL,
    [Location]        NTEXT        NULL,
	[UniqueId]        NTEXT        NULL
   
);

DROP TABLE [dbo].[Readings]


select count(*) from readings 