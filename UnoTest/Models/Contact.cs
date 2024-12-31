namespace UnoTest.Models;

public class Contact
{
    public Contact()
    {
        Addresses = new List<Address>();
        Phones = new List<Phone>();
    }

    public int ContactId { get; set; }
    public required string FirstName { get; set; }
    public required string MiddleName { get; set; }
    public required string LastName { get; set; }
    public required string EmailAddress { get; set; }
    public required DateTime Birthday { get; set; }
    public DateTime? Anniversary { get; set; }
    public required string ImageUrl { get; set; }
    public required List<Address> Addresses { get; set; }
    public required List<Phone> Phones { get; set; }

    public string FullName
    {
        get
        {
            if (string.IsNullOrEmpty(FirstName) && string.IsNullOrEmpty(LastName) &&
                string.IsNullOrEmpty(MiddleName))
            {
                return "Could not determine the contact name";
            }

            if (string.IsNullOrEmpty(MiddleName))
            {
                return $"{FirstName} {LastName}";
            }

            return $"{FirstName} {MiddleName} {LastName}";
        }
    }
}
