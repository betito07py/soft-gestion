# Prompts para Cursor — Categorias / SubCategorias / Grupos

## 1. Prompt para SQL
```text
Usa como contexto:
- docs/04-convenciones-codigo.md
- docs/05-modelo-datos-productos-3-niveles.md

Genera el script SQL Server para crear las tablas:
- Categorias
- SubCategorias
- Grupos

Requisitos:
- PK identity
- FKs
- UNIQUE
- DEFAULTs en Activo y FechaCreacion
- DATETIME2(0)
- CHECKs básicos de no vacío
- índices recomendados
- integridad entre Grupos.CategoriaId y Grupos.SubCategoriaId
- no usar Entity Framework
- no generar SP todavía
```

## 2. Prompt para Categorias
```text
Usa como contexto:
- docs/03-roadmap.md
- docs/04-convenciones-codigo.md
- docs/05-modelo-datos-productos-3-niveles.md

Genera el submódulo Maestros > Categorias.

Necesito:
1. Domain
- Dto/CategoriaResumen.vb
- Dto/CategoriaSelectorItem.vb

2. Data
- CategoriaRepository:
  - Listar(filtroTexto)
  - ObtenerPorId
  - ExisteCodigo(codigo, excluirCategoriaId?)
  - ExisteNombre(nombre, excluirCategoriaId?)
  - Insertar
  - Actualizar
  - Activar
  - Desactivar
  - ListarActivasParaCombo

3. Business
- CategoriaService con validaciones

4. UI
- FrmCategorias con grilla, búsqueda, alta/edición, activar/desactivar
- ClaveFormularioPermiso = ClavesFormulario.MaestrosCategorias

5. Integración en FrmPrincipal

Usar ADO.NET puro.
```

## 3. Prompt para SubCategorias
```text
Usa como contexto:
- docs/03-roadmap.md
- docs/04-convenciones-codigo.md
- docs/05-modelo-datos-productos-3-niveles.md

Genera el submódulo Maestros > SubCategorias.

Necesito:
1. Domain
- Dto/SubCategoriaResumen.vb
- Dto/SubCategoriaSelectorItem.vb

2. Data
- SubCategoriaRepository:
  - Listar(filtroTexto, categoriaId?)
  - ObtenerPorId
  - ExisteCodigo(categoriaId, codigo, excluirSubCategoriaId?)
  - ExisteNombre(categoriaId, nombre, excluirSubCategoriaId?)
  - Insertar
  - Actualizar
  - Activar
  - Desactivar
  - ListarActivasParaCombo(categoriaId?)

3. Business
- SubCategoriaService con validaciones y selector de categorías activas

4. UI
- FrmSubCategorias con filtro por categoría
- ClaveFormularioPermiso = ClavesFormulario.MaestrosSubCategorias

5. Integración en FrmPrincipal

Usar ADO.NET puro.
```

## 4. Prompt para Grupos
```text
Usa como contexto:
- docs/03-roadmap.md
- docs/04-convenciones-codigo.md
- docs/05-modelo-datos-productos-3-niveles.md

Genera el submódulo Maestros > Grupos.

Necesito:
1. Domain
- Dto/GrupoResumen.vb
- Dto/GrupoSelectorItem.vb

2. Data
- GrupoRepository:
  - Listar(filtroTexto, categoriaId?, subCategoriaId?)
  - ObtenerPorId
  - ExisteCodigo(subCategoriaId, codigo, excluirGrupoId?)
  - ExisteNombre(subCategoriaId, nombre, excluirGrupoId?)
  - Insertar
  - Actualizar
  - Activar
  - Desactivar
  - ListarActivasParaCombo(subCategoriaId?)

3. Business
- GrupoService con validaciones
- validar consistencia entre CategoriaId y SubCategoriaId

4. UI
- FrmGrupos con filtros en cascada:
  - categoría
  - subcategoría
- ClaveFormularioPermiso = ClavesFormulario.MaestrosGrupos

5. Integración en FrmPrincipal

Usar ADO.NET puro.
```
