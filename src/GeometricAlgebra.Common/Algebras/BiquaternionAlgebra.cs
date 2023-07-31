using GeometricAlgebra.Attributes;

namespace GeometricAlgebra.Common.Algebras;

[GeometricAlgebra(N = 2, ComponentType = typeof(ComplexAlgebra))]
public readonly partial record struct BiquaternionAlgebra
{
    public static BiquaternionAlgebra ComplexProduct(in BiquaternionAlgebra biquaternionValue, in ComplexAlgebra complexValue)
    {
        return new BiquaternionAlgebra
        {
            S = biquaternionValue.S * complexValue,
            N1 = biquaternionValue.N1 * complexValue,
            N2 = biquaternionValue.N2 * complexValue,
            N1N2 = biquaternionValue.N1N2 * complexValue,
        };
    }

    public static BiquaternionAlgebra ComplexConjugate(in BiquaternionAlgebra biquaternionValue)
    {
        return new BiquaternionAlgebra
        {
            S = ComplexAlgebra.Conjugate(biquaternionValue.S),
            N1 = ComplexAlgebra.Conjugate(biquaternionValue.N1),
            N2 = ComplexAlgebra.Conjugate(biquaternionValue.N2),
            N1N2 = ComplexAlgebra.Conjugate(biquaternionValue.N1N2),
        };
    }

    public static BiquaternionAlgebra operator !(BiquaternionAlgebra biquaternionValue)
    {
        return ComplexConjugate(biquaternionValue);
    }

    public static BiquaternionAlgebra operator *(BiquaternionAlgebra biquaternionValue, ComplexAlgebra complexValue)
    {
        return ComplexProduct(biquaternionValue, complexValue);
    }

    public static BiquaternionAlgebra operator *(ComplexAlgebra complexValue, BiquaternionAlgebra biquaternionValue)
    {
        return ComplexProduct(biquaternionValue, complexValue);
    }
}