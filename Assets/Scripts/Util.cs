
public static class ObjectExtensions
{
    public static bool TryDowncast<T>(this object obj, out T castObj)
    {
        castObj = default;
        if (typeof(T).IsAssignableFrom(obj.GetType()))
        {
            castObj = (T)obj;
            return true;
        }
        return false;
    }
}
