namespace E_CommerceStore.Utilities
{
    public interface IImagePathProvider
    {
        public string GetImagePath(HttpContext context, string? possibleImageName);
    }
}
