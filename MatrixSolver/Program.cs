using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            DirectoryHelper directoryHelper = DirectoryHelper.Init(args.FirstOrDefault());

            if (directoryHelper != null)
            {              
                foreach (MatrixFileHelper fileHelper in directoryHelper.ReadFiles())
                {
                    fileHelper.PerformOperation();
                    directoryHelper.WriteFile(fileHelper);
                }
            }

            Console.ReadKey();
        }
    }
}
