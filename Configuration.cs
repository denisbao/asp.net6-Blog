namespace Blog
{
  public static class Configuration
  {
    // JSON Web Token (JWT):
    public static string JwtKey = "ZmVkYWY3ZDg4NjNiNDhlMTk3YjkyODdkNDkyYjcwOGU=";


    // Autenticação por API KEY:
    public static string ApiKeyName = "api_key";
    public static string ApiKey = "IlTevUM/z0ey3NwCV/unWg==";


    // Exposição da configuração de envio de email para a aplicação:
    public static SmtpConfiguration Smtp = new();

    // Envio de Email:
    public class SmtpConfiguration
    {
      public string Host { get; set; }
      public int Port { get; set; }
      public string UserName { get; set; }
      public string Password { get; set; }
    }
  }
}