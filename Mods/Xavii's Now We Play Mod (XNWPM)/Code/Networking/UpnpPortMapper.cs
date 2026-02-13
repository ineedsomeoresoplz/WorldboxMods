using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace XaviiNowWePlayMod.Code.Networking
{
    internal static class UpnpPortMapper
    {
        
        private const string SsdpQuery = "M-SEARCH * HTTP/1.1\r\nHost:239.255.255.250:1900\r\nMan:\"ssdp:discover\"\r\nMx:1\r\nSt:urn:schemas-upnp-org:service:WANIPConnection:1\r\n\r\n";

        public static async Task<bool> TryMapPortAsync(int port, string description, TimeSpan timeout)
        {
            try
            {
                var endpoint = await DiscoverIgdAsync(timeout);
                if (endpoint == null)
                {
                    return false;
                }

                string body = $"<?xml version=\"1.0\"?>\n<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" soap:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">\n<soap:Body>\n<u:AddPortMapping xmlns:u=\"urn:schemas-upnp-org:service:WANIPConnection:1\">\n<NewRemoteHost></NewRemoteHost>\n<NewExternalPort>{port}</NewExternalPort>\n<NewProtocol>TCP</NewProtocol>\n<NewInternalPort>{port}</NewInternalPort>\n<NewInternalClient>{GetLocalIPAddress()}</NewInternalClient>\n<NewEnabled>1</NewEnabled>\n<NewPortMappingDescription>{description}</NewPortMappingDescription>\n<NewLeaseDuration>0</NewLeaseDuration>\n</u:AddPortMapping>\n</soap:Body>\n</soap:Envelope>";
                var req = (HttpWebRequest)WebRequest.Create(endpoint);
                req.Method = "POST";
                req.ContentType = "text/xml; charset=\"utf-8\"";
                req.Headers.Add("SOAPACTION", "\"urn:schemas-upnp-org:service:WANIPConnection:1#AddPortMapping\"");
                byte[] bodyBytes = Encoding.UTF8.GetBytes(body);
                using (var stream = await req.GetRequestStreamAsync())
                {
                    await stream.WriteAsync(bodyBytes, 0, bodyBytes.Length);
                }

                using var resp = (HttpWebResponse)await req.GetResponseAsync();
                return resp.StatusCode == HttpStatusCode.OK;
            }
            catch
            {
                return false;
            }
        }

        public static async Task TryUnmapPortAsync(int port)
        {
            try
            {
                var endpoint = await DiscoverIgdAsync(TimeSpan.FromSeconds(1));
                if (endpoint == null)
                {
                    return;
                }

                string body = $"<?xml version=\"1.0\"?>\n<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" soap:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">\n<soap:Body>\n<u:DeletePortMapping xmlns:u=\"urn:schemas-upnp-org:service:WANIPConnection:1\">\n<NewRemoteHost></NewRemoteHost>\n<NewExternalPort>{port}</NewExternalPort>\n<NewProtocol>TCP</NewProtocol>\n</u:DeletePortMapping>\n</soap:Body>\n</soap:Envelope>";
                var req = (HttpWebRequest)WebRequest.Create(endpoint);
                req.Method = "POST";
                req.ContentType = "text/xml; charset=\"utf-8\"";
                req.Headers.Add("SOAPACTION", "\"urn:schemas-upnp-org:service:WANIPConnection:1#DeletePortMapping\"");
                byte[] bodyBytes = Encoding.UTF8.GetBytes(body);
                using (var stream = await req.GetRequestStreamAsync())
                {
                    await stream.WriteAsync(bodyBytes, 0, bodyBytes.Length);
                }

                using var resp = (HttpWebResponse)await req.GetResponseAsync();
            }
            catch
            {
                
            }
        }

        public static void Forget(this Task task)
        {
            
        }

        private static async Task<string> DiscoverIgdAsync(TimeSpan timeout)
        {
            try
            {
                using var client = new UdpClient();
                client.Client.ReceiveTimeout = (int)timeout.TotalMilliseconds;
                client.Client.SendTimeout = (int)timeout.TotalMilliseconds;
                var target = new IPEndPoint(IPAddress.Parse("239.255.255.250"), 1900);
                byte[] data = Encoding.ASCII.GetBytes(SsdpQuery);
                await client.SendAsync(data, data.Length, target);

                var receiveTask = client.ReceiveAsync();
                if (await Task.WhenAny(receiveTask, Task.Delay(timeout)) != receiveTask)
                {
                    return null;
                }

                var response = receiveTask.Result;
                string text = Encoding.ASCII.GetString(response.Buffer);
                string location = null;
                foreach (string line in text.Split('\n'))
                {
                    if (line.Trim().StartsWith("location:", StringComparison.OrdinalIgnoreCase))
                    {
                        location = line.Split(':', 2)[1].Trim();
                        break;
                    }
                }

                return location;
            }
            catch
            {
                return null;
            }
        }

        private static string GetLocalIPAddress()
        {
            foreach (var ip in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "127.0.0.1";
        }
    }
}