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
            Console.WriteLine(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));
            string[] filePaths = Directory.GetFiles(ImportersDirectory); 
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