namespace GeometricAlgebra.ProductAccelerators;

public class ProductAccelerator<TNumber, TValue>
    where TNumber : IProductAcceleratorNumber<TNumber, TValue>
{
    private readonly TValue[] _leftInputBuffer;
    private readonly TValue[] _rightInputBuffer;
    private readonly TValue[] _outputBuffer;

    private int _nextIndex = 0;

    public ProductAccelerator(int numberOfProducts)
    {
        var bufferSize = numberOfProducts * TNumber.ComponentCount;

        _leftInputBuffer = new TValue[bufferSize];
        _rightInputBuffer = new TValue[bufferSize];
        _outputBuffer = new TValue[bufferSize];
    }

    private static Span<TValue> GetSpan(int index, TValue[] array)
    {
        return array.AsSpan(index * TNumber.ComponentCount, TNumber.ComponentCount);
    }

    public int SetInputs(TNumber left, TNumber right)
    {
        var index = _nextIndex++;

        TNumber.SetInputs(left, right, GetSpan(index, _leftInputBuffer), GetSpan(index, _rightInputBuffer));

        return index;
    }

    public void Execute(IProductAcceleratorEngine<TValue> productAcceleratorEngine)
    {
        productAcceleratorEngine.Execute(_leftInputBuffer, _rightInputBuffer, _outputBuffer);
    }

    public TNumber GetOutput(int index)
    {
        return TNumber.GetOutput(GetSpan(index, _outputBuffer));
    }

    public void ResetIndex()
    {
        _nextIndex = 0;
    }
}