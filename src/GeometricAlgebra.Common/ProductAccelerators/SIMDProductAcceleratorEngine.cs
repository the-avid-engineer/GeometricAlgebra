using System.Runtime.Intrinsics;
using GeometricAlgebra.ProductAccelerators;

namespace GeometricAlgebra.Common.ProductAccelerators;

public class SIMDProductAcceleratorEngine<TValue> : IProductAcceleratorEngine<TValue>
    where TValue : struct
{
    private static readonly int VectorSize = Vector256<TValue>.Count;

    public void Execute(TValue[] leftArray, TValue[] rightArray, TValue[] productArray)
    {
        for (var startIndex = 0; startIndex < productArray.Length; startIndex += VectorSize)
        {
            ReadOnlySpan<TValue> leftSpan = leftArray.AsSpan(startIndex, VectorSize);
            ReadOnlySpan<TValue> rightSpan = rightArray.AsSpan(startIndex, VectorSize);

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
