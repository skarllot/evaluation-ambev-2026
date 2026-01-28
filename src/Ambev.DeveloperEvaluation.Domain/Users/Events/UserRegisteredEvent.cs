using Ambev.DeveloperEvaluation.Domain.Users.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Users.Events
{
    public class UserRegisteredEvent
    {
        public User User { get; }

        public UserRegisteredEvent(User user)
        {
            User = user;
        }
    }
}
