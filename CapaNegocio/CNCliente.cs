using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CNCliente
    {
        private readonly CDCliente cdc = new CDCliente();

        public List<Cliente> ListarClientes()
        {
            return cdc.ListarClientes();
        }

        public Guid? Registrar(Cliente obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                Mensaje = "El nombre del cliente no puede ser vacío.";
            }
            else if(string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "El apellido del cliente no puede ser vacío.";
            }
            else if(string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El correo del cliente no puede ser vacío.";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                obj.Clave = CNRecursos.EncriptarSha256(obj.Clave);
                return cdc.Registrar(obj, out Mensaje);
            }
            else
            {
                return null;
            }
        }

        public bool CambiarClave(Guid id, string nuevaClave, out string Mensaje)
        {
            return cdc.CambiarClave(id, nuevaClave, out Mensaje);
        }

        public bool ReestablecerClave(Guid idCliente, string correoCliente, out string Mensaje)
        {
            Mensaje = string.Empty;
            string nuevaClave = CNRecursos.GenerarClave();
            bool resultado = cdc.ReestablecerClave(idCliente, CNRecursos.EncriptarSha256(nuevaClave), out Mensaje);
            if (resultado)
            {
                //Envío del correo con la nueva contraseña del cliente
                string asunto = "Reestablecimiento de contraseña clientes - Mi Tienda";
                string mensaje = "<h3>Usted acaba de recibir este correo porque ha solicitado el reestablecimiento de su contraseña.</h3><br>" +
                    "<p>A continuación enviamos su nueva contraseña para acceder al sistema.</p><br>" +
                    $"<h4>Nueva contraseña: {nuevaClave}</h4>";
                bool respuesta = CNRecursos.EnviarCorreo(correoCliente, asunto, mensaje);
                if (respuesta)
                {
                    return true;
                }
                else
                {
                    Mensaje = "No se puedo enviar el correo.";
                    return false;
                }
            }
            else
            {
                Mensaje = "No se pudo reestablecer la contraseña.";
                return false;
            }
        }
    }
}
