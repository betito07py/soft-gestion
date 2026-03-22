# Guía rápida para usar Git en Soft_Gestion

## 1. Ubicate en la carpeta raíz del proyecto
```bash
cd ruta\a\Soft_Gestion
```

## 2. Inicializá Git
```bash
git init
```

## 3. Agregá archivos
```bash
git add .
```

## 4. Hacé el primer commit
```bash
git commit -m "Inicial - Base del sistema Soft_Gestion"
```

## 5. Creá repositorio en GitHub
Nombre sugerido:
- `soft-gestion`

## 6. Vinculá el remoto
```bash
git remote add origin https://github.com/TU_USUARIO/soft-gestion.git
git branch -M main
git push -u origin main
```

## 7. Estrategia recomendada de ramas
- `main` -> producción / estable
- `dev` -> integración
- `feature/...` -> trabajo puntual

## 8. Ejemplos de ramas
```bash
git checkout -b feature/seguridad-usuarios
git checkout -b feature/maestros-clientes
git checkout -b feature/productos-grupoid
git checkout -b feature/stock-modelo
```

## 9. Ejemplos de commits buenos
```bash
git commit -m "Seguridad - Usuarios implementado"
git commit -m "Seguridad - Roles y permisos implementado"
git commit -m "Maestros - Clientes implementado"
git commit -m "Maestros - Productos migrado a GrupoId"
git commit -m "UnidadesMedida - Integracion con SIFEN"
```

## 10. Qué sí versionar
- solución `.sln`
- proyectos `.vbproj`
- código `.vb`
- scripts SQL
- documentación `.md`

## 11. Qué no versionar
- `.vs/`
- `bin/`
- `obj/`
- `*.user`
- `*.suo`
- `*.exe.config`
