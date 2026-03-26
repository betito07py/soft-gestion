# Ajuste funcional y técnico — Productos con 3 niveles

## Objetivo
Reemplazar el esquema simple de categorías de productos por una clasificación jerárquica de 3 niveles:

1. Categorias
2. SubCategorias
3. Grupos

## Nombres definitivos de tablas
Los nombres de tablas definidos para esta estructura serán exactamente:

- `Categorias`
- `SubCategorias`
- `Grupos`

## Relación jerárquica
La relación correcta será:

- una **Categoria** tiene muchas **SubCategorias**
- una **SubCategoria** pertenece a una **Categoria**
- un **Grupo** pertenece a una **SubCategoria**
- un **Grupo** puede guardar también `CategoriaId` para facilitar validaciones, integridad de negocio y consultas
- un **Producto** deberá apuntar a `GrupoId`

## Ejemplo funcional
- Categoría: Bebidas
- SubCategoría: Sin Alcohol
- Grupo: Agua

Otro ejemplo:
- Categoría: Bebidas
- SubCategoría: Sin Alcohol
- Grupo: Gaseosas

## Cambio recomendado en Productos
En vez de usar una tabla simple de categorías de producto, el diseño recomendado pasa a ser:

- quitar el uso de `CategoriaProductoId`
- incorporar `GrupoId`

Desde `GrupoId` se obtiene por relación:
- `SubCategoriaId`
- `CategoriaId`

## Beneficios
1. Mejor clasificación comercial
2. Filtros más ricos para stock, compras y ventas
3. Reportes por categoría, subcategoría y grupo
4. Mejor escalabilidad para catálogos grandes
5. Menos duplicación de descripciones

## Decisiones de diseño
1. `Categorias`:
   - `Codigo` único global
   - `Nombre` único global

2. `SubCategorias`:
   - `Codigo` único por `CategoriaId`
   - `Nombre` único por `CategoriaId`

3. `Grupos`:
   - `Codigo` único por `SubCategoriaId`
   - `Nombre` único por `SubCategoriaId`

4. Todas las tablas:
   - deben usar PK identity
   - deben tener `Activo`
   - deben tener auditoría
   - no se deben borrar físicamente en UI

## Ajustes que deben hacerse en el proyecto
1. Actualizar documentación del modelo de datos
2. Actualizar roadmap funcional de maestros/productos
3. Crear scripts SQL para:
   - `Categorias`
   - `SubCategorias`
   - `Grupos`
4. Ajustar el diseño futuro de `Productos`
5. Crear formularios por separado:
   - `FrmCategorias`
   - `FrmSubCategorias`
   - `FrmGrupos`
6. En `FrmProductos`, usar combos en cascada:
   - Categoría
   - SubCategoría
   - Grupo

## Permisos sugeridos
Agregar estas claves en `ClavesFormulario` y en `dbo.Permisos`:

- `Maestros.Categorias`
- `Maestros.SubCategorias`
- `Maestros.Grupos`
- `Maestros.Productos`

## Orden recomendado de implementación
1. Categorias
2. SubCategorias
3. Grupos
4. Marcas
5. Unidades de Medida
6. Productos

## Observación importante
Como todavía no se implementó el módulo de productos, este es el momento correcto para cambiar el diseño sin costo alto de migración.
