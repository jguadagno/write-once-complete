namespace UnoTest.Models;

public class Phone
{
    public int PhoneId { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Extension { get; set; }
    public required PhoneType PhoneType { get; set; }
}
