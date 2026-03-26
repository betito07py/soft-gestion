# Reglas de Clasificación de Productos - Soft_Gestion

## 1. Estructura

La clasificación de productos se maneja con una jerarquía de 3 niveles:

- Categoría
- Subcategoría
- Grupo

Relaciones:

- Subcategoría pertenece a Categoría
- Grupo pertenece a Subcategoría

---

## 2. Codificación

Todos los códigos deben cumplir:

- Tipo: INT
- Sin ceros a la izquierda
- Estructura jerárquica

Ejemplo:

- Categoría: 2 (Bebidas)
- Subcategoría: 201 (Bebidas sin alcohol)
- Grupo: 20101 (Gaseosas)

---

## 3. Reglas de negocio

- No permitir códigos duplicados
- No permitir nombres duplicados dentro del mismo nivel padre
- No permitir eliminar registros (solo desactivar)
- No cambiar códigos si existen productos asociados

---

## 4. Bebidas

Separación obligatoria:

- 201 → Bebidas sin alcohol
- 202 → Bebidas con alcohol

---

## 5. Tipos de datos

- Categorias.Codigo → INT
- SubCategorias.Codigo → INT
- Grupos.Codigo → INT

---

## 6. Uso en código

Todas las consultas deben:

- usar JOINs para obtener jerarquía
- no duplicar nombres en tablas de productos