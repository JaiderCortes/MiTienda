CREATE PROCEDURE SP_EliminarProducto(
	@Id UNIQUEIDENTIFIER,
	@Mensaje VARCHAR(500) OUTPUT,
	@Resultado BIT OUTPUT
)
AS
BEGIN
	SET @Resultado = 0
	IF NOT EXISTS (SELECT * FROM DETALLE_VENTA dv INNER JOIN PRODUCTO p ON p.Id = dv.IdProducto WHERE p.Id = @Id)
	BEGIN
		DELETE FROM PRODUCTO WHERE Id = @Id
		SET @Resultado = 1
	END
	ELSE
		SET @Mensaje = 'No se puede eliminar, el producto tiene movimientos en compras.'
END