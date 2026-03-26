# Prompt Fase 1 — Base del sistema

Contexto: `docs/03-roadmap.md` sección 8.

## Alcance Fase 1

1. Seguridad: login, usuarios, roles, permisos
2. Empresas y sucursales
3. Depósitos
4. Clientes
5. Proveedores
6. Productos
7. Listas de precios
8. Condiciones de pago
9. Monedas

## Estructura ya creada

- `src/Soft_Gestion.Domain` — EntidadBase, Entities, Enums, Contracts
- `src/Soft_Gestion.Common` — ConfiguracionConexion
- `src/Soft_Gestion.Data` — DbConexion, Repositories, Mappers
- `src/Soft_Gestion.Business` — Services
- `src/Soft_Gestion.UI` — FrmPrincipal, Program
- `database/` — schema, procedures, views, functions, seeds, scripts

## Siguiente paso

Generar scripts SQL de tablas maestras (Fase 1) según `docs/05-modelo-datos-inicial.md`.
