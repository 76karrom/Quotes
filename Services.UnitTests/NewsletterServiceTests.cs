using NSubstitute;
using Quotes.Models;
using Quotes.Repositories;
using Quotes.Services;

namespace Services.UnitTests
{
    public class NewsletterServiceTests
    {
        [Fact]
        public async Task SignUpForNewsletterAsync_WithValidSubscriber_ReturnsSuccess()
        {
            // Arrange
            var subscriber = new Subscriber { Name = "Test User", Email = "user@example.com" };

            var repository = Substitute.For<ISubscriberRepository>();         
            repository.ExistsAsync(subscriber.Email).Returns(false);
            repository.AddAsync(subscriber).Returns(true);

            var service = new NewsletterService(repository);

            // Act
            var result = await service.SignUpForNewsletterAsync(subscriber);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Contains("You'll receive updates soon", result.Message);
        }

        [Fact]
        public async Task SignUpForNewsletterAsync_WithDuplicateEmail_ReturnsFailure()
        {
            // Arrange
            var subscriber = new Subscriber { Name = "Nisse", Email = "pelle@example.com" };

            var repository = Substitute.For<ISubscriberRepository>();
            repository.ExistsAsync(subscriber.Email).Returns(true);

            var service = new NewsletterService(repository);

            // Act
            var result = await service.SignUpForNewsletterAsync(subscriber);

            // Assert

            await repository.Received(1).ExistsAsync(subscriber.Email);

            await repository.DidNotReceive().AddAsync(Arg.Any<Subscriber>());

            Assert.False(result.IsSuccess);
            
            Assert.Contains("You are already subscribed to our newsletter", result.Message);
        }

        [Fact]
        public async Task OptOutFromNewsletterAsync_WithExistingEmail_ReturnsSuccess()
        {
            // Arrange
            var subscriber = new Subscriber { Name = "Test User", Email = "pelle@example.com" };

            var repository = Substitute.For<ISubscriberRepository>();
            repository.GetByEmailAsync(subscriber.Email).Returns(subscriber);
            repository.DeleteAsync(subscriber.Email).Returns(true);

            var service = new NewsletterService(repository);

            // Act
            var result = await service.OptOutFromNewsletterAsync(subscriber.Email);

            // Assert

            await repository.Received(1).GetByEmailAsync(subscriber.Email);

            await repository.Received(1).DeleteAsync(subscriber.Email);

            Assert.True(result.IsSuccess);
            
            Assert.Contains("You have been successfully removed from our newsletter. We're sorry to see you go!", result.Message);
        }

        [Fact]
        public async Task OptOutFromNewsletterAsync_WithNonexistentEmail_ReturnsFailure()
        {
            // Arrange
            var subscriber = new Subscriber { Name = "Test User", Email = "notsignedup@example.com" };

            var repository = Substitute.For<ISubscriberRepository>();
            repository.GetByEmailAsync(subscriber.Email).Returns(null as Subscriber);

            var service = new NewsletterService(repository);

            // Act
            var result = await service.OptOutFromNewsletterAsync(subscriber.Email);

            // Assert

            await repository.Received(1).GetByEmailAsync(subscriber.Email);

            await repository.DidNotReceive().DeleteAsync(Arg.Any<string>());

            Assert.False(result.IsSuccess);
            
            Assert.Contains("We couldn't find your subscription in our system", result.Message);
        }

        [Fact]
        public async Task GetActiveSubscribersAsync_ReturnsAllSubscribers()
        {
            // Arrange

            var subscribers = new List<Subscriber>() 
            { 
                new Subscriber { Name = "pelle", Email = "pelle@example.com" }, 
                new Subscriber { Name = "nisse", Email = "nisse@example.com" } 
            };

            var repository = Substitute.For<ISubscriberRepository>();
            repository.GetAllAsync().Returns(subscribers);

            var service = new NewsletterService(repository);

            // Act
            var listOfSubscribers = await service.GetActiveSubscribersAsync();

            // Assert

            await repository.Received(1).GetAllAsync();

            Assert.True(listOfSubscribers.Count() == 2); 

            Assert.Contains(subscribers, s => s.Email == "pelle@example.com");

            Assert.Contains(subscribers, s => s.Email == "nisse@example.com");
        }
    }
}
