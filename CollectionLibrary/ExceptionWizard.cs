using CollectionLibrary.Associative;
using System.Reflection;

namespace CollectionLibrary
{
    /// <summary>
    /// Статический класс, содержащий методы для вызова исключений.
    /// </summary>
    internal static class ExceptionWizard
    {
        /// <summary>
        /// Выбрасывает исключение - пустая коллекция.
        /// </summary>
        public static void ThrowEmptyCollection<T>(IEnumerable<T> collection)
        {
            throw new InvalidOperationException($"{collection.GetType().Name} is empty.");
        }

        /// <summary>
        /// Выбрасывает исключение - обращение к null свойству.
        /// </summary>
        public static void ThrowNullPropertyCollection<T>(IEnumerable<T> collection, PropertyInfo property)
        {
            throw new InvalidOperationException($"{collection.GetType().Name}.{property.Name} is null.");
        }
    }
}
