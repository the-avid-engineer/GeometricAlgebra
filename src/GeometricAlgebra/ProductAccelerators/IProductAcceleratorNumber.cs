namespace GeometricAlgebra.ProductAccelerators;

public interface IProductAcceleratorNumber<TNumber>
{
    static abstract int ComponentCount { get; }
    static abstract void SetInputs(TNumber left, TNumber right, Span<float> leftSpan, Span<float> rightSpan);
    static abstract TNumber GetOutput(Span<float> productSpan);
}
