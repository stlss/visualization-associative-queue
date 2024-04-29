namespace CollectionLibrary.Associative
{
    /// <summary>
    /// Статический класс, содержащий методы для вызова исключений.
    /// </summary>
    internal static class ExceptionWizard
    {
        /// <summary>
        /// Выбрасывает исключение - пустая коллекция.
        /// </summary>
        public static void ThrowEmptyAssociativeCollection<T>(IAssociativeCollection<T> collection)
        {
            throw new InvalidOperationException($"{collection.GetType().Name} is empty.");
        }

        /// <summary>
        /// Выбрасывает исключение - отсутствие ассоциативной операции.
        /// </summary>
        public static void ThrowNullOperationAssociativeCollection<T>(IAssociativeCollection<T> collection)
        {
            throw new InvalidOperationException($"{collection.GetType().Name}.Operation is null.");
        }
    }
}
