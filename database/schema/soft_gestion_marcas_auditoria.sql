/*
    Soft_Gestion — Auditoría en dbo.Marcas (bases que ya tenían la tabla sin columnas de auditoría).
    Ejecutar una vez en SQL Server. Idempotente.
*/
SET NOCOUNT ON;
GO

IF OBJECT_ID(N'dbo.Marcas', N'U') IS NOT NULL
BEGIN
    IF COL_LENGTH('dbo.Marcas', 'FechaCreacion') IS NULL
    BEGIN
        ALTER TABLE dbo.Marcas ADD
            FechaCreacion        DATETIME2(0)   NOT NULL
                CONSTRAINT DF_Marcas_FechaCreacion DEFAULT (SYSDATETIME()),
            UsuarioCreacion      NVARCHAR(50)   NOT NULL
                CONSTRAINT DF_Marcas_UsuarioCreacion DEFAULT (N'SISTEMA'),
            FechaModificacion    DATETIME2(0)   NULL,
            UsuarioModificacion  NVARCHAR(50)   NULL;
    END
END
GO

PRINT N'Auditoría en dbo.Marcas aplicada (o ya existía).';
GO
