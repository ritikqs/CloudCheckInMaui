using System;

namespace CloudCheckInMaui.Models
{
    public class Message
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsRead { get; set; }
        public string SenderName { get; set; }
        public string SenderAvatar { get; set; }
    }
} 