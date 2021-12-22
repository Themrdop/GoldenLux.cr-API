namespace EnvioCorreo.Interfaces;
public interface IEmailSender
{
    Task<SendResponse> sendEmail(EmailData email);
}