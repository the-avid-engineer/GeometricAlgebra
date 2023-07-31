This package provides a source-generator that can generate the geometric algebra object of your choice!

You can choose how many basis vectors square to +1 (P), -1 (N), or 0 (Z).

A complex algebra (containing complex numbers) is as simple as:

```cs
[GeometricAlgebra(N = 1)]
public readonly partial record ComplexAlgebra;
```
