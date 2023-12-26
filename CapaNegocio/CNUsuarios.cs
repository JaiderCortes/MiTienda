﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CNUsuarios
    {
        private CDUsuarios cdu = new CDUsuarios();
        public List<Usuario> ListarUsuarios()
        {
            return cdu.ListarUsuarios();
        }

        public Guid? Registrar(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                Mensaje = "El nombre del usuario no puede ser vacío.";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "El apellido del usuario no puede ser vacío.";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El correo del usuario no puede ser vacío.";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                //Envío del correo con la contraseña del nuevo usuario
                string clave = CNRecursos.GenerarClave();
                string asunto = "Activación de cuenta - Mi Tienda";
                string mensaje = "<h3>¡Su cuenta ha sido creada exitosamente!</h3><br>" +
                    "<p>Bienvenido al sistema Mi Tienda.</p><br>" +
                    $"<h4>Su contraseña para acceder al sistema es: {clave}</h4>";
                bool respuesta = CNRecursos.EnviarCorreo(obj.Correo, asunto, mensaje);
                if (respuesta)
                {
                    obj.Clave = CNRecursos.EncriptarSha256(clave);
                    return cdu.Registrar(obj, out Mensaje);
                }
                else
                {
                    Mensaje = "No se pudo enviar el correo";
                    return null;
                }

            }
            else
            {
                return null;
            }
        }

        public bool Editar(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                Mensaje = "El nombre del usuario no puede ser vacío.";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "El apellido del usuario no puede ser vacío.";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El correo del usuario no puede ser vacío.";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return cdu.Editar(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool Eliminar(Guid id, out string Mensaje)
        {
            return cdu.Eliminar(id, out Mensaje);
        }
    }
}
