using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VisualizationAssociativeQueue.Models
{
    internal static class QueueGenerator
    {
        public static void DoRandomQueueOperation(int countElements, object queue, out int lastElement, int seed = 0, int minValue = 0, int maxValue = 100)
        {
            if (countElements < 1)
                throw new ArgumentException("Argument countOperation must be greater 0.");

            if (queue is not Queue<int>)
                throw new ArgumentException("Argument queue is not Queue<int>.");


            Type typeQueue = queue.GetType();
            MethodInfo methodEnqueue = typeQueue.GetMethod("Enqueue")!;
            MethodInfo methodDequeue = typeQueue.GetMethod("Dequeue")!;

            Random random = new(seed);

            int countElementsPopStack = random.Next(0, countElements);
            int countElementsPushStack = countElements - countElementsPopStack;
            lastElement = 0;

            methodEnqueue.Invoke(queue, [0]);

            for (int i = 0; i < countElementsPopStack; i++)
            {
                var number = random.Next(minValue, maxValue);
                lastElement = number;
                methodEnqueue.Invoke(queue, [number]);
            }

            methodDequeue.Invoke(queue, []);


            for (int i = 0; i < countElementsPushStack; i++)
            {
                var number = random.Next(minValue, maxValue);
                lastElement = number;
                methodEnqueue.Invoke(queue, [number]);
            }
        }
    }
}
