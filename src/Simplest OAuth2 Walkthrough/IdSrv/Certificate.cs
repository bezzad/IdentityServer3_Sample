using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace IdentityServer
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
                        assembly.GetManifestResourceStream("IdentityServer.keys.pfx"))
                {
                    var data = ReadStream(stream);
                    //LogManager.GetCurrentClassLogger().Info("data : " + Encoding.UTF8.GetString(data));
                    var x509 = new X509Certificate2(data, "taaghcheThe#1", X509KeyStorageFlags.MachineKeySet);
                    var publicKey = x509.GetPublicKeyString();
                    return x509;
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
