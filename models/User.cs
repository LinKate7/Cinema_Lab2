using Microsoft.AspNetCore.Identity;

namespace Lab2OOP.models;

public class User : IdentityUser<Guid>
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public ICollection<Purchase> Purchases { get; set; } 
}