namespace GeometricAlgebra.ProductAccelerators;

public class ProductAccelerator<TNumber>
    where TNumber : IProductAcceleratorNumber<TNumber>
{
    private readonly float[] _leftInputBuffer;
    private readonly float[] _rightInputBuffer;
    private readonly float[] _outputBuffer;

    private int _nextIndex = 0;

    public ProductAccelerator(int numberOfProducts)
    {
        var bufferSize = numberOfProducts * TNumber.ComponentCount;

        _leftInputBuffer = new float[bufferSize];
        _rightInputBuffer = new float[bufferSize];
        _outputBuffer = new float[bufferSize];
    }

    private static Span<float> GetSpan(int index, float[] array)
    {
        return array.AsSpan(index * TNumber.ComponentCount, TNumber.ComponentCount);
    }

    public int SetInputs(TNumber left, TNumber right)
    {
        var index = _nextIndex++;

        TNumber.SetInputs(left, right, GetSpan(index, _leftInputBuffer), GetSpan(index, _rightInputBuffer));

        return index;
    }

    public void Execute(IProductAcceleratorEngine productAcceleratorEngine)
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