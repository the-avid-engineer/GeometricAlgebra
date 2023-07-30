using GeometricAlgebra.Common.Algebras;

namespace GeometricAlgebra.Common.Tests.Algebras;

public class Euclidian2DAlgebraTests
{
    private static readonly Euclidian2DAlgebra X = new(P1: 1);
    private static readonly Euclidian2DAlgebra Y = new(P2: 1);
    private static readonly Euclidian2DAlgebra XY = new(P1P2: 1);

    public static IEnumerable<(Euclidian2DAlgebra left, Euclidian2DAlgebra right)> AntiEqualityCases()
    {
        yield return (X * Y, -XY);

        yield return (X * XY, -Y);
        yield return (X * XY * XY, X);
        yield return (X * XY * XY * XY, Y);
        yield return (X * XY * XY * XY * XY, -X);
    }

    public static IEnumerable<object[]> BoxedAntiEqualityCases()
    {
        return AntiEqualityCases().Select(pair => new object[] { pair.left, pair.right });
    }

    [Theory]
    [MemberData(nameof(BoxedAntiEqualityCases))]
    public void AntiEqualityTests(Euclidian2DAlgebra left, Euclidian2DAlgebra right)
    {
        Assert.NotEqual(left, right);
    }

    public static IEnumerable<(Euclidian2DAlgebra left, Euclidian2DAlgebra right)> EqualityCases()
    {
        yield return (X * Y, XY);

        yield return (X * XY, Y);
        yield return (X * XY * XY, -X);
        yield return (X * XY * XY * XY, -Y);
        yield return (X * XY * XY * XY * XY, X);
    }

    public static IEnumerable<object[]> BoxedEqualityCases()
    {
        return EqualityCases().Select(pair => new object[] { pair.left, pair.right });
    }

    [Theory]
    [MemberData(nameof(BoxedEqualityCases))]
    public void EqualityTests(Euclidian2DAlgebra left, Euclidian2DAlgebra right)
    {
        Assert.Equal(left, right);
    }
}