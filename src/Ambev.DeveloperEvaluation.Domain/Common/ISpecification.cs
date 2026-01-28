namespace Ambev.DeveloperEvaluation.Domain.Common;

public interface ISpecification<T>
{
    bool IsSatisfiedBy(T entity);
}
