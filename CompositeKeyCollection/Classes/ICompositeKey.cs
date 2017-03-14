namespace KeyCollectionTest.Classes
{
    internal interface ICompositeKey<TKey1, TKey2>
    {
        TKey1 Id { get; }
        TKey2 Name { get; }
    }
}
