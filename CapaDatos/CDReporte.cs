using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using CapaEntidad;
using System.Globalization;

namespace CapaDatos
{
    public class CDReporte
    {
        public Dashboard VerDashboard()
        {
            Dashboard dashboard = new Dashboard();

            try
            {
                SqlConnection con = new SqlConnection(Conexion.conexion);
                using (con)
                {
                    SqlCommand cmd = new SqlCommand("SP_ReporteDashboard", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            dashboard = new Dashboard()
                            {
                                CantidadClientes = Convert.ToInt32(rdr["CantidadClientes"]),
                                CantidadVentas = Convert.ToInt32(rdr["CantidadVentas"]),
                                CantidadProductos = Convert.ToInt32(rdr["CantidadProductos"])
                            };
                        }
                    }
                }
            }
            catch (Exception e)
            {
                dashboard = new Dashboard();
                Console.WriteLine($"Ocurrió un error durante la ejecución del procedimiento: {e.Message}");
                throw;
            }

            return dashboard;
        }

        public List<Reporte> Ventas(string fechaInicio, string fechaFin, string idTransaccion)
        {
            List<Reporte> listaReporte = new List<Reporte>();

            try
            {
                SqlConnection con = new SqlConnection(Conexion.conexion);
                using (con)
                {
                    SqlCommand cmd = new SqlCommand("SP_ReporteVentas", con);
                    cmd.Parameters.AddWithValue("FechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("FechaFin", fechaFin);
                    cmd.Parameters.AddWithValue("IdTransaccion", idTransaccion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            listaReporte.Add(
                                new Reporte()
                                {
                                    FechaVenta = rdr["FechaVenta"].ToString(),
                                    Cliente = rdr["Cliente"].ToString(),
                                    Producto = rdr["Producto"].ToString(),
                                    Precio = Convert.ToDecimal(rdr["Precio"], new CultureInfo("es-CO")),
                                    Cantidad = Convert.ToInt32(rdr["Cantidad"]),
                                    Total = Convert.ToInt32(rdr["Total"], new CultureInfo("es-CO")),
                                    IdTransaccion = rdr["IdTransaccion"].ToString()
                                }
                            );
                        }
                    }
                }
            }
            catch (Exception e)
            {
                listaReporte = new List<Reporte>();
                Console.WriteLine($"Ocurrió un error durante la ejecución del procedimiento: {e.Message}");
                throw;
            }

            return listaReporte;
        }
    }
}
