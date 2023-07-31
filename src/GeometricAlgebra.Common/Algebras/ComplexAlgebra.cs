using GeometricAlgebra.Attributes;

namespace GeometricAlgebra.Common.Algebras;

[GeometricAlgebra(N = 1)]
public readonly partial record struct ComplexAlgebra
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
}