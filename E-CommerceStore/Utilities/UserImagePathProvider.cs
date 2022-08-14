namespace E_CommerceStore.Utilities
{
    public class UserImagePathProvider : IImagePathProvider
    {
        protected const string DefaultUserImage = "DefaultUser.png";
        public string GetImagePath(HttpContext context, string? possibleImageName)
        {
            throw new NotImplementedException();
        }
    }
}
