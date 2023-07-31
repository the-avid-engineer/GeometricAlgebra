using GeometricAlgebra.Common.Algebras;

namespace GeometricAlgebra.Common.Tests.Algebras;

public class BiquaternionAlgebraTests
{
    public static IEnumerable<(BiquaternionAlgebra left, BiquaternionAlgebra right)> EqualityCases()
    {
        var C1 = new ComplexAlgebra(S: 1);
        var Ci = new ComplexAlgebra(N1: 1);

        var H1 = new BiquaternionAlgebra(S: C1);
        var Hi = new BiquaternionAlgebra(N1: C1);
        var Hj = new BiquaternionAlgebra(N2: C1);
        var Hk = new BiquaternionAlgebra(N1N2: C1);

        yield return (~!(Hi * Hj * Hk * Ci), (-Hk * -Hj * -Hi * -Ci));

        yield return (Hi ^ 2, -H1);
        yield return (Hj ^ 2, -H1);
        yield return (Hk ^ 2, -H1);

        yield return ((Hi * Ci) ^ 2, H1);
        yield return ((Hj * Ci) ^ 2, H1);
        yield return ((Hk * Ci) ^ 2, H1);

        yield return (Hi * Hj, Hk);
        yield return (Hj * Hk, Hi);
        yield return (Hk * Hi, Hj);

        yield return (Hi * Hj * Ci, Hk * Ci);
        yield return (Hj * Hk * Ci, Hi * Ci);
        yield return (Hk * Hi * Ci, Hj * Ci);

        yield return (Hj * Hi, -Hk);
        yield return (Hk * Hj, -Hi);
        yield return (Hi * Hk, -Hj);

        yield return (Hj * Hi * Ci, -Hk * Ci);
        yield return (Hk * Hj * Ci, -Hi * Ci);
        yield return (Hi * Hk * Ci, -Hj * Ci);
    }

    public static IEnumerable<object[]> BoxedEqualityCases()
    {
        return EqualityCases().Select(pair => new object[] { pair.left, pair.right });
    }

    [Theory]
    [MemberData(nameof(BoxedEqualityCases))]
    public void EqualityTests(BiquaternionAlgebra left, BiquaternionAlgebra right)
    {
        Assert.Equal(left, right);
    }
}