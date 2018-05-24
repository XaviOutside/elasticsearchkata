using Nest;
using System;
using System.Configuration;
using System.IO;
using System.Text;


namespace Extensions.Logger
{
    public static class Log
    {
        private static object _lock = new object();
        public static void ToLog(this string message, bool putPrefix = true)
        {
            try
            {
                string path = ConfigurationManager.AppSettings["PATH_LOG"].ToString();

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);


                StringBuilder stringBuilder = new StringBuilder();

                stringBuilder.Append(path);
                stringBuilder.Append("logfile.log");

                string fileName = stringBuilder.ToString();
                string text = "";

                lock (_lock)
                {
                    using (StreamWriter streamWriter = File.AppendText(fileName))
                    {
                        if (putPrefix)
                        {
                            text = (System.DateTime.Now.ToString("dd-MM-yyyy") + (" "
                            + (System.DateTime.Now.ToString("T") + ("   " + message))));

                            streamWriter.WriteLine(text);
                        }
                        else
                        {
                            text = message;
                            streamWriter.WriteLine(text);
                        }

                        streamWriter.Close();
                    }
                }
                
            }
            catch (Exception)
            {
                // Nothing
            }
        }

        public static void ToLog(this IResponse response, bool putPrefix = false)
        {
            string message = string.Empty;

            message = "\r\n" + message + "HTTP METHOD: " + response.ApiCall.HttpMethod.ToString() + "\r\n";

            if (response.ApiCall.HttpStatusCode != null)
            {
                message = message + "HTTP STATUS CODE: " + response.ApiCall.HttpStatusCode.ToString() + "\r\n";
            }
            
            if(!string.IsNullOrEmpty(response.ApiCall.Uri.AbsoluteUri))
            {
                message = message + "ABSOLUTE URI: " + response.ApiCall.Uri.AbsoluteUri + "\r\n";
            }


            if (response.ApiCall.RequestBodyInBytes != null)
            {
                message = message + "REQUEST: \r\n" + Encoding.UTF8.GetString(response.ApiCall.RequestBodyInBytes) + "\r\n";
            }

            if (response.ApiCall.ResponseBodyInBytes != null)
            {
                message = message + "RESPONSE: \r\n" + Encoding.UTF8.GetString(response.ApiCall.ResponseBodyInBytes) + "\r\n";
            }

            message = message + "IS VALID: " + response.IsValid.ToString() + "\r\n";

            message.ToLog(putPrefix);
        }
    }
}
