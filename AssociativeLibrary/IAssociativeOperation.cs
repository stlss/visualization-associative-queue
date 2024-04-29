namespace AssociativeLibrary
{
    public interface IAssociativeOperation<T>
    {
        public string Name { get; }

        public string Description { get; }

        public Func<T, T, T> Func { get; }
    }
}
