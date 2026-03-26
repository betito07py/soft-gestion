# Soft_Gestion

Sistema de gestión empresarial desktop desarrollado en **VB.NET WinForms** con **SQL Server** y arquitectura por capas.

## Arquitectura

La solución está organizada en los siguientes proyectos:

- `Soft_Gestion.Domain`
- `Soft_Gestion.Data`
- `Soft_Gestion.Business`
- `Soft_Gestion.Common`
- `Soft_Gestion.UI`

## Tecnologías

- **Lenguaje:** VB.NET
- **UI:** WinForms
- **Base de datos:** SQL Server
- **Acceso a datos:** ADO.NET puro
- **Control de versiones:** Git

## Estructura recomendada del repositorio

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
│   └── 06-prompts-cursor-productos-3-niveles.md
│
├── database/
│   ├── schema/
│   ├── seeds/
│   └── scripts/
│
├── src/
│   ├── Soft_Gestion.Domain/
│   ├── Soft_Gestion.Data/
│   ├── Soft_Gestion.Business/
│   ├── Soft_Gestion.Common/
│   └── Soft_Gestion.UI/
│
├── .gitignore
└── README.md
```

## Módulos implementados / en construcción

### Seguridad
- Login
- Usuarios
- Roles
- Permisos
- RolPermisos
- Política de acceso al menú

### Maestros
- Empresas
- Sucursales
- Depósitos
- Clientes
- Proveedores
- Categorías
- SubCategorías
- Grupos
- Marcas
- Unidades de Medida
- Integración SIFEN en Unidades de Medida
- Productos

## Reglas de desarrollo

- No poner SQL en formularios
- No usar Entity Framework
- Usar ADO.NET puro
- Respetar arquitectura por capas
- No borrar físicamente registros críticos
- Mantener auditoría en tablas principales

## Git — estrategia recomendada

### Ramas
- `main`: estable
- `dev`: integración de desarrollo
- `feature/<nombre>`: trabajo por módulo o funcionalidad

Ejemplos:
- `feature/seguridad-usuarios`
- `feature/maestros-clientes`
- `feature/productos-grupoid`
- `feature/stock-modelo-inicial`

### Convención de commits
Usar commits por bloque funcional.

Ejemplos:
- `Inicial - Base del sistema y arquitectura en capas`
- `Seguridad - Login y sesión de aplicación`
- `Seguridad - Usuarios implementado`
- `Seguridad - Roles y permisos implementado`
- `Maestros - Empresas implementado`
- `Maestros - Sucursales implementado`
- `Maestros - Depositos implementado`
- `Maestros - Clientes implementado`
- `Maestros - Proveedores implementado`
- `Maestros - Categorias/SubCategorias/Grupos implementado`
- `Maestros - Marcas implementado`
- `Maestros - Unidades de medida con SIFEN`
- `Maestros - Productos migrado a GrupoId`

## Primeros pasos con Git

### Inicializar repositorio
```bash
git init
git add .
git commit -m "Inicial - Base del sistema Soft_Gestion"
```

### Conectar a GitHub
```bash
git remote add origin https://github.com/TU_USUARIO/soft-gestion.git
git branch -M main
git push -u origin main
```

### Flujo sugerido
```bash
git checkout -b feature/maestros-productos
git add .
git commit -m "Maestros - Productos implementado"
git checkout dev
git merge feature/maestros-productos
```

## Notas importantes

- Los archivos `*.exe.config` no deben versionarse si contienen configuración local o sensible.
- Los scripts SQL de creación y migración sí deben versionarse.
- La documentación en `docs/` debe mantenerse alineada con los cambios estructurales del sistema.
