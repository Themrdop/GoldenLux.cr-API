namespace EnvioCorreo.Services;
public class EmailSender: IEmailSender
{
    private readonly IFluentEmail _email;
    private readonly ILogger _logger;
    private readonly IConfiguration _configuration;

    public EmailSender(IFluentEmail email, 
                       ILogger<EmailSender> logger,
                       IConfiguration configuration){
        _email = email;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<SendResponse> sendEmail(EmailData email){
        try
        {
             var result = await _email
                                    .To(_configuration.GetSection("mail")["Receiver"])
                                    .Subject(email.subject)
                                    .Body(email.body)
                                    .ReplyTo(email.from)
                                    .SendAsync();
        
            if (result.Successful)
            {
                _logger.Log(LogLevel.Information,"email send correctly", result.MessageId);
            }else{
                _logger.LogError("Failed to send an email.\n{Errors}", string.Join(Environment.NewLine, result.ErrorMessages));
            }

            return result;
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex,"Failed to send an email.\n{Errors}",string.Empty);
        }

        return new SendResponse{ErrorMessages = new List<string>{"Error trying to send email"}};
    }
}