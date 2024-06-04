CREATE PROCEDURE SP_RegistrarCliente(
	@Nombres VARCHAR(100),
	@Apellidos VARCHAR(100),
	@Correo VARCHAR(100),
	@Clave VARCHAR(100),
	@Mensaje VARCHAR(500) OUTPUT,
	@Id UNIQUEIDENTIFIER OUTPUT
)
AS
BEGIN
	SET @Id = NEWID()
	IF NOT EXISTS (SELECT * FROM CLIENTE WHERE Correo = @Correo)
	BEGIN
		INSERT INTO CLIENTE(Id, Nombres, Apellidos, Correo, Clave, Reestablecer)
		VALUES(@Id, @Nombres, @Apellidos, @Correo, @Clave, 0)
	END
	ELSE
		SET @Mensaje = 'Ya existe un cliente con ese correo.'
END