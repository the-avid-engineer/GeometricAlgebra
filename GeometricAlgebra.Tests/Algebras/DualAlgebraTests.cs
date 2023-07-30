using GeometricAlgebra.Common.Algebras;

namespace GeometricAlgebra.Common.Tests.Algebras;

public class DualAlgebraTests
{
    private static IEnumerable<(DualAlgebra left, DualAlgebra right)> EqualityCases()
    {
        var ε = new DualAlgebra(Z1: 1);

        yield return (ε ^ 2, 0);
    }

    public static IEnumerable<object[]> BoxedEqualityCases()
    {
        return EqualityCases().Select(pair => new object[] { pair.left, pair.right });
    }

    [Theory]
    [MemberData(nameof(BoxedEqualityCases))]
    public void EqualityTests(DualAlgebra left, DualAlgebra right)
    {
        Assert.Equal(left, right);
    }
}
