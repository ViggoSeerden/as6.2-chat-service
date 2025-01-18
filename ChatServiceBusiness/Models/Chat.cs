namespace ChatServiceBusiness.Models;

public enum Status
{
    Open,
    Closed
}

public class Chat
{
    public Guid Id { get; set; }
    
    public Guid Landlord { get; set; }

    public Guid Tenant { get; set; }

    public List<Message> Messages { get; set; }

    public Status Status { get; set; }
}