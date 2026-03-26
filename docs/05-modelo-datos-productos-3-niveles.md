# Modelo de Datos — Productos con 3 niveles

## Objetivo
Definir el modelo de datos para la clasificación jerárquica de productos usando las tablas:

- `Categorias`
- `SubCategorias`
- `Grupos`

y dejar preparado el modelo futuro de `Productos` para asociarse a `GrupoId`.

---

## 1. Tabla: Categorias

### Objetivo
Representa el nivel superior de clasificación.

### Campos sugeridos
- `CategoriaId` INT IDENTITY PK
- `Codigo` NVARCHAR(20) NOT NULL
- `Nombre` NVARCHAR(100) NOT NULL
- `Activo` BIT NOT NULL
- `FechaCreacion` DATETIME2(0) NOT NULL
- `UsuarioCreacion` NVARCHAR(50) NOT NULL
- `FechaModificacion` DATETIME2(0) NULL
- `UsuarioModificacion` NVARCHAR(50) NULL

### Restricciones
- UNIQUE en `Codigo`
- UNIQUE en `Nombre`

### Índices
- índice por `Nombre`
- índice por `Activo`

---

## 2. Tabla: SubCategorias

### Objetivo
Representa el segundo nivel de clasificación, subordinado a una categoría.

### Campos sugeridos
- `SubCategoriaId` INT IDENTITY PK
- `Codigo` NVARCHAR(20) NOT NULL
- `Nombre` NVARCHAR(100) NOT NULL
- `CategoriaId` INT NOT NULL
- `Activo` BIT NOT NULL
- `FechaCreacion` DATETIME2(0) NOT NULL
- `UsuarioCreacion` NVARCHAR(50) NOT NULL
- `FechaModificacion` DATETIME2(0) NULL
- `UsuarioModificacion` NVARCHAR(50) NULL

### Restricciones
- FK a `Categorias(CategoriaId)`
- UNIQUE (`CategoriaId`, `Codigo`)
- UNIQUE (`CategoriaId`, `Nombre`)

### Índices
- índice por `CategoriaId`
- índice por `Nombre`
- índice por `Activo`

---

## 3. Tabla: Grupos

### Objetivo
Representa el tercer nivel de clasificación, subordinado a una subcategoría.

### Campos sugeridos
- `GrupoId` INT IDENTITY PK
- `Codigo` NVARCHAR(20) NOT NULL
- `Nombre` NVARCHAR(100) NOT NULL
- `CategoriaId` INT NOT NULL
- `SubCategoriaId` INT NOT NULL
- `Activo` BIT NOT NULL
- `FechaCreacion` DATETIME2(0) NOT NULL
- `UsuarioCreacion` NVARCHAR(50) NOT NULL
- `FechaModificacion` DATETIME2(0) NULL
- `UsuarioModificacion` NVARCHAR(50) NULL

### Restricciones
- FK a `Categorias(CategoriaId)`
- FK a `SubCategorias(SubCategoriaId)`
- UNIQUE (`SubCategoriaId`, `Codigo`)
- UNIQUE (`SubCategoriaId`, `Nombre`)
- Regla lógica: `CategoriaId` debe coincidir con la categoría de la `SubCategoriaId`

### Índices
- índice por `CategoriaId`
- índice por `SubCategoriaId`
- índice por `Nombre`
- índice por `Activo`

---

## 4. Consistencia de integridad para Grupos
Como `Grupos` guarda tanto `CategoriaId` como `SubCategoriaId`, se debe asegurar que ambos sean consistentes.

### Recomendación
Usar una FK compuesta adicional:

- `SubCategorias` debe tener UNIQUE (`SubCategoriaId`, `CategoriaId`)
- `Grupos(SubCategoriaId, CategoriaId)` debe referenciar `SubCategorias(SubCategoriaId, CategoriaId)`

Esto evita que un grupo quede enlazado a una subcategoría de otra categoría.

---

## 5. Ajuste futuro en Productos

### Estructura recomendada
En `Productos`, en vez de usar una categoría simple:

- usar `GrupoId` INT NULL o NOT NULL según política final

### Beneficio
Desde `GrupoId` se obtiene:
- `SubCategoriaId`
- `CategoriaId`

### Regla UI
En `FrmProductos`:
1. seleccionar categoría
2. filtrar subcategorías
3. filtrar grupos
4. guardar `GrupoId`

---

## 6. DTOs sugeridos para Domain

### CategoriaResumen
- `CategoriaId`
- `Codigo`
- `Nombre`
- `Activo`

### SubCategoriaResumen
- `SubCategoriaId`
- `CategoriaId`
- `CategoriaCodigo`
- `CategoriaNombre`
- `Codigo`
- `Nombre`
- `Activo`

### GrupoResumen
- `GrupoId`
- `CategoriaId`
- `SubCategoriaId`
- `CategoriaCodigo`
- `CategoriaNombre`
- `SubCategoriaCodigo`
- `SubCategoriaNombre`
- `Codigo`
- `Nombre`
- `Activo`

### SelectorItem sugeridos
- `CategoriaSelectorItem`
- `SubCategoriaSelectorItem`
- `GrupoSelectorItem`

---

## 7. Formularios sugeridos

### FrmCategorias
- grilla
- búsqueda por código o nombre
- alta/edición
- activar/desactivar

### FrmSubCategorias
- grilla
- filtro por categoría
- búsqueda por código o nombre
- alta/edición
- activar/desactivar

### FrmGrupos
- grilla
- filtro por categoría
- filtro por subcategoría
- búsqueda por código o nombre
- alta/edición
- activar/desactivar

---

## 8. Permisos sugeridos
- `Maestros.Categorias`
- `Maestros.SubCategorias`
- `Maestros.Grupos`

---

## 9. Orden de construcción en Cursor
1. SQL de `Categorias`
2. SQL de `SubCategorias`
3. SQL de `Grupos`
4. DTOs y entidades
5. Repositorios
6. Servicios
7. Formularios
8. Integración en `FrmPrincipal`
