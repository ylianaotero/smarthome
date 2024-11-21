using IBusinessLogic;
using System.Reflection;

namespace BusinessLogic
{
    public class ModelValidatorLogic : IModelValidatorLogic
    {
        private const string ImportersDirectory = "ActiveModels";
        private const string DllExtension = "dll";

        public List<IModelValidator> GetAllValidators()
        {
            string baseDirectory = AppContext.BaseDirectory;
        
            string codigoFuenteDirectory = Directory.GetParent(baseDirectory)?.Parent?.Parent?.Parent?.Parent?.Parent?.FullName ?? throw new InvalidOperationException("No se pudo encontrar el directorio 'Codigo Fuente'.");
        
            string directoryOfDlll = Path.Combine(codigoFuenteDirectory, ImportersDirectory);
            
            string dllPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), directoryOfDlll));
            string[] filePaths = Directory.GetFiles(dllPath ); 
            
            string[] dllFiles = filePaths.Where(file => FileIsDll(file)).ToArray();
            
            if (dllFiles.Length == 0)
            {
                throw new FileNotFoundException($"No se encontraron archivos DLL en el directorio '{ImportersDirectory}'.");
            }
            
            List<IModelValidator> availableImporters = new List<IModelValidator>();

            foreach (string file in filePaths)
            {
                if (FileIsDll(file))
                {
                    FileInfo dllFile = new FileInfo(file);
                    Assembly myAssembly = Assembly.LoadFile(dllFile.FullName);

                    foreach (Type type in myAssembly.GetTypes())
                    {
                        if (ImplementsRequiredInterface(type)) 
                        {
                            IModelValidator instance = (IModelValidator)Activator.CreateInstance(type);
                            availableImporters.Add(instance);
                        }
                    }
                }
            }
            return availableImporters;
        }

        private bool FileIsDll(string file)
        {
            return file.EndsWith(DllExtension);
        }

        public bool ImplementsRequiredInterface(Type type)
        {
            return typeof(IModelValidator).IsAssignableFrom(type) && !type.IsInterface;
        }
    }
}