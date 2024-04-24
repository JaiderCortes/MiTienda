using DocumentFormat.OpenXml.Office2016.Presentation.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;
using DocumentFormat.OpenXml.Bibliography;

namespace CapaPresentacionAdmin.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CambiarClave()
        {
            return View();
        }

        public ActionResult ReestablecerClave()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            List<Usuario> usuarios = new List<Usuario>();
            usuarios = new CNUsuarios().ListarUsuarios();
            Usuario usuario = new Usuario();
            string claveDecrypt = CNRecursos.EncriptarSha256(clave);
            usuario = new CNUsuarios().ListarUsuarios().Where(u => u.Correo == correo && u.Clave == claveDecrypt).FirstOrDefault();

            if (usuario == null)
            {
                ViewBag.Error = "Usuario o contraseña incorrectos.";
                return View();
            }
            else
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Home");
            }
        }
    }
}