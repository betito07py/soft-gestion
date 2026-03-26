# Plan maestro para Cursor — ERP Desktop Tradicional

## 1. Objetivo del proyecto
Construir un sistema de gestión para empresas con arquitectura desktop tradicional, orientado inicialmente a los módulos de:

- Seguridad y acceso
- Maestros básicos
- Stock
- Compras
- Ventas
- Cuentas a cobrar
- Cuentas a pagar
- Tesorería

El sistema debe ser modular, transaccional, auditable y preparado para crecer luego a reportes avanzados, contabilidad integrada y otras extensiones.

---

## 2. Stack tecnológico recomendado

### Aplicación
- Lenguaje: VB.NET
- Plataforma: .NET Desktop
- UI: WinForms
- Patrón recomendado: arquitectura por capas

### Base de datos
- SQL Server
- Stored Procedures para operaciones críticas
- Vistas para consultas y reportes
- Triggers solo cuando sean estrictamente necesarios

### Reportes
- Crystal Reports, RDLC o exportación a Excel/PDF

### Control de código
- Git

---

## 3. Objetivo de este documento para Cursor
Este documento existe para que Cursor entienda:

1. qué se está construyendo
2. en qué orden debe construirse
3. cuáles son las reglas de negocio
4. cómo debe estar estructurado el código
5. qué convenciones deben respetarse

Cursor no debe intentar construir todo el ERP de una sola vez.
Debe trabajar por fases, módulos y casos de uso concretos.

---

## 4. Orden ideal de desarrollo

### Fase 1 — Base del sistema
Construir primero:

1. autenticación
2. usuarios
3. roles y permisos
4. empresas
5. sucursales
6. depósitos
7. clientes
8. proveedores
9. productos
10. listas de precios
11. condiciones de pago
12. monedas

### Fase 2 — Stock
Construir luego:

1. tipos de movimiento de stock
2. movimientos de stock
3. stock actual por depósito
4. ajustes de inventario
5. transferencias entre depósitos
6. consulta de existencia
7. kardex de movimientos

### Fase 3 — Compras
Construir luego:

1. ordenes de compra
2. recepción de mercadería
3. factura de compra
4. nota de crédito de proveedor
5. actualización de costos
6. generación automática de cuenta a pagar

### Fase 4 — Ventas
Construir luego:

1. presupuestos o pedidos
2. factura de venta
3. remisión si aplica
4. devolución de venta
5. descuento automático de stock
6. generación automática de cuenta a cobrar
7. venta contado con impacto directo en tesorería

### Fase 5 — Finanzas
Construir luego:

1. cobranzas
2. pagos a proveedores
3. caja
4. bancos
5. movimientos de tesorería
6. transferencias de fondos
7. arqueo y cierre de caja
8. conciliación básica

### Fase 6 — Reportes
Construir finalmente:

1. stock actual
2. kardex
3. compras por proveedor
4. ventas por cliente, producto y vendedor
5. saldos de clientes
6. saldos de proveedores
7. caja y bancos
8. vencimientos a cobrar y pagar

---

## 5. Arquitectura del proyecto

## 5.1 Capas recomendadas

### ERP.Domain
Contiene:
- entidades de negocio
- enums
- contratos base
- reglas simples del dominio

### ERP.Data
Contiene:
- acceso a SQL Server
- repositorios
- ejecución de stored procedures
- mapeo de datos

### ERP.Business
Contiene:
- servicios de negocio
- validaciones de negocio
- coordinación transaccional
- casos de uso

### ERP.UI
Contiene:
- formularios
- menús
- navegación
- binding de datos
- interacción con usuario

### ERP.Common
Contiene:
- utilidades
- configuración
- helpers
- auditoría
- seguridad compartida

---

## 5.2 Estructura sugerida de carpetas

```text
ERP-Gestion/
│
├── docs/
│   ├── 01-vision-general.md
│   ├── 02-reglas-negocio.md
│   ├── 03-roadmap.md
│   ├── 04-convenciones-codigo.md
│   ├── 05-modelo-datos-inicial.md
│   └── modulos/
│       ├── seguridad.md
│       ├── maestros.md
│       ├── stock.md
│       ├── compras.md
│       ├── ventas.md
│       ├── cuentas-cobrar.md
│       ├── cuentas-pagar.md
│       └── tesoreria.md
│
├── database/
│   ├── schema/
│   ├── procedures/
│   ├── views/
│   ├── functions/
│   ├── seeds/
│   └── scripts/
│
├── src/
│   ├── ERP.Domain/
│   ├── ERP.Data/
│   ├── ERP.Business/
│   ├── ERP.Common/
│   └── ERP.UI/
│
├── prompts/
│   ├── cursor-rules.md
│   ├── fase-1-base.md
│   ├── fase-2-stock.md
│   ├── fase-3-compras.md
│   ├── fase-4-ventas.md
│   ├── fase-5-finanzas.md
│   └── fase-6-reportes.md
│
└── README.md
```

---

## 6. Reglas globales de negocio

1. No borrar físicamente comprobantes confirmados.
2. Toda anulación debe quedar auditada.
3. Toda operación debe registrar usuario, fecha, hora y terminal.
4. Una compra puede impactar stock y cuentas a pagar.
5. Una venta puede impactar stock y cuentas a cobrar o tesorería.
6. Un cobro cancela parcial o totalmente una cuenta a cobrar.
7. Un pago cancela parcial o totalmente una cuenta a pagar.
8. El stock debe manejarse por depósito.
9. El sistema debe soportar ventas contado y crédito.
10. El sistema debe soportar compras contado y crédito.
11. Las operaciones críticas deben ejecutarse dentro de transacciones SQL.
12. Debe existir control de permisos por formulario y por acción.
13. No permitir stock negativo, salvo permiso explícito.
14. Toda modificación sensible debe quedar en log.
15. Los saldos deben recalcularse desde movimientos, no desde campos manuales sin trazabilidad.

---

## 7. Convenciones para Cursor

### 7.1 Convenciones generales
- No generar código fuera de la fase actual.
- No mezclar UI con acceso a datos.
- No escribir SQL embebido en formularios si puede ir en repositorios o SP.
- No duplicar lógica de negocio entre formularios.
- Usar nombres consistentes y descriptivos.
- Separar cabecera y detalle en documentos transaccionales.
- Siempre contemplar auditoría.

### 7.2 Convenciones de nombres

#### Tablas
- Prefijo por módulo cuando convenga.
- Nombres claros.
- Ejemplos:
  - Usuarios
  - Roles
  - Productos
  - Depositos
  - StockMovimientos
  - ComprasCabecera
  - ComprasDetalle
  - VentasCabecera
  - VentasDetalle
  - CuentasCobrarMovimientos
  - CuentasPagarMovimientos
  - TesoreriaMovimientos

#### Stored Procedures
- `usp_Entidad_Accion`
- Ejemplos:
  - `usp_Usuario_Login`
  - `usp_Producto_Guardar`
  - `usp_Stock_Ajustar`
  - `usp_Compra_Confirmar`
  - `usp_Venta_Confirmar`
  - `usp_Cobranza_Aplicar`
  - `usp_Pago_Aplicar`

#### Formularios
- Prefijo `Frm`
- Ejemplos:
  - `FrmLogin`
  - `FrmPrincipal`
  - `FrmProductos`
  - `FrmCompras`
  - `FrmVentas`
  - `FrmCobranzas`

#### Clases de servicio
- Sufijo `Service`
- Ejemplos:
  - `ProductoService`
  - `CompraService`
  - `VentaService`

#### Repositorios
- Sufijo `Repository`
- Ejemplos:
  - `ProductoRepository`
  - `UsuarioRepository`
  - `VentaRepository`

---

## 8. Definición funcional de la Fase 1 — Base del sistema

## 8.1 Seguridad

### Objetivo
Permitir acceso seguro al sistema y controlar qué puede hacer cada usuario.

### Alcance
- login
- logout
- usuarios
- roles
- permisos por módulo / formulario / acción
- cambio de contraseña
- usuario activo / inactivo

### Tablas mínimas
- Usuarios
- Roles
- Permisos
- UsuarioRoles
- RolPermisos
- AuditoriaAcceso

### Casos de uso
1. iniciar sesión
2. validar credenciales
3. cargar permisos al menú principal
4. bloquear acceso a formularios no autorizados
5. registrar intentos de acceso

---

## 8.2 Empresas y sucursales

### Objetivo
Permitir que el sistema opere con una o varias empresas y varias sucursales.

### Tablas mínimas
- Empresas
- Sucursales

### Casos de uso
1. crear empresa
2. crear sucursal
3. asignar sucursal a usuario
4. usar sucursal activa en transacciones

---

## 8.3 Depósitos

### Objetivo
Definir dónde se guarda físicamente el stock.

### Tablas mínimas
- Depositos

### Casos de uso
1. crear depósito
2. asociar depósito a sucursal
3. marcar depósito activo

---

## 8.4 Clientes

### Objetivo
Administrar clientes para ventas y cuentas a cobrar.

### Datos mínimos sugeridos
- código
- razón social
- nombre fantasía
- RUC o documento
- dirección
- teléfono
- email
- límite de crédito
- condición de pago
- lista de precios
- activo

### Tablas mínimas
- Clientes

---

## 8.5 Proveedores

### Objetivo
Administrar proveedores para compras y cuentas a pagar.

### Datos mínimos sugeridos
- código
- razón social
- RUC
- dirección
- teléfono
- email
- condición de pago
- activo

### Tablas mínimas
- Proveedores

---

## 8.6 Productos

### Objetivo
Administrar los artículos que se compran, venden y controlan en stock.

### Datos mínimos sugeridos
- código interno
- código de barras
- descripción
- categoría
- marca
- unidad de medida
- costo
- precio
- IVA
- permite stock negativo
- activo

### Tablas mínimas
- Productos
- CategoriasProducto
- Marcas
- UnidadesMedida

---

## 8.7 Listas de precios

### Objetivo
Permitir precios diferenciados por tipo de cliente o canal.

### Tablas mínimas
- ListasPrecios
- ListaPreciosDetalle

---

## 8.8 Condiciones de pago

### Objetivo
Definir plazos y comportamiento de contado o crédito.

### Tablas mínimas
- CondicionesPago

Ejemplos:
- contado
- 15 días
- 30 días
- 30/60 días

---

## 8.9 Monedas

### Objetivo
Definir moneda base y monedas secundarias.

### Tablas mínimas
- Monedas
- Cotizaciones

En primera etapa puede mantenerse simple.

---

## 9. Entregables que Cursor debe generar primero

### Paso 1
Crear la solución y proyectos por capas.

### Paso 2
Crear scripts SQL de tablas maestras de Fase 1.

### Paso 3
Crear entidades del dominio para Fase 1.

### Paso 4
Crear repositorios base de Fase 1.

### Paso 5
Crear servicios de negocio de Fase 1.

### Paso 6
Crear formularios base:
- login
- menú principal
- usuarios
- roles
- clientes
- proveedores
- productos
- depósitos
- sucursales

### Paso 7
Crear navegación, validaciones y permisos.

---

## 10. Checklist funcional de Fase 1

- [ ] Login funcional
- [ ] Menú principal con permisos
- [ ] CRUD de usuarios
- [ ] CRUD de roles
- [ ] CRUD de empresas
- [ ] CRUD de sucursales
- [ ] CRUD de depósitos
- [ ] CRUD de clientes
- [ ] CRUD de proveedores
- [ ] CRUD de productos
- [ ] CRUD de listas de precios
- [ ] CRUD de condiciones de pago
- [ ] CRUD de monedas
- [ ] Auditoría básica

---

## 11. Prompt maestro para Cursor

Usar este prompt como contexto inicial del proyecto:

```text
Estoy construyendo un ERP desktop tradicional en capas con SQL Server y WinForms.
La arquitectura del proyecto debe separarse en Domain, Data, Business, Common y UI.
Debes trabajar únicamente en la fase actual y no adelantar módulos futuros.
No mezcles lógica de negocio en formularios.
No mezcles SQL directamente en la UI salvo casos excepcionales.
Usa repositorios para acceso a datos y servicios para reglas de negocio.
Toda operación crítica debe ser auditable y transaccional.
Respeta los nombres definidos en la documentación del proyecto.
Empieza por la Fase 1: seguridad, usuarios, roles, empresas, sucursales, depósitos, clientes, proveedores, productos, listas de precios, condiciones de pago y monedas.
Genera código mantenible, claro y preparado para crecer.
```

---

## 12. Prompt específico para arrancar en Cursor

```text
Con base en la documentación del proyecto, genera la estructura inicial de una solución desktop tradicional en WinForms con SQL Server y arquitectura por capas.

Necesito:
1. estructura de proyectos Domain, Data, Business, Common y UI
2. clases base compartidas
3. configuración inicial de conexión a base de datos
4. formulario de login
5. formulario principal con menú modular
6. entidades iniciales de Fase 1
7. repositorios base
8. servicios base

No implementes todavía compras, ventas, stock transaccional ni tesorería. Solo deja preparada la base del sistema.
```

---

## 13. Prompts por submódulo de Fase 1

### Seguridad
```text
Genera el submódulo de seguridad para una aplicación WinForms en capas. Incluye entidades Usuario, Rol, Permiso, UsuarioRol y RolPermiso, sus tablas SQL, repositorios, servicios, validación de login y carga de permisos al menú principal.
```

### Clientes
```text
Genera el submódulo de clientes para una aplicación WinForms en capas. Incluye tabla SQL, entidad, repositorio, servicio y formulario CRUD con validaciones básicas. Debe contemplar código, razón social, documento, dirección, teléfono, email, condición de pago, lista de precios y estado activo.
```

### Proveedores
```text
Genera el submódulo de proveedores para una aplicación WinForms en capas. Incluye tabla SQL, entidad, repositorio, servicio y formulario CRUD con validaciones básicas.
```

### Productos
```text
Genera el submódulo de productos para una aplicación WinForms en capas. Incluye tablas SQL necesarias, entidad, repositorio, servicio y formulario CRUD. Debe contemplar código, descripción, unidad de medida, categoría, marca, costo, precio, IVA, stock negativo permitido y estado activo.
```

### Depósitos
```text
Genera el submódulo de depósitos para una aplicación WinForms en capas. Incluye tabla SQL, entidad, repositorio, servicio y formulario CRUD. Debe permitir asociar depósitos a sucursales.
```

---

## 14. Qué no debe hacer Cursor

1. No crear todo el ERP en un solo paso.
2. No inventar campos sin documentarlos.
3. No mezclar cuentas a cobrar con ventas sin fase definida.
4. No generar stock transaccional antes de terminar Fase 1.
5. No duplicar entidades similares.
6. No hacer formularios gigantes con toda la lógica adentro.
7. No usar nombres ambiguos.

---

## 15. Siguiente documento recomendado
Después de este archivo, el siguiente documento a crear debe ser:

- `05-modelo-datos-inicial.md`

Ese documento debe definir en detalle:
- tablas
- campos
- claves primarias
- claves foráneas
- índices
- auditoría

Luego deben generarse los documentos individuales por módulo.

