CREATE PROCEDURE SP_RegistrarCategoria(
	@Descripcion VARCHAR(150),
	@Activo BIT,
	@Mensaje VARCHAR(500) OUTPUT,
	@Id UNIQUEIDENTIFIER OUTPUT
)
AS
BEGIN
	SET @Id = NEWID()
	IF NOT EXISTS (SELECT * FROM CATEGORIA WHERE Descripcion = @Descripcion)
	BEGIN
		INSERT INTO CATEGORIA(Id, Descripcion, Activo)
		VALUES(@Id, @Descripcion, @Activo)
	END
	ELSE
		SET @Mensaje = 'Esta categoría ya existe.'
END