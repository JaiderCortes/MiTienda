using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacionAdmin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Usuarios()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarUsuarios()
        {
            List<Usuario> usuarios;
            usuarios = new CNUsuarios().ListarUsuarios();
            return Json(new { data = usuarios }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarUsuario(Usuario usuario)
        {
            object result;
            string mensaje = string.Empty;

            if (usuario.Id == Guid.Empty)
            {
                result = new CNUsuarios().Registrar(usuario, out mensaje);
            }
            else
            {
                result = new CNUsuarios().Editar(usuario, out mensaje);

            }
            return Json(new { result = result, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
    }
}