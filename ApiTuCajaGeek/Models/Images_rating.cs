namespace ApiTuCajaGeek.Models
{
    public class Images_rating
    {
        public long Image_rating_Id { get; set; }
        public long Rating_Id { get; set; }
        public string Image_Url { get; set; } = string.Empty;
        public User_rating? Rating { get; set; }
    }
}
