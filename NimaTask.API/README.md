Migration NLOGDB
Add-Migration NLOGInitDatabase -C NlogDbContext -o Data/NLogDatabase/Migrations
Update-Database -C NlogDbContext

Migration NimaTaskDB
Add-Migration NimaTaskInitDatabase -C NimaTaskDbContext -o Data/NimaTaskDatabase/Migrations
Update-Database -Context NimaTaskDbContext