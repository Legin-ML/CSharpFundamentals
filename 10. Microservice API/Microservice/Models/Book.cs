using System.ComponentModel.DataAnnotations;

namespace Microservice.Models
{
    public class Book
    {

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(100)]
        public string Author { get; set; }

        public decimal Price { get; set; }

        public string Genre { get; set; }

        public DateTime PublishedDate { get; set; }


    }

    public class CreateBookRequest
    {
        [Required]
        [StringLength(100)]
        public string Author { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }
        public DateTime PublishedDate { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
    }

    public class UpdateBookRequest
    {
        [Required]
        [StringLength(100)]
        public string Author { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }
        public DateTime PublishedDate { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }
    }


}
