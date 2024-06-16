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
        private static readonly string _s_nameConfig = "Config.xml";

        /// <summary>
        /// Путь до сборки VisualizationAssociativeQueue.dll.
        /// </summary>
        private static readonly string _s_pathAssembly = Assembly.GetExecutingAssembly().Location;

        /// <summary>
        /// Конфиг, содержащий сборки и их пути, от которых ожидается содержание классов, реализующих интерфейс IAssociativeOperation<int>.
        /// </summary>
        private static readonly XDocument _s_config = ReadConfig();


        /// <summary>
        /// Возвращает экземпляры классов, приведённые к IAssociativeOperation<int>, сборок, указанных в конфиге, реализующие интерфейс IAssociativeOperation<int>.
        /// </summary>
        public static List<IAssociativeOperation<int>> GetAssociativeOperations()
        {
            var pathAssemblies = _s_config.Root!.Elements().
                Where(element => element.Name == "Assembly" && element.Attribute("Path") != null).
                Select(assembly => assembly.Attribute("Path")!.Value).
                Distinct();

            var assemblies = pathAssemblies.Where(path => Path.GetExtension(path) == ".dll" && Path.Exists(path)).
                Select(Assembly.LoadFrom);

            var typesAssemblies = assemblies.Select(assembly => 
                assembly.GetTypes().Where(type => type.GetInterfaces().
                    Any(interface_ =>
                        interface_.Name == "IAssociativeOperation`1" &&
                        interface_.IsGenericType &&
                        interface_.GenericTypeArguments.Any(argument => argument.Name == "Int32"))));

            var operationsAssemblies = typesAssemblies.Select(types => 
                types.Select(type => type.GetConstructor([])).
                    Select(constructor => constructor!.Invoke([])).
                    Select(operation => (IAssociativeOperation<int>)operation).
                    DistinctBy(operation => operation.Name).
                    OrderBy(operation => operation.Name));

            return operationsAssemblies.SelectMany(operations => operations).ToList();
        }


        /// <summary>
        /// Считывает xml-документ конфига, в случае его отсутствия или некорректности создаёт новый.
        /// </summary>
        private static XDocument ReadConfig()
        {
            XDocument config;

            try
            {
                config = XDocument.Load(_s_nameConfig);
            }
            catch (FileNotFoundException)
            {
                config = CreateConfig();
                config.Save(_s_nameConfig);

                return config;
            }
            catch (XmlException)
            {
                config = CreateConfig();
                config.Save(_s_nameConfig);

                return config;
            }

            if (config.Root!.Name != "Assemblies")
            {
                config.Root!.Name = "Assemblies";
                config.Save(_s_nameConfig);
            }


            bool isAssemblyVisualizationAssociativeQueue = config.Root!.Elements().
                Any(assembly => assembly.Name == "Assembly" &&
                    assembly.Attribute("Path")?.Value == _s_pathAssembly);

            if (!isAssemblyVisualizationAssociativeQueue)
            {
                config.Root.Add(new XElement("Assembly", new XAttribute("Path", _s_pathAssembly)));
                config.Save(_s_nameConfig);
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
                    new XElement("Assemblies",
                        new XElement("Assembly",  
                            new XAttribute("Path", _s_pathAssembly))));

            return config;
        }
    }
}
