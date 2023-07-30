using System.Runtime.Intrinsics;
using GeometricAlgebra.ProductAccelerators;

namespace GeometricAlgebra.Common.ProductAccelerators;

public class SIMDProductAcceleratorEngine : IProductAcceleratorEngine
{
    private static readonly int VectorSize = Vector256<float>.Count;

    public void Execute(float[] leftArray, float[] rightArray, float[] productArray)
    {
        for (var startIndex = 0; startIndex < productArray.Length; startIndex += VectorSize)
        {
            ReadOnlySpan<float> leftSpan = leftArray.AsSpan(startIndex, VectorSize);
            ReadOnlySpan<float> rightSpan = rightArray.AsSpan(startIndex, VectorSize);

            Vector256
                .Multiply
                (
                    Vector256.Create(leftSpan),
                    Vector256.Create(rightSpan)
                )
                .CopyTo(productArray, startIndex);
        }
    }
}
