using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using System.Text;

namespace E_CommerceStore.Utilities
{
    public class BaseEmailVerificator : IEmailVerificator
   {
        //one code for each email
        public Dictionary<string,string> EmailCode { get; set; }
        //and one timer for each code
        public List<Timer> timers { get; set; }

        private readonly long codeLifeLength;

        public BaseEmailVerificator(long codeLifeLength)
        {
            EmailCode = new Dictionary<string,string>();
            timers = new List<Timer>();
            this.codeLifeLength = codeLifeLength;
        }

        public void SetCodeForEmail(string email)
        {
            throw new NotImplementedException();
        }

        public void ExpireCode(object code)
        {
            throw new NotImplementedException();
        }
    }
}
