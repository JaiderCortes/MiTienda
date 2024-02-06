using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;
using ClosedXML.Excel;

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

        [HttpPost]
        public JsonResult EliminarUsuario(Guid idUsuario)
        {
            bool result = false;
            string mensaje = string.Empty;

            result = new CNUsuarios().Eliminar(idUsuario, out mensaje);

            return Json(new { result = result, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult VerDashboard()
        {
            Dashboard dashboard = new CNReporte().VerDashboard();
            return Json(new { result = dashboard }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListaReporte(string fechaInicio, string fechaFin, string idTransaccion)
        {
            List<Reporte> reporte = new List<Reporte>();
            reporte = new CNReporte().Ventas(fechaInicio, fechaFin, idTransaccion);
            return Json(new { data = reporte }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public FileResult ExportarVentas(string fechaInicio, string fechaFin, string idTransaccion)
        {
            List<Reporte> reporte = new List<Reporte>();
            reporte = new CNReporte().Ventas(fechaInicio, fechaFin, idTransaccion);

            DataTable dataTable = new DataTable();

            dataTable.Locale = new System.Globalization.CultureInfo("es-CO");
            dataTable.Columns.Add("Fecha Venta", typeof(string));
            dataTable.Columns.Add("Cliente", typeof(string));
            dataTable.Columns.Add("Producto", typeof(string));
            dataTable.Columns.Add("Precio", typeof(decimal));
            dataTable.Columns.Add("Cantidad", typeof(int));
            dataTable.Columns.Add("Total", typeof(decimal));
            dataTable.Columns.Add("Codigo Transaccion", typeof(string));

            foreach (Reporte rpt in reporte)
            {
                dataTable.Rows.Add(new object[]
                {
                    rpt.FechaVenta,
                    rpt.Cliente,
                    rpt.Producto,
                    rpt.Precio,
                    rpt.Cantidad,
                    rpt.Total,
                    rpt.IdTransaccion
                });
            }

            dataTable.TableName = "Ventas";

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"ReporteDeVentas{DateTime.Now.Day}-{DateTime.Now.Month}-{DateTime.Now.Year}.xlsx");
                }
            };
        }
    }
}