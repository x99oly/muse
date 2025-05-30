
public interface IFactory<T, in TArgs>
{
    public T Create(TArgs args);
}