# Modelo de Datos Inicial — Fase 1

## 1. Objetivo

Definir la estructura inicial de base de datos para la Fase 1 del ERP Desktop, contemplando:

- Seguridad
- Empresas
- Sucursales
- Depósitos
- Clientes
- Proveedores
- Productos
- Categorías
- Marcas
- Unidades de medida
- Listas de precios
- Condiciones de pago
- Monedas

Base de datos objetivo: **SQL Server**  
Lenguaje de aplicación: **VB.NET**  
Arquitectura: **ERP.Domain / ERP.Data / ERP.Business / ERP.Common / ERP.UI**

---

## 2. Criterios generales de diseño

1. Todas las tablas maestras deben tener clave primaria entera identity.
2. Todas las tablas deben contemplar estado activo/inactivo cuando corresponda.
3. Todas las tablas principales deben contemplar auditoría mínima.
4. Se evitará borrar físicamente registros críticos.
5. Los nombres estarán en singular lógico de negocio pero como tabla física se usarán nombres plurales.
6. Se usarán claves foráneas explícitas.
7. Se crearán índices en campos de búsqueda frecuente.
8. Los catálogos simples deben quedar separados para evitar datos repetidos.

---

## 3. Convención de auditoría

En las tablas principales usar:

- `FechaCreacion` DATETIME NOT NULL
- `UsuarioCreacion` NVARCHAR(50) NOT NULL
- `FechaModificacion` DATETIME NULL
- `UsuarioModificacion` NVARCHAR(50) NULL
- `Activo` BIT NOT NULL

No todas las tablas necesitan modificación si son históricas o de relación técnica, pero en Fase 1 se recomienda usarlo en casi todas las maestras.

---

# 4. Tablas de Seguridad

## 4.1 Usuarios

### Objetivo
Administrar acceso al sistema.

### Campos
- `UsuarioId` INT IDENTITY PK
- `Login` NVARCHAR(50) NOT NULL
- `NombreCompleto` NVARCHAR(150) NOT NULL
- `PasswordHash` NVARCHAR(255) NOT NULL
- `Email` NVARCHAR(150) NULL
- `EsAdministrador` BIT NOT NULL
- `SucursalId` INT NULL
- `Activo` BIT NOT NULL
- `FechaCreacion` DATETIME NOT NULL
- `UsuarioCreacion` NVARCHAR(50) NOT NULL
- `FechaModificacion` DATETIME NULL
- `UsuarioModificacion` NVARCHAR(50) NULL

### Restricciones
- UNIQUE en `Login`
- FK a `Sucursales(SucursalId)` si se desea asociar sucursal por defecto

### Índices
- Índice único por `Login`
- Índice por `Activo`

---

## 4.2 Roles

### Objetivo
Definir perfiles de acceso.

### Campos
- `RolId` INT IDENTITY PK
- `Nombre` NVARCHAR(100) NOT NULL
- `Descripcion` NVARCHAR(255) NULL
- `Activo` BIT NOT NULL
- `FechaCreacion` DATETIME NOT NULL
- `UsuarioCreacion` NVARCHAR(50) NOT NULL
- `FechaModificacion` DATETIME NULL
- `UsuarioModificacion` NVARCHAR(50) NULL

### Restricciones
- UNIQUE en `Nombre`

---

## 4.3 Permisos

### Objetivo
Definir permisos por módulo, formulario o acción.

### Campos
- `PermisoId` INT IDENTITY PK
- `Codigo` NVARCHAR(100) NOT NULL
- `Nombre` NVARCHAR(150) NOT NULL
- `Modulo` NVARCHAR(100) NOT NULL
- `Formulario` NVARCHAR(100) NULL
- `Accion` NVARCHAR(50) NULL
- `Descripcion` NVARCHAR(255) NULL
- `Activo` BIT NOT NULL

### Restricciones
- UNIQUE en `Codigo`

### Ejemplos de Código
- `SEGURIDAD_USUARIOS_VER`
- `SEGURIDAD_USUARIOS_EDITAR`
- `MAESTROS_PRODUCTOS_VER`
- `MAESTROS_CLIENTES_EDITAR`

---

## 4.4 UsuarioRoles

### Objetivo
Relacionar usuarios con roles.

### Campos
- `UsuarioRolId` INT IDENTITY PK
- `UsuarioId` INT NOT NULL
- `RolId` INT NOT NULL
- `FechaCreacion` DATETIME NOT NULL
- `UsuarioCreacion` NVARCHAR(50) NOT NULL

### Restricciones
- FK a `Usuarios`
- FK a `Roles`
- UNIQUE (`UsuarioId`, `RolId`)

---

## 4.5 RolPermisos

### Objetivo
Relacionar roles con permisos.

### Campos
- `RolPermisoId` INT IDENTITY PK
- `RolId` INT NOT NULL
- `PermisoId` INT NOT NULL
- `FechaCreacion` DATETIME NOT NULL
- `UsuarioCreacion` NVARCHAR(50) NOT NULL

### Restricciones
- FK a `Roles`
- FK a `Permisos`
- UNIQUE (`RolId`, `PermisoId`)

---

## 4.6 AuditoriaAcceso

### Objetivo
Registrar ingresos y eventos de autenticación.

### Campos
- `AuditoriaAccesoId` INT IDENTITY PK
- `UsuarioId` INT NULL
- `LoginIngresado` NVARCHAR(50) NOT NULL
- `FechaHora` DATETIME NOT NULL
- `Equipo` NVARCHAR(100) NULL
- `IpLocal` NVARCHAR(50) NULL
- `Exitoso` BIT NOT NULL
- `Observacion` NVARCHAR(255) NULL

### Restricciones
- FK opcional a `Usuarios`

---

# 5. Tablas organizativas

## 5.1 Empresas

### Objetivo
Permitir operar una o varias empresas.

### Campos
- `EmpresaId` INT IDENTITY PK
- `Codigo` NVARCHAR(20) NOT NULL
- `RazonSocial` NVARCHAR(150) NOT NULL
- `NombreFantasia` NVARCHAR(150) NULL
- `RUC` NVARCHAR(30) NULL
- `Direccion` NVARCHAR(200) NULL
- `Telefono` NVARCHAR(50) NULL
- `Email` NVARCHAR(150) NULL
- `Activo` BIT NOT NULL
- `FechaCreacion` DATETIME NOT NULL
- `UsuarioCreacion` NVARCHAR(50) NOT NULL
- `FechaModificacion` DATETIME NULL
- `UsuarioModificacion` NVARCHAR(50) NULL

### Restricciones
- UNIQUE en `Codigo`

---

## 5.2 Sucursales

### Objetivo
Definir sucursales por empresa.

### Campos
- `SucursalId` INT IDENTITY PK
- `EmpresaId` INT NOT NULL
- `Codigo` NVARCHAR(20) NOT NULL
- `Nombre` NVARCHAR(100) NOT NULL
- `Direccion` NVARCHAR(200) NULL
- `Telefono` NVARCHAR(50) NULL
- `Responsable` NVARCHAR(100) NULL
- `Activo` BIT NOT NULL
- `FechaCreacion` DATETIME NOT NULL
- `UsuarioCreacion` NVARCHAR(50) NOT NULL
- `FechaModificacion` DATETIME NULL
- `UsuarioModificacion` NVARCHAR(50) NULL

### Restricciones
- FK a `Empresas`
- UNIQUE (`EmpresaId`, `Codigo`)

### Índices
- Índice por `EmpresaId`

---

## 5.3 Depositos

### Objetivo
Definir depósitos de stock por sucursal.

### Campos
- `DepositoId` INT IDENTITY PK
- `SucursalId` INT NOT NULL
- `Codigo` NVARCHAR(20) NOT NULL
- `Nombre` NVARCHAR(100) NOT NULL
- `Descripcion` NVARCHAR(255) NULL
- `EsPrincipal` BIT NOT NULL
- `Activo` BIT NOT NULL
- `FechaCreacion` DATETIME NOT NULL
- `UsuarioCreacion` NVARCHAR(50) NOT NULL
- `FechaModificacion` DATETIME NULL
- `UsuarioModificacion` NVARCHAR(50) NULL

### Restricciones
- FK a `Sucursales`
- UNIQUE (`SucursalId`, `Codigo`)

### Índices
- Índice por `SucursalId`

---

# 6. Tablas maestras comerciales

## 6.1 CondicionesPago

### Objetivo
Definir contado y crédito.

### Campos
- `CondicionPagoId` INT IDENTITY PK
- `Codigo` NVARCHAR(20) NOT NULL
- `Nombre` NVARCHAR(100) NOT NULL
- `DiasPlazo` INT NOT NULL
- `EsContado` BIT NOT NULL
- `Activo` BIT NOT NULL

### Restricciones
- UNIQUE en `Codigo`

### Ejemplos
- CONTADO
- CREDITO15
- CREDITO30

---

## 6.2 Monedas

### Objetivo
Definir monedas del sistema.

### Campos
- `MonedaId` INT IDENTITY PK
- `Codigo` NVARCHAR(10) NOT NULL
- `Nombre` NVARCHAR(50) NOT NULL
- `Simbolo` NVARCHAR(10) NULL
- `EsMonedaBase` BIT NOT NULL
- `CantidadDecimales` INT NOT NULL
- `Activo` BIT NOT NULL

### Restricciones
- UNIQUE en `Codigo`

### Ejemplos
- PYG
- USD

---

## 6.3 Cotizaciones

### Objetivo
Guardar tipo de cambio por fecha.

### Campos
- `CotizacionId` INT IDENTITY PK
- `MonedaId` INT NOT NULL
- `Fecha` DATE NOT NULL
- `Compra` DECIMAL(18,6) NOT NULL
- `Venta` DECIMAL(18,6) NOT NULL
- `Activo` BIT NOT NULL
- `FechaCreacion` DATETIME NOT NULL
- `UsuarioCreacion` NVARCHAR(50) NOT NULL

### Restricciones
- FK a `Monedas`
- UNIQUE (`MonedaId`, `Fecha`)

---

## 6.4 ListasPrecios

### Objetivo
Definir listas de precios.

### Campos
- `ListaPrecioId` INT IDENTITY PK
- `Codigo` NVARCHAR(20) NOT NULL
- `Nombre` NVARCHAR(100) NOT NULL
- `MonedaId` INT NOT NULL
- `Activo` BIT NOT NULL
- `FechaCreacion` DATETIME NOT NULL
- `UsuarioCreacion` NVARCHAR(50) NOT NULL
- `FechaModificacion` DATETIME NULL
- `UsuarioModificacion` NVARCHAR(50) NULL

### Restricciones
- FK a `Monedas`
- UNIQUE en `Codigo`

---

## 6.5 ListaPrecioDetalles

### Objetivo
Definir precio por producto dentro de una lista.

### Campos
- `ListaPrecioDetalleId` INT IDENTITY PK
- `ListaPrecioId` INT NOT NULL
- `ProductoId` INT NOT NULL
- `Precio` DECIMAL(18,4) NOT NULL
- `FechaVigenciaDesde` DATE NULL
- `FechaVigenciaHasta` DATE NULL
- `Activo` BIT NOT NULL

### Restricciones
- FK a `ListasPrecios`
- FK a `Productos`
- UNIQUE (`ListaPrecioId`, `ProductoId`)

### Nota
Si luego se desea historial real de precios, esta tabla puede evolucionar.

---

# 7. Tablas de terceros

## 7.1 Clientes

### Objetivo
Administrar clientes.

### Campos
- `ClienteId` INT IDENTITY PK
- `Codigo` NVARCHAR(20) NOT NULL
- `RazonSocial` NVARCHAR(150) NOT NULL
- `NombreFantasia` NVARCHAR(150) NULL
- `Documento` NVARCHAR(30) NULL
- `RUC` NVARCHAR(30) NULL
- `Direccion` NVARCHAR(200) NULL
- `Telefono` NVARCHAR(50) NULL
- `Email` NVARCHAR(150) NULL
- `CondicionPagoId` INT NULL
- `ListaPrecioId` INT NULL
- `LimiteCredito` DECIMAL(18,2) NOT NULL
- `Observaciones` NVARCHAR(255) NULL
- `Activo` BIT NOT NULL
- `FechaCreacion` DATETIME NOT NULL
- `UsuarioCreacion` NVARCHAR(50) NOT NULL
- `FechaModificacion` DATETIME NULL
- `UsuarioModificacion` NVARCHAR(50) NULL

### Restricciones
- UNIQUE en `Codigo`
- FK a `CondicionesPago`
- FK a `ListasPrecios`

### Índices
- Índice por `RazonSocial`
- Índice por `Documento`
- Índice por `RUC`

---

## 7.2 Proveedores

### Objetivo
Administrar proveedores.

### Campos
- `ProveedorId` INT IDENTITY PK
- `Codigo` NVARCHAR(20) NOT NULL
- `RazonSocial` NVARCHAR(150) NOT NULL
- `RUC` NVARCHAR(30) NULL
- `Direccion` NVARCHAR(200) NULL
- `Telefono` NVARCHAR(50) NULL
- `Email` NVARCHAR(150) NULL
- `CondicionPagoId` INT NULL
- `Observaciones` NVARCHAR(255) NULL
- `Activo` BIT NOT NULL
- `FechaCreacion` DATETIME NOT NULL
- `UsuarioCreacion` NVARCHAR(50) NOT NULL
- `FechaModificacion` DATETIME NULL
- `UsuarioModificacion` NVARCHAR(50) NULL

### Restricciones
- UNIQUE en `Codigo`
- FK a `CondicionesPago`

### Índices
- Índice por `RazonSocial`
- Índice por `RUC`

---

# 8. Tablas de productos

## 8.1 CategoriasProducto

### Objetivo
Clasificar productos.

### Campos
- `CategoriaProductoId` INT IDENTITY PK
- `Codigo` NVARCHAR(20) NOT NULL
- `Nombre` NVARCHAR(100) NOT NULL
- `Descripcion` NVARCHAR(255) NULL
- `Activo` BIT NOT NULL

### Restricciones
- UNIQUE en `Codigo`
- UNIQUE en `Nombre`

---

## 8.2 Marcas

### Objetivo
Definir marcas.

### Campos
- `MarcaId` INT IDENTITY PK
- `Codigo` NVARCHAR(20) NOT NULL
- `Nombre` NVARCHAR(100) NOT NULL
- `Activo` BIT NOT NULL

### Restricciones
- UNIQUE en `Codigo`
- UNIQUE en `Nombre`

---

## 8.3 UnidadesMedida

### Objetivo
Definir unidades de medida.

### Campos
- `UnidadMedidaId` INT IDENTITY PK
- `Codigo` NVARCHAR(20) NOT NULL
- `Nombre` NVARCHAR(100) NOT NULL
- `Abreviatura` NVARCHAR(10) NOT NULL
- `Activo` BIT NOT NULL

### Restricciones
- UNIQUE en `Codigo`
- UNIQUE en `Abreviatura`

### Ejemplos
- UN
- KG
- LT
- CJ

---

## 8.4 Productos

### Objetivo
Administrar artículos del sistema.

### Campos
- `ProductoId` INT IDENTITY PK
- `Codigo` NVARCHAR(30) NOT NULL
- `CodigoBarras` NVARCHAR(50) NULL
- `Descripcion` NVARCHAR(200) NOT NULL
- `CategoriaProductoId` INT NULL
- `MarcaId` INT NULL
- `UnidadMedidaId` INT NOT NULL
- `CostoUltimo` DECIMAL(18,4) NOT NULL
- `PorcentajeIVA` DECIMAL(5,2) NOT NULL
- (Si en la base existe `PrecioBase`, la aplicación no la usa; precios de venta vía listas de precios.)
- `PermiteStockNegativo` BIT NOT NULL
- `ControlaStock` BIT NOT NULL
- `EsServicio` BIT NOT NULL
- `Observaciones` NVARCHAR(255) NULL
- `Activo` BIT NOT NULL
- `FechaCreacion` DATETIME NOT NULL
- `UsuarioCreacion` NVARCHAR(50) NOT NULL
- `FechaModificacion` DATETIME NULL
- `UsuarioModificacion` NVARCHAR(50) NULL

### Restricciones
- UNIQUE en `Codigo`
- FK a `CategoriasProducto`
- FK a `Marcas`
- FK a `UnidadesMedida`

### Índices
- Índice por `Descripcion`
- Índice por `CodigoBarras`
- Índice por `Activo`

---

# 9. Relaciones principales

## Seguridad
- `Usuarios` 1..N `UsuarioRoles`
- `Roles` 1..N `UsuarioRoles`
- `Roles` 1..N `RolPermisos`
- `Permisos` 1..N `RolPermisos`

## Organización
- `Empresas` 1..N `Sucursales`
- `Sucursales` 1..N `Depositos`

## Configuración comercial
- `Monedas` 1..N `ListasPrecios`
- `Monedas` 1..N `Cotizaciones`
- `CondicionesPago` 1..N `Clientes`
- `CondicionesPago` 1..N `Proveedores`
- `ListasPrecios` 1..N `Clientes`

## Productos
- `CategoriasProducto` 1..N `Productos`
- `Marcas` 1..N `Productos`
- `UnidadesMedida` 1..N `Productos`
- `ListasPrecios` 1..N `ListaPrecioDetalles`
- `Productos` 1..N `ListaPrecioDetalles`

---

# 10. Orden recomendado de creación en SQL Server

1. Empresas
2. Sucursales
3. Depositos
4. CondicionesPago
5. Monedas
6. Cotizaciones
7. ListasPrecios
8. CategoriasProducto
9. Marcas
10. UnidadesMedida
11. Productos
12. ListaPrecioDetalles
13. Clientes
14. Proveedores
15. Usuarios
16. Roles
17. Permisos
18. UsuarioRoles
19. RolPermisos
20. AuditoriaAcceso

---

# 11. Observaciones de diseño para Cursor

Cursor debe respetar estas decisiones:

1. `Productos` debe soportar artículos y servicios.
2. `Clientes` y `Proveedores` se manejan por separado en esta primera etapa.
3. `Usuarios` podrá tener sucursal por defecto.
4. `ListasPrecios` se diseñan simples en Fase 1.
5. `Cotizaciones` queda preparada para multimoneda futura.
6. La auditoría mínima ya debe estar contemplada desde el inicio.
7. No mezclar todavía cuentas corrientes ni stock transaccional en estas tablas.

---

# 12. Entidades VB.NET esperadas en Fase 1

Cursor deberá luego generar clases de entidad para:

- Usuario
- Rol
- Permiso
- UsuarioRol
- RolPermiso
- Empresa
- Sucursal
- Deposito
- Cliente
- Proveedor
- Producto
- CategoriaProducto
- Marca
- UnidadMedida
- ListaPrecio
- ListaPrecioDetalle
- CondicionPago
- Moneda
- Cotizacion

---

# 13. Próximo paso después de este documento

Una vez aprobado este modelo de datos inicial, el siguiente trabajo en Cursor debe ser:

1. generar scripts SQL Server
2. generar entidades VB.NET
3. generar repositorios
4. generar servicios
5. generar formularios CRUD de Fase 1

No avanzar todavía a:
- Stock transaccional
- Compras
- Ventas
- Tesorería
- Cuentas corrientes