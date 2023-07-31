using GeometricAlgebra.Attributes;

namespace GeometricAlgebra.Common.Algebras;

[GeometricAlgebra(N = 2, ComponentType = typeof(ComplexAlgebra))]
public partial record struct BiquaternionAlgebra
{
    public static BiquaternionAlgebra Product(BiquaternionAlgebra biquaternionValue, ComplexAlgebra complexValue)
    {
        return new BiquaternionAlgebra
        {
            S = biquaternionValue.S * complexValue,
            N1 = biquaternionValue.N1 * complexValue,
            N2 = biquaternionValue.N2 * complexValue,
            N1N2 = biquaternionValue.N1N2 * complexValue,
        };
    }

    public static BiquaternionAlgebra operator *(BiquaternionAlgebra biquaternionValue, ComplexAlgebra complexValue)
    {
        return Product(biquaternionValue, complexValue);
    }

    public static BiquaternionAlgebra operator *(ComplexAlgebra complexValue, BiquaternionAlgebra biquaternionValue)
    {
        return Product(biquaternionValue, complexValue);
    }
}