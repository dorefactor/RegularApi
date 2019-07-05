namespace RegularApi.Protector
{
    public interface IProtector
    {
        string Protect(string text);
        string Unprotect(string protectedText);
    }
}