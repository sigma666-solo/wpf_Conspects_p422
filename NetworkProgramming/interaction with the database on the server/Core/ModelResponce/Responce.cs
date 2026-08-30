namespace Core.ModelResponce;

public enum TypeResponse
{
    Create,
    Delete,
    Update,
    Read
}

public class Responce
{
    public TypeResponse TypeResponse { get; set; }
    public string Body { get; set; } = string.Empty;
}
