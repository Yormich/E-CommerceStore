namespace E_CommerceStore.Utilities
{
    public class ItemImagePathProvider : IImagePathProvider
    {
        protected const string DefaultItemImage = "DefaultItem.png";

        public string GetImagePath(HttpContext context, string? possibleImageName)
        {
            HttpRequest request = context.Request;
            string imagePath = String.Concat(request.Scheme,
                "://", request.Host.ToUriComponent(),
                request.PathBase.ToUriComponent(),
                "/StaticImages",
                "/ProductImages", "/");
            imagePath += String.IsNullOrEmpty(possibleImageName) ? DefaultItemImage : possibleImageName;
            return imagePath;
        }
    }
}
