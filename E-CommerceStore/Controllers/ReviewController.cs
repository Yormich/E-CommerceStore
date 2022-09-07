using Microsoft.AspNetCore.Mvc;
using E_CommerceStore.Database;
using E_CommerceStore.Utilities;
using E_CommerceStore.Models.DatabaseModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using E_CommerceStore.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceStore.Controllers
{
    [Authorize(Roles = "Buyer")]
    [Route("Review")]
    public class ReviewController : Controller
    {
        private readonly EStoreContext db;
        private readonly IUserClaimsManager claimManager;

        public ReviewController([FromServices] EStoreContext db, [FromServices] IUserClaimsManager manager)
        {
            this.db = db;
            this.claimManager = manager;
        }

        [HttpGet("AddNew/{itemId:int}")]
        public ViewResult AddNewReviewForm(int itemId)
        {
            int UserId = Int32.Parse(claimManager.TryGetClaimValue("Id") ?? "-1");
            Review review = new Review(UserId,itemId);
            return View("AddReview",review);
        }

        [HttpPost("AddNew/Confirm")]
        public async Task<IActionResult> ConfirmReviewAdd(Review review)
        {
            if(ModelState.IsValid)
            {
                await db.Reviews.AddAsync(review);
                await db.SaveChangesAsync();
                return RedirectToAction("ProductPage", "ProductCatalog", new { itemId = review.ItemId });
            }
            return View("AddReview",review);
        }

        [HttpGet("EditReview/{reviewId:int}")]
        public async Task<IActionResult> EditReviewForm(int reviewId)
        {
            Review? review = await db.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);
            if (review == null)
                return NotFound();

            return View("EditReview", review);
        }

        [HttpPost("EditReview/Confirm")]
        public async Task<IActionResult> ConfirmReviewEdit(Review review)
        {
            Console.WriteLine(review.Rating);
            if(ModelState.IsValid)
            {
                Review? reviewToUpdate = await db.Reviews.FirstOrDefaultAsync(r => r.Id == review.Id);
                if (reviewToUpdate == null)
                    return NotFound();
                reviewToUpdate.ShortComment = review.ShortComment;
                reviewToUpdate.LongComment = review.LongComment;
                reviewToUpdate.Rating = review.Rating;
                reviewToUpdate.ReviewTime = DateTime.Now;
                await db.SaveChangesAsync();

                return RedirectToAction("ProductPage", "ProductCatalog", new { itemId = review.ItemId });
            }
            return View("EditReview", review);
        }

        [HttpGet("DeleteReview/{reviewId:int}")]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            int ItemId = 0;
            Review? reviewToDelete = await db.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);
            if (reviewToDelete == null)
                return NotFound();

            ItemId = reviewToDelete.ItemId;
            db.Reviews.Remove(reviewToDelete);
            await db.SaveChangesAsync();
            return RedirectToAction("ProductPage", "ProductCatalog", new { itemId = ItemId });
        }


        /*немного shit-code`a не помешает)) 
        [HttpGet("IncreaseLikes/{reviewId:int}")]
        public async Task<IActionResult> IncreaseLikesAmount(int reviewId)
        {
            int itemId = -1;
            Review? review = await db.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);
            if (review == null)
                return NotFound();
            review.NumberOfLikes += 1;
            await db.SaveChangesAsync();
            return RedirectToAction("ProductPage", "ProductCatalog", new { itemId = itemId });
        }*/
    }
}
