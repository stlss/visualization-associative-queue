namespace AssociativeLibrary
{
    /// <summary>
    /// Интерфейс для ассоциативных операций, предоставляющий имя, описание и делегат. 
    /// </summary>
    public interface IAssociativeOperation<T>
    {
        public string Name { get; }

        public string Description { get; }

        public Func<T, T, T> Func { get; }
    }
}
