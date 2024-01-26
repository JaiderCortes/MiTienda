using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CNReporte
    {
        private CDReporte cdr = new CDReporte();
        public Dashboard VerDashboard()
        {
            return cdr.VerDashboard();
        }
    }
}
