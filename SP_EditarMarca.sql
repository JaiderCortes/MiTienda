CREATE PROCEDURE SP_EditarMarca(
	@Id UNIQUEIDENTIFIER,
	@Descripcion VARCHAR(150),
	@Activo BIT,
	@Mensaje VARCHAR(500) OUTPUT,
	@Resultado BIT OUTPUT
)
AS
BEGIN
	SET @Resultado = 0
	IF NOT EXISTS (SELECT * FROM MARCA WHERE Descripcion = @Descripcion AND Id != @Id)
	BEGIN
		UPDATE MARCA SET
		Descripcion = @Descripcion, 
		Activo = @Activo
		WHERE Id = @Id

		SET @Resultado = 1
	END
	ELSE
		SET @Mensaje = 'No se pudo actualizar la marca, ya existe esta marca.'
END