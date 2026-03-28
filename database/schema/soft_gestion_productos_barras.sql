/*
  Soft_Gestion — Códigos de barras múltiples por producto (Productos_Barras)
  SQL Server

  Ejecutar después de tener dbo.Productos creada.
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;

IF OBJECT_ID(N'dbo.Productos_Barras', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Productos_Barras
    (
        ProductoBarraId     INT            IDENTITY(1,1) NOT NULL,
        ProductoId          INT            NOT NULL,
        CodBarras           NVARCHAR(14)   NOT NULL,
        FechaAlta           DATETIME2(0)   NOT NULL CONSTRAINT DF_Productos_Barras_FechaAlta DEFAULT (SYSDATETIME()),
        CONSTRAINT PK_Productos_Barras PRIMARY KEY CLUSTERED (ProductoBarraId),
        CONSTRAINT FK_Productos_Barras_Productos FOREIGN KEY (ProductoId)
            REFERENCES dbo.Productos (ProductoId) ON DELETE CASCADE,
        CONSTRAINT UQ_Productos_Barras_CodBarras UNIQUE (CodBarras),
        CONSTRAINT CK_Productos_Barras_CodBarras_NoVacio CHECK (LEN(LTRIM(RTRIM(CodBarras))) > 0)
    );

    CREATE NONCLUSTERED INDEX IX_Productos_Barras_ProductoId
        ON dbo.Productos_Barras (ProductoId);
END;
GO

/* Migración: copiar CodigoBarras legado a la primera fila de Productos_Barras */
IF OBJECT_ID(N'dbo.Productos_Barras', N'U') IS NOT NULL
   AND OBJECT_ID(N'dbo.Productos', N'U') IS NOT NULL
BEGIN
    INSERT INTO dbo.Productos_Barras (ProductoId, CodBarras, FechaAlta)
    SELECT
        p.ProductoId,
        LTRIM(RTRIM(p.CodigoBarras)),
        p.FechaCreacion
    FROM dbo.Productos p
    WHERE p.CodigoBarras IS NOT NULL
      AND LEN(LTRIM(RTRIM(p.CodigoBarras))) > 0
      AND NOT EXISTS (
          SELECT 1
          FROM dbo.Productos_Barras x
          WHERE x.ProductoId = p.ProductoId
            AND x.CodBarras = LTRIM(RTRIM(p.CodigoBarras))
      );
END;
GO

/* Sincronizar columna legado con el primer código (orden por alta e id) */
IF OBJECT_ID(N'dbo.Productos_Barras', N'U') IS NOT NULL
BEGIN
    UPDATE p
    SET p.CodigoBarras = (
        SELECT TOP (1) pb.CodBarras
        FROM dbo.Productos_Barras pb
        WHERE pb.ProductoId = p.ProductoId
        ORDER BY pb.FechaAlta ASC, pb.ProductoBarraId ASC
    )
    FROM dbo.Productos p
    WHERE EXISTS (SELECT 1 FROM dbo.Productos_Barras x WHERE x.ProductoId = p.ProductoId);

    UPDATE p
    SET p.CodigoBarras = NULL
    FROM dbo.Productos p
    WHERE NOT EXISTS (SELECT 1 FROM dbo.Productos_Barras pb WHERE pb.ProductoId = p.ProductoId)
      AND p.CodigoBarras IS NOT NULL;
END;
GO
