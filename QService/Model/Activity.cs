
namespace QService.Model
{
    public class Activity
    {
        public string Id { get; set; }
        public Company Company { get; set; }
        public string Name { get; set; }
        public string UrlPicture { get; set; }
        public string QrCode { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }
        public QueueInfo QueueInfo { get; set; }
    }
}
