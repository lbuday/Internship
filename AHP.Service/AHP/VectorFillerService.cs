using AHP.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHP.Service
{
    public class VectorFiller: IVectorFiller
    {
        public double[] NthRoots(int dimension, double[,] matrix)
        {
            double[] Nth = new double[dimension];
            for (int i = 0; i < dimension; i++)
            {
                double temp = 1;
                for (int j = 0; j < dimension; j++)
                {
                    temp *= matrix[i, j];
                }
                Nth[i] = Math.Pow(temp, 1.0 / dimension);
            }
            return Weights(dimension, Nth);
        }

        public double[] Weights(int dimension, double[] vector)
        {
            double temp = 0;
            for (int i = 0; i < dimension; i++)
            {
                temp += vector[i];
            }
            double[] weight = new double[dimension];
            for (int i = 0; i < dimension; i++)
            {
                weight[i] = vector[i] / temp;
            }
            return weight;
        }
    }
}
