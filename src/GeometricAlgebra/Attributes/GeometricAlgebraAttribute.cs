namespace GeometricAlgebra.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class GeometricAlgebraAttribute : Attribute
{
    /// <summary>
    /// The number of basis vectors that square to positive one.
    /// </summary>
    public byte P { get; set; }

    /// <summary>
    /// The number of basis vectors that square to negative one.
    /// </summary>
    public byte N { get; set; }

    /// <summary>
    /// The number of basis vectors that square to zero.
    /// </summary>
    public byte Z { get; set; }

    /// <summary>
    /// The type of the component
    /// </summary>
    public Type ComponentType { get; set; } = typeof(float);
}