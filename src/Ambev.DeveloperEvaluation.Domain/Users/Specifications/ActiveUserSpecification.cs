using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Users.Entities;
using Ambev.DeveloperEvaluation.Domain.Users.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Users.Specifications;

public class ActiveUserSpecification : ISpecification<User>
{
    public bool IsSatisfiedBy(User user)
    {
        return user.Status == UserStatus.Active;
    }
}