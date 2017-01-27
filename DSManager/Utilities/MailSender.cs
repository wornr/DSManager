using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using DSManager.Model.Entities.Dictionaries;
using DSManager.Model.Services;

namespace DSManager.Utilities {
    public sealed class MailSender {
        private static MailSender _instance;
        private SmtpClient _client;
        private string _from;
        private Settings smtpSettings;
        private static bool _correctConfiguration;

        private MailSender() {
            _client = new SmtpClient();
            using (var repository = new BaseRepository()) {
                smtpSettings = repository.ToList<Settings>().FirstOrDefault();
            }
            if (smtpSettings == null || string.IsNullOrEmpty(smtpSettings.SmtpHost) || string.IsNullOrEmpty(smtpSettings.SmtpMail)) {
                _correctConfiguration = false;
            } else {
                _client.Host = smtpSettings.SmtpHost;
                _client.Port = smtpSettings.SmtpPort;
                _client.EnableSsl = smtpSettings.SmtpSsl;
                _client.Timeout = 5000;
                _client.UseDefaultCredentials = string.IsNullOrEmpty(smtpSettings.SmtpUsername) && string.IsNullOrEmpty(smtpSettings.SmtpPassword);
                _client.Credentials = new NetworkCredential(smtpSettings.SmtpUsername, smtpSettings.SmtpPassword);
                _from = smtpSettings.SmtpMail;
                _correctConfiguration = true;
            }
        }

        public static MailSender Instance {
            get {
                if(_instance == null || !_correctConfiguration) {
                    _instance = new MailSender();
                }
                return _instance;
            }
        }

        public bool SendNewAccountMail(string to, string firstName, string lastName, string confirmationKey) {
            if (_correctConfiguration) {
                string subject = $"Utworzono konto w systemie DSManager dla {firstName} {lastName}";
                string body =
                    $@"Witaj!

    Właśnie zostało utworzone nowe konto w systemie DSManager dla {firstName} {lastName}.
    Możesz się w nim zalogować podając następujące dane: 
    Login: {to}
    Hasło: {confirmationKey}

    Ten email został wygenerowany automatycznie, nie odpowiadaj na niego!
    Jeżeli nie jesteś adresatem ninejszej wiadomości niezwłocznie powiadom o tym nadawcę i usuń tego maila!

    Pozdrawiam,
    Administrator systemu DSManager";
                MailMessage message = new MailMessage(_from, to, subject, body);
                _client.Send(message);
            }

            return _correctConfiguration;
        }

        public bool SendChangeEmailAdressMail(string oldEmail, string newEmail, string firstName, string lastName) {
            if (_correctConfiguration) {
                string subject = $"Zmieniono adres mailowy w systemie DSManager dla {firstName} {lastName}";
                string body =
                    $@"Witaj {firstName} {lastName}!

    Właśnie został zmieniony adres mailowy twojego konta w systemie DSManager. Przyszła korespondencja będzie kierowana na nowy adres: {newEmail}
    Od teraz logując się do systemu korzystaj z nowego loginu: 
    Login: {newEmail}

    Ten email został wygenerowany automatycznie, nie odpowiadaj na niego!
    Jeżeli nie jesteś adresatem ninejszej wiadomości niezwłocznie powiadom o tym nadawcę i usuń tego maila!

    Pozdrawiam,
    Administrator systemu DSManager";
                MailMessage message = new MailMessage(_from, oldEmail, subject, body);
                _client.Send(message);
            }

            return _correctConfiguration;
        }
    }
}
