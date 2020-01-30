create trigger TRG_NEW_USER
on dbo.AspNetUsers
after insert as
begin
set nocount on;
insert into Logic.Logins(username, aspnetuserguid) select UserName, Id from inserted -- Adds user to internal logins table
insert into dbo.AspNetUserRoles(UserId, RoleId) select inserted.id, AspRoles.Id from inserted, (select id from AspNetRoles where NormalizedName = 'USER') as AspRoles -- grants user the user role
insert into users.users(id) select Logic.Logins.id from Logic.Logins join inserted on aspnetuserguid = inserted.id -- adds user to internal users table
end