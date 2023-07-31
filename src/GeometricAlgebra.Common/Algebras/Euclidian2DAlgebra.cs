using GeometricAlgebra.Attributes;
using GeometricAlgebra.ProductAccelerators;

namespace GeometricAlgebra.Common.Algebras;

[GeometricAlgebra(P = 2)]
public partial record struct Euclidian2DAlgebra : IProductAcceleratorNumber<Euclidian2DAlgebra, float>
{
    static int IProductAcceleratorNumber<Euclidian2DAlgebra, float>.ComponentCount => 16;

    static void IProductAcceleratorNumber<Euclidian2DAlgebra, float>.SetInputs
    (
        Euclidian2DAlgebra left,
        Euclidian2DAlgebra right,
        Span<float> leftSpan,
        Span<float> rightSpan
    )
    {
        leftSpan[0] = left.S;
        leftSpan[1] = left.S;
        leftSpan[2] = left.S;
        leftSpan[3] = left.S;
        leftSpan[4] = left.P1;
        leftSpan[5] = left.P1;
        leftSpan[6] = left.P1;
        leftSpan[7] = left.P1;
        leftSpan[8] = left.P2;
        leftSpan[9] = left.P2;
        leftSpan[10] = left.P2;
        leftSpan[11] = left.P2;
        leftSpan[12] = left.P1P2;
        leftSpan[13] = left.P1P2;
        leftSpan[14] = left.P1P2;
        leftSpan[15] = left.P1P2;

        rightSpan[0] = right.S;
        rightSpan[1] = right.P1;
        rightSpan[2] = right.P2;
        rightSpan[3] = right.P1P2;
        rightSpan[4] = right.P1;
        rightSpan[5] = right.S;
        rightSpan[6] = right.P1P2;
        rightSpan[7] = right.P2;
        rightSpan[8] = right.P2;
        rightSpan[9] = right.P1P2;
        rightSpan[10] = right.S;
        rightSpan[11] = right.P1;
        rightSpan[12] = right.P1P2;
        rightSpan[13] = right.P2;
        rightSpan[14] = right.P1;
        rightSpan[15] = right.S;
    }

    static Euclidian2DAlgebra IProductAcceleratorNumber<Euclidian2DAlgebra, float>.GetOutput
    (
        Span<float> productSpan
    )
    {
        return new Euclidian2DAlgebra
        {
            S = (productSpan[0]) + (productSpan[4]) + (productSpan[8]) - (productSpan[12]),
            P1 = (productSpan[1]) + (productSpan[5]) - (productSpan[9]) + (productSpan[13]),
            P2 = (productSpan[2]) + (productSpan[6]) + (productSpan[10]) - (productSpan[14]),
            P1P2 = (productSpan[3]) + (productSpan[7]) - (productSpan[11]) + (productSpan[15]),
        };
    }
}