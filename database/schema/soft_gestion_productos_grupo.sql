/*
    Soft_Gestion — Productos: CategoriaProductoId → GrupoId (clasificación 3 niveles).
    Ejecutar tras existir dbo.Grupos. Idempotente en lo posible.
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

IF OBJECT_ID(N'dbo.Productos', N'U') IS NULL
BEGIN
    RAISERROR(N'dbo.Productos no existe.', 16, 1);
    RETURN;
END;
GO

IF COL_LENGTH('dbo.Productos', 'GrupoId') IS NULL
BEGIN
    ALTER TABLE dbo.Productos ADD GrupoId INT NULL;
END;
GO

IF EXISTS (
    SELECT 1 FROM sys.foreign_keys
    WHERE parent_object_id = OBJECT_ID(N'dbo.Productos')
      AND name = N'FK_Productos_CategoriasProducto')
    ALTER TABLE dbo.Productos DROP CONSTRAINT FK_Productos_CategoriasProducto;
GO

IF COL_LENGTH('dbo.Productos', 'CategoriaProductoId') IS NOT NULL
BEGIN
    IF EXISTS (
        SELECT 1 FROM sys.indexes
        WHERE object_id = OBJECT_ID(N'dbo.Productos')
          AND name = N'IX_Productos_CategoriaProductoId')
        DROP INDEX IX_Productos_CategoriaProductoId ON dbo.Productos;

    ALTER TABLE dbo.Productos DROP COLUMN CategoriaProductoId;
END;
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.foreign_keys
    WHERE parent_object_id = OBJECT_ID(N'dbo.Productos')
      AND name = N'FK_Productos_Grupos')
   AND OBJECT_ID(N'dbo.Grupos', N'U') IS NOT NULL
    ALTER TABLE dbo.Productos
        ADD CONSTRAINT FK_Productos_Grupos FOREIGN KEY (GrupoId) REFERENCES dbo.Grupos (GrupoId);
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.Productos')
      AND name = N'IX_Productos_GrupoId')
    CREATE NONCLUSTERED INDEX IX_Productos_GrupoId ON dbo.Productos (GrupoId);
GO

PRINT N'Productos.GrupoId aplicado (migración desde CategoriaProductoId si correspondía).';
GO
