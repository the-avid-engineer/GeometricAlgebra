using System.Numerics;
using GeometricAlgebra.ProductAccelerators;
using ILGPU;
using ILGPU.Runtime;
using ILGPU.Runtime.OpenCL;

namespace GeometricAlgebra.Common.ProductAccelerators;

public class OpenCLProductAcceleratorEngine<TValue> : IProductAcceleratorEngine<TValue>, IDisposable
    where TValue : unmanaged, IMultiplyOperators<TValue, TValue, TValue>
{
    private static void Kernel
    (
        Index1D index,
        ArrayView<TValue> leftView,
        ArrayView<TValue> rightView,
        ArrayView<TValue> productView
    )
    {
        productView[index] = leftView[index] * rightView[index];
    }

    private readonly int _size;
    private readonly Context _context;
    private readonly Accelerator _accelerator;
    private readonly MemoryBuffer1D<TValue, Stride1D.Dense> _leftBuffer;
    private readonly MemoryBuffer1D<TValue, Stride1D.Dense> _rightBuffer;
    private readonly MemoryBuffer1D<TValue, Stride1D.Dense> _productBuffer;
    private readonly Action<Index1D, ArrayView<TValue>, ArrayView<TValue>, ArrayView<TValue>> _kernel;

    public OpenCLProductAcceleratorEngine(int size, int deviceIndex = 0)
    {
        _size = size;
        _context = Context.CreateDefault();
        _accelerator = _context.CreateCLAccelerator(deviceIndex);
        _leftBuffer = _accelerator.Allocate1D<TValue>(size);
        _rightBuffer = _accelerator.Allocate1D<TValue>(size);
        _productBuffer = _accelerator.Allocate1D<TValue>(size);

        _kernel = _accelerator
            .LoadAutoGroupedStreamKernel<Index1D, ArrayView<TValue>, ArrayView<TValue>, ArrayView<TValue>>(Kernel);
    }

    public static OpenCLProductAcceleratorEngine<TValue> Create<TNumber>(int numberOfNumbers, int deviceIndex = 0)
        where TNumber : IProductAcceleratorNumber<TNumber, TValue>
    {
        return new OpenCLProductAcceleratorEngine<TValue>(numberOfNumbers * TNumber.ComponentCount, deviceIndex);
    }

    public void Execute
    (
        TValue[] leftArray,
        TValue[] rightArray,
        TValue[] productArray
    )
    {
        _leftBuffer.CopyFromCPU(leftArray);
        _rightBuffer.CopyFromCPU(rightArray);

        _kernel.Invoke(_size, _leftBuffer.View, _rightBuffer.View, _productBuffer.View);

        _productBuffer.CopyToCPU(productArray);
    }

    public void Dispose()
    {
        _productBuffer.Dispose();
        _rightBuffer.Dispose();
        _leftBuffer.Dispose();
        _accelerator.Dispose();
        _context.Dispose();
    }
}
