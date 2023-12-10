using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Ciudad
    {
        public Guid Id { get; set; }
        public Departamento IdDepartamento { get; set; }
        public string Nombre { get; set; }
    }
}
