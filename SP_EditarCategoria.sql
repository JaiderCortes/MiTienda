CREATE PROCEDURE SP_EditarCategoria(
	@Id UNIQUEIDENTIFIER,
	@Descripcion VARCHAR(150),
	@Activo BIT,
	@Mensaje VARCHAR(500) OUTPUT,
	@Resultado BIT OUTPUT
)
AS
BEGIN
	SET @Resultado = 0
	IF NOT EXISTS (SELECT * FROM CATEGORIA WHERE Descripcion = @Descripcion AND Id != @Id)
	BEGIN
		UPDATE CATEGORIA SET
		Descripcion = @Descripcion, 
		Activo = @Activo
		WHERE Id = @Id

		SET @Resultado = 1
	END
	ELSE
		SET @Mensaje = 'No se pudo actualizar la categoría, ya existe esta categoría.'
END