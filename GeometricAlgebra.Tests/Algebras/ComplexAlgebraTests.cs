using GeometricAlgebra.Common.Algebras;

namespace GeometricAlgebra.Common.Tests.Algebras;

public class ComplexAlgebraTests
{
    public static IEnumerable<(ComplexAlgebra left, ComplexAlgebra right)> EqualityCases()
    {
        var i = new ComplexAlgebra(N1: 1);

        yield return (1 + i, i + 1);
        yield return ((1 + i) * i, i - 1);
        yield return (i ^ 2, -1);
    }

    public static IEnumerable<object[]> BoxedEqualityCases()
    {
        return EqualityCases().Select(pair => new object[] { pair.left, pair.right });
    }

    [Theory]
    [MemberData(nameof(BoxedEqualityCases))]
    public void EqualityTests(ComplexAlgebra left, ComplexAlgebra right)
    {
        Assert.Equal(left, right);
    }
}