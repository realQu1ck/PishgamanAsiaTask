Migration NLOGDB
Add-Migration NLOGInitDatabase -C NlogDbContext -o Data/NLogDatabase/Migrations
Update-Database -C NlogDbContext