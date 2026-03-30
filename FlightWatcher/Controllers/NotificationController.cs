namespace FlightWatcher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [Authorize]
        [HttpGet("notifications")]
        public async Task<IActionResult> GetAllNotifications()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _notificationService.GetNotificationsAsync(userId);
            return Ok();
        }

        [Authorize]
        [HttpGet("unread-notifications")]
        public async Task<IActionResult> GetUnreadNotifications()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _notificationService.GetUnreadNotificationsAsync(userId);
            return Ok();
        }

        [Authorize]
        [HttpPatch("{id}/read")]
        public async Task<IActionResult> MarkOneNotificationRead(int notificationId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _notificationService.MarkOneNotificationReadAsync(userId, notificationId);
            return Ok();
        }

        [Authorize]
        [HttpPatch("read-all")]
        public async Task<IActionResult> MarkAllNotificationsRead()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _notificationService.MarkAllNotificationsReadAsync(userId);
            return Ok();
        }
    }
}
