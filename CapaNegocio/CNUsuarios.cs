using System;
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
    }
}
