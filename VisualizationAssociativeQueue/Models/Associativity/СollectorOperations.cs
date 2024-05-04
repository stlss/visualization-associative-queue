using AssociativeLibrary;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace VisualizationAssociativeQueue.Models.Associativity
{
    /// <summary>
    /// Статический класс, предоставляющий метод, возвращающий список ассоциативных операций.
    /// </summary>
    internal static class СollectorOperations
    {
        /// <summary>
        /// Путь до сборки VisualizationAssociativeQueue.dll.
        /// </summary>
        private static readonly string s_pathAssembly = Assembly.GetExecutingAssembly().Location;

        /// <summary>
        /// Конфиг, содержащий сборки и их пути, от которых ожидается содержание классов, реализующих интерфейс IAssociativeOperation<int>.
        /// </summary>
        private static readonly XDocument s_config = ReadConfig();


        /// <summary>
        /// Возвращает экземпляры классов, приведённые к IAssociativeOperation<int>, сборок, указанных в конфиге, реализующие интерфейс IAssociativeOperation<int>.
        /// </summary>
        public static List<IAssociativeOperation<int>> GetAssociativeOperations()
        {
            var pathAssemblies = s_config.Root!.Elements().
                Where(element => element.Name == "Assembly" && element.Attribute("Path") != null).
                Select(assembly => assembly.Attribute("Path")!.Value).
                Distinct();

            var assemblies = pathAssemblies.Where(path => Path.GetExtension(path) == ".dll" && Path.Exists(path)).
                Select(Assembly.LoadFrom);

            var types = assemblies.SelectMany(assembly => assembly.GetTypes()).
                Where(type => type.GetInterfaces().Any(interface_ =>
                    interface_.Name == "IAssociativeOperation`1" &&
                    interface_.IsGenericType &&
                    interface_.GenericTypeArguments.Any(argument => argument.Name == "Int32")));

            var operations = types.Select(type => type.GetConstructor([])).
                Select(constructor => constructor!.Invoke([])).
                Select(operation => (IAssociativeOperation<int>)operation).
                DistinctBy(operation => operation.Name).
                OrderBy(operation => operation.Name);

            return operations.ToList();
        }


        /// <summary>
        /// Считывает xml-документ конфига, в случае его отсутствия или некорректности создаёт новый.
        /// </summary>
        private static XDocument ReadConfig()
        {
            XDocument config;
            string nameConfig = "Config.xml";

            try
            {
                config = XDocument.Load(nameConfig);
            }
            catch (XmlException)
            {
                config = CreateConfig();
                config.Save(nameConfig);

                return config;
            }

            if (config.Root!.Name != "Assemblies")
            {
                config.Root!.Name = "Assemblies";
                config.Save(nameConfig);
            }

            if (!config.Root!.Elements().Any(assembly => assembly.Attribute("Path")?.Value == s_pathAssembly))
            {
                config.Root.Add(new XElement("Assembly", new XAttribute("Path", s_pathAssembly)));
                config.Save(nameConfig);
            }

            return config;
        }

        /// <summary>
        /// Создаёт xml-документ конфига.
        /// </summary>
        private static XDocument CreateConfig()
        {
            var config = new XDocument(
                    new XDeclaration("1.0", "utf-8", "true"),
                    new XComment("В случае отсутствия конфига или его некорректности, он создаться заново"),
                    new XElement("Assemblies",
                        new XComment("В случае отсутствия сборки VisualizationAssociativeQueue.dll, она автоматически добавится"),
                        new XElement("Assembly", 
                            new XAttribute("Name", "VisualizationAssociativeQueue.dll"), 
                            new XAttribute("Path", s_pathAssembly))));

            return config;
        }
    }
}
