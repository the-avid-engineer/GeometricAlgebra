using BenchmarkDotNet.Attributes;
using GeometricAlgebra.Common.Algebras;

namespace GeometricAlgebra.Common.Benchmarks;

[MemoryDiagnoser]
public class ProductBenchmarks
{
    private static readonly Random random = new Random(0);

    private Euclidian2DAlgebra LeftInput = default;
    private Euclidian2DAlgebra RightInput = default;

    [IterationSetup]
    public void Setup()
    {
        LeftInput = new Euclidian2DAlgebra
        (
            random.NextSingle(),
            random.NextSingle(),
            random.NextSingle(),
            random.NextSingle()
        );

        RightInput = new Euclidian2DAlgebra
        (
            random.NextSingle(),
            random.NextSingle(),
            random.NextSingle(),
            random.NextSingle()
        );
    }

    [Benchmark]
    public void Product()
    {
        _ = Euclidian2DAlgebra.Product(in LeftInput, in RightInput);
    }
}