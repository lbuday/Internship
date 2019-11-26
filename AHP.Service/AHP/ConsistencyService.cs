using AHP.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHP.Service
{
    public class Consistency: IConsistency
    {
        IVectorFiller _vectorFiller;
        public Consistency(IVectorFiller vectorFiller)
        {
            _vectorFiller = vectorFiller;
        }
        public double CalculateConsistency(int dimension, double[,] matrix)
        {
            double[] NthRoot =_vectorFiller.NthRoots(dimension, matrix);
            double[] Weight = _vectorFiller.Weights(dimension, NthRoot);

            double[] ColumnSum = new double[dimension];
            for (int j = 0; j < dimension; j++)
            {
                double temp = 0;
                for (int i = 0; i < dimension; i++)
                {
                    temp += matrix[i, j];
                }
                ColumnSum[j] = temp;
            }

            double LambdaMax = 0;

            for (int i = 0; i < dimension; i++)
            {
                LambdaMax += (ColumnSum[i] * Weight[i]);
            }

            double[] Coefficients = new double[9] { 0, 0, 0.58, 0.9, 1.12, 1.24, 1.32, 1.41, 1.45 };

            return dimension <= 2 ? 0 : ((LambdaMax - dimension) / (dimension - 1)) / Coefficients[dimension];
        }
    }
}
