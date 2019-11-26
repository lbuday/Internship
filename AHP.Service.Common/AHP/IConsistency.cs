using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHP.Service.Common
{
    public interface IConsistency
    {
        double CalculateConsistency(int dimension, double[,] matrix);
    }
}
