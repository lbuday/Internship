using AHP.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHP.Service
{
    public class FinalScoreCalculator: IFinalScoreCalculator
    {
        public double[] CalculateFinalScore(double[,] AlternativeWeights, double[] CriteriaWeights )  
        {
            double[] FinalScore = new double[AlternativeWeights.GetLength(0)];
            for (int i = 0; i < AlternativeWeights.GetLength(0); i++)
            {
                double temp = 0;
                for (int j = 0; j < CriteriaWeights.Length; j++)
                {
                    temp += AlternativeWeights[i, j] * CriteriaWeights[j];
                }
                FinalScore[i] = temp;
            }

            return FinalScore;
        }
    }
}
