using System.Diagnostics;

namespace GeometricAlgebra.Attributes;

[AttributeUsage(AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public class GeometricAlgebraAttribute : Attribute
{
    /// <summary>
    ///     Generate a geometric algebra.
    /// </summary>
    /// <param name="componentStrings">
    ///     Overrides the component strings (e.g., p1, n2, z1) on the ToString override.
    /// </param>
    public GeometricAlgebraAttribute(params string[] componentStrings)
    {
        Debug.Assert(componentStrings != null);
    }

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