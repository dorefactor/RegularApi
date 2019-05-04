namespace RegularApi.Transformers
{
    public interface ITransformer<View, Model>
    {
        Model Transform(View view);
    }
}
