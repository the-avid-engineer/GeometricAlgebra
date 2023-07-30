using BenchmarkDotNet.Attributes;
using GeometricAlgebra.Common.Algebras;
using GeometricAlgebra.Common.ProductAccelerators;
using GeometricAlgebra.ProductAccelerators;

namespace GeometricAlgebra.Common.Benchmarks;

[MemoryDiagnoser]
public class ProductBenchmarks
{
    private const int NumberOfProducts = 1_000_000;

    private static readonly Euclidian2DAlgebra LeftInput = new(S: 1, P1: 1);
    private static readonly Euclidian2DAlgebra RightInput = new(S: 1, P2: 1);

    private static readonly ProductAccelerator<Euclidian2DAlgebra, float> ProductAccelerator = new(NumberOfProducts);
    private static readonly BoringProductAcceleratorEngine<float> BoringEngine = new();
    private static readonly SIMDProductAcceleratorEngine<float> SIMDEngine = new();
    private static readonly OpenCLProductAcceleratorEngine<float> OpenCLEngine = OpenCLProductAcceleratorEngine<float>.Create<Euclidian2DAlgebra>(NumberOfProducts);

    [IterationCleanup]
    public void Cleanup()
    {
        ProductAccelerator.ResetIndex();
    }

    [Benchmark]
    public void NonAccelerated()
    {
        for (var n = 0; n < NumberOfProducts; n++)
        {
            _ = LeftInput * RightInput;
        }
    }

    [Benchmark]
    public void Boring()
    {
        for (var n = 0; n < NumberOfProducts; n++)
        {
            _ = ProductAccelerator.SetInputs(LeftInput, RightInput);
        }

        ProductAccelerator.Execute(BoringEngine);

        for (var n = 0; n < NumberOfProducts; n++)
        {
            _ = ProductAccelerator.GetOutput(n);
        }
    }

    [Benchmark]
    public void SIMD()
    {
        for (var n = 0; n < NumberOfProducts; n++)
        {
            _ = ProductAccelerator.SetInputs(LeftInput, RightInput);
        }

        ProductAccelerator.Execute(SIMDEngine);

        for (var n = 0; n < NumberOfProducts; n++)
        {
            _ = ProductAccelerator.GetOutput(n);
        }
    }

    [Benchmark]
    public void OpenCL()
    {
        for (var n = 0; n < NumberOfProducts; n++)
        {
            _ = ProductAccelerator.SetInputs(LeftInput, RightInput);
        }

        ProductAccelerator.Execute(OpenCLEngine);

        for (var n = 0; n < NumberOfProducts; n++)
        {
            _ = ProductAccelerator.GetOutput(n);
        }
    }
}