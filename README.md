<p align="center">
    <img src="Images/Logo.png" alt="Logo">
    <h1 align="center">VisualizationAssociativeQueue</h1>
    <h4 align="center">Визуализация ассоциативной очереди</h4>
</p>

## Об очереди

Обычная очередь имеет несколько реализаций, среди них существует __[реализация на двух стеках](https://neerc.ifmo.ru/wiki/index.php?title=%D0%9E%D1%87%D0%B5%D1%80%D0%B5%D0%B4%D1%8C#.D0.A0.D0.B5.D0.B0.D0.BB.D0.B8.D0.B7.D0.B0.D1.86.D0.B8.D1.8F_.D0.BD.D0.B0_.D0.B4.D0.B2.D1.83.D1.85_.D1.81.D1.82.D0.B5.D0.BA.D0.B0.D1.85)__. Эти два стека обычно называют либо __LeftStack__ и __RightStack__, либо __PushStack__ и __PopStack__. Мы будем использовать второй вариант.

Данную реализацию можно модифицировать, добавив два дополнительных стека __PushAssociativeStack__ и __PopAssociativeStack__.

Значение __AssociativeStack[i]__ (i-ый элемент стека, где __AssociativeStack[0]__ - первый добавленый элемент) равняется __f(Stack[i], AssociativeStack[i - 1])__, где функция __f__ обладает свойством __[ассоциативности](https://ru.wikipedia.org/wiki/%D0%90%D1%81%D1%81%D0%BE%D1%86%D0%B8%D0%B0%D1%82%D0%B8%D0%B2%D0%BD%D0%BE%D1%81%D1%82%D1%8C_(%D0%BC%D0%B0%D1%82%D0%B5%D0%BC%D0%B0%D1%82%D0%B8%D0%BA%D0%B0))__. Первый элемент вычисляется как __AssociativeStack[0]__ = __Stack[0]__.

Таким образом, значение ассоциативной операции на множестве элементов очереди, то есть значение ассоциативной функции __f(Queue[0], Queue[1], ..., Queue[n - 1])__, будет равняться __f(PushAssociativePeek, PopAssociativePeek)__, где аргументами функции являются последние элементы ассоциативных стеков.

Итого, мы получаем очередь, которая умеет вычислять значение ассоциативной операции (например, нахождение максимума или минимума) на множестве элементов очереди за __O(1)__ при условии, что операция тоже совершается за константное время.

## О приложении

Приложение предназначено для взаимодействия с ассоциативной очередью, содержащую неотрицательные числа.

Приложение написано на __WPF'е__ в соответствии c паттерном __MVVM__.

<p align="center">
    <img src="Images/Application.png" alt="Application" width="750">
</p>

### Проекты

<p align="center">
    <img src="Images/Projects.png" alt="Projects" width="335" align="right">
</p>

* __[VisualizationAssociativeQueue](VisualizationAssociativeQueue)__ - WPF-приложение, основной проект.
* __[CollectionLibrary](CollectionLibrary)__ - библиотека коллекций, в ней реализованы ассоциативные и наблюдаемые (реализующие __[INotifyCollectionChanged](https://learn.microsoft.com/ru-ru/dotnet/api/system.collections.specialized.inotifycollectionchanged?view=net-8.0))__ стеки и очереди.
* __[TestCollectionLibrary](TestCollectionLibrary)__ - юнит-тестирование библиотеки коллекций.
* __[AssociativeLibrary](AssociativeLibrary)__ - библиотека, предоставляющая интерфейс __IAssociativeOperation\<T>__.
* __ForeignLibraries__ - библиотеки классов, реализующие __IAssociativeOperation\<int>__.
  * __[ArithmeticOperationLibrary](ArithmeticOperationLibrary)__ - библиотека арифметических операций.
  * __[BitwiseOperationLibrary](BitwiseOperationLibrary)__ - библиотека побитовых операций.

### Интеграция ассоциативных операций

__Основной проект__ заранее не знает об __ForeignLibraries__ (в зависимостях их нет), поэтому они указаны как внешние. 

__VisualizationAssociativeQueue__ получает ассоциативные операции благодаря [конфигу](VisualizationAssociativeQueue/Config.xml), в котором указаны пути до сборок, от которых ожидается содержание классов, реализующие интерфейс __IAssociativeOperation\<int>.__

Таким образом, сторонний разработчик может добавить в приложение свои ассоциативные операции, не имея доступа к исходникам самого приложения, указав в конфиге пути до своих сборок.
