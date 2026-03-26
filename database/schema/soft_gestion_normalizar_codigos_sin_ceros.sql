/*
    Soft_Gestion — Normalización de códigos de maestros (sin ceros a la izquierda)

    Alineado con Soft_Gestion.Common.CodigoNegocio.NormalizarSinCerosIzquierda:
    - LTRIM/RTRIM
    - Quita ceros iniciales; si solo había ceros → '0'
    - Si tras quitar ceros el primer carácter no es dígito, letra (Latin1 básico + rango extendido latino común) ni '(', se deja el valor solo recortado

    NO incluye: dbo.Permisos (códigos funcionales tipo Maestros.xxx), dbo.UnidadesMedidaSIFEN.Codigo (SMALLINT catálogo SIFEN).

    Riesgo: si dos filas pasan a tener el mismo código dentro del mismo ámbito UNIQUE, el UPDATE fallará.
    Ejecutar primero la sección 2 (diagnóstico de duplicados) y resolver conflictos antes del UPDATE masivo.
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

IF OBJECT_ID(N'dbo.fn_NormalizarCodigoSinCerosIzquierda', N'FN') IS NOT NULL
    DROP FUNCTION dbo.fn_NormalizarCodigoSinCerosIzquierda;
GO

CREATE FUNCTION dbo.fn_NormalizarCodigoSinCerosIzquierda (@s NVARCHAR(4000))
RETURNS NVARCHAR(4000)
AS
BEGIN
    IF @s IS NULL
        RETURN NULL;

    DECLARE @t NVARCHAR(4000) = LTRIM(RTRIM(@s));
    IF LEN(@t) = 0
        RETURN N'';

    DECLARE @cut INT = 0;
    WHILE @cut < LEN(@t) AND SUBSTRING(@t, @cut + 1, 1) = N'0'
        SET @cut += 1;

    IF @cut = LEN(@t)
        RETURN N'0';

    DECLARE @r NVARCHAR(4000) = SUBSTRING(@t, @cut + 1, LEN(@t) - @cut);
    DECLARE @fc NCHAR(1) = LEFT(@r, 1);

    IF @cut > 0
       AND @fc NOT LIKE N'[0-9]'
       AND @fc NOT LIKE N'[A-Za-z]'
       AND @fc NOT LIKE N'[À-ÿ]'
       AND @fc <> N'('
        RETURN @t;

    RETURN @r;
END;
GO

/* =============================================================================
   2. Diagnóstico: posibles colisiones tras normalizar (revisar antes de UPDATE)
   ============================================================================= */
/*
-- Empresas
SELECT dbo.fn_NormalizarCodigoSinCerosIzquierda(Codigo) AS CodigoNorm, COUNT(*) AS Cnt
FROM dbo.Empresas
GROUP BY dbo.fn_NormalizarCodigoSinCerosIzquierda(Codigo)
HAVING COUNT(*) > 1;

-- Sucursales (por empresa)
SELECT EmpresaId, dbo.fn_NormalizarCodigoSinCerosIzquierda(Codigo) AS CodigoNorm, COUNT(*) AS Cnt
FROM dbo.Sucursales
GROUP BY EmpresaId, dbo.fn_NormalizarCodigoSinCerosIzquierda(Codigo)
HAVING COUNT(*) > 1;

-- Depósitos (por sucursal)
SELECT SucursalId, dbo.fn_NormalizarCodigoSinCerosIzquierda(Codigo) AS CodigoNorm, COUNT(*) AS Cnt
FROM dbo.Depositos
GROUP BY SucursalId, dbo.fn_NormalizarCodigoSinCerosIzquierda(Codigo)
HAVING COUNT(*) > 1;

-- Repetir patrón para el resto de tablas con UNIQUE sobre Codigo (o compuesto).
*/

/* =============================================================================
   3. Actualización (descomentar y ejecutar en transacción tras validar §2)
   ============================================================================= */
/*
BEGIN TRAN;

UPDATE dbo.Empresas
SET Codigo = dbo.fn_NormalizarCodigoSinCerosIzquierda(Codigo)
WHERE Codigo <> dbo.fn_NormalizarCodigoSinCerosIzquierda(Codigo);

UPDATE dbo.Sucursales
SET Codigo = dbo.fn_NormalizarCodigoSinCerosIzquierda(Codigo)
WHERE Codigo <> dbo.fn_NormalizarCodigoSinCerosIzquierda(Codigo);

UPDATE dbo.Depositos
SET Codigo = dbo.fn_NormalizarCodigoSinCerosIzquierda(Codigo)
WHERE Codigo <> dbo.fn_NormalizarCodigoSinCerosIzquierda(Codigo);

UPDATE dbo.CondicionesPago
SET Codigo = dbo.fn_NormalizarCodigoSinCerosIzquierda(Codigo)
WHERE Codigo <> dbo.fn_NormalizarCodigoSinCerosIzquierda(Codigo);

UPDATE dbo.Monedas
SET Codigo = dbo.fn_NormalizarCodigoSinCerosIzquierda(Codigo)
WHERE Codigo <> dbo.fn_NormalizarCodigoSinCerosIzquierda(Codigo);

UPDATE dbo.ListasPrecios
SET Codigo = dbo.fn_NormalizarCodigoSinCerosIzquierda(Codigo)
WHERE Codigo <> dbo.fn_NormalizarCodigoSinCerosIzquierda(Codigo);

UPDATE dbo.CategoriasProducto
SET Codigo = dbo.fn_NormalizarCodigoSinCerosIzquierda(Codigo)
WHERE Codigo <> dbo.fn_NormalizarCodigoSinCerosIzquierda(Codigo);

UPDATE dbo.Marcas
SET Codigo = dbo.fn_NormalizarCodigoSinCerosIzquierda(Codigo)
WHERE Codigo <> dbo.fn_NormalizarCodigoSinCerosIzquierda(Codigo);

UPDATE dbo.UnidadesMedida
SET Codigo = dbo.fn_NormalizarCodigoSinCerosIzquierda(Codigo)
WHERE Codigo <> dbo.fn_NormalizarCodigoSinCerosIzquierda(Codigo);

UPDATE dbo.Productos
SET Codigo = dbo.fn_NormalizarCodigoSinCerosIzquierda(Codigo)
WHERE Codigo <> dbo.fn_NormalizarCodigoSinCerosIzquierda(Codigo);

UPDATE dbo.Clientes
SET Codigo = dbo.fn_NormalizarCodigoSinCerosIzquierda(Codigo)
WHERE Codigo <> dbo.fn_NormalizarCodigoSinCerosIzquierda(Codigo);

UPDATE dbo.Proveedores
SET Codigo = dbo.fn_NormalizarCodigoSinCerosIzquierda(Codigo)
WHERE Codigo <> dbo.fn_NormalizarCodigoSinCerosIzquierda(Codigo);

IF OBJECT_ID(N'dbo.Categorias', N'U') IS NOT NULL
    UPDATE dbo.Categorias
    SET Codigo = dbo.fn_NormalizarCodigoSinCerosIzquierda(Codigo)
    WHERE Codigo <> dbo.fn_NormalizarCodigoSinCerosIzquierda(Codigo);

IF OBJECT_ID(N'dbo.SubCategorias', N'U') IS NOT NULL
    UPDATE dbo.SubCategorias
    SET Codigo = dbo.fn_NormalizarCodigoSinCerosIzquierda(Codigo)
    WHERE Codigo <> dbo.fn_NormalizarCodigoSinCerosIzquierda(Codigo);

IF OBJECT_ID(N'dbo.Grupos', N'U') IS NOT NULL
    UPDATE dbo.Grupos
    SET Codigo = dbo.fn_NormalizarCodigoSinCerosIzquierda(Codigo)
    WHERE Codigo <> dbo.fn_NormalizarCodigoSinCerosIzquierda(Codigo);

-- COMMIT TRAN;
-- ROLLBACK TRAN;
*/

GO
