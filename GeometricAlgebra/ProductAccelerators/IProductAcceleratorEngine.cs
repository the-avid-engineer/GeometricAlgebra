namespace GeometricAlgebra.ProductAccelerators;

public interface IProductAcceleratorEngine
{
    void Execute(float[] leftArray, float[] rightArray, float[] productArray);
}
