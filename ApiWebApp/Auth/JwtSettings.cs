namespace ApiWebApp.Auth
{
    public class JwtSettings
    {
        internal string Issuer;

        public string SecretKey { get; set; }
        public string Audience { get; internal set; }
    }
}
