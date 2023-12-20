CREATE PROCEDURE SP_RegistrarUsuario(
	@Nombres VARCHAR(100),
	@Apellidos VARCHAR(100),
	@Correo VARCHAR(100),
	@Clave VARCHAR(100),
	@Activo BIT,
	@Mensaje VARCHAR(500) OUTPUT,
	@Id UNIQUEIDENTIFIER OUTPUT
)
AS
BEGIN
	SET @Id = NEWID()
	IF NOT EXISTS (SELECT * FROM USUARIO WHERE Correo = @Correo)
	BEGIN
		INSERT INTO USUARIO(Id, Nombres, Apellidos, Correo, Clave, Activo)
		VALUES(@Id, @Nombres, @Apellidos, @Correo, @Clave, @Activo)
	END
	ELSE
		SET @Mensaje = 'Ya existe un usuario con ese correo.'
END