using AssociativeLibrary;

namespace CollectionLibrary.Associative
{
    /// <summary>
    /// Обобщённый интерфейс коллекций, предоставляющих получение значения ассоциативной операции на множестве всех элементов коллекции.
    /// </summary>
    public interface IAssociativeCollection<T>
    {
        /// <summary>
        /// Ассоциативная операция, например нахождение максимума.
        /// </summary>
        public IAssociativeOperation<T>? Operation { get; set; }

        /// <summary>
        /// Возвращает результат ассоциативной операции, например максимальный элемент в коллекции.
        /// </summary>
        public T GetResultAssociativeOperation();

        /// <summary>
        /// Пытается вернуть результат ассоциативной операции, например максимальный элемент в коллекции.
        /// </summary>
        public bool TryGetResultAssociativeOperation(out T? result);
    }
}
