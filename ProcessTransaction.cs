using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Mail;
using Newtonsoft.Json.Linq;

namespace UiPath_REFramework_CSharp.ProcessTransaction
{
    public class ProcessTransaction
    {
        public string Process(
            Dictionary<string, object> config,
            Dictionary<string, object> transactionItem,
            out string result)
        {
            result = string.Empty;

            try
            {
                Console.WriteLine("🔄 Iniciando processamento da transação...");

                
                string coop = transactionItem["coop"].ToString();
                string idProcesso = transactionItem["id_processo"].ToString();
                string solicitante = transactionItem["solicitante"].ToString();
                string strAssinantes = transactionItem["str_assinantes"].ToString();

                
                string filePath = @"C:\TEMP\user\enviar";
                string[] arquivos = Directory.GetFiles(filePath, "*.pdf");

                
                JArray documentos = new JArray();
                foreach (var arquivo in arquivos)
                {
                    byte[] bytes = File.ReadAllBytes(arquivo);
                    string base64 = Convert.ToBase64String(bytes);

                    var doc = new JObject
                    {
                        ["filename"] = Path.GetFileName(arquivo),
                        ["bytes"] = base64
                    };

                    documentos.Add(doc);
                }

                
                var client = new HttpClient();
                var jsonUpload = new JObject
                {
                    ["documents"] = documentos
                };

                var content = new StringContent(jsonUpload.ToString(), System.Text.Encoding.UTF8, "application/json");
                var response = client.PostAsync("https://api.portaldeassinaturas.com.br/api/v2/document/upload", content).Result;

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Erro ao enviar documentos: " + response.StatusCode);
                }

                
                string responseContent = response.Content.ReadAsStringAsync().Result;
                JObject jsonResponse = JObject.Parse(responseContent);
                Console.WriteLine("✅ Upload realizado com sucesso.");

                
                string smtpServer = config["smtp_server"].ToString();
                string smtpPort = config["smtp_port"].ToString();
                string smtpUser = config["smtp_user"].ToString();
                string smtpPassword = config["smtp_password"].ToString();
                string emailTo = config["email_to"].ToString();
                string emailSubject = "Resultado do Processamento de Transação";
                string emailBody = $"O processamento da transação {idProcesso} foi concluído com sucesso.\n\nResposta da API:\n{jsonResponse.ToString()}";

                var mailMessage = new MailMessage(smtpUser, emailTo, emailSubject, emailBody);
                var smtpClient = new SmtpClient(smtpServer, int.Parse(smtpPort))
                {
                    Credentials = new System.Net.NetworkCredential(smtpUser, smtpPassword),
                    EnableSsl = true
                };

                smtpClient.Send(mailMessage);
                Console.WriteLine("✅ E-mail enviado com sucesso.");

                
                result = jsonResponse.ToString();
                return "Success";
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Erro no processamento: " + ex.Message);
                result = ex.Message;
                return "SystemException";
            }
        }
    }
}
