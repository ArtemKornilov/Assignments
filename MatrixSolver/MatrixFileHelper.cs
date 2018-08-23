using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixSolver
{
    public class MatrixFileHelper
    {
        private static ConsoleLogger _logger;

        public MatrixOperations Operation { get; private set; }
        public IList<Matrix> Matrixes { get; private set; }
        public string FileName { get; private set; }       

        private MatrixFileHelper(string _fileName, MatrixOperations _operation, IList<Matrix> _matrixes)
        {
            FileName = _fileName;
            Operation = _operation;
            Matrixes = _matrixes;
        }

        public static MatrixFileHelper TryInit(string fileName, string fileBody)
        {
            _logger = new ConsoleLogger(typeof(MatrixFileHelper).Name);
            
            //Get operation line
            string operationLine = fileBody.Split('\n')
                .SkipWhile(line => line.StartsWith(StringHelper.Dash) || line.StartsWith(StringHelper.LongDash) || string.IsNullOrWhiteSpace(line))
                .FirstOrDefault();

            if (Enum.TryParse(operationLine, out MatrixOperations operation))
            {
               
                //Split file contents to matrixes, remove borderline dashes and skip to first matrix
                string[] matrixStrings = fileBody
                    .Split(new string[] { StringHelper.MatrixDelimiter }, StringSplitOptions.RemoveEmptyEntries)
                    .Where(line => !line.StartsWith(StringHelper.Dash) && !line.StartsWith(StringHelper.LongDash))
                    .SkipWhile(line => !char.IsDigit(line[0]))
                    .ToArray();
                
                IList<Matrix> matrixes = TryReadMatrixes(matrixStrings);

                if (matrixes == null)
                {
                    _logger.Error("Either there is no matrixes in file, or matrixes dimensions are not correct");
                    return null;
                }
               
                bool allMatrixesCompatibleForOperation = true;

                if (operation == MatrixOperations.add || operation == MatrixOperations.substruct)
                {
                    //Check correctnes of matrixes dimensions
                     allMatrixesCompatibleForOperation = matrixes
                        .Take(matrixes.Count - 1)
                        .Select((matrix, index) => index < matrixes.Count - 1 && Matrix.CanBeAddedOrSubstructed(matrix, matrixes[index + 1]))
                        .All(compatible => compatible == true);         
                    
                    if (matrixes.Count <= 1)
                    {
                        _logger.Log(string.Format("There is no need to perform operation {0} with only 1 matrix", operation.ToString()));
                        return null;
                    }
                }

                if (operation == MatrixOperations.multiply)
                {
                    //Check correctnes of matrixes dimensions
                    allMatrixesCompatibleForOperation = matrixes
                        .Take(matrixes.Count - 1)
                        .Select((matrix, index) => index < matrixes.Count - 1 && Matrix.CanBeMultiplied(matrix, matrixes[index + 1]))
                        .All(compatible => compatible == true);

                    if (matrixes.Count == 1)
                    {
                        _logger.Log(string.Format("There is no need to perform operation {0} with only 1 matrix", operation.ToString()));
                        return null;
                    }
                }                

                if (!allMatrixesCompatibleForOperation)
                {
                    _logger.Error("Not all matrixes are compatible for operation " + operation.ToString());
                    return null;
                }


                return new MatrixFileHelper(fileName, operation, matrixes);                
            }
            else
            {

            }

            return null;            
        }

        public static IList<Matrix> TryReadMatrixes(string[] matrixStrings)
        {
            _logger.Log(string.Format("Parsing {0} matrixes", matrixStrings.Length));

            IList<Matrix> matrixes = new List<Matrix>();

            int currentMatrix = 0;
            foreach (string matrixString in matrixStrings)
            {
                _logger.Log(string.Format("Parsing matrix {0} out of {1}", ++currentMatrix, matrixStrings.Length));

                Matrix parsedMatrix = Matrix.Parse(matrixString);
                if (parsedMatrix != null)
                {
                    matrixes.Add(parsedMatrix);
                }
                else
                {
                    return null;
                }
            }

            return matrixes;
        }                    

        public void PerformOperation()
        {
            _logger.Log(string.Format("Applying operation {0} to {1} matrixes", Operation.ToString(), Matrixes.Count));

            //Map/Reduce
            switch (Operation)
            {
                case MatrixOperations.add:
                    Matrixes = new List<Matrix>()
                    {
                        Matrixes.Aggregate((result, currentMatrix) => result + currentMatrix)
                    };
                    break;

                case MatrixOperations.substruct:
                    Matrixes = new List<Matrix>()
                    {
                        Matrixes.Aggregate((result, currentMatrix) => result - currentMatrix)
                    };
                    break;

                case MatrixOperations.multiply:
                    Matrixes = new List<Matrix>()
                    {
                        Matrixes.Aggregate((result, currentMatrix) => result * currentMatrix)
                    };
                    break;

                case MatrixOperations.transpose:
                    Matrixes = Matrixes.Select(matrix => Matrix.Transpose(matrix)).ToList();
                    break;
            }
        }        

    }
}
