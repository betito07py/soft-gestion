# Estructura recomendada del repositorio

```text
Soft_Gestion/
│
├── docs/
│   ├── 01-vision-general.md
│   ├── 02-reglas-negocio.md
│   ├── 03-roadmap.md
│   ├── 04-convenciones-codigo.md
│   ├── 05-modelo-datos-inicial.md
│   ├── 05-modelo-datos-productos-3-niveles.md
│   ├── 06-prompts-cursor-productos-3-niveles.md
│   └── ajustes/
│       ├── ajuste_productos_3_niveles.md
│       └── ajuste_unidades_sifen.md
│
├── database/
│   ├── schema/
│   │   ├── 01_Fase1_Tablas_Maestras.sql
│   │   ├── soft_gestion_categorias_subcategorias_grupos.sql
│   │   ├── soft_gestion_productos_grupo.sql
│   │   ├── soft_gestion_unidades_medida_sifen.sql
│   │   └── soft_gestion_marcas_auditoria.sql
│   │
│   ├── seeds/
│   │   ├── seed_admin.sql
│   │   └── seed_permisos.sql
│   │
│   └── scripts/
│       └── utilitarios.sql
│
├── src/
│   ├── Soft_Gestion.Domain/
│   ├── Soft_Gestion.Data/
│   ├── Soft_Gestion.Business/
│   ├── Soft_Gestion.Common/
│   └── Soft_Gestion.UI/
│
├── .gitignore
├── README.md
└── Soft_Gestion.sln
```

## Regla práctica
- `docs/` -> documentación funcional y técnica
- `database/schema/` -> creación y migraciones estructurales
- `database/seeds/` -> datos iniciales
- `database/scripts/` -> scripts auxiliares
- `src/` -> código fuente
