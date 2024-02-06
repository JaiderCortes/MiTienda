CREATE PROCEDURE SP_ReporteVentas(
	@FechaInicio VARCHAR(10),
	@FechaFin VARCHAR(10),
	@IdTransaccion VARCHAR(50)
)
AS
BEGIN

SET DATEFORMAT dmy;

SELECT
	CONVERT(CHAR(10), v.FechaVenta, 103) FechaVenta,
	CONCAT(c.Nombres, ' ', c.Apellidos) Cliente,
	p.Nombre Producto,
	p.Precio,
	dv.Cantidad,
	dv.Total,
	v.IdTransaccion
FROM DETALLE_VENTA dv
INNER JOIN PRODUCTO p ON p.Id = dv.IdProducto
INNER JOIN VENTA v ON v.Id = dv.IdVenta
INNER JOIN CLIENTE c ON c.Id = v.IdCliente
WHERE CONVERT(DATE, V.FechaVenta) BETWEEN @FechaInicio AND @FechaFin
AND v.IdTransaccion = IIF(@IdTransaccion = '', v.IdTransaccion, @IdTransaccion)

END