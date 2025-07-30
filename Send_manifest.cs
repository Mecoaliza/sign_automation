using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Mail;
using Newtonsoft.Json.Linq;

namespace UiPath_REFramework_CSharp.ProcessTransaction
{
    public class ProcessTransactionPart2
    {
        public string Process(
            Dictionary<string, object> config,
            Dictionary<string, object> transactionItem,
            out string result)
        {
            result = string.Empty;

            try
            {
                Console.WriteLine("🔄 Continuando processamento da transação...");

                
                string assinatura = transactionItem["assinatura"].ToString();
                if (!ValidarAssinatura(assinatura))
                {
                    throw new Exception("Assinatura inválida.");
                }

                
                string zipUrl = transactionItem["zipUrl"].ToString();
                string zipPath = DownloadZip(zipUrl);

                
                string email = transactionItem["email"].ToString();
                EnviarEmailAlerta(email, zipPath);

               
                ReprocessarItens(transactionItem);

                /
                ChamarApiExterna(transactionItem);

                
                result = "Processamento concluído com sucesso.";
                return "Success";
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Erro no processamento: " + ex.Message);
                result = ex.Message;
                return "SystemException";
            }
        }

        private bool ValidarAssinatura(string assinatura)
        {
            // TODO: Adicionar lógica de validação de assinatura
            Console.WriteLine("Validando assinatura...");
            return true;
        }

        private string DownloadZip(string url)
        {
            // TODO: Adicionar lógica para baixar arquivos ZIP
            Console.WriteLine("Baixando arquivo ZIP...");
            return "caminho/do/arquivo.zip";
        }

        private void EnviarEmailAlerta(string email, string zipPath)
        {
            // TODO: Adicionar lógica para enviar e-mails de alerta
            Console.WriteLine("Enviando e-mail de alerta...");

            var mail = new MailMessage();
            var smtpServer = new SmtpClient("smtp.seuservidor.com");

            mail.From = new MailAddress("seuemail@dominio.com");
            mail.To.Add(email);
            mail.Subject = "Alerta de Processamento";
            mail.Body = "O processamento foi concluído. O arquivo ZIP está anexado.";
            mail.Attachments.Add(new Attachment(zipPath));

            smtpServer.Port = 587;
            smtpServer.Credentials = new System.Net.NetworkCredential("seuemail@dominio.com", "suasenha");
            smtpServer.EnableSsl = true;

            smtpServer.Send(mail);
        }

        private void ReprocessarItens(Dictionary<string, object> transactionItem)
        {
            // TODO: Adicionar lógica para reprocessar itens
            Console.WriteLine("Reprocessando itens...");
        }

        private void ChamarApiExterna(Dictionary<string, object> transactionItem)
        {
            // TODO: Adicionar lógica para chamar APIs externas
            Console.WriteLine("Chamando API externa...");

            var client = new HttpClient();
            var json = new JObject
            {
                ["transactionItem"] = JObject.FromObject(transactionItem)
            };

            var content = new StringContent(json.ToString(), System.Text.Encoding.UTF8, "application/json");
            var response = client.PostAsync("https://api.externa.com/process", content).Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Erro ao chamar API externa: " + response.StatusCode);
            }
        }
    }
}