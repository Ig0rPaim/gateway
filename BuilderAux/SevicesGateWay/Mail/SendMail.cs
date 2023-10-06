using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;

namespace BuilderAux.SevicesGateWay.Mail
{
    //https://mailtrap.io/blog/send-email-in-asp-net-c-sharp/
    public class SendMail
    {
        public static async Task SendGridMail(string remetenteEmail, string nomeRemetente, string destinatarioEmail, string nomeDestinatario, string titulo, string? corpo, string? link)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);

            var from = new EmailAddress(remetenteEmail, nomeRemetente);
            var subject = titulo;
            var to = new EmailAddress(destinatarioEmail, nomeDestinatario);
            var plainTextContent = corpo;
            var htmlContent =
                @$"<html>
                    <head>
                        <meta charset=""UTF-8"">
                        <title>Email Formal</title>
                    </head>
                    <body>
                        <table align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""600"">
                            <tr>
                                <td align=""center"" bgcolor=""#0073e6"" style=""padding: 20px 0;"">
                                    <h1 style=""color: #ffffff;"">{titulo}</h1>
                                </td>
                            </tr>
                            <tr>
                                <td style=""padding: 20px;"">
                                    <h2>Prezado(a) {nomeDestinatario},</h2>
                                    <p>Não Responda!!</p>
                                    <p>Para recuperar a senha Clique no Link abaixo</p>
                                    <h3>{link}</h3>
                                    <h2>Atenciosamente,
                                    {nomeRemetente}</h2>
                                </td>
                            </tr>
                        </table>
                    </body>
                </html>";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var response = await client.SendEmailAsync(msg);

            if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                Console.WriteLine("E-mail enviado com sucesso!");
            }
            else
            {
                Console.WriteLine($"Erro ao enviar o e-mail: {response.StatusCode}");
            }
        }

        public static async Task SendGridMail(string remetenteEmail, string nomeRemetente, string destinatarioEmail, string nomeDestinatario, string titulo)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);

            var from = new EmailAddress(remetenteEmail, nomeRemetente);
            var subject = titulo;
            var to = new EmailAddress(destinatarioEmail, nomeDestinatario);
            var plainTextContent = string.Empty;
            var htmlContent =
                @$"<html>
                    <head>
                        <meta charset=""UTF-8"">
                        <title>Email Formal</title>
                    </head>
                    <body>
                        <table align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""600"">
                            <tr>
                                <td align=""center"" bgcolor=""#0073e6"" style=""padding: 20px 0;"">
                                    <h1 style=""color: #ffffff;"">{titulo}</h1>
                                </td>
                            </tr>
                            <tr>
                                <td style=""padding: 20px;"">
                                    <h2>Prezado(a) {nomeDestinatario},</h2>
                                    <p>Não Responda!!</p>
                                    <p>Para recuperar a senha Clique no Link abaixo</p>
                                    <h2>Atenciosamente,
                                    {nomeRemetente}</h2>
                                </td>
                            </tr>
                        </table>
                    </body>
                </html>";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var response = await client.SendEmailAsync(msg);

            if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                Console.WriteLine("E-mail enviado com sucesso!");
            }
            else
            {
                Console.WriteLine($"Erro ao enviar o e-mail: {response.StatusCode}");
            }
        }


        public static async Task SendGridMail(string remetenteEmail, string nomeRemetente, string destinatarioEmail, string nomeDestinatario, string titulo, string? corpo)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);

            var from = new EmailAddress(remetenteEmail, nomeRemetente);
            var subject = titulo;
            var to = new EmailAddress(destinatarioEmail, nomeDestinatario);
            var plainTextContent = corpo;
            var htmlContent =
                @$"<html>
                    <head>
                        <meta charset=""UTF-8"">
                        <title>Email Formal</title>
                    </head>
                    <body>
                        <table align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""600"">
                            <tr>
                                <td align=""center"" bgcolor=""#0073e6"" style=""padding: 20px 0;"">
                                    <h1 style=""color: #ffffff;"">{titulo}</h1>
                                </td>
                            </tr>
                            <tr>
                                <td style=""padding: 20px;"">
                                    <h2>Prezado(a) {nomeDestinatario},</h2>
                                    <p>Não Responda!!</p>
                                    <p>Para recuperar a senha Clique no Link abaixo</p>
                                    <h2>Atenciosamente,
                                    {nomeRemetente}</h2>
                                </td>
                            </tr>
                        </table>
                    </body>
                </html>";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var response = await client.SendEmailAsync(msg);

            if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                Console.WriteLine("E-mail enviado com sucesso!");
            }
            else
            {
                Console.WriteLine($"Erro ao enviar o e-mail: {response.StatusCode}");
            }
        }
    }
}
