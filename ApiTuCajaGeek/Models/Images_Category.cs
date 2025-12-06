namespace ApiTuCajaGeek.Models
{
    public class Images_Category
    {
        public long Image_category_Id { get; set; }
        public long Category_Id { get; set; }
        public string Image_Url { get; set; } = string.Empty;
        public Category? Category { get; set; }
    }
}
