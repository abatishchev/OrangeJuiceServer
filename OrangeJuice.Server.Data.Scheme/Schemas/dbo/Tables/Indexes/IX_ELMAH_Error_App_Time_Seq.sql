CREATE NONCLUSTERED INDEX [IX_ELMAH_Error_App_Time_Seq] ON dbo.[ELMAH_Error] 
(
    [Application]   ASC,
    [TimeUtc]       DESC,
    [Sequence]      DESC
) 
ON [PRIMARY]
GO