using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHP.Service.Common
{
    public interface IAHPService
    {
        double[] CalculatePriortyVector(double[,] matrix);
        double[] FinalCalculate(double[] criteriaPriorityVector, double[,] alternativesPriorityMatrix);
        double CalculateConsistency(double[,] matrix, double[] weight);
    }
}
