using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using System.Text;

namespace E_CommerceStore.Utilities
{
    public class BaseEmailVerificator : IEmailVerificator
   {
        //one code for each email
        protected Dictionary<string,string> EmailCode { get; set; }
        //and one timer for each code
        protected Dictionary<string,Timer> timers { get; set; }

        private readonly long codeLifeLength;

        public BaseEmailVerificator(long codeLifeLength)
        {
            EmailCode = new Dictionary<string,string>();
            timers = new Dictionary<string, Timer>();
            this.codeLifeLength = codeLifeLength;
        }

        public void SetCodeForEmail(string email)
        {
            if(EmailCode.ContainsKey(email))
            {
                EmailCode[email] = IEmailVerificator.FormCode(6);
                timers[email] = new Timer(ExpireCode,email,codeLifeLength,Timeout.Infinite);
            }
            else
            {
                EmailCode.Add(email, IEmailVerificator.FormCode(6));
                timers.Add(email, new Timer(ExpireCode,email,codeLifeLength,Timeout.Infinite));
            }
        }
        

        public void ExpireCode(object? email)
        {
            if(email != null && email is string)
            {
                string key = (string)email;

                if(EmailCode.ContainsKey(key) && timers.ContainsKey(key))
                {
                    EmailCode.Remove(key);
                    timers.Remove(key);
                }
            }
        }

        public bool Verify(string email, string code)
        {
            return EmailCode.ContainsKey(email) && code == EmailCode[email];
        }

        public string? GetCodeByEmail(string email)
        {
            if(EmailCode.ContainsKey(email))
            {
                return EmailCode[email];
            }
            return null;
        }

        public void EraseCode(string email)
        {
            if(EmailCode.ContainsKey(email) && timers.ContainsKey(email))
            {
                EmailCode.Remove(email);
                timers.Remove(email);
            }
        }
    }
}
