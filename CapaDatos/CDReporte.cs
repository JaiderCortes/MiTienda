using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using CapaEntidad;

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
    }
}
