using System.Numerics;
using GeometricAlgebra.ProductAccelerators;

namespace GeometricAlgebra.Common.ProductAccelerators;

public class BoringProductAcceleratorEngine<TValue> : IProductAcceleratorEngine<TValue>
    where TValue : IMultiplyOperators<TValue, TValue, TValue>
{
    public void Execute(TValue[] leftArray, TValue[] rightArray, TValue[] productArray)
    {
        for (var index = 0; index < productArray.Length; index++)
        {
            productArray[index] = leftArray[index] * rightArray[index];
        }
    }
}
