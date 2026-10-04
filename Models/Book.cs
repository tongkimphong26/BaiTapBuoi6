using System.ComponentModel.DataAnnotations;

namespace BookManagement.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên sách")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên tác giả")]
        public string Author { get; set; }

        [Range(0, 100000000, ErrorMessage = "Giá không hợp lệ")]
        public decimal Price { get; set; }

        [Range(1000, 2100, ErrorMessage = "Năm xuất bản không hợp lệ")]
        public int PublishedYear { get; set; }
    }
}