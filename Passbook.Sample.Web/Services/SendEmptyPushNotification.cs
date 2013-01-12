using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Security.Authentication;
using System.IO;
using WhereAreMyStaff.Services;
using System.Diagnostics;

namespace WhereAreMyStaff.Services.Mobile
{
    public class iPhoneMobileNotificationService
    {
        public void SendEmptyPushNotification(string deviceIdentifier, string thumbprint)
        {
            string server = "gateway.push.apple.com";
            using (TcpClient tcpClient = new TcpClient(server, 2195))
            {
                Trace.TraceInformation("Opening SSL Connection...");
                using (SslStream sslStream = new SslStream(tcpClient.GetStream()))
                {
                    try
                    {
                        X509Certificate2Collection certs = new X509Certificate2Collection();

                        Trace.TraceInformation("Adding certificate to connection...");
                        X509Certificate cert = GetAppleServerCert(thumbprint);
                        certs.Add(cert);

                        Trace.TraceInformation("Authenticating against the SSL stream...");
                        sslStream.AuthenticateAsClient(server, certs, SslProtocols.Default, false);
                    }
                    catch (AuthenticationException exp)
                    {
                        Trace.TraceError("Failed to authenticate to APNS - {0}", exp.Message);
                        return;
                    }
                    catch (IOException exp)
                    {
                        Trace.TraceError("Failed to connect to APNS - {0}", exp.Message);
                        return;
                    }

                    byte[] buf = new byte[256];
                    MemoryStream ms = new MemoryStream();
                    BinaryWriter bw = new BinaryWriter(ms);
                    bw.Write(new byte[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 32 });

                    byte[] deviceToken = HexToData(deviceIdentifier);
                    bw.Write(deviceToken);
                    
                    string msg = "{}";

                    bw.Write(new byte[] { 0, 2 });
                    bw.Write(msg.ToCharArray());
                    bw.Flush();

                    Trace.TraceInformation("Message sent. Closing stream...");

                    if (sslStream != null)
                    {
                        sslStream.Write(ms.ToArray());
                    }

                    sslStream.Flush();

                    byte[] response = new byte[6];
                    sslStream.Read(response, 0, 6);
                }
            }
        }

        private static X509Certificate GetAppleServerCert(string thumbprint)
        {
            X509Store store;
            store = new X509Store(StoreLocation.CurrentUser);

            if (store != null)
            {
                store.Open(OpenFlags.ReadOnly);

                X509Certificate2Collection certs = store.Certificates;

                if (certs.Count > 0)
                {
                    for (int i = 0; i < certs.Count; i++)
                    {
                        X509Certificate2 cert = certs[i];

                        if (cert.Thumbprint.Equals(thumbprint, StringComparison.InvariantCultureIgnoreCase))
                        {
                            return certs[i];
                        }
                    }
                }
            }

            Trace.TraceError("Could not find the certification containing: {0} ", "R5QS56362W:R5QS56362W");

            throw new InvalidDataException("Could not find the Apple Push Notification certificate");
        }

        private static byte[] HexToData(string hexString)
        {
            if (hexString == null)
            {
                return null;
            }

            if (hexString.Length % 2 == 1)
            {
                hexString = '0' + hexString; // Up to you whether to pad the first or last byte
            }

            byte[] data = new byte[hexString.Length / 2];

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return data;
        }
    }
}
