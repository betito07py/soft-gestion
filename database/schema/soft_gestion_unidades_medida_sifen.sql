/*
    Soft_Gestion — Catálogo SIFEN (Descrip_Unidad, Codigo_Repr) y columna Codigo_SIFEN en UnidadesMedida.
    Ejecutar en SQL Server (idempotente donde aplica).
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

/* Migración desde esquema anterior (columna Descripcion) */
IF OBJECT_ID(N'dbo.UnidadesMedidaSIFEN', N'U') IS NOT NULL
   AND COL_LENGTH('dbo.UnidadesMedidaSIFEN', 'Descrip_Unidad') IS NULL
   AND COL_LENGTH('dbo.UnidadesMedidaSIFEN', 'Descripcion') IS NOT NULL
BEGIN
    IF EXISTS (
        SELECT 1 FROM sys.foreign_keys
        WHERE parent_object_id = OBJECT_ID(N'dbo.UnidadesMedida')
          AND name = N'FK_UnidadesMedida_UnidadesMedidaSIFEN')
        ALTER TABLE dbo.UnidadesMedida DROP CONSTRAINT FK_UnidadesMedida_UnidadesMedidaSIFEN;

    ALTER TABLE dbo.UnidadesMedidaSIFEN ADD Descrip_Unidad VARCHAR(30) NULL, Codigo_Repr VARCHAR(15) NULL;

    UPDATE dbo.UnidadesMedidaSIFEN
    SET Descrip_Unidad = LEFT(LTRIM(RTRIM(CAST(Descripcion AS NVARCHAR(150)))), 30),
        Codigo_Repr = LEFT(LTRIM(RTRIM(CAST(Descripcion AS NVARCHAR(150)))), 15);

    UPDATE dbo.UnidadesMedidaSIFEN
    SET Codigo_Repr = RIGHT(N'00000' + CAST(Codigo AS VARCHAR(5)), 5)
    WHERE LTRIM(RTRIM(Codigo_Repr)) = N'';

    ALTER TABLE dbo.UnidadesMedidaSIFEN ALTER COLUMN Descrip_Unidad VARCHAR(30) NOT NULL;
    ALTER TABLE dbo.UnidadesMedidaSIFEN ALTER COLUMN Codigo_Repr VARCHAR(15) NOT NULL;

    ALTER TABLE dbo.UnidadesMedidaSIFEN DROP CONSTRAINT CK_UnidadesMedidaSIFEN_Descripcion_NoVacia;
    ALTER TABLE dbo.UnidadesMedidaSIFEN DROP COLUMN Descripcion;

    ALTER TABLE dbo.UnidadesMedidaSIFEN ADD CONSTRAINT CK_UnidadesMedidaSIFEN_Descrip_NoVacia CHECK (LEN(LTRIM(RTRIM(Descrip_Unidad))) > 0);
    ALTER TABLE dbo.UnidadesMedidaSIFEN ADD CONSTRAINT CK_UnidadesMedidaSIFEN_CodigoRepr_NoVacio CHECK (LEN(LTRIM(RTRIM(Codigo_Repr))) > 0);
END;
GO

IF OBJECT_ID(N'dbo.UnidadesMedidaSIFEN', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.UnidadesMedidaSIFEN
    (
        Codigo          SMALLINT      NOT NULL,
        Descrip_Unidad  VARCHAR(30)   NOT NULL,
        Codigo_Repr     VARCHAR(15)   NOT NULL,
        CONSTRAINT PK_UnidadesMedidaSIFEN PRIMARY KEY CLUSTERED (Codigo),
        CONSTRAINT CK_UnidadesMedidaSIFEN_Descrip_NoVacia CHECK (LEN(LTRIM(RTRIM(Descrip_Unidad))) > 0),
        CONSTRAINT CK_UnidadesMedidaSIFEN_CodigoRepr_NoVacio CHECK (LEN(LTRIM(RTRIM(Codigo_Repr))) > 0)
    );
END;
GO

/* Datos mínimos de ejemplo; reemplazar/ampliar según normativa vigente */
IF NOT EXISTS (SELECT 1 FROM dbo.UnidadesMedidaSIFEN WHERE Codigo = 1)
    INSERT INTO dbo.UnidadesMedidaSIFEN (Codigo, Descrip_Unidad, Codigo_Repr) VALUES (1, 'Unidad', 'UNI');
IF NOT EXISTS (SELECT 1 FROM dbo.UnidadesMedidaSIFEN WHERE Codigo = 2)
    INSERT INTO dbo.UnidadesMedidaSIFEN (Codigo, Descrip_Unidad, Codigo_Repr) VALUES (2, 'Kilogramo', 'kg');
IF NOT EXISTS (SELECT 1 FROM dbo.UnidadesMedidaSIFEN WHERE Codigo = 3)
    INSERT INTO dbo.UnidadesMedidaSIFEN (Codigo, Descrip_Unidad, Codigo_Repr) VALUES (3, 'Litro', 'Lt');
GO

IF OBJECT_ID(N'dbo.UnidadesMedida', N'U') IS NOT NULL
   AND COL_LENGTH('dbo.UnidadesMedida', 'Codigo_SIFEN') IS NULL
BEGIN
    ALTER TABLE dbo.UnidadesMedida ADD Codigo_SIFEN SMALLINT NULL;
END;
GO

IF OBJECT_ID(N'dbo.UnidadesMedida', N'U') IS NOT NULL
   AND COL_LENGTH('dbo.UnidadesMedida', 'Codigo_SIFEN') IS NOT NULL
   AND NOT EXISTS (
        SELECT 1 FROM sys.foreign_keys
        WHERE parent_object_id = OBJECT_ID(N'dbo.UnidadesMedida')
          AND name = N'FK_UnidadesMedida_UnidadesMedidaSIFEN')
BEGIN
    ALTER TABLE dbo.UnidadesMedida
        ADD CONSTRAINT FK_UnidadesMedida_UnidadesMedidaSIFEN
        FOREIGN KEY (Codigo_SIFEN) REFERENCES dbo.UnidadesMedidaSIFEN (Codigo);
END;
GO

PRINT N'UnidadesMedidaSIFEN y Codigo_SIFEN aplicados (o ya existían).';
GO
