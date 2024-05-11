using System.Reflection;

namespace VisualizationAssociativeQueue.Models
{
    /// <summary>
    /// Статический класс, предоставляющий метод, слкучайного заполнения очереди.
    /// </summary>
    internal static class QueueGenerator
    {
        /// <summary>
        /// Случайно заполняет очередь, путём совершения случайных операций (Enqueue и Dequeue).
        /// </summary>
        public static void DoRandomQueueOperation(int countElements, Queue<int> queue, out int lastElement, int seed = 0, int minValue = 0, int maxValue = 100)
        {
            if (countElements < 1)
                throw new ArgumentException("Argument countOperation must be greater 0.");

            // Используем рефлексию, так как методы Enqueue и Dequeue у Queue<T> не виртуальные. 
            Type typeQueue = queue.GetType();
            MethodInfo methodEnqueue = typeQueue.GetMethod("Enqueue")!;
            MethodInfo methodDequeue = typeQueue.GetMethod("Dequeue")!;

            Random random = new(seed);

            int countElementsPopStack = random.Next(0, countElements);
            int countElementsPushStack = countElements - countElementsPopStack;
            lastElement = 0;

            // Добавляем фиктивный элемент
            methodEnqueue.Invoke(queue, [0]);

            // Все добавленные элементы в цикле окажутся в PopStack'е.
            for (int i = 0; i < countElementsPopStack; i++)
            {
                var number = random.Next(minValue, maxValue);
                methodEnqueue.Invoke(queue, [number]);
                lastElement = number;
            }

            // Удаляем фиктивный элемент.
            methodDequeue.Invoke(queue, []);

            // Все добавленные элементы в цикле окажутся в PushStack'е.
            for (int i = 0; i < countElementsPushStack; i++)
            {
                var number = random.Next(minValue, maxValue);
                methodEnqueue.Invoke(queue, [number]);
                lastElement = number;
            }
        }
    }
}
