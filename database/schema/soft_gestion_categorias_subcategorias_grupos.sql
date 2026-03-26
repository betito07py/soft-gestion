/*
    Soft_Gestion
    Ajuste de clasificación de productos a 3 niveles:
    - Categorias
    - SubCategorias
    - Grupos

    Script listo para ejecutar en SQL Server
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

/* =============================================================================
   1. Categorias
   ============================================================================= */
IF OBJECT_ID(N'dbo.Categorias', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Categorias
    (
        CategoriaId          INT            IDENTITY(1,1) NOT NULL,
        Codigo               NVARCHAR(20)   NOT NULL,
        Nombre               NVARCHAR(100)  NOT NULL,
        Activo               BIT            NOT NULL CONSTRAINT DF_Categorias_Activo DEFAULT (1),
        FechaCreacion        DATETIME2(0)   NOT NULL CONSTRAINT DF_Categorias_FechaCreacion DEFAULT (SYSDATETIME()),
        UsuarioCreacion      NVARCHAR(50)   NOT NULL,
        FechaModificacion    DATETIME2(0)   NULL,
        UsuarioModificacion  NVARCHAR(50)   NULL,
        CONSTRAINT PK_Categorias PRIMARY KEY CLUSTERED (CategoriaId),
        CONSTRAINT UQ_Categorias_Codigo UNIQUE (Codigo),
        CONSTRAINT UQ_Categorias_Nombre UNIQUE (Nombre),
        CONSTRAINT CK_Categorias_Codigo_NoVacio CHECK (LEN(LTRIM(RTRIM(Codigo))) > 0),
        CONSTRAINT CK_Categorias_Nombre_NoVacio CHECK (LEN(LTRIM(RTRIM(Nombre))) > 0)
    );

    CREATE NONCLUSTERED INDEX IX_Categorias_Nombre
        ON dbo.Categorias (Nombre);

    CREATE NONCLUSTERED INDEX IX_Categorias_Activo
        ON dbo.Categorias (Activo);
END;
GO

/* =============================================================================
   2. SubCategorias
   ============================================================================= */
IF OBJECT_ID(N'dbo.SubCategorias', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.SubCategorias
    (
        SubCategoriaId       INT            IDENTITY(1,1) NOT NULL,
        Codigo               NVARCHAR(20)   NOT NULL,
        Nombre               NVARCHAR(100)  NOT NULL,
        CategoriaId          INT            NOT NULL,
        Activo               BIT            NOT NULL CONSTRAINT DF_SubCategorias_Activo DEFAULT (1),
        FechaCreacion        DATETIME2(0)   NOT NULL CONSTRAINT DF_SubCategorias_FechaCreacion DEFAULT (SYSDATETIME()),
        UsuarioCreacion      NVARCHAR(50)   NOT NULL,
        FechaModificacion    DATETIME2(0)   NULL,
        UsuarioModificacion  NVARCHAR(50)   NULL,
        CONSTRAINT PK_SubCategorias PRIMARY KEY CLUSTERED (SubCategoriaId),
        CONSTRAINT FK_SubCategorias_Categorias FOREIGN KEY (CategoriaId)
            REFERENCES dbo.Categorias (CategoriaId),
        CONSTRAINT UQ_SubCategorias_CategoriaId_Codigo UNIQUE (CategoriaId, Codigo),
        CONSTRAINT UQ_SubCategorias_CategoriaId_Nombre UNIQUE (CategoriaId, Nombre),
        CONSTRAINT UQ_SubCategorias_SubCategoriaId_CategoriaId UNIQUE (SubCategoriaId, CategoriaId),
        CONSTRAINT CK_SubCategorias_Codigo_NoVacio CHECK (LEN(LTRIM(RTRIM(Codigo))) > 0),
        CONSTRAINT CK_SubCategorias_Nombre_NoVacio CHECK (LEN(LTRIM(RTRIM(Nombre))) > 0)
    );

    CREATE NONCLUSTERED INDEX IX_SubCategorias_CategoriaId
        ON dbo.SubCategorias (CategoriaId);

    CREATE NONCLUSTERED INDEX IX_SubCategorias_Nombre
        ON dbo.SubCategorias (Nombre);

    CREATE NONCLUSTERED INDEX IX_SubCategorias_Activo
        ON dbo.SubCategorias (Activo);
END;
GO

/* =============================================================================
   3. Grupos
   ============================================================================= */
IF OBJECT_ID(N'dbo.Grupos', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Grupos
    (
        GrupoId              INT            IDENTITY(1,1) NOT NULL,
        Codigo               NVARCHAR(20)   NOT NULL,
        Nombre               NVARCHAR(100)  NOT NULL,
        CategoriaId          INT            NOT NULL,
        SubCategoriaId       INT            NOT NULL,
        Activo               BIT            NOT NULL CONSTRAINT DF_Grupos_Activo DEFAULT (1),
        FechaCreacion        DATETIME2(0)   NOT NULL CONSTRAINT DF_Grupos_FechaCreacion DEFAULT (SYSDATETIME()),
        UsuarioCreacion      NVARCHAR(50)   NOT NULL,
        FechaModificacion    DATETIME2(0)   NULL,
        UsuarioModificacion  NVARCHAR(50)   NULL,
        CONSTRAINT PK_Grupos PRIMARY KEY CLUSTERED (GrupoId),
        CONSTRAINT FK_Grupos_Categorias FOREIGN KEY (CategoriaId)
            REFERENCES dbo.Categorias (CategoriaId),
        CONSTRAINT FK_Grupos_SubCategorias FOREIGN KEY (SubCategoriaId)
            REFERENCES dbo.SubCategorias (SubCategoriaId),
        CONSTRAINT FK_Grupos_SubCategorias_CategoriaConsistente FOREIGN KEY (SubCategoriaId, CategoriaId)
            REFERENCES dbo.SubCategorias (SubCategoriaId, CategoriaId),
        CONSTRAINT UQ_Grupos_SubCategoriaId_Codigo UNIQUE (SubCategoriaId, Codigo),
        CONSTRAINT UQ_Grupos_SubCategoriaId_Nombre UNIQUE (SubCategoriaId, Nombre),
        CONSTRAINT CK_Grupos_Codigo_NoVacio CHECK (LEN(LTRIM(RTRIM(Codigo))) > 0),
        CONSTRAINT CK_Grupos_Nombre_NoVacio CHECK (LEN(LTRIM(RTRIM(Nombre))) > 0)
    );

    CREATE NONCLUSTERED INDEX IX_Grupos_CategoriaId
        ON dbo.Grupos (CategoriaId);

    CREATE NONCLUSTERED INDEX IX_Grupos_SubCategoriaId
        ON dbo.Grupos (SubCategoriaId);

    CREATE NONCLUSTERED INDEX IX_Grupos_Nombre
        ON dbo.Grupos (Nombre);

    CREATE NONCLUSTERED INDEX IX_Grupos_Activo
        ON dbo.Grupos (Activo);
END;
GO

PRINT N'Tablas Categorias, SubCategorias y Grupos creadas correctamente si no existían.';
GO
