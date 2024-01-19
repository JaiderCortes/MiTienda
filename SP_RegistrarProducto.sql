CREATE PROCEDURE SP_RegistrarProducto(
	@IdMarca UNIQUEIDENTIFIER,
	@IdCategoria UNIQUEIDENTIFIER,
	@Nombre VARCHAR(500),
	@Descripcion VARCHAR(500),
	@Precio DECIMAL(10, 2),
	@Stock INT,
	@Activo BIT,
	@Mensaje VARCHAR(500) OUTPUT,
	@Id UNIQUEIDENTIFIER OUTPUT
)
AS
BEGIN
	SET @Id = NEWID()
	IF NOT EXISTS (SELECT * FROM PRODUCTO WHERE Nombre = @Nombre)
	BEGIN
		INSERT INTO PRODUCTO(Id, IdMarca, IdCategoria, Nombre, Descripcion, Precio, Stock, Activo)
		VALUES(@Id, @IdMarca, @IdCategoria, @Nombre, @Descripcion, @Precio, @Stock, @Activo)
	END
	ELSE
		SET @Mensaje = 'Este producto ya existe.'
END