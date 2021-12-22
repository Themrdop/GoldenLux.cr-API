var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});

builder.Services.AddSingleton<IEmailSender, EmailSender>();

builder.Configuration.AddJsonFile("appsettings.json", false, true);

builder.Services.AddFluentEmail(builder.Configuration.GetSection("mail")["Sender"])
                .AddRazorRenderer()
                .AddSmtpSender(new SmtpClient(builder.Configuration.GetSection("mail")["Server"], 
                                              int.Parse(builder.Configuration.GetSection("mail")["Port"]))
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(builder.Configuration.GetSection("mail")["Sender"], 
                                                        builder.Configuration.GetSection("mail")["Password"]),
                    EnableSsl = true
                });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "SendEmailAPI", Version = "v1" });
});

var app = builder.Build();

if(app.Environment.IsDevelopment()){
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SendEmailAPI v1"));
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.ConfigureApi();

app.Run();