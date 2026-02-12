using Quotes.Models;

namespace Quotes.Services
{
    public interface INewsletterService
    {
        Task<IEnumerable<Subscriber>> GetActiveSubscribersAsync();

        Task<OperationResult> SignUpForNewsletterAsync(Subscriber subscriber);

        Task<OperationResult> OptOutFromNewsletterAsync(string email);
    }
}
