namespace Core.ModelRequest;

public enum TypeRequest
{
    Create,
    Delete,
    Update,
    Read
}

public class Request
{
    public TypeRequest TypeRequest { get; set; }
    public string Body { get; set; } = string.Empty;
}
