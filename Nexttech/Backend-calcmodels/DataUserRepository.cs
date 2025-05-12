// Data/MockUserRepository.cs

using system;
using system.Collections.Generic;
using system.Linq; // Til Linq-metoder til Defult
Using user.cs; // bruger user.cs klassen

namespace PBI24-SEM2
{
    public static class MockUserRepository
    {
    // En liste der simulerer en database med brugere
    private static List<User> _users = new List<User>()
        {
    new User {Email = "belham01@iba.dk", Password = "BMango101"},
    new User {Email = "hamder01@iba.dk", Password = "HKittty010"}
        };

    // Find en bruger ud fra e-mail (case-insensitive søgning)
    public static User GetUserByEmail(string email)
        {
                return _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

        }
    }
}