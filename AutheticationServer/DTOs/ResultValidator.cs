namespace AutheticationServer.DTOs
{
    public class ResultValidator
    {
        public bool authenticated { get; set; }
        public string lifetime { get; set; }
        public string accessToken { get; set; }
        public string message { get; set; }
    }
}
