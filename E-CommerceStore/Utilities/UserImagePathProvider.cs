namespace E_CommerceStore.Utilities
{
    public class UserImagePathProvider : IImagePathProvider
    {
        protected const string DefaultUserImage = "DefaultUser.png";
        public string GetImagePath(HttpContext context, string? possibleImageName)
        {
            HttpRequest request = context.Request;
            string imagePath = String.Concat(request.Scheme,
                "://", request.Host.ToUriComponent(),
                request.PathBase.ToUriComponent(),
                "/StaticImages",
                "/UserImages", "/");
            imagePath += String.IsNullOrEmpty(possibleImageName) ? DefaultUserImage : possibleImageName;
            return imagePath;
        }
    }
}
