@REM Mapea la base de datos.
cd ..
dotnet ef dbcontext scaffold "Server=DESKTOP-HFHOG6G\SQLEXPRESS;Initial Catalog=automotriz;User ID=sa;Password=qwerty123!;" Microsoft.EntityFrameworkCore.SqlServer -o Models --force