public class RegistryService
{
    private List<User> usersRegistered = new List<User>();

    public void RegistryUser(User user)
    {
        usersRegistered.Add(user);
    }

    public List<User> RegisteredUsers()
    {
        return usersRegistered;
    }
}
