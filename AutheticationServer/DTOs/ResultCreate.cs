namespace AutheticationServer.DTOs
{
    public class ResultCreate
    {
        public bool authenticated { get; set; }
        public string created { get; set; }
        public string expiration { get; set; }
        public string accessToken { get; set; }
        public string message { get; set; }

        //public ResultCreate(bool authenticated, string created, string expiration, string accessToken, string message)
        //{
        //    this.authenticated = authenticated;
        //    this.created = created ?? throw new ArgumentNullException(nameof(created));
        //    this.expiration = expiration ?? throw new ArgumentNullException(nameof(expiration));
        //    this.accessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
        //    this.message = message ?? throw new ArgumentNullException(nameof(message));
        //}
    }
}
