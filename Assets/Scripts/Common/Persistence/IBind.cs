namespace Common.Persistence
{
    public interface IBind<T> where T : ISaveable
    {
        void Bind(T data);
    }
}
