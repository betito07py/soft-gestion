# Convenciones de Código

## 1. Stack confirmado

- Lenguaje: VB.NET
- UI: WinForms
- Base de datos: SQL Server
- Arquitectura: por capas

---

## 2. Estructura de proyectos

La solución debe separarse en:

- ERP.Domain
- ERP.Data
- ERP.Business
- ERP.Common
- ERP.UI

### ERP.Domain
Contiene:
- entidades
- enums
- contratos simples
- modelos del negocio

### ERP.Data
Contiene:
- acceso a SQL Server
- repositorios
- ejecución de stored procedures
- mapeo de datos

### ERP.Business
Contiene:
- servicios de negocio
- validaciones
- coordinación de operaciones

### ERP.Common
Contiene:
- utilidades
- configuración
- helpers
- clases compartidas

### ERP.UI
Contiene:
- formularios WinForms
- navegación
- binding
- interacción con usuario

---

## 3. Reglas obligatorias

1. No mezclar UI con lógica de negocio.
2. No escribir SQL directamente en formularios.
3. Usar repositorios para acceso a datos.
4. Usar servicios para lógica de negocio.
5. Separar documentos de cabecera y detalle cuando existan módulos transaccionales.
6. Usar nombres consistentes y descriptivos.
7. No duplicar lógica entre formularios.
8. Priorizar claridad, mantenibilidad y trazabilidad.

---

## 4. Convenciones de nombres

### Tablas
Usar nombres en plural y claros.

Ejemplos:
- Usuarios
- Roles
- Permisos
- Empresas
- Sucursales
- Depositos
- Clientes
- Proveedores
- Productos
- ComprasCabecera
- ComprasDetalle
- VentasCabecera
- VentasDetalle
- StockMovimientos

### Stored Procedures
Formato:
`usp_Entidad_Accion`

Ejemplos:
- `usp_Usuario_Login`
- `usp_Usuario_Guardar`
- `usp_Producto_Guardar`
- `usp_Producto_Listar`
- `usp_Venta_Confirmar`

### Formularios
Prefijo:
`Frm`

Ejemplos:
- `FrmLogin`
- `FrmPrincipal`
- `FrmUsuarios`
- `FrmProductos`
- `FrmClientes`

### Servicios
Sufijo:
`Service`

Ejemplos:
- `UsuarioService`
- `ProductoService`
- `ClienteService`

### Repositorios
Sufijo:
`Repository`

Ejemplos:
- `UsuarioRepository`
- `ProductoRepository`
- `ClienteRepository`

### Clases de entidad
Nombre singular.

Ejemplos:
- `Usuario`
- `Rol`
- `Permiso`
- `Empresa`
- `Sucursal`
- `Deposito`
- `Producto`

---

## 5. Convenciones de código VB.NET

1. `Option Strict On`
2. `Option Explicit On`
3. `Option Infer On`
4. Evitar conversiones implícitas peligrosas.
5. Usar `Using` para conexiones, comandos y readers.
6. Capturar excepciones donde corresponda y propagar con contexto.
7. Evitar lógica excesiva en eventos de formularios.
8. Mover reglas de negocio a la capa Business.

---

## 6. Convenciones para acceso a datos

1. Centralizar cadena de conexión.
2. Preferir Stored Procedures para operaciones críticas.
3. Usar parámetros SQL tipados.
4. No concatenar SQL con datos del usuario.
5. Manejar transacciones en operaciones críticas.
6. Crear métodos claros:
   - Guardar
   - ObtenerPorId
   - Listar
   - EliminarLogico
   - Activar
   - Desactivar

---

## 7. Convenciones de UI WinForms

1. Un formulario por responsabilidad principal.
2. No hacer formularios gigantes con toda la lógica adentro.
3. Separar acciones de:
   - nuevo
   - editar
   - guardar
   - cancelar
   - buscar

4. Validar antes de guardar.
5. Mostrar mensajes claros al usuario.
6. Respetar permisos al abrir formularios y habilitar botones.

---

## 8. Convenciones de auditoría

En tablas principales usar al menos:
- FechaCreacion
- UsuarioCreacion
- FechaModificacion
- UsuarioModificacion
- Activo

No eliminar físicamente registros críticos.

---

## 9. Reglas para Cursor

Cursor debe:

- respetar la arquitectura por capas
- generar código VB.NET coherente con WinForms
- generar clases simples, legibles y mantenibles
- trabajar por fases
- seguir exactamente la documentación de `/docs`

Cursor no debe:

- inventar estructuras no documentadas
- mezclar varias fases en una sola entrega
- generar todo el ERP de una vez
- meter lógica SQL dentro de la UI
