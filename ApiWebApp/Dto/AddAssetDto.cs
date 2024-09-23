namespace ApiWebApp.Dto
{
    public class AddAssetDto
    {
        public string Type { get; set; }
        public string IpAddress { get; set; }

        public string Url { get; set; }

        public string License { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


        public string SupportExpiration { get; set; }
        public string Notes { get; set; }
    }
}
