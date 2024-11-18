namespace ChatServiceBusiness.Models;

public class Chat
{
    public Guid Id { get; set; }
    
    public Guid Landlord { get; set; }

    public Guid Tenant { get; set; }

    public List<Message> Messages { get; set; }
}