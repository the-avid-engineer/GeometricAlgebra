namespace GeometricAlgebra.ProductAccelerators;

public interface IProductAcceleratorEngine<TValue>
{
    void Execute(TValue[] leftArray, TValue[] rightArray, TValue[] productArray);
}
