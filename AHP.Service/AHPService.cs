using AHP.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHP.Service
{
    class AHPService : IAHPService
    {
        public double[] CalculatePriortyVector(double[,] matrix)
        {
            int n = matrix.GetLength(0);
            double[] vector = new double[n];
            for (int i = 0; i < n; i++)
            {
                vector[i] = 1;
                for (int j = 0; j < n; j++)
                {
                    vector[i] *= matrix[i, j];
                }
                vector[i] = Math.Pow(vector[i], 1.0 / n);
            }
            var sum = vector.Sum();
            for (int i = 0; i < vector.Length; i++)
            {
                vector[i] /= sum;
            }
            return vector;
        }

        public double[] FinalCalculate(double[] criteriaPriorityVector, double[,] alternativesPriorityMatrix)
        {
            var n = alternativesPriorityMatrix.GetLength(0);
            var m = alternativesPriorityMatrix.GetLength(1);
            double[] scores = new double[n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    scores[i] += criteriaPriorityVector[j] * alternativesPriorityMatrix[i, j];
                }
            }
            return scores;
        }
        public double CalculateConsistency(double[,] matrix, double[] weight)
        {
            int dimension = matrix.GetLength(0);
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
                LambdaMax += (ColumnSum[i] * weight[i]);
            }

            double[] Coefficients = new double[9] { 0, 0, 0.58, 0.9, 1.12, 1.24, 1.32, 1.41, 1.45 };

            return dimension <= 2 ? 0 : ((LambdaMax - dimension) / (dimension - 1)) / Coefficients[dimension];
        }
    }
}
