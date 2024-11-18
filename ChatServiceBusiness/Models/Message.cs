namespace ChatServiceBusiness.Models;

public class Message
{
    public Guid Id { get; set; }
    
    public Guid ChatId { get; set; }
    
    public Guid Sender { get; set; }

    public string Content { get; set; }
}