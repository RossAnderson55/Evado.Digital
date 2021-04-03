DECLARE @Extension nVARCHAR(5)
DECLARE @DATE_STAMP NVARCHAR(12)
DECLARE @Path nVARCHAR(250)
DECLARE @PathPrefix nVARCHAR(250)

SET @DATE_STAMP = CONVERT(varchar(12),GETDATE(), 112) 
SET @PathPrefix = 'C:\Data\SQL_BACKUPS\DB_'
SET @Extension  =  +'.BAK'

PRINT '@DATE_STAMP=' +@DATE_STAMP
PRINT '@PathPrefix=' +@PathPrefix
PRINT '@Extension=' +@Extension

SET @Path = @PathPrefix + 'EVADO_DIGITAL' + '_'+ @DATE_STAMP + @Extension  
BACKUP DATABASE [EVADO_DIGITAL] TO DISK = @Path