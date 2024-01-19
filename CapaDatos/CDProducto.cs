using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Collections;

namespace CapaDatos
{
    public class CDProducto
    {
        public List<Producto> ListarProductos()
        {
            List<Producto> productos = new List<Producto>();

            try
            {
                SqlConnection con = new SqlConnection(Conexion.conexion);
                using (con)
                {
                    string sql = "SELECT " +
                        "\r\n\tp.Id, " +
                        "\r\n\tp.Nombre, " +
                        "\r\n\tp.Descripcion, " +
                        "\r\n\tm.Id IdMarca, " +
                        "\r\n\tm.Descripcion DescMarca, " +
                        "\r\n\tc.Id IdCategoria, " +
                        "\r\n\tc.Descripcion DescCategoria, " +
                        "\r\n\tp.Precio, " +
                        "\r\n\tp.Stock, " +
                        "\r\n\tp.RutaImagen, " +
                        "\r\n\tp.NombreImagen, " +
                        "\r\n\tp.Activo " +
                        "\r\nFROM PRODUCTO p " +
                        "\r\nINNER JOIN MARCA m on m.Id = p.IdMarca " +
                        "\r\nINNER JOIN CATEGORIA c on c.Id = p.IdCategoria;";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.CommandType = CommandType.Text;

                    con.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Producto producto = new Producto
                            {
                                Id = rdr.GetGuid(rdr.GetOrdinal("Id")),
                                Nombre = rdr["Nombre"].ToString(),
                                Descripcion = rdr["Descripcion"].ToString(),
                                IdMarca = new Marca()
                                {
                                    Id = rdr.GetGuid(rdr.GetOrdinal("IdMarca")),
                                    Descripcion = rdr["DescMarca"].ToString()
                                },
                                IdCategoria = new Categoria()
                                {
                                    Id = rdr.GetGuid(rdr.GetOrdinal("IdCategoria")),
                                    Descripcion = rdr["DescCategoria"].ToString()
                                },
                                Precio = Convert.ToDecimal(rdr["Precio"], new CultureInfo("es-CO")),
                                Stock = Convert.ToInt32(rdr["Stock"], new CultureInfo("es-CO")),
                                RutaImagen = rdr["RutaImagen"].ToString(),
                                NombreImagen = rdr["NombreImagen"].ToString(),
                                Activo = Convert.ToBoolean(rdr["Activo"])
                            };
                            productos.Add(producto);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                productos = new List<Producto>();
                Console.WriteLine($"Ocurrió un error durante la ejecución del procedimiento: {e.Message}");
                throw;
            }

            return productos;
        }

        public Guid Registrar(Producto obj, out string Mensaje)
        {
            string id = "";
            Guid nuevoId;
            Mensaje = string.Empty;
            try
            {
                SqlConnection con = new SqlConnection(Conexion.conexion);
                using (con)
                {
                    SqlCommand cmd = new SqlCommand("SP_RegistrarProducto", con);
                    cmd.Parameters.AddWithValue("IdMarca", obj.IdMarca.Id);
                    cmd.Parameters.AddWithValue("IdCategoria", obj.IdCategoria.Id);
                    cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("Precio", obj.Precio);
                    cmd.Parameters.AddWithValue("Stock", obj.Stock);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    cmd.Parameters.Add("Id", SqlDbType.UniqueIdentifier).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    cmd.ExecuteNonQuery();

                    id = Convert.ToString(cmd.Parameters["Id"].Value);
                    nuevoId = new Guid(id);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception e)
            {
                id = "";
                Mensaje = e.Message;
                Console.WriteLine($"Ocurrió un error durante la ejecución del procedimiento: {e.Message}");
                throw;
            }
            return nuevoId;
        }

        public bool Editar(Producto obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                SqlConnection con = new SqlConnection(Conexion.conexion);
                using (con)
                {
                    SqlCommand cmd = new SqlCommand("SP_EditarProducto", con);
                    cmd.Parameters.AddWithValue("Id", obj.Id);
                    cmd.Parameters.AddWithValue("IdMarca", obj.IdMarca.Id);
                    cmd.Parameters.AddWithValue("IdCategoria", obj.IdCategoria.Id);
                    cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("Precio", obj.Precio);
                    cmd.Parameters.AddWithValue("Stock", obj.Stock);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception e)
            {
                resultado = false;
                Mensaje = e.Message;
                Console.WriteLine($"Ocurrió un error durante la ejecución del procedimiento: {e.Message}");
                throw;
            }
            return resultado;
        }

        public bool Eliminar(Guid id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                SqlConnection con = new SqlConnection(Conexion.conexion);
                using (con)
                {
                    SqlCommand cmd = new SqlCommand("SP_EliminarProducto", con);
                    cmd.Parameters.AddWithValue("Id", id);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception e)
            {
                resultado = false;
                Mensaje = e.Message;
                Console.WriteLine($"Ocurrió un error durante la ejecución del procedimiento: {e.Message}");
                throw;
            }
            return resultado;
        }

        public bool GuardarImagen(Producto obj, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            try
            {
                SqlConnection con = new SqlConnection(Conexion.conexion);
                using (con)
                {
                    string sql = "UPDATE PRODUCTO SET RutaImagen = @RutaImagen, NombreImagen = @NombreImagen " +
                        "WHERE Id = @Id;";

                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@RutaImagen", obj.RutaImagen);
                    cmd.Parameters.AddWithValue("@NombreImagen", obj.NombreImagen);
                    cmd.Parameters.AddWithValue("@Id", obj.Id);

                    con.Open();
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        resultado = true;
                    }
                    else
                    {
                        mensaje = "No se pudo actualizar la imagen.";
                    }
                }
            }
            catch (Exception e)
            {
                resultado = false;
                mensaje = e.Message;
                Console.WriteLine($"Ocurrió un error durante la ejecución del procedimiento: {e.Message}");
                throw;
            }
            return resultado;
        }
    }
}
