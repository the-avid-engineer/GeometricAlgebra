namespace GeometricAlgebra.ProductAccelerators;

public interface IProductAcceleratorNumber<TNumber, TValue>
{
    static abstract int ComponentCount { get; }
    static abstract void SetInputs(TNumber left, TNumber right, Span<TValue> leftSpan, Span<TValue> rightSpan);
    static abstract TNumber GetOutput(Span<TValue> productSpan);
}
