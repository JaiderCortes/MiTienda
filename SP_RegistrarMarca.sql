CREATE PROCEDURE SP_RegistrarMarca(
	@Descripcion VARCHAR(150),
	@Activo BIT,
	@Mensaje VARCHAR(500) OUTPUT,
	@Id UNIQUEIDENTIFIER OUTPUT
)
AS
BEGIN
	SET @Id = NEWID()
	IF NOT EXISTS (SELECT * FROM MARCA WHERE Descripcion = @Descripcion)
	BEGIN
		INSERT INTO MARCA(Id, Descripcion, Activo)
		VALUES(@Id, @Descripcion, @Activo)
	END
	ELSE
		SET @Mensaje = 'Esta marca ya existe.'
END