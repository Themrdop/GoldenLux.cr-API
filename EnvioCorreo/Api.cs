public static class Api{
    public static void ConfigureApi(this WebApplication app){
        app.MapPost("/sendEmail",sendEmail);
    }

    private static async Task<IResult> sendEmail(EmailData emailData, IEmailSender emailSender){
        try
        {
            var response = await emailSender.sendEmail(emailData);

            if(response.Successful){
                return Results.Ok(response.MessageId);
            }else{
                return Results.Problem(string.Join(Environment.NewLine, response.ErrorMessages));
            }
        }
        catch (System.Exception ex)
        {
            return Results.Problem(ex.Message);
        }
        
        
    }
}