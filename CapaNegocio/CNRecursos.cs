using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CNRecursos
    {
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
    }
}
