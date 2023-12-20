CREATE PROCEDURE SP_EditarUsuario(
	@Id UNIQUEIDENTIFIER,
	@Nombres VARCHAR(100),
	@Apellidos VARCHAR(100),
	@Correo VARCHAR(100),
	@Activo BIT,
	@Mensaje VARCHAR(500) OUTPUT,
	@Resultado BIT OUTPUT
)
AS
BEGIN
	SET @Resultado = 0
	IF NOT EXISTS (SELECT * FROM USUARIO WHERE Correo = @Correo AND Id != @Id)
	BEGIN
		UPDATE USUARIO SET
		Nombres = @Nombres, 
		Apellidos=  @Apellidos, 
		Correo = @Correo, 
		Activo = @Activo
		WHERE Id = @Id

		SET @Resultado = 1
	END
	ELSE
		SET @Mensaje = 'No se pudo actualizar el usuario, ya existe un usuario con ese correo.'
END