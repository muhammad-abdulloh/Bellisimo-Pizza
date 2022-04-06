namespace BellisimoPizza.Domain.Entities.Users
{
    public class UserRegister
    {
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
