namespace UnoTest.Models;

public class Address
{
    public int AddressId { get; set; }
    public required string StreetAddress { get; set; }
    public required string SecondaryAddress { get; set; }
    public required string Unit { get; set; }
    public required string City { get; set; }
    public required string State { get; set; }
    public required string Country { get; set; }
    public required string PostalCode { get; set; }
    public required AddressType AddressType { get; set; }
}
