namespace E_CommerceStore.Utilities
{
    public interface IFileProvider
    {
        private string GetContentType(string filename)
        {
            var types = GetExtensions();
            string extension = Path.GetExtension(filename).ToLowerInvariant();
            return types[extension];
        }

        private Dictionary<string,string> GetExtensions()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},  
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
    }
}
