using Quotes.Models;
using System.Collections.Concurrent;

namespace Quotes.Repositories
{
    public class InMemorySubscriberRepository : ISubscriberRepository
    {
        private readonly ConcurrentDictionary<string, Subscriber> _subscribers = new(StringComparer.OrdinalIgnoreCase);

        public Task<bool> AddAsync(Subscriber subscriber)
        {
            if (subscriber == null || string.IsNullOrEmpty(subscriber.Email))
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(_subscribers.TryAdd(subscriber.Email, subscriber));
        }

        public Task<bool> DeleteAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(_subscribers.TryRemove(email, out _));
        }

        public Task<bool> ExistsAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(_subscribers.ContainsKey(email));
        }

        public Task<IEnumerable<Subscriber>> GetAllAsync()
        {
            return Task.FromResult(_subscribers.Values.AsEnumerable());
        }

        public Task<Subscriber?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return Task.FromResult<Subscriber?>(null);
            }

            _subscribers.TryGetValue(email, out var subscriber);

            return Task.FromResult(subscriber);
        }

        public Task<bool> UpdateAsync(Subscriber subscriber)
        {
            if (subscriber == null || string.IsNullOrEmpty(subscriber.Email))
            {
                return Task.FromResult(false);
            }

            if (!_subscribers.ContainsKey(subscriber.Email))
            {
                return Task.FromResult(false);
            }

            _subscribers.AddOrUpdate(
                subscriber.Email,
                subscriber,
                (key, oldValue) => subscriber
            );

            return Task.FromResult(true);
        }
    }
}
