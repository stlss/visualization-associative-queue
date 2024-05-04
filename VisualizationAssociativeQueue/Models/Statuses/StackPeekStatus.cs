namespace VisualizationAssociativeQueue.Models.Statuses
{
    internal enum StackPeekStatus
    {
        New,    // Недавно изменённая верхушка стека.
        Old,    // Давно изменённая верхушка стека.
        Missing // Отсутствующая верхушка стека (стек пустой).
    }
}
