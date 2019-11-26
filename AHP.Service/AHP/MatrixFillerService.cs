using AHP.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHP.Service
{
    public class MatrixFiller : IMatrixFiller
    {
        IVectorFiller _vectorFiller;
        public MatrixFiller(IVectorFiller vectorFiller)
        {
            _vectorFiller = vectorFiller;
        }
        public double[] FillMatrix(int dimension, double[] values)
        {
            double[,] M = new double[dimension, dimension];

            for (int i = 0; i < dimension; i++)
            {
                for (int j = i + 1; j < dimension; j++)
                {
                    M[i, j] = (values[i + j - 1] > 0) ? values[i + j - 1] : -(1 / values[i + j - 1]);
                    M[j, i] = 1 / M[i, j];
                }
                M[i, i] = 1;
            }
            return _vectorFiller.NthRoots(dimension, M);
        }
    }
}
