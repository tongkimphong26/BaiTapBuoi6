namespace BookManagement.Models
{
    public class BookImage
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        public string? ImagePath { get; set; }

        public Book? Book { get; set; }
    }
}