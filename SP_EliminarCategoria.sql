CREATE PROCEDURE SP_EliminarCategoria(
	@Id UNIQUEIDENTIFIER,
	@Mensaje VARCHAR(500) OUTPUT,
	@Resultado BIT OUTPUT
)
AS
BEGIN
	SET @Resultado = 0
	IF NOT EXISTS (SELECT * FROM PRODUCTO p INNER JOIN CATEGORIA c ON c.Id = p.IdCategoria WHERE p.IdCategoria = @Id)
	BEGIN
		DELETE FROM CATEGORIA WHERE Id = @Id
		SET @Resultado = 1
	END
	ELSE
		SET @Mensaje = 'No se puede eliminar, la categoría está relacionada con un producto.'
END