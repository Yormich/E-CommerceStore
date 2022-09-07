using System.ComponentModel.DataAnnotations;
using E_CommerceStore.Models.DatabaseModels;

namespace E_CommerceStore.Models.DatabaseModels
{
    public class Review
    {
        public int Id { get; set; }

        public Item? ItemReviewed { get; set; }
        public int ItemId { get; set; }

        public User? ReviewCreator { get; set; }
        public int UserId { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "Length should be below 40 characters", MinimumLength = 0)]
        public string ShortComment { get; set; } = String.Empty;

        [StringLength(150,ErrorMessage = "Length should be between 2 and 150 characters",MinimumLength = 2)]
        public string? LongComment { get; set; }

        [Required]
        [Range(1.0,10.0, ErrorMessage = "Rating should be between 1 and 10",
            ConvertValueInInvariantCulture = true)]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public double Rating { get; set; }

        public DateTime ReviewTime { get; set; }

        public Review(int itemId, int userId, string shortComment, string? longComment, double rating,
            DateTime reviewTime)
        {
            this.ItemId = itemId;
            this.UserId = userId;
            this.ShortComment = shortComment;
            this.LongComment = longComment;
            this.Rating = rating;
            this.ReviewTime = reviewTime;
        }

        public Review(int userId,int itemId)
        {
            this.UserId = userId;
            this.ItemId = itemId;
        }
        
        public Review() { }
    }   
}
