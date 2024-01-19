CREATE PROCEDURE SP_EditarProducto(
	@Id UNIQUEIDENTIFIER,
	@IdMarca UNIQUEIDENTIFIER,
	@IdCategoria UNIQUEIDENTIFIER,
	@Nombre VARCHAR(500),
	@Descripcion VARCHAR(500),
	@Precio DECIMAL(10, 2),
	@Stock INT,
	@Activo BIT,
	@Mensaje VARCHAR(500) OUTPUT,
	@Resultado BIT OUTPUT
)
AS
BEGIN
	SET @Resultado = 0
	IF NOT EXISTS (SELECT * FROM PRODUCTO WHERE Nombre = Nombre AND Id != @Id)
	BEGIN
		UPDATE PRODUCTO SET
		IdMarca = @IdMarca,
		IdCategoria = @IdCategoria,
		Nombre = @Nombre,
		Descripcion = @Descripcion,
		Precio = @Precio,
		Stock = @Stock,
		Activo = @Activo
		WHERE Id = @Id

		SET @Resultado = 1
	END
	ELSE
		SET @Mensaje = 'No se pudo actualizar el producto, ya existe este producto.'
END