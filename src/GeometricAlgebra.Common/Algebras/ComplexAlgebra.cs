using GeometricAlgebra.Attributes;
using GeometricAlgebra.ProductAccelerators;

namespace GeometricAlgebra.Common.Algebras;

[GeometricAlgebra(N = 1)]
public partial record ComplexAlgebra : IProductAcceleratorNumber<ComplexAlgebra>
{
    static int IProductAcceleratorNumber<ComplexAlgebra>.ComponentCount => 4;

    static void IProductAcceleratorNumber<ComplexAlgebra>.SetInputs
    (
        ComplexAlgebra left,
        ComplexAlgebra right,
        Span<float> leftSpan,
        Span<float> rightSpan
    )
    {
        leftSpan[0] = left.S;
        leftSpan[1] = left.S;
        leftSpan[2] = left.N1;
        leftSpan[3] = left.N1;

        rightSpan[0] = right.S;
        rightSpan[1] = right.N1;
        rightSpan[2] = right.N1;
        rightSpan[3] = right.S;
    }

    static ComplexAlgebra IProductAcceleratorNumber<ComplexAlgebra>.GetOutput
    (
        Span<float> productSpan
    )
    {
        return new ComplexAlgebra
        {
            S = (productSpan[0])-(productSpan[2]),
            N1 = (productSpan[1])+(productSpan[3]),
        };
    }
}