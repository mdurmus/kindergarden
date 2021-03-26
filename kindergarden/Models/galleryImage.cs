namespace kindergarden.Models
{
    public class galleryImage
    {
        public int Id { get; set; }

        public string filePath { get; set; }

        
        public int galleryId { get; set; }
        public gallery gallery { get; set; }
    }
}