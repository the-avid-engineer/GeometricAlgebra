using GeometricAlgebra.Attributes;
using GeometricAlgebra.ProductAccelerators;

namespace GeometricAlgebra.Common.Algebras;

[GeometricAlgebra(N = 1)]
public readonly partial record struct ComplexAlgebra : IProductAcceleratorNumber<ComplexAlgebra, float>
{
    public static ComplexAlgebra ReciprocalEstimate(ComplexAlgebra value)
    {
        if (value.N1 != 0)
        {
            throw new NotSupportedException($"This is only meant to be used for Invert in an Algebra with {nameof(GeometricAlgebraAttribute.ComponentType)} = typeof({nameof(ComplexAlgebra)}).");
        }

        return new ComplexAlgebra
        (
            S: float.ReciprocalEstimate((value.S * value.S) + (value.N1 * value.N1))
        );
    }

    public static ComplexAlgebra ReciprocalSqrtEstimate(ComplexAlgebra value)
    {
        if (value.N1 != 0)
        {
            throw new NotSupportedException($"This is only meant to be used for Normalize in an Algebra with {nameof(GeometricAlgebraAttribute.ComponentType)} = typeof({nameof(ComplexAlgebra)}).");
        }

        return new ComplexAlgebra
        (
            S: float.ReciprocalSqrtEstimate((value.S * value.S) + (value.N1 * value.N1))
        );
    }

    public static ComplexAlgebra Cos(ComplexAlgebra value)
    {
        return new ComplexAlgebra
        (
            S: float.Cos(value.S) * float.Cosh(value.N1),
            N1: -(float.Sin(value.S) * float.Sinh(value.N1))
        );
    }

    public static ComplexAlgebra Sin(ComplexAlgebra value)
    {
        return new ComplexAlgebra
        (
            S: float.Sin(value.S) * float.Cosh(value.N1),
            N1: float.Cos(value.S) * float.Sinh(value.N1)
        );
    }

    public static ComplexAlgebra Cosh(ComplexAlgebra value)
    {
        return new ComplexAlgebra
        (
            S: float.Cosh(value.S) * float.Cos(value.N1),
            N1: float.Sinh(value.S) * float.Sin(value.N1)
        );
    }

    public static ComplexAlgebra Sinh(ComplexAlgebra value)
    {
        return new ComplexAlgebra
        (
            S: float.Sinh(value.S) * float.Cos(value.N1),
            N1: float.Cosh(value.S) * float.Sin(value.N1)
        );
    }

    static int IProductAcceleratorNumber<ComplexAlgebra, float>.ComponentCount => 4;

    static void IProductAcceleratorNumber<ComplexAlgebra, float>.SetInputs
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

    static ComplexAlgebra IProductAcceleratorNumber<ComplexAlgebra, float>.GetOutput
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