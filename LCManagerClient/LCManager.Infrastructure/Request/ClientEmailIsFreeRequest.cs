namespace LCManager.Infrastructure.Request
{
    public class ClientEmailIsFreeRequest
    {
        public string Email { get; set; }
        public short Operator { get; set; }
    }
}
