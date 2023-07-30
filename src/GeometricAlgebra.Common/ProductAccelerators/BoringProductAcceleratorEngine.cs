using GeometricAlgebra.ProductAccelerators;

namespace GeometricAlgebra.Common.ProductAccelerators;

public class BoringProductAcceleratorEngine : IProductAcceleratorEngine
{
    public void Execute(float[] leftArray, float[] rightArray, float[] productArray)
    {
        for (var index = 0; index < productArray.Length; index++)
        {
            productArray[index] = leftArray[index] * rightArray[index];
        }
    }
}
