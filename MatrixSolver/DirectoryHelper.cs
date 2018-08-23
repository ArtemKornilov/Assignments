using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixSolver
{
    public class DirectoryHelper
    {

        private readonly string _directory;
        private static ConsoleLogger _logger;
        
        private DirectoryHelper(string directory)
        {
            _directory = directory;
            _logger.Log(string.Format("Working with {0} directory", _directory));
        }

        public static DirectoryHelper Init(string directory)
        {
            _logger = new ConsoleLogger(typeof(DirectoryHelper).Name);

            if (string.IsNullOrWhiteSpace(directory))
            {
                _logger.Error("Directory is not specified");
                return null;
            }
            else if (!Directory.Exists(directory))
            {
                _logger.Error(string.Format("Directory {0} is not exist", directory));
                return null;
            }
            else
            {
                return new DirectoryHelper(directory);
            }
        }

        public IEnumerable<MatrixFileHelper> ReadFiles()
        {           
            IList<string> fileNames = Directory.GetFiles(_directory);
            int filesCount = fileNames.Count;

            _logger.Log(string.Format("Reading {0} files", filesCount));

            int currentFile = 0;
            foreach (string name in fileNames)
            {
                _logger.Log(string.Format("Reading file {0} out of {1}: {2}", ++currentFile, filesCount, name));

                if (!Path.GetExtension(name).Equals(".txt"))
                {
                    _logger.Error(string.Format("File {0} doesn't have the .txt extension", name));
                }
                else
                {
                    var fileHelper = MatrixFileHelper.TryInit(name, File.ReadAllText(name));
                    if (fileHelper != null)
                    {
                        yield return fileHelper;
                    }
                }
            }            
        }

        public void WriteFile(MatrixFileHelper helper)
        {
            Directory.SetCurrentDirectory(_directory);

            string resultFileName = string.Concat(
                Path.GetFileNameWithoutExtension(helper.FileName),
                "_result",
                Path.GetExtension(helper.FileName)
                );

            _logger.Log(string.Format("Writing file {0} to directory {1}", resultFileName, _directory));

            using (var fileStream = File.Create(resultFileName))
            {
                using (var writer = new StreamWriter(fileStream))
                {
                    writer.WriteLine(helper.Operation.ToString());
                    writer.WriteLine(StringHelper.OperationDelimiter);

                    int currentMatrix = 0;
                    foreach (Matrix matrix in helper.Matrixes)
                    {
                        _logger.Log(string.Format("Writing matrix {0} out of {1}", ++currentMatrix, helper.Matrixes.Count));

                        writer.WriteLine(matrix.ToString());
                        writer.WriteLine(StringHelper.MatrixDelimiter);
                    }
                }
            }
        }

    }
}
