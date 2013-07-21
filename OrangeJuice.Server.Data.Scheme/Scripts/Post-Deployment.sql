/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

if not exists (select 1 from dbo.[AccountType])
begin
	insert into dbo.[AccountType] ([AccountTypeId], [Name])
	values
	(1, 'Checking'),
	(2, 'Saving')
end
go

if not exists (select 1 from dbo.[Category])
begin
	insert into dbo.[Category] ([Name])
	values ('(None)')
end
go