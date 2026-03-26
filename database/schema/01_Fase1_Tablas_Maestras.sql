/*
  Soft_Gestion — Fase 1: script revisado y mejorado
  SQL Server
  Alcance:
  - Seguridad
  - Organización
  - Configuración comercial
  - Terceros
  - Productos

  Mejoras aplicadas sobre la versión inicial:
  - DATETIME2(0) para auditoría
  - DEFAULTs en campos clave
  - CHECK constraints básicos
  - Índices faltantes en claves foráneas y búsquedas frecuentes
  - Validaciones mínimas de coherencia de negocio
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;

-- USE [SoftGestion];
-- GO

/* =============================================================================
   1. Empresas
   ============================================================================= */
IF OBJECT_ID(N'dbo.Empresas', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Empresas
    (
        EmpresaId           INT            IDENTITY(1,1) NOT NULL,
        Codigo              NVARCHAR(20)   NOT NULL,
        RazonSocial         NVARCHAR(150)  NOT NULL,
        NombreFantasia      NVARCHAR(150)  NULL,
        RUC                 NVARCHAR(30)   NULL,
        Direccion           NVARCHAR(200)  NULL,
        Telefono            NVARCHAR(50)   NULL,
        Email               NVARCHAR(150)  NULL,
        Activo              BIT            NOT NULL CONSTRAINT DF_Empresas_Activo DEFAULT (1),
        FechaCreacion       DATETIME2(0)   NOT NULL CONSTRAINT DF_Empresas_FechaCreacion DEFAULT (SYSDATETIME()),
        UsuarioCreacion     NVARCHAR(50)   NOT NULL,
        FechaModificacion   DATETIME2(0)   NULL,
        UsuarioModificacion NVARCHAR(50)   NULL,
        CONSTRAINT PK_Empresas PRIMARY KEY CLUSTERED (EmpresaId),
        CONSTRAINT UQ_Empresas_Codigo UNIQUE (Codigo),
        CONSTRAINT CK_Empresas_Codigo_NoVacio CHECK (LEN(LTRIM(RTRIM(Codigo))) > 0),
        CONSTRAINT CK_Empresas_RazonSocial_NoVacio CHECK (LEN(LTRIM(RTRIM(RazonSocial))) > 0)
    );

    CREATE NONCLUSTERED INDEX IX_Empresas_RazonSocial
        ON dbo.Empresas (RazonSocial);
END;
GO

/* =============================================================================
   2. Sucursales
   ============================================================================= */
IF OBJECT_ID(N'dbo.Sucursales', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Sucursales
    (
        SucursalId          INT            IDENTITY(1,1) NOT NULL,
        EmpresaId           INT            NOT NULL,
        Codigo              NVARCHAR(20)   NOT NULL,
        Nombre              NVARCHAR(100)  NOT NULL,
        Direccion           NVARCHAR(200)  NULL,
        Telefono            NVARCHAR(50)   NULL,
        Responsable         NVARCHAR(100)  NULL,
        Activo              BIT            NOT NULL CONSTRAINT DF_Sucursales_Activo DEFAULT (1),
        FechaCreacion       DATETIME2(0)   NOT NULL CONSTRAINT DF_Sucursales_FechaCreacion DEFAULT (SYSDATETIME()),
        UsuarioCreacion     NVARCHAR(50)   NOT NULL,
        FechaModificacion   DATETIME2(0)   NULL,
        UsuarioModificacion NVARCHAR(50)   NULL,
        CONSTRAINT PK_Sucursales PRIMARY KEY CLUSTERED (SucursalId),
        CONSTRAINT FK_Sucursales_Empresas FOREIGN KEY (EmpresaId)
            REFERENCES dbo.Empresas (EmpresaId),
        CONSTRAINT UQ_Sucursales_EmpresaId_Codigo UNIQUE (EmpresaId, Codigo),
        CONSTRAINT CK_Sucursales_Codigo_NoVacio CHECK (LEN(LTRIM(RTRIM(Codigo))) > 0),
        CONSTRAINT CK_Sucursales_Nombre_NoVacio CHECK (LEN(LTRIM(RTRIM(Nombre))) > 0)
    );

    CREATE NONCLUSTERED INDEX IX_Sucursales_EmpresaId
        ON dbo.Sucursales (EmpresaId);
END;
GO

/* =============================================================================
   3. Depositos
   ============================================================================= */
IF OBJECT_ID(N'dbo.Depositos', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Depositos
    (
        DepositoId          INT            IDENTITY(1,1) NOT NULL,
        SucursalId          INT            NOT NULL,
        Codigo              NVARCHAR(20)   NOT NULL,
        Nombre              NVARCHAR(100)  NOT NULL,
        Descripcion         NVARCHAR(255)  NULL,
        EsPrincipal         BIT            NOT NULL CONSTRAINT DF_Depositos_EsPrincipal DEFAULT (0),
        Activo              BIT            NOT NULL CONSTRAINT DF_Depositos_Activo DEFAULT (1),
        FechaCreacion       DATETIME2(0)   NOT NULL CONSTRAINT DF_Depositos_FechaCreacion DEFAULT (SYSDATETIME()),
        UsuarioCreacion     NVARCHAR(50)   NOT NULL,
        FechaModificacion   DATETIME2(0)   NULL,
        UsuarioModificacion NVARCHAR(50)   NULL,
        CONSTRAINT PK_Depositos PRIMARY KEY CLUSTERED (DepositoId),
        CONSTRAINT FK_Depositos_Sucursales FOREIGN KEY (SucursalId)
            REFERENCES dbo.Sucursales (SucursalId),
        CONSTRAINT UQ_Depositos_SucursalId_Codigo UNIQUE (SucursalId, Codigo),
        CONSTRAINT CK_Depositos_Codigo_NoVacio CHECK (LEN(LTRIM(RTRIM(Codigo))) > 0),
        CONSTRAINT CK_Depositos_Nombre_NoVacio CHECK (LEN(LTRIM(RTRIM(Nombre))) > 0)
    );

    CREATE NONCLUSTERED INDEX IX_Depositos_SucursalId
        ON dbo.Depositos (SucursalId);
END;
GO

/* =============================================================================
   4. CondicionesPago
   ============================================================================= */
IF OBJECT_ID(N'dbo.CondicionesPago', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.CondicionesPago
    (
        CondicionPagoId INT            IDENTITY(1,1) NOT NULL,
        Codigo          NVARCHAR(20)   NOT NULL,
        Nombre          NVARCHAR(100)  NOT NULL,
        DiasPlazo       INT            NOT NULL CONSTRAINT DF_CondicionesPago_DiasPlazo DEFAULT (0),
        EsContado       BIT            NOT NULL CONSTRAINT DF_CondicionesPago_EsContado DEFAULT (0),
        Activo          BIT            NOT NULL CONSTRAINT DF_CondicionesPago_Activo DEFAULT (1),
        CONSTRAINT PK_CondicionesPago PRIMARY KEY CLUSTERED (CondicionPagoId),
        CONSTRAINT UQ_CondicionesPago_Codigo UNIQUE (Codigo),
        CONSTRAINT CK_CondicionesPago_Codigo_NoVacio CHECK (LEN(LTRIM(RTRIM(Codigo))) > 0),
        CONSTRAINT CK_CondicionesPago_Nombre_NoVacio CHECK (LEN(LTRIM(RTRIM(Nombre))) > 0),
        CONSTRAINT CK_CondicionesPago_DiasPlazo_NoNegativo CHECK (DiasPlazo >= 0)
    );

    CREATE NONCLUSTERED INDEX IX_CondicionesPago_Nombre
        ON dbo.CondicionesPago (Nombre);
END;
GO

/* =============================================================================
   5. Monedas
   ============================================================================= */
IF OBJECT_ID(N'dbo.Monedas', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Monedas
    (
        MonedaId           INT           IDENTITY(1,1) NOT NULL,
        Codigo             NVARCHAR(10)  NOT NULL,
        Nombre             NVARCHAR(50)  NOT NULL,
        Simbolo            NVARCHAR(10)  NULL,
        EsMonedaBase       BIT           NOT NULL CONSTRAINT DF_Monedas_EsMonedaBase DEFAULT (0),
        CantidadDecimales  INT           NOT NULL CONSTRAINT DF_Monedas_CantidadDecimales DEFAULT (2),
        Activo             BIT           NOT NULL CONSTRAINT DF_Monedas_Activo DEFAULT (1),
        CONSTRAINT PK_Monedas PRIMARY KEY CLUSTERED (MonedaId),
        CONSTRAINT UQ_Monedas_Codigo UNIQUE (Codigo),
        CONSTRAINT CK_Monedas_Codigo_NoVacio CHECK (LEN(LTRIM(RTRIM(Codigo))) > 0),
        CONSTRAINT CK_Monedas_Nombre_NoVacio CHECK (LEN(LTRIM(RTRIM(Nombre))) > 0),
        CONSTRAINT CK_Monedas_CantidadDecimales CHECK (CantidadDecimales BETWEEN 0 AND 6)
    );

    CREATE NONCLUSTERED INDEX IX_Monedas_Nombre
        ON dbo.Monedas (Nombre);
END;
GO

/* =============================================================================
   6. Cotizaciones
   ============================================================================= */
IF OBJECT_ID(N'dbo.Cotizaciones', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Cotizaciones
    (
        CotizacionId      INT            IDENTITY(1,1) NOT NULL,
        MonedaId          INT            NOT NULL,
        Fecha             DATE           NOT NULL,
        Compra            DECIMAL(18,6)  NOT NULL,
        Venta             DECIMAL(18,6)  NOT NULL,
        Activo            BIT            NOT NULL CONSTRAINT DF_Cotizaciones_Activo DEFAULT (1),
        FechaCreacion     DATETIME2(0)   NOT NULL CONSTRAINT DF_Cotizaciones_FechaCreacion DEFAULT (SYSDATETIME()),
        UsuarioCreacion   NVARCHAR(50)   NOT NULL,
        CONSTRAINT PK_Cotizaciones PRIMARY KEY CLUSTERED (CotizacionId),
        CONSTRAINT FK_Cotizaciones_Monedas FOREIGN KEY (MonedaId)
            REFERENCES dbo.Monedas (MonedaId),
        CONSTRAINT UQ_Cotizaciones_MonedaId_Fecha UNIQUE (MonedaId, Fecha),
        CONSTRAINT CK_Cotizaciones_Compra_Positiva CHECK (Compra > 0),
        CONSTRAINT CK_Cotizaciones_Venta_Positiva CHECK (Venta > 0)
    );

    CREATE NONCLUSTERED INDEX IX_Cotizaciones_MonedaId
        ON dbo.Cotizaciones (MonedaId);

    CREATE NONCLUSTERED INDEX IX_Cotizaciones_Fecha
        ON dbo.Cotizaciones (Fecha);
END;
GO

/* =============================================================================
   7. ListasPrecios
   ============================================================================= */
IF OBJECT_ID(N'dbo.ListasPrecios', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ListasPrecios
    (
        ListaPrecioId       INT            IDENTITY(1,1) NOT NULL,
        Codigo              NVARCHAR(20)   NOT NULL,
        Nombre              NVARCHAR(100)  NOT NULL,
        MonedaId            INT            NOT NULL,
        Activo              BIT            NOT NULL CONSTRAINT DF_ListasPrecios_Activo DEFAULT (1),
        FechaCreacion       DATETIME2(0)   NOT NULL CONSTRAINT DF_ListasPrecios_FechaCreacion DEFAULT (SYSDATETIME()),
        UsuarioCreacion     NVARCHAR(50)   NOT NULL,
        FechaModificacion   DATETIME2(0)   NULL,
        UsuarioModificacion NVARCHAR(50)   NULL,
        CONSTRAINT PK_ListasPrecios PRIMARY KEY CLUSTERED (ListaPrecioId),
        CONSTRAINT FK_ListasPrecios_Monedas FOREIGN KEY (MonedaId)
            REFERENCES dbo.Monedas (MonedaId),
        CONSTRAINT UQ_ListasPrecios_Codigo UNIQUE (Codigo),
        CONSTRAINT CK_ListasPrecios_Codigo_NoVacio CHECK (LEN(LTRIM(RTRIM(Codigo))) > 0),
        CONSTRAINT CK_ListasPrecios_Nombre_NoVacio CHECK (LEN(LTRIM(RTRIM(Nombre))) > 0)
    );

    CREATE NONCLUSTERED INDEX IX_ListasPrecios_MonedaId
        ON dbo.ListasPrecios (MonedaId);
END;
GO

/* =============================================================================
   8. CategoriasProducto
   ============================================================================= */
IF OBJECT_ID(N'dbo.CategoriasProducto', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.CategoriasProducto
    (
        CategoriaProductoId INT            IDENTITY(1,1) NOT NULL,
        Codigo              NVARCHAR(20)   NOT NULL,
        Nombre              NVARCHAR(100)  NOT NULL,
        Descripcion         NVARCHAR(255)  NULL,
        Activo              BIT            NOT NULL CONSTRAINT DF_CategoriasProducto_Activo DEFAULT (1),
        CONSTRAINT PK_CategoriasProducto PRIMARY KEY CLUSTERED (CategoriaProductoId),
        CONSTRAINT UQ_CategoriasProducto_Codigo UNIQUE (Codigo),
        CONSTRAINT UQ_CategoriasProducto_Nombre UNIQUE (Nombre),
        CONSTRAINT CK_CategoriasProducto_Codigo_NoVacio CHECK (LEN(LTRIM(RTRIM(Codigo))) > 0),
        CONSTRAINT CK_CategoriasProducto_Nombre_NoVacio CHECK (LEN(LTRIM(RTRIM(Nombre))) > 0)
    );
END;
GO

/* =============================================================================
   9. Marcas
   ============================================================================= */
IF OBJECT_ID(N'dbo.Marcas', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Marcas
    (
        MarcaId INT            IDENTITY(1,1) NOT NULL,
        Codigo  NVARCHAR(20)   NOT NULL,
        Nombre  NVARCHAR(100)  NOT NULL,
        Activo  BIT            NOT NULL CONSTRAINT DF_Marcas_Activo DEFAULT (1),
        FechaCreacion        DATETIME2(0)   NOT NULL CONSTRAINT DF_Marcas_FechaCreacion DEFAULT (SYSDATETIME()),
        UsuarioCreacion      NVARCHAR(50)   NOT NULL,
        FechaModificacion    DATETIME2(0)   NULL,
        UsuarioModificacion  NVARCHAR(50)   NULL,
        CONSTRAINT PK_Marcas PRIMARY KEY CLUSTERED (MarcaId),
        CONSTRAINT UQ_Marcas_Codigo UNIQUE (Codigo),
        CONSTRAINT UQ_Marcas_Nombre UNIQUE (Nombre),
        CONSTRAINT CK_Marcas_Codigo_NoVacio CHECK (LEN(LTRIM(RTRIM(Codigo))) > 0),
        CONSTRAINT CK_Marcas_Nombre_NoVacio CHECK (LEN(LTRIM(RTRIM(Nombre))) > 0)
    );
END;
GO

/* =============================================================================
   10. UnidadesMedidaSIFEN (catálogo de referencia para facturación electrónica)
   ============================================================================= */
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

    /* UnidadesMedida se crea más abajo; solo recrear FK si la tabla ya existe (BD previa a este script). */
    IF OBJECT_ID(N'dbo.UnidadesMedida', N'U') IS NOT NULL
       AND COL_LENGTH('dbo.UnidadesMedida', 'Codigo_SIFEN') IS NOT NULL
       AND NOT EXISTS (
            SELECT 1 FROM sys.foreign_keys
            WHERE parent_object_id = OBJECT_ID(N'dbo.UnidadesMedida')
              AND name = N'FK_UnidadesMedida_UnidadesMedidaSIFEN')
        ALTER TABLE dbo.UnidadesMedida
            ADD CONSTRAINT FK_UnidadesMedida_UnidadesMedidaSIFEN FOREIGN KEY (Codigo_SIFEN)
                REFERENCES dbo.UnidadesMedidaSIFEN (Codigo);
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.UnidadesMedidaSIFEN WHERE Codigo = 1)
    INSERT INTO dbo.UnidadesMedidaSIFEN (Codigo, Descrip_Unidad, Codigo_Repr) VALUES (1, 'Unidad', 'UNI');
IF NOT EXISTS (SELECT 1 FROM dbo.UnidadesMedidaSIFEN WHERE Codigo = 2)
    INSERT INTO dbo.UnidadesMedidaSIFEN (Codigo, Descrip_Unidad, Codigo_Repr) VALUES (2, 'Kilogramo', 'kg');
IF NOT EXISTS (SELECT 1 FROM dbo.UnidadesMedidaSIFEN WHERE Codigo = 3)
    INSERT INTO dbo.UnidadesMedidaSIFEN (Codigo, Descrip_Unidad, Codigo_Repr) VALUES (3, 'Litro', 'Lt');
GO

/* =============================================================================
   11. UnidadesMedida
   ============================================================================= */
IF OBJECT_ID(N'dbo.UnidadesMedida', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.UnidadesMedida
    (
        UnidadMedidaId INT            IDENTITY(1,1) NOT NULL,
        Codigo         NVARCHAR(20)   NOT NULL,
        Nombre         NVARCHAR(100)  NOT NULL,
        Abreviatura    NVARCHAR(10)   NOT NULL,
        Activo         BIT            NOT NULL CONSTRAINT DF_UnidadesMedida_Activo DEFAULT (1),
        Codigo_SIFEN   SMALLINT       NULL,
        CONSTRAINT PK_UnidadesMedida PRIMARY KEY CLUSTERED (UnidadMedidaId),
        CONSTRAINT UQ_UnidadesMedida_Codigo UNIQUE (Codigo),
        CONSTRAINT UQ_UnidadesMedida_Abreviatura UNIQUE (Abreviatura),
        CONSTRAINT CK_UnidadesMedida_Codigo_NoVacio CHECK (LEN(LTRIM(RTRIM(Codigo))) > 0),
        CONSTRAINT CK_UnidadesMedida_Nombre_NoVacio CHECK (LEN(LTRIM(RTRIM(Nombre))) > 0),
        CONSTRAINT CK_UnidadesMedida_Abreviatura_NoVacia CHECK (LEN(LTRIM(RTRIM(Abreviatura))) > 0),
        CONSTRAINT FK_UnidadesMedida_UnidadesMedidaSIFEN FOREIGN KEY (Codigo_SIFEN)
            REFERENCES dbo.UnidadesMedidaSIFEN (Codigo)
    );
END;
GO

/* =============================================================================
   12. Productos
   ============================================================================= */
IF OBJECT_ID(N'dbo.Productos', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Productos
    (
        ProductoId           INT            IDENTITY(1,1) NOT NULL,
        Codigo               NVARCHAR(30)   NOT NULL,
        CodigoBarras         NVARCHAR(50)   NULL,
        Descripcion          NVARCHAR(200)  NOT NULL,
        GrupoId              INT            NULL,
        MarcaId              INT            NULL,
        UnidadMedidaId       INT            NOT NULL,
        CostoUltimo          DECIMAL(18,4)  NOT NULL CONSTRAINT DF_Productos_CostoUltimo DEFAULT (0),
        PrecioBase           DECIMAL(18,4)  NOT NULL CONSTRAINT DF_Productos_PrecioBase DEFAULT (0),
        PorcentajeIVA        DECIMAL(5,2)   NOT NULL CONSTRAINT DF_Productos_PorcentajeIVA DEFAULT (0),
        PermiteStockNegativo BIT            NOT NULL CONSTRAINT DF_Productos_PermiteStockNegativo DEFAULT (0),
        ControlaStock        BIT            NOT NULL CONSTRAINT DF_Productos_ControlaStock DEFAULT (1),
        EsServicio           BIT            NOT NULL CONSTRAINT DF_Productos_EsServicio DEFAULT (0),
        Observaciones        NVARCHAR(255)  NULL,
        Activo               BIT            NOT NULL CONSTRAINT DF_Productos_Activo DEFAULT (1),
        FechaCreacion        DATETIME2(0)   NOT NULL CONSTRAINT DF_Productos_FechaCreacion DEFAULT (SYSDATETIME()),
        UsuarioCreacion      NVARCHAR(50)   NOT NULL,
        FechaModificacion    DATETIME2(0)   NULL,
        UsuarioModificacion  NVARCHAR(50)   NULL,
        CONSTRAINT PK_Productos PRIMARY KEY CLUSTERED (ProductoId),
        CONSTRAINT UQ_Productos_Codigo UNIQUE (Codigo),
        CONSTRAINT FK_Productos_Grupos FOREIGN KEY (GrupoId)
            REFERENCES dbo.Grupos (GrupoId),
        CONSTRAINT FK_Productos_Marcas FOREIGN KEY (MarcaId)
            REFERENCES dbo.Marcas (MarcaId),
        CONSTRAINT FK_Productos_UnidadesMedida FOREIGN KEY (UnidadMedidaId)
            REFERENCES dbo.UnidadesMedida (UnidadMedidaId),
        CONSTRAINT CK_Productos_Codigo_NoVacio CHECK (LEN(LTRIM(RTRIM(Codigo))) > 0),
        CONSTRAINT CK_Productos_Descripcion_NoVacia CHECK (LEN(LTRIM(RTRIM(Descripcion))) > 0),
        CONSTRAINT CK_Productos_CostoUltimo_NoNegativo CHECK (CostoUltimo >= 0),
        CONSTRAINT CK_Productos_PrecioBase_NoNegativo CHECK (PrecioBase >= 0),
        CONSTRAINT CK_Productos_PorcentajeIVA_Valido CHECK (PorcentajeIVA >= 0 AND PorcentajeIVA <= 100),
        CONSTRAINT CK_Productos_Servicio_NoStock CHECK (
            (EsServicio = 1 AND ControlaStock = 0)
            OR
            (EsServicio = 0)
        )
    );

    CREATE NONCLUSTERED INDEX IX_Productos_Descripcion
        ON dbo.Productos (Descripcion);

    CREATE NONCLUSTERED INDEX IX_Productos_CodigoBarras
        ON dbo.Productos (CodigoBarras);

    CREATE NONCLUSTERED INDEX IX_Productos_Activo
        ON dbo.Productos (Activo);

    CREATE NONCLUSTERED INDEX IX_Productos_GrupoId
        ON dbo.Productos (GrupoId);

    CREATE NONCLUSTERED INDEX IX_Productos_MarcaId
        ON dbo.Productos (MarcaId);

    CREATE NONCLUSTERED INDEX IX_Productos_UnidadMedidaId
        ON dbo.Productos (UnidadMedidaId);
END;
GO

/* =============================================================================
   13. ListaPrecioDetalles
   Nota:
   - En Fase 1 se mantiene una sola fila por Lista + Producto.
   - No modela historial múltiple por vigencias superpuestas.
   ============================================================================= */
IF OBJECT_ID(N'dbo.ListaPrecioDetalles', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ListaPrecioDetalles
    (
        ListaPrecioDetalleId INT            IDENTITY(1,1) NOT NULL,
        ListaPrecioId        INT            NOT NULL,
        ProductoId           INT            NOT NULL,
        Precio               DECIMAL(18,4)  NOT NULL CONSTRAINT DF_ListaPrecioDetalles_Precio DEFAULT (0),
        FechaVigenciaDesde   DATE           NULL,
        FechaVigenciaHasta   DATE           NULL,
        Activo               BIT            NOT NULL CONSTRAINT DF_ListaPrecioDetalles_Activo DEFAULT (1),
        CONSTRAINT PK_ListaPrecioDetalles PRIMARY KEY CLUSTERED (ListaPrecioDetalleId),
        CONSTRAINT FK_ListaPrecioDetalles_ListasPrecios FOREIGN KEY (ListaPrecioId)
            REFERENCES dbo.ListasPrecios (ListaPrecioId),
        CONSTRAINT FK_ListaPrecioDetalles_Productos FOREIGN KEY (ProductoId)
            REFERENCES dbo.Productos (ProductoId),
        CONSTRAINT UQ_ListaPrecioDetalles_ListaPrecioId_ProductoId UNIQUE (ListaPrecioId, ProductoId),
        CONSTRAINT CK_ListaPrecioDetalles_Precio_NoNegativo CHECK (Precio >= 0),
        CONSTRAINT CK_ListaPrecioDetalles_Vigencia CHECK (
            FechaVigenciaHasta IS NULL
            OR FechaVigenciaDesde IS NULL
            OR FechaVigenciaHasta >= FechaVigenciaDesde
        )
    );

    CREATE NONCLUSTERED INDEX IX_ListaPrecioDetalles_ListaPrecioId
        ON dbo.ListaPrecioDetalles (ListaPrecioId);

    CREATE NONCLUSTERED INDEX IX_ListaPrecioDetalles_ProductoId
        ON dbo.ListaPrecioDetalles (ProductoId);
END;
GO

/* =============================================================================
   14. Clientes
   ============================================================================= */
IF OBJECT_ID(N'dbo.Clientes', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Clientes
    (
        ClienteId           INT            IDENTITY(1,1) NOT NULL,
        Codigo              NVARCHAR(20)   NOT NULL,
        RazonSocial         NVARCHAR(150)  NOT NULL,
        NombreFantasia      NVARCHAR(150)  NULL,
        Documento           NVARCHAR(30)   NULL,
        RUC                 NVARCHAR(30)   NULL,
        Direccion           NVARCHAR(200)  NULL,
        Telefono            NVARCHAR(50)   NULL,
        Email               NVARCHAR(150)  NULL,
        CondicionPagoId     INT            NULL,
        ListaPrecioId       INT            NULL,
        LimiteCredito       DECIMAL(18,2)  NOT NULL CONSTRAINT DF_Clientes_LimiteCredito DEFAULT (0),
        Observaciones       NVARCHAR(255)  NULL,
        Activo              BIT            NOT NULL CONSTRAINT DF_Clientes_Activo DEFAULT (1),
        FechaCreacion       DATETIME2(0)   NOT NULL CONSTRAINT DF_Clientes_FechaCreacion DEFAULT (SYSDATETIME()),
        UsuarioCreacion     NVARCHAR(50)   NOT NULL,
        FechaModificacion   DATETIME2(0)   NULL,
        UsuarioModificacion NVARCHAR(50)   NULL,
        CONSTRAINT PK_Clientes PRIMARY KEY CLUSTERED (ClienteId),
        CONSTRAINT UQ_Clientes_Codigo UNIQUE (Codigo),
        CONSTRAINT FK_Clientes_CondicionesPago FOREIGN KEY (CondicionPagoId)
            REFERENCES dbo.CondicionesPago (CondicionPagoId),
        CONSTRAINT FK_Clientes_ListasPrecios FOREIGN KEY (ListaPrecioId)
            REFERENCES dbo.ListasPrecios (ListaPrecioId),
        CONSTRAINT CK_Clientes_Codigo_NoVacio CHECK (LEN(LTRIM(RTRIM(Codigo))) > 0),
        CONSTRAINT CK_Clientes_RazonSocial_NoVacia CHECK (LEN(LTRIM(RTRIM(RazonSocial))) > 0),
        CONSTRAINT CK_Clientes_LimiteCredito_NoNegativo CHECK (LimiteCredito >= 0)
    );

    CREATE NONCLUSTERED INDEX IX_Clientes_RazonSocial
        ON dbo.Clientes (RazonSocial);

    CREATE NONCLUSTERED INDEX IX_Clientes_Documento
        ON dbo.Clientes (Documento);

    CREATE NONCLUSTERED INDEX IX_Clientes_RUC
        ON dbo.Clientes (RUC);

    CREATE NONCLUSTERED INDEX IX_Clientes_CondicionPagoId
        ON dbo.Clientes (CondicionPagoId);

    CREATE NONCLUSTERED INDEX IX_Clientes_ListaPrecioId
        ON dbo.Clientes (ListaPrecioId);

    CREATE NONCLUSTERED INDEX IX_Clientes_Activo
        ON dbo.Clientes (Activo);
END;
GO

/* =============================================================================
   15. Proveedores
   ============================================================================= */
IF OBJECT_ID(N'dbo.Proveedores', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Proveedores
    (
        ProveedorId         INT            IDENTITY(1,1) NOT NULL,
        Codigo              NVARCHAR(20)   NOT NULL,
        RazonSocial         NVARCHAR(150)  NOT NULL,
        RUC                 NVARCHAR(30)   NULL,
        Direccion           NVARCHAR(200)  NULL,
        Telefono            NVARCHAR(50)   NULL,
        Email               NVARCHAR(150)  NULL,
        CondicionPagoId     INT            NULL,
        Observaciones       NVARCHAR(255)  NULL,
        Activo              BIT            NOT NULL CONSTRAINT DF_Proveedores_Activo DEFAULT (1),
        FechaCreacion       DATETIME2(0)   NOT NULL CONSTRAINT DF_Proveedores_FechaCreacion DEFAULT (SYSDATETIME()),
        UsuarioCreacion     NVARCHAR(50)   NOT NULL,
        FechaModificacion   DATETIME2(0)   NULL,
        UsuarioModificacion NVARCHAR(50)   NULL,
        CONSTRAINT PK_Proveedores PRIMARY KEY CLUSTERED (ProveedorId),
        CONSTRAINT UQ_Proveedores_Codigo UNIQUE (Codigo),
        CONSTRAINT FK_Proveedores_CondicionesPago FOREIGN KEY (CondicionPagoId)
            REFERENCES dbo.CondicionesPago (CondicionPagoId),
        CONSTRAINT CK_Proveedores_Codigo_NoVacio CHECK (LEN(LTRIM(RTRIM(Codigo))) > 0),
        CONSTRAINT CK_Proveedores_RazonSocial_NoVacia CHECK (LEN(LTRIM(RTRIM(RazonSocial))) > 0)
    );

    CREATE NONCLUSTERED INDEX IX_Proveedores_RazonSocial
        ON dbo.Proveedores (RazonSocial);

    CREATE NONCLUSTERED INDEX IX_Proveedores_RUC
        ON dbo.Proveedores (RUC);

    CREATE NONCLUSTERED INDEX IX_Proveedores_CondicionPagoId
        ON dbo.Proveedores (CondicionPagoId);

    CREATE NONCLUSTERED INDEX IX_Proveedores_Activo
        ON dbo.Proveedores (Activo);
END;
GO

/* =============================================================================
   16. Usuarios
   ============================================================================= */
IF OBJECT_ID(N'dbo.Usuarios', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Usuarios
    (
        UsuarioId           INT            IDENTITY(1,1) NOT NULL,
        Login               NVARCHAR(50)   NOT NULL,
        NombreCompleto      NVARCHAR(150)  NOT NULL,
        PasswordHash        NVARCHAR(255)  NOT NULL,
        Email               NVARCHAR(150)  NULL,
        EsAdministrador     BIT            NOT NULL CONSTRAINT DF_Usuarios_EsAdministrador DEFAULT (0),
        SucursalId          INT            NULL,
        Activo              BIT            NOT NULL CONSTRAINT DF_Usuarios_Activo DEFAULT (1),
        FechaCreacion       DATETIME2(0)   NOT NULL CONSTRAINT DF_Usuarios_FechaCreacion DEFAULT (SYSDATETIME()),
        UsuarioCreacion     NVARCHAR(50)   NOT NULL,
        FechaModificacion   DATETIME2(0)   NULL,
        UsuarioModificacion NVARCHAR(50)   NULL,
        CONSTRAINT PK_Usuarios PRIMARY KEY CLUSTERED (UsuarioId),
        CONSTRAINT UQ_Usuarios_Login UNIQUE (Login),
        CONSTRAINT FK_Usuarios_Sucursales FOREIGN KEY (SucursalId)
            REFERENCES dbo.Sucursales (SucursalId),
        CONSTRAINT CK_Usuarios_Login_NoVacio CHECK (LEN(LTRIM(RTRIM(Login))) > 0),
        CONSTRAINT CK_Usuarios_NombreCompleto_NoVacio CHECK (LEN(LTRIM(RTRIM(NombreCompleto))) > 0),
        CONSTRAINT CK_Usuarios_PasswordHash_NoVacio CHECK (LEN(LTRIM(RTRIM(PasswordHash))) > 0)
    );

    CREATE NONCLUSTERED INDEX IX_Usuarios_Activo
        ON dbo.Usuarios (Activo);

    CREATE NONCLUSTERED INDEX IX_Usuarios_SucursalId
        ON dbo.Usuarios (SucursalId);
END;
GO

/* =============================================================================
   17. Roles
   ============================================================================= */
IF OBJECT_ID(N'dbo.Roles', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Roles
    (
        RolId               INT            IDENTITY(1,1) NOT NULL,
        Nombre              NVARCHAR(100)  NOT NULL,
        Descripcion         NVARCHAR(255)  NULL,
        Activo              BIT            NOT NULL CONSTRAINT DF_Roles_Activo DEFAULT (1),
        FechaCreacion       DATETIME2(0)   NOT NULL CONSTRAINT DF_Roles_FechaCreacion DEFAULT (SYSDATETIME()),
        UsuarioCreacion     NVARCHAR(50)   NOT NULL,
        FechaModificacion   DATETIME2(0)   NULL,
        UsuarioModificacion NVARCHAR(50)   NULL,
        CONSTRAINT PK_Roles PRIMARY KEY CLUSTERED (RolId),
        CONSTRAINT UQ_Roles_Nombre UNIQUE (Nombre),
        CONSTRAINT CK_Roles_Nombre_NoVacio CHECK (LEN(LTRIM(RTRIM(Nombre))) > 0)
    );

    CREATE NONCLUSTERED INDEX IX_Roles_Activo
        ON dbo.Roles (Activo);
END;
GO

/* =============================================================================
   18. Permisos
   ============================================================================= */
IF OBJECT_ID(N'dbo.Permisos', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Permisos
    (
        PermisoId   INT            IDENTITY(1,1) NOT NULL,
        Codigo      NVARCHAR(100)  NOT NULL,
        Nombre      NVARCHAR(150)  NOT NULL,
        Modulo      NVARCHAR(100)  NOT NULL,
        Formulario  NVARCHAR(100)  NULL,
        Accion      NVARCHAR(50)   NULL,
        Descripcion NVARCHAR(255)  NULL,
        Activo      BIT            NOT NULL CONSTRAINT DF_Permisos_Activo DEFAULT (1),
        CONSTRAINT PK_Permisos PRIMARY KEY CLUSTERED (PermisoId),
        CONSTRAINT UQ_Permisos_Codigo UNIQUE (Codigo),
        CONSTRAINT CK_Permisos_Codigo_NoVacio CHECK (LEN(LTRIM(RTRIM(Codigo))) > 0),
        CONSTRAINT CK_Permisos_Nombre_NoVacio CHECK (LEN(LTRIM(RTRIM(Nombre))) > 0),
        CONSTRAINT CK_Permisos_Modulo_NoVacio CHECK (LEN(LTRIM(RTRIM(Modulo))) > 0)
    );

    CREATE NONCLUSTERED INDEX IX_Permisos_Modulo
        ON dbo.Permisos (Modulo);

    CREATE NONCLUSTERED INDEX IX_Permisos_Activo
        ON dbo.Permisos (Activo);
END;
GO

/* =============================================================================
   19. UsuarioRoles
   ============================================================================= */
IF OBJECT_ID(N'dbo.UsuarioRoles', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.UsuarioRoles
    (
        UsuarioRolId    INT            IDENTITY(1,1) NOT NULL,
        UsuarioId       INT            NOT NULL,
        RolId           INT            NOT NULL,
        FechaCreacion   DATETIME2(0)   NOT NULL CONSTRAINT DF_UsuarioRoles_FechaCreacion DEFAULT (SYSDATETIME()),
        UsuarioCreacion NVARCHAR(50)   NOT NULL,
        CONSTRAINT PK_UsuarioRoles PRIMARY KEY CLUSTERED (UsuarioRolId),
        CONSTRAINT FK_UsuarioRoles_Usuarios FOREIGN KEY (UsuarioId)
            REFERENCES dbo.Usuarios (UsuarioId),
        CONSTRAINT FK_UsuarioRoles_Roles FOREIGN KEY (RolId)
            REFERENCES dbo.Roles (RolId),
        CONSTRAINT UQ_UsuarioRoles_UsuarioId_RolId UNIQUE (UsuarioId, RolId)
    );

    CREATE NONCLUSTERED INDEX IX_UsuarioRoles_UsuarioId
        ON dbo.UsuarioRoles (UsuarioId);

    CREATE NONCLUSTERED INDEX IX_UsuarioRoles_RolId
        ON dbo.UsuarioRoles (RolId);
END;
GO

/* =============================================================================
   20. RolPermisos
   ============================================================================= */
IF OBJECT_ID(N'dbo.RolPermisos', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.RolPermisos
    (
        RolPermisoId    INT            IDENTITY(1,1) NOT NULL,
        RolId           INT            NOT NULL,
        PermisoId       INT            NOT NULL,
        FechaCreacion   DATETIME2(0)   NOT NULL CONSTRAINT DF_RolPermisos_FechaCreacion DEFAULT (SYSDATETIME()),
        UsuarioCreacion NVARCHAR(50)   NOT NULL,
        CONSTRAINT PK_RolPermisos PRIMARY KEY CLUSTERED (RolPermisoId),
        CONSTRAINT FK_RolPermisos_Roles FOREIGN KEY (RolId)
            REFERENCES dbo.Roles (RolId),
        CONSTRAINT FK_RolPermisos_Permisos FOREIGN KEY (PermisoId)
            REFERENCES dbo.Permisos (PermisoId),
        CONSTRAINT UQ_RolPermisos_RolId_PermisoId UNIQUE (RolId, PermisoId)
    );

    CREATE NONCLUSTERED INDEX IX_RolPermisos_RolId
        ON dbo.RolPermisos (RolId);

    CREATE NONCLUSTERED INDEX IX_RolPermisos_PermisoId
        ON dbo.RolPermisos (PermisoId);
END;
GO

/* =============================================================================
   21. AuditoriaAcceso
   ============================================================================= */
IF OBJECT_ID(N'dbo.AuditoriaAcceso', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.AuditoriaAcceso
    (
        AuditoriaAccesoId INT            IDENTITY(1,1) NOT NULL,
        UsuarioId         INT            NULL,
        LoginIngresado    NVARCHAR(50)   NOT NULL,
        FechaHora         DATETIME2(0)   NOT NULL CONSTRAINT DF_AuditoriaAcceso_FechaHora DEFAULT (SYSDATETIME()),
        Equipo            NVARCHAR(100)  NULL,
        IpLocal           NVARCHAR(50)   NULL,
        Exitoso           BIT            NOT NULL CONSTRAINT DF_AuditoriaAcceso_Exitoso DEFAULT (0),
        Observacion       NVARCHAR(255)  NULL,
        CONSTRAINT PK_AuditoriaAcceso PRIMARY KEY CLUSTERED (AuditoriaAccesoId),
        CONSTRAINT FK_AuditoriaAcceso_Usuarios FOREIGN KEY (UsuarioId)
            REFERENCES dbo.Usuarios (UsuarioId),
        CONSTRAINT CK_AuditoriaAcceso_LoginIngresado_NoVacio CHECK (LEN(LTRIM(RTRIM(LoginIngresado))) > 0)
    );

    CREATE NONCLUSTERED INDEX IX_AuditoriaAcceso_FechaHora
        ON dbo.AuditoriaAcceso (FechaHora);

    CREATE NONCLUSTERED INDEX IX_AuditoriaAcceso_LoginIngresado
        ON dbo.AuditoriaAcceso (LoginIngresado);

    CREATE NONCLUSTERED INDEX IX_AuditoriaAcceso_UsuarioId
        ON dbo.AuditoriaAcceso (UsuarioId);
END;
GO

PRINT N'Fase 1: script revisado y mejorado ejecutado correctamente (si los objetos no existían).';
GO
