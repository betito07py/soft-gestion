/*
  Soft_Gestion — Datos iniciales: Empresa, Sucursal y Usuario administrador.
  Ejecutar después de 01_Fase1_Tablas_Maestras.sql.

  PASO 1: Generar el hash de la contraseña:
    cd tools\GenerarHashPassword\bin\Debug
    GenerarHashPassword.exe TuContraseña
  (o ejecutar sin argumentos para que pida la contraseña por consola)

  PASO 2: En la línea siguiente, sustituir SOLO el texto PEGAR_HASH_AQUI (entre comillas)
         por el hash completo que imprime la consola, sin espacios de más.
         Ejemplo correcto:
           N'100000$CV+mW756EWfzh//cFqkurg==$0UdpDbiY7NjhaVuzOql4/...'
         Ejemplo incorrecto: dejar N'PEGAR_HASH_AQUI' o pegar el hash fuera de las comillas.
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;

-- Una sola línea: el valor entre comillas simples tras N debe ser el hash entero.
DECLARE @PasswordHash NVARCHAR(255) = N'100000$tEn1/wwjcuTlOeH+uzyIiQ==$zdJn7Q41kP3/y3pVHKJb9PnGWNRDhy4eAZ2S4PxkwoU=';

-- Solo dispara error si aún está el marcador (no confundir con la línea de arriba).
IF @PasswordHash = N'PEGAR_HASH_AQUI'
BEGIN
    RAISERROR(N'Edita el script: reemplaza PEGAR_HASH_AQUI (dentro de las comillas) por el hash devuelto por GenerarHashPassword.exe.', 16, 1);
    RETURN;
END

IF NOT EXISTS (SELECT 1 FROM dbo.Empresas WHERE Codigo = '001')
BEGIN
    DECLARE @EmpresaId INT;
    DECLARE @SucursalId INT;

    BEGIN TRANSACTION;

    INSERT INTO dbo.Empresas (Codigo, RazonSocial, NombreFantasia, RUC, Direccion, Telefono, Email, Activo, UsuarioCreacion)
    VALUES (
        N'001',
        N'Empresa Ejemplo S.A.',
        N'Empresa Ejemplo S.A.',
        N'12345678-1',
        N'Villarica',
        N'0984731072',
        NULL,
        1,
        N'SEED'
    );
    SET @EmpresaId = SCOPE_IDENTITY();

    INSERT INTO dbo.Sucursales (EmpresaId, Codigo, Nombre, Direccion, Telefono, Responsable, Activo, UsuarioCreacion)
    VALUES (
        @EmpresaId,
        '001',
        'Sucursal Central',
        NULL,
        NULL,
        NULL,
        1,
        'SEED'
    );
    SET @SucursalId = SCOPE_IDENTITY();

    INSERT INTO dbo.Usuarios (Login, NombreCompleto, PasswordHash, Email, EsAdministrador, SucursalId, Activo, UsuarioCreacion)
    VALUES (
        'admin',
        'Administrador',
        @PasswordHash,
        NULL,
        1,
        @SucursalId,
        1,
        'SEED'
    );

    COMMIT TRANSACTION;

    PRINT N'Datos iniciales insertados correctamente (Empresa 001, Sucursal 001, Usuario admin).';
END
ELSE
BEGIN
    PRINT N'Ya existen datos iniciales (Empresa 001). No se insertó nada.';
END
GO
