namespace DataProtection.Protectors
{
    public interface IProtector
    {
        T ProtectObject<T>(T obj);
        T UnprotectObject<T>(T obj);

        string ProtectText(string text);
        string UnprotectText(string text);
    }
}