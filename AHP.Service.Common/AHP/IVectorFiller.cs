using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHP.Service.Common
{
    public interface IVectorFiller
    {
        double[] NthRoots(int dimension, double[,] matrix);
        double[] Weights(int dimension, double[] vector);
    }
}
