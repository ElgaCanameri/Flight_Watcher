namespace FlightWatcher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookmarkController : ControllerBase
    {
        private readonly IBookmarkService _bookmarkService;

        public BookmarkController(IBookmarkService bookmarkService)
        {
            _bookmarkService = bookmarkService;
        }

        [Authorize]
        [HttpPost("bookmark")]
        public async Task<IActionResult> AddBookmark([FromBody] AddBookmarkRequest request)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _bookmarkService.AddBookmarkAsync(userId, request.FlightIata);
            return Ok();
        }

        [Authorize]
        [HttpGet("bookmarks")]
        public async Task<IActionResult> GetBookmarks()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var bookmarks = await _bookmarkService.GetByUserIdAsync(userId);
            if (bookmarks == null || !bookmarks.Any())
                return NotFound(new { message = "No bookmarks found" });
            return Ok(bookmarks);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveBookmark(string id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _bookmarkService.RemoveBookmarkAsync(userId, id);
            return NoContent();
        }
    }
}
