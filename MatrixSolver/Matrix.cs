using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixSolver
{
    public class Matrix
    {

        public int n { get; private set;}
        public int m { get; private set;}
        public List<List<int>> matrix { get; private set; }

        public Matrix(List<List<int>> _matrix)
        {
            matrix = _matrix;
            n = matrix.Count;
            m = matrix.First().Count;
        }

        public Matrix(int _n, int _m)
        {
            n = _n;
            m = _m;

            matrix = new List<List<int>>(n);
            for (int i = 0; i < n; ++i)
            {
                matrix.Add(new List<int>(m));
            }
        }

        public static Matrix operator +(Matrix left, Matrix right)
        {
            if (!CanBeAddedOrSubstructed(left, right))
            {
                return null;
            }

            int n = left.n;
            int m = left.m;
            Matrix result = new Matrix(n, m);
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < m; ++j)
                {
                    result.matrix[i].Add(left.matrix[i][j] + right.matrix[i][j]);
                }
            }

            return result;
        }

        public static Matrix operator -(Matrix left, Matrix right)
        {
            if (!CanBeAddedOrSubstructed(left, right))
            {
                return null;
            }

            int n = left.n;
            int m = left.m;
            Matrix result = new Matrix(n, m);
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < m; ++j)
                {
                    result.matrix[i].Add(left.matrix[i][j] - right.matrix[i][j]);
                }
            }

            return result;
        }

        public static Matrix operator *(Matrix left, Matrix right)
        {
            if (!CanBeMultiplied(left, right))
            {
                return null;
            }

            int n = left.n;
            int l = left.m;
            int m = right.m;

            Matrix result = new Matrix(n, m);
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < m; ++j)
                {
                    result.matrix[i].Add(0);

                    for (int k = 0; k < l; ++k)
                    {
                        result.matrix[i][j] = result.matrix[i][j] + left.matrix[i][k] * right.matrix[k][j];   
                    }
                    
                }
            }

            return result;
        }
        public static Matrix Transpose(Matrix matrixToTranspose)
        {            
            int n = matrixToTranspose.n;
            int m = matrixToTranspose.m;
            Matrix result = new Matrix(m, n);

            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < m; ++j)
                {
                    result.matrix[j].Add(matrixToTranspose.matrix[i][j]);
                }
            }

            return result;
        }

        public static bool CanBeMultiplied(Matrix left, Matrix right)
        {
            return left.m == right.n;
        }

        public static bool CanBeAddedOrSubstructed(Matrix left, Matrix right)
        {
            return left.n == right.n && left.m == right.m;
        }

        public static Matrix Parse(string matrixString)
        {
            if (string.IsNullOrWhiteSpace(matrixString))
            {
                return null;
            }

            //Split matrix string to rows and casting to int
            List<List<int>> rows = matrixString
                .Split(new string[] { StringHelper.RowDelimiter }, StringSplitOptions.RemoveEmptyEntries)
                .Select(row => row.Split(' ').Select(item => int.Parse(item)).ToList())
                .ToList();

            int rowSize = rows
                .FirstOrDefault()
                .Count;

            //Check if all rows are the same size
            foreach (var row in rows)
            {
                if (row.Count != rowSize)
                {
                    return null;
                }
            }        

            return new Matrix(rows);
        }

        override public string ToString()
        {
            string result = string.Empty;
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < m; ++j)
                {
                    result = string.Concat(result, matrix[i][j].ToString());

                    if (j != m - 1)
                    {
                        result = string.Concat(result, ' ');
                    }
                }

                if (i != n - 1)
                {
                    result = string.Concat(result, StringHelper.RowDelimiter);
                }
            }

            return result;
        }

    }
}
