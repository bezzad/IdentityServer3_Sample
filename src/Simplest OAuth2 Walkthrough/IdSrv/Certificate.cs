using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.IO;

namespace IdSrv
{
    static class Certificate
    {
        public static X509Certificate2 Get()
        {
            try
            {
                var assembly = typeof(Certificate).Assembly;
                using (
                    var stream =
                        assembly.GetManifestResourceStream("IdSrv.keys.pfx"))
                {
                    var data = ReadStream(stream);
                    //LogManager.GetCurrentClassLogger().Info("data : " + Encoding.UTF8.GetString(data));
                    return new X509Certificate2(data, "taaghcheThe#1", X509KeyStorageFlags.MachineKeySet);
                }
            }
            catch (Exception ex)
            {
                //LogManager.GetCurrentClassLogger().Fatal(ex.Message + " : " + ex.StackTrace);
                //LogManager.GetCurrentClassLogger().Fatal("inner exception" + ex.InnerException?.Message + " : " + ex.InnerException?.StackTrace);
                throw;
            }
        }

        public static byte[] ReadStream(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
