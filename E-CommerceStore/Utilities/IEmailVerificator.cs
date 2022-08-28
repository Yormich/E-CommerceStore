using System.Net.Mail;
using System.Text;

namespace E_CommerceStore.Utilities
{
    public interface IEmailVerificator
    {
        public void SetCodeForEmail(string email);

        public void ExpireCode(object code);

        public string? GetCodeByEmail(string email);

        public void EraseCode(string email);

        public bool Verify(string email, string code);

        public static string FormCode(int length)
        {
            const string codeCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder code = new StringBuilder();
            Random charRetriever = new Random();
            for(int i = 0; i < length; i++)
            {
                code.Append(codeCharacters[charRetriever.Next(0, codeCharacters.Length)]);
            }
            return code.ToString();
        }
    }
}
