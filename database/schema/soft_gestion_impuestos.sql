/*
  Soft_Gestion — Maestro de impuestos (Paraguay / facturación / SIFEN)
  SQL Server

  Ejecutar en base existente (tras dbo.Productos).
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;

IF OBJECT_ID(N'dbo.Impuestos', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Impuestos
    (
        ImpuestoId          INT            IDENTITY(1,1) NOT NULL,
        Codigo              INT            NOT NULL,
        Nombre              NVARCHAR(100)  NOT NULL,
        TipoImpuesto        NVARCHAR(20)   NOT NULL,
        Porcentaje          DECIMAL(5,2)   NOT NULL,
        EsExento            BIT            NOT NULL CONSTRAINT DF_Impuestos_EsExento DEFAULT (0),
        CodigoSIFEN         SMALLINT       NULL,
        EsActivo            BIT            NOT NULL CONSTRAINT DF_Impuestos_EsActivo DEFAULT (1),
        UsuarioCreacion     NVARCHAR(50)   NOT NULL,
        FechaCreacion       DATETIME2(0)   NOT NULL CONSTRAINT DF_Impuestos_FechaCreacion DEFAULT (SYSDATETIME()),
        UsuarioModificacion NVARCHAR(50)   NULL,
        FechaModificacion   DATETIME2(0)   NULL,
        CONSTRAINT PK_Impuestos PRIMARY KEY CLUSTERED (ImpuestoId),
        CONSTRAINT UQ_Impuestos_Codigo UNIQUE (Codigo),
        CONSTRAINT CK_Impuestos_Codigo_Positivo CHECK (Codigo > 0),
        CONSTRAINT CK_Impuestos_Nombre_NoVacio CHECK (LEN(LTRIM(RTRIM(Nombre))) > 0),
        CONSTRAINT CK_Impuestos_TipoImpuesto_NoVacio CHECK (LEN(LTRIM(RTRIM(TipoImpuesto))) > 0),
        CONSTRAINT CK_Impuestos_Porcentaje_NoNegativo CHECK (Porcentaje >= 0),
        CONSTRAINT CK_Impuestos_Exento_Porcentaje CHECK (
            (EsExento = 0)
            OR
            (EsExento = 1 AND Porcentaje = 0)
        )
    );

    CREATE NONCLUSTERED INDEX IX_Impuestos_Nombre
        ON dbo.Impuestos (Nombre);

    CREATE NONCLUSTERED INDEX IX_Impuestos_EsActivo
        ON dbo.Impuestos (EsActivo);
END;
GO

/* Datos iniciales */
IF OBJECT_ID(N'dbo.Impuestos', N'U') IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM dbo.Impuestos WHERE Codigo = 1)
        INSERT INTO dbo.Impuestos (Codigo, Nombre, TipoImpuesto, Porcentaje, EsExento, CodigoSIFEN, EsActivo, UsuarioCreacion)
        VALUES (1, N'IVA 10%', N'IVA', 10.00, 0, NULL, 1, N'system');

    IF NOT EXISTS (SELECT 1 FROM dbo.Impuestos WHERE Codigo = 2)
        INSERT INTO dbo.Impuestos (Codigo, Nombre, TipoImpuesto, Porcentaje, EsExento, CodigoSIFEN, EsActivo, UsuarioCreacion)
        VALUES (2, N'IVA 5%', N'IVA', 5.00, 0, NULL, 1, N'system');

    IF NOT EXISTS (SELECT 1 FROM dbo.Impuestos WHERE Codigo = 3)
        INSERT INTO dbo.Impuestos (Codigo, Nombre, TipoImpuesto, Porcentaje, EsExento, CodigoSIFEN, EsActivo, UsuarioCreacion)
        VALUES (3, N'EXENTO', N'IVA', 0.00, 1, NULL, 1, N'system');
END;
GO

/* Productos: columna e índice */
IF COL_LENGTH(N'dbo.Productos', N'ImpuestoId') IS NULL
   AND OBJECT_ID(N'dbo.Productos', N'U') IS NOT NULL
   AND OBJECT_ID(N'dbo.Impuestos', N'U') IS NOT NULL
BEGIN
    ALTER TABLE dbo.Productos ADD ImpuestoId INT NULL;

    DECLARE @IdIva10 INT;
    SELECT @IdIva10 = ImpuestoId FROM dbo.Impuestos WHERE Codigo = 1;

    IF @IdIva10 IS NOT NULL
        UPDATE dbo.Productos SET ImpuestoId = @IdIva10 WHERE ImpuestoId IS NULL;

    ALTER TABLE dbo.Productos ALTER COLUMN ImpuestoId INT NOT NULL;

    ALTER TABLE dbo.Productos
        ADD CONSTRAINT FK_Productos_Impuestos FOREIGN KEY (ImpuestoId)
            REFERENCES dbo.Impuestos (ImpuestoId);

    CREATE NONCLUSTERED INDEX IX_Productos_ImpuestoId
        ON dbo.Productos (ImpuestoId);
END;
GO

/* Permiso de menú (idempotente) */
IF OBJECT_ID(N'dbo.Permisos', N'U') IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM dbo.Permisos WHERE Codigo = N'Maestros.Impuestos')
BEGIN
    INSERT INTO dbo.Permisos (Codigo, Nombre, Modulo, Formulario, Activo)
    VALUES (N'Maestros.Impuestos', N'Impuestos', N'Maestros', N'FrmImpuestos', 1);
END;
GO
