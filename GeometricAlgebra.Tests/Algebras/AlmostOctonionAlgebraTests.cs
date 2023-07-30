using GeometricAlgebra.Common.Algebras;

namespace GeometricAlgebra.Common.Tests.Algebras;

public class AlmostOctonionAlgebraTests
{
    public static IEnumerable<(AlmostOctonionAlgebra left, AlmostOctonionAlgebra right)> EqualityCases()
    {
        var e1 = new AlmostOctonionAlgebra(N1: 1);
        var e2 = new AlmostOctonionAlgebra(N2: 1);
        var e3 = new AlmostOctonionAlgebra(N1N2: 1);
        var e4 = new AlmostOctonionAlgebra(N3: 1);
        var e5 = new AlmostOctonionAlgebra(N1N3: 1);
        var e6 = new AlmostOctonionAlgebra(N2N3: 1);
        var e7 = new AlmostOctonionAlgebra(N1N2N3: 1);

        yield return (e1 ^ 2, -1);
        yield return (e2 ^ 2, -1);
        yield return (e3 ^ 2, -1);
        yield return (e4 ^ 2, -1);
        yield return (e5 ^ 2, -1);
        yield return (e6 ^ 2, -1);
        yield return (e7 ^ 2, +1); // Should be -1 for Octonion

        yield return (e1 * e2, e3);
        yield return (e2 * e3, e1);
        yield return (e3 * e1, e2);
        yield return (e1 * e4, e5);
        yield return (e2 * e4, e6);
        yield return (e3 * e4, e7);
        yield return (e6 * e1, e7);
        yield return (e7 * e2, e5);
        yield return (e5 * e3, -e6); // Should be e6 for Octonion
    }

    public static IEnumerable<object[]> BoxedEqualityCases()
    {
        return EqualityCases().Select(pair => new object[] { pair.left, pair.right });
    }

    [Theory]
    [MemberData(nameof(BoxedEqualityCases))]
    public void EqualityTests(AlmostOctonionAlgebra left, AlmostOctonionAlgebra right)
    {
        Assert.Equal(left, right);
    }
}