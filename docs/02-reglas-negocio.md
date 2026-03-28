# Reglas de Negocio Globales

## 1. Principios generales

1. El sistema debe operar por módulos integrados.
2. Toda operación importante debe ser trazable.
3. Toda operación sensible debe ser auditable.
4. No se deben borrar físicamente registros críticos una vez confirmados.
5. El diseño debe priorizar consistencia de datos antes que velocidad de carga manual.

---

## 2. Reglas de seguridad

1. Todo usuario debe ingresar con credenciales válidas.
2. Un usuario inactivo no puede acceder al sistema.
3. Los permisos deben definirse por rol.
4. Un usuario puede tener uno o más roles.
5. El menú principal debe mostrar solo opciones autorizadas.
6. Toda autenticación debe poder registrarse en auditoría de acceso.
7. Debe existir posibilidad de usuario administrador con privilegios amplios.

---

## 3. Reglas de auditoría

1. Toda tabla principal debe registrar al menos:
   - FechaCreacion
   - UsuarioCreacion
   - FechaModificacion
   - UsuarioModificacion
   - Activo

2. Toda operación crítica futura debe registrar:
   - usuario
   - fecha
   - hora
   - equipo o terminal

3. Las anulaciones deben registrarse; no deben reemplazarse por borrado directo.

---

## 4. Reglas organizativas

1. El sistema debe soportar una o varias empresas.
2. Cada empresa puede tener una o varias sucursales.
3. Cada sucursal puede tener uno o varios depósitos.
4. Un usuario puede tener sucursal por defecto.
5. El stock debe controlarse por depósito.

---

## 5. Reglas de clientes y proveedores

1. Los clientes y proveedores se administran por separado en esta primera etapa.
2. Cada cliente puede tener:
   - condición de pago
   - lista de precios
   - límite de crédito

3. Cada proveedor puede tener:
   - condición de pago
   - datos de contacto
   - observaciones

4. No deben duplicarse códigos.
5. Deben poder inactivarse sin necesidad de borrar registros.

---

## 6. Reglas de productos

1. Cada producto debe tener código único.
2. El sistema debe soportar:
   - productos físicos
   - servicios

3. Un producto puede:
   - controlar stock
   - no controlar stock
   - permitir o no stock negativo

4. Cada producto debe poder asociarse a:
   - categoría
   - marca
   - unidad de medida

5. Debe existir costo último; los precios de venta se definen por listas de precios.
6. Debe quedar prevista la gestión de IVA por producto.

---

## 7. Reglas de listas de precios y monedas

1. El sistema debe soportar varias listas de precios.
2. Una lista de precios debe estar asociada a una moneda.
3. Un producto puede tener precio por lista.
4. El sistema debe soportar moneda base y monedas secundarias.
5. La cotización debe poder registrarse por fecha.

---

## 8. Reglas futuras de operaciones transaccionales

Estas reglas no se implementan todavía en Fase 1, pero deben guiar el diseño:

1. Una compra puede impactar stock y cuentas a pagar.
2. Una venta puede impactar stock y cuentas a cobrar o tesorería.
3. Un cobro cancela parcial o totalmente una cuenta a cobrar.
4. Un pago cancela parcial o totalmente una cuenta a pagar.
5. No debe permitirse stock negativo, salvo permiso explícito.
6. Toda operación crítica debe ejecutarse mediante transacción.

---

## 9. Reglas para desarrollo con Cursor

1. Cursor debe trabajar únicamente en la fase actual.
2. Cursor no debe adelantar módulos no definidos.
3. Cursor no debe mezclar UI con acceso a datos.
4. Cursor no debe poner SQL directamente dentro de formularios salvo casos mínimos y justificados.
5. Cursor debe generar:
   - entidades en VB.NET
   - repositorios
   - servicios
   - formularios WinForms

6. Cursor debe respetar exactamente los nombres y estructuras definidas en la documentación.
