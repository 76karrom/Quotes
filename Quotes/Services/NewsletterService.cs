using Quotes.Models;
using System.Xml.Linq;

namespace Quotes.Services
{
    public class NewsletterService : INewsletterService
    {
        private static readonly List<Subscriber> _subscribers = [];

        public async Task<OperationResult> SignUpForNewsletterAsync(Subscriber subscriber)
        {
            // Simulate a long running operation
            return await Task.Run(() =>
            {
                if (subscriber == null || string.IsNullOrWhiteSpace(subscriber.Email))
                {
                    return OperationResult.Failure("Invalid subscriber information.");
                }

                if (IsAlreadySubscribed(subscriber.Email))
                {
                    return OperationResult.Failure("You are already subscribed to our newsletter.");
                }

                _subscribers.Add(subscriber);

                return OperationResult.Success($"Welcome to our newsletter, {subscriber.Name}! You'll receive updates soon.");
            });
        }

        public async Task<OperationResult> OptOutFromNewsletterAsync(string email)
        {
            // Simulate a long running operation
            return await Task.Run(() =>
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return OperationResult.Failure("Invalid email address.");
                }

                var subscriber = FindSubscriberByEmail(email);

                if (subscriber == null)
                {
                    return OperationResult.Failure("We couldn't find your subscription in our system.");
                }

                _subscribers.Remove(subscriber);

                return OperationResult.Success("You have been successfully removed from our newsletter. We're sorry to see you go!");
            });
        }

        public async Task<IEnumerable<Subscriber>> GetActiveSubscribersAsync()
        {
            // Simulate a long running operation and return the list of subscribers
            return await Task.Run(() => _subscribers.ToList());
        }

        private static bool IsAlreadySubscribed(string email)
        {
            return _subscribers.Any(s => s.Email!.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        private static Subscriber? FindSubscriberByEmail(string email)
        {
            return _subscribers.FirstOrDefault(s =>
                s.Email!.Equals(email, StringComparison.OrdinalIgnoreCase));
        }
    }
}
