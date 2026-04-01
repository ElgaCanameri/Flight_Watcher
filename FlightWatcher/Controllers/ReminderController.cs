namespace FlightWatcher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReminderController : ControllerBase
    {
        private readonly IReminderService _reminderService;

        public ReminderController(IReminderService service)
        {
            _reminderService = service;
        }

        [Authorize]
        [HttpGet("reminders-for-user")]
        public async Task<IActionResult> GetRemindersForUser()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var reminders = await _reminderService.GetUserRemindersAsync(userId);
            if (reminders == null || !reminders.Any())
                return NotFound(new { message = "No bookmarks found" });
            return Ok(reminders);
        }

        [Authorize]
        [HttpGet("reminders-for-bookmark")]
        public async Task<IActionResult> GetRemindersForBookmark(int bookmarkId)
        {
            var reminders = await _reminderService.GetBookmarkRemindersAsync(bookmarkId);
            if (reminders == null || !reminders.Any())
                return NotFound(new { message = "No bookmarks found for the required bookmark" });
            return Ok(reminders);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var reminder = await _reminderService.GetByIdAsync(id);
            if (reminder == null)
                return NotFound(new { message = "No bookmarks found for the required bookmark" });
            return Ok(reminder);
        }

        [Authorize]
        [HttpPut("sent/{id}")]
        public async Task<IActionResult> MarkReminderAsSent(int id)
        {
            await _reminderService.MarkAsSentAsync(id);
            return Ok(new { message = "Reminder marked as sent" });
        }
    }
}
