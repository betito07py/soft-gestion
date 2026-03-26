# Visión General del Sistema ERP

## 1. Objetivo

Desarrollar un sistema ERP desktop para gestión empresarial, orientado a pequeñas y medianas empresas, con módulos integrados de:

- Stock
- Compras
- Ventas
- Cuentas a cobrar
- Cuentas a pagar
- Tesorería

El sistema debe permitir una operación diaria ordenada, con trazabilidad, control de permisos, auditoría y base preparada para crecer a futuro.

---

## 2. Tipo de solución

- Aplicación de escritorio
- Plataforma: WinForms
- Lenguaje: VB.NET
- Base de datos: SQL Server
- Arquitectura: por capas

---

## 3. Objetivo funcional

Centralizar en un solo sistema las operaciones comerciales y administrativas, evitando:

- duplicación de datos
- errores manuales
- falta de trazabilidad
- inconsistencias entre stock, ventas, compras y tesorería

---

## 4. Objetivo técnico

Construir una solución:

- modular
- mantenible
- auditable
- transaccional
- escalable

Debe estar preparada para que Cursor pueda asistir en la generación de código de manera ordenada, por fases y sin mezclar responsabilidades.

---

## 5. Usuarios del sistema

Perfiles previstos:

- Administrador del sistema
- Operador de ventas
- Operador de compras
- Operador de stock
- Usuario de tesorería
- Gerencia / consulta

---

## 6. Alcance inicial

La primera etapa del proyecto contempla únicamente la base del sistema:

- seguridad
- usuarios
- roles y permisos
- empresas
- sucursales
- depósitos
- clientes
- proveedores
- productos
- categorías
- marcas
- unidades de medida
- listas de precios
- condiciones de pago
- monedas

Los módulos transaccionales se desarrollarán en fases posteriores.

---

## 7. Módulos previstos del sistema

### Maestros
- Empresas
- Sucursales
- Depósitos
- Clientes
- Proveedores
- Productos
- Categorías
- Marcas
- Unidades de medida
- Monedas
- Condiciones de pago
- Listas de precios

### Seguridad
- Login
- Usuarios
- Roles
- Permisos
- Auditoría de acceso

### Operaciones futuras
- Stock
- Compras
- Ventas
- Cuentas a cobrar
- Cuentas a pagar
- Tesorería
- Reportes

---

## 8. Principio general de integración

El sistema debe diseñarse desde el inicio para que una misma operación pueda impactar en varios módulos.

Ejemplos:

- una compra puede impactar stock y cuentas a pagar
- una venta puede impactar stock y cuentas a cobrar
- un cobro puede impactar cuentas a cobrar y tesorería
- un pago puede impactar cuentas a pagar y tesorería

En Fase 1 no se implementan todavía esas operaciones, pero el diseño debe dejarlas previstas.

---

## 9. Alcance técnico del desarrollo con Cursor

Cursor debe utilizar esta documentación como base para:

- generar scripts SQL Server
- generar entidades VB.NET
- generar repositorios
- generar servicios de negocio
- generar formularios WinForms CRUD
- respetar la arquitectura por capas
- trabajar por fases y no por implementación total

---

## 10. Resultado esperado

Al finalizar el proyecto, el sistema debe permitir una administración integrada, confiable y extensible, con una base sólida para crecer a reportes, finanzas avanzadas y contabilidad integrada.
