namespace E_CommerceStore.Models
{
    public enum Countries
    {
        Ukraine,
        England,
        Poland,
        Germany,
        USA,
        France
    };
    public abstract class BaseUser
    {
        public long Id { get; set; }
        public string Email { get; set; } = "";

        public string Password { get; set; } = "";
        
        public DateTime? DateOfBirth { get; set; }

        public Countries? country { get; set; }

        public string? AccountImageSource { get; set; } = "";
    }
}
