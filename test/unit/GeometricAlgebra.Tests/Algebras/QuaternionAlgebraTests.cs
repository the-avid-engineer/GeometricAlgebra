using GeometricAlgebra.Common.Algebras;

namespace GeometricAlgebra.Common.Tests.Algebras;

public class QuaternionAlgebraTests
{
    public static IEnumerable<(QuaternionAlgebra left, QuaternionAlgebra right)> EqualityCases()
    {
        var i = new QuaternionAlgebra(N1: 1);
        var j = new QuaternionAlgebra(N2: 1);
        var k = new QuaternionAlgebra(N1N2: 1);

        yield return (i ^ 2, -1);
        yield return (j ^ 2, -1);
        yield return (k ^ 2, -1);

        yield return (i * j, k);
        yield return (j * k, i);
        yield return (k * i, j);

        yield return (j * i, -k);
        yield return (k * j, -i);
        yield return (i * k, -j);
    }

    public static IEnumerable<object[]> BoxedEqualityCases()
    {
        return EqualityCases().Select(pair => new object[] { pair.left, pair.right });
    }

    [Theory]
    [MemberData(nameof(BoxedEqualityCases))]
    public void EqualityTests(QuaternionAlgebra left, QuaternionAlgebra right)
    {
        Assert.Equal(left, right);
    }
}