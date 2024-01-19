CREATE PROCEDURE SP_EliminarMarca(
	@Id UNIQUEIDENTIFIER,
	@Mensaje VARCHAR(500) OUTPUT,
	@Resultado BIT OUTPUT
)
AS
BEGIN
	SET @Resultado = 0
	IF NOT EXISTS (SELECT * FROM PRODUCTO p INNER JOIN MARCA m ON m.Id = p.IdMarca WHERE p.IdMarca = @Id)
	BEGIN
		DELETE FROM MARCA WHERE Id = @Id
		SET @Resultado = 1
	END
	ELSE
		SET @Mensaje = 'No se puede eliminar, la marca está relacionada con un producto.'
END