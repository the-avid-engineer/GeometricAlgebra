using GeometricAlgebra.Common.Algebras;

namespace GeometricAlgebra.Common.Tests.Algebras;

public class HyperbolicAlgebraTests
{
    public static IEnumerable<(HyperbolicAlgebra left, HyperbolicAlgebra right)> EqualityCases()
    {
        var j = new HyperbolicAlgebra(P1: 1);

        yield return (j ^ 2, +1);
    }

    public static IEnumerable<object[]> BoxedEqualityCases()
    {
        return EqualityCases().Select(pair => new object[] { pair.left, pair.right });
    }

    [Theory]
    [MemberData(nameof(BoxedEqualityCases))]
    public void EqualityTests(HyperbolicAlgebra left, HyperbolicAlgebra right)
    {
        Assert.Equal(left, right);
    }
}