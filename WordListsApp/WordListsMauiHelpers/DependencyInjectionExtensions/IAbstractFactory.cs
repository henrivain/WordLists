namespace WordListsMauiHelpers.DependencyInjectionExtensions;
public interface IAbstractFactory<T>
{
    T Create();
}