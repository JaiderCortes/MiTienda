using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace CapaNegocio
{
    public class CNRecursos
    {

        public static string GenerarClave()
        {
            string clave = Guid.NewGuid().ToString("N").Substring(0, 6);
            return clave;
        }

        //Método para encriptar contraseña en SHA256
        public static string EncriptarSha256(string input)
        {
            StringBuilder sb = new StringBuilder();
            using(SHA256 sha256 = SHA256Managed.Create())
            {
                Encoding encoding = Encoding.UTF8;
                byte[] result = sha256.ComputeHash(encoding.GetBytes(input));
                foreach(byte b in result)
                {
                    sb.Append(b.ToString("x2"));
                }
            }
            return sb.ToString();
        }

        public static bool EnviarCorreo(string correo, string asunto, string mensaje)
        {
            bool resultado = false;
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(correo);
                mail.From = new MailAddress("correoorigen@gmail.com");
                mail.Subject = asunto;
                mail.Body = mensaje;
                mail.IsBodyHtml = true;

                var smtp = new SmtpClient()
                {
                    Credentials = new NetworkCredential("correoorigen@gmail.com", "pswgenerada"),
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true
                };

                smtp.Send(mail);
                resultado = true;

            }
            catch (Exception e)
            {
                Console.WriteLine($"Ocurrió un error durante la ejecución del procedimiento: {e.Message}");
                resultado = false;
                throw;
            }
            return resultado;
        }
    }
}
