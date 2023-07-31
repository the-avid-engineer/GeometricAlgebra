using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis;
using System.Text;
using System.Diagnostics;

namespace GeometricAlgebra.Analyzers
{
    [Generator]
    public class GeometricAlgebraGenerator : ISourceGenerator
    {
        private static string ConjugateInitializers(IEnumerable<KVector> basisKVectors)
        {
            var source = new StringBuilder();

            foreach (var kVector in basisKVectors)
            {
                source.AppendLine($"""
                            {kVector} = {(kVector.K == 0 ? "" : "-")}value.{kVector},
                """);
            }

            return source.ToString().TrimEnd();
        }

        private static string CastInitializers(IEnumerable<KVector> basisKVectors)
        {
            var source = new StringBuilder();

            foreach (var kVector in basisKVectors)
            {
                if (kVector.K != 0)
                {
                    continue;
                }

                source.AppendLine($"""
                            {kVector} = scalar,
                """);
            }

            return source.ToString().TrimEnd();
        }

        private static string SumInitializers(IEnumerable<KVector> basisKVectors)
        {
            var source = new StringBuilder();

            foreach (var kVector in basisKVectors)
            {
                source.AppendLine($"""
                            {kVector} = left.{kVector} + right.{kVector},
                """);
            }

            return source.ToString().TrimEnd();
        }

        private static string ProductInitializers(IEnumerable<KVector> basisKVectors, bool? parallelParts)
        {
            var componentsDictionary = new Dictionary<string, List<string>>();

            foreach (var basisKVector in basisKVectors)
            {
                componentsDictionary.Add(basisKVector.ToString(), new List<string>());
            }

            foreach (var left in basisKVectors)
            {
                foreach (var right in basisKVectors)
                {
                    var (initializeComponent, sign, parallel) = KVector.GetBasisAndSign(left, right);

                    if (parallelParts.HasValue && parallelParts.Value != parallel)
                    {
                        continue;
                    }
                    
                    componentsDictionary[initializeComponent.ToString()].Add(sign switch
                    {
                        +1 => $"+(left.{left} * right.{right})",
                        0 => "+ComponentAdditiveIdentity",
                        -1 => $"-(left.{left} * right.{right})",
                        _ => throw new NotSupportedException("Sign must be +1, 0, or -1"),
                    });
                }
            }

            var source = new StringBuilder();

            foreach (var components in componentsDictionary)
            {
                if (components.Value.Count > 0)
                {
                    source.AppendLine($"""
                            {components.Key} = {string.Join("", components.Value).TrimStart('+')},
                """);
                }
                else
                {
                    source.AppendLine($"""
                            {components.Key} = ComponentAdditiveIdentity,
                """);
                }

            }

            return source.ToString().TrimEnd();
        }

        private static string RotorInitializers(string componentType, KVector pseudoScalar)
        {
            var (_, sign, _) = KVector.GetBasisAndSign(pseudoScalar, pseudoScalar);

            return sign switch
            {
                +1 => $"""
                            {KVector.S} = {componentType}.Cosh(angle),
                            {pseudoScalar} = {componentType}.Sinh(angle),
                """,
                0 => $"""
                            {KVector.S} = ComponentMultiplicativeIdentity,
                            {pseudoScalar} = angle,
                """,
                -1 => $"""
                            {KVector.S} = {componentType}.Cos(angle),
                            {pseudoScalar} = {componentType}.Sin(angle),
                """,
                _ => throw new NotSupportedException("Sign must be +1, 0, or -1")
            };
        }

        private static string ToStringAddComponents(IEnumerable<KVector> basisKVectors)
        {
            var source = new StringBuilder();

            foreach (var kVector in basisKVectors)
            {
                if (kVector.K == 0)
                {
                    source.AppendLine($"""
                        AddComponent({kVector});
                """);
                }
                else
                {
                    source.AppendLine($"""
                        AddComponent({kVector}, "{kVector.ToString().ToLowerInvariant()}");
                """);
                }
            }

            return source.ToString().TrimEnd();
        }

        private static string GenerateSource(ISymbol recordSymbol, (byte p, byte n, byte z, string componentType) metricSignature)
        {
            var (p, n, z, componentType) = metricSignature;

            var basisVectors = new List<Vector>();

            for (byte i = 1; i <= p; i++)
            {
                basisVectors.Add(new Vector(+1, 'P', 1, i));
            }

            for (byte i = 1; i <= n; i++)
            {
                basisVectors.Add(new Vector(-1, 'N', 2, i));
            }

            for (byte i = 1; i <= z; i++)
            {
                basisVectors.Add(new Vector(0, 'Z', 3, i));
            }

            var basisKVectors = new List<KVector>();

            foreach (var @base in basisVectors)
            {
                foreach (var incompletePowerSet in basisKVectors.ToArray())
                {
                    basisKVectors.Add(incompletePowerSet.AppendUnorderedBasisVector(@base));
                }

                basisKVectors.Add(KVector.FromBasisVector(@base));
            }

            basisKVectors.Add(KVector.S);

            basisKVectors = basisKVectors.OrderBy(x => x.K).ToList();

            var pseudoScalar = basisKVectors.Last();

            var components = basisKVectors
                .Select(kVector => kVector.ToString());

            var source = new StringBuilder();

            var accessModifier = recordSymbol.DeclaredAccessibility switch
            {
                Accessibility.NotApplicable => "",
                Accessibility.Private => "private ",
                Accessibility.ProtectedAndInternal => "private protected ",
                Accessibility.Protected => "protected ",
                Accessibility.Internal => "internal ",
                Accessibility.ProtectedOrInternal => "protected internal ",
                Accessibility.Public => "public ",
                _ => throw new NotSupportedException($"Unkwown accesibility modifier {recordSymbol.DeclaredAccessibility}"),
            };

            source.AppendLine($$"""
            using System.Numerics;

            namespace {{recordSymbol.ContainingNamespace.ToDisplayString()}};
            
            {{accessModifier}}partial record struct {{recordSymbol.Name}}({{componentType}} {{string.Join($" = default, {componentType} ", components)}} = default) : IMultiplyOperators<{{recordSymbol.Name}}, {{recordSymbol.Name}}, {{recordSymbol.Name}}>, IAdditionOperators<{{recordSymbol.Name}}, {{recordSymbol.Name}}, {{recordSymbol.Name}}>
            {
                private static readonly {{componentType}} ComponentAdditiveIdentity = {{componentType}}.Sin(default);

                private static readonly {{componentType}} ComponentMultiplicativeIdentity = {{componentType}}.Cos(default);

                public static readonly {{recordSymbol.Name}} PseudoScalar = new {{recordSymbol.Name}}({{pseudoScalar}}: ComponentMultiplicativeIdentity);
            
                public static implicit operator {{recordSymbol.Name}}({{componentType}} scalar)
                {
                    return new {{recordSymbol.Name}}
                    {
            {{CastInitializers(basisKVectors)}}
                    };
                }

                public static {{recordSymbol.Name}} Rotor({{componentType}} angle)
                {
                    return new {{recordSymbol.Name}}
                    {
            {{RotorInitializers(componentType, pseudoScalar)}}
                    };
                }

                public static {{recordSymbol.Name}} Conjugate({{recordSymbol.Name}} value)
                {
                    return new {{recordSymbol.Name}}
                    {
            {{ConjugateInitializers(basisKVectors)}}
                    };
                }
            
                public static {{recordSymbol.Name}} Product({{recordSymbol.Name}} left, {{recordSymbol.Name}} right)
                {
                    return new {{recordSymbol.Name}}
                    {
            {{ProductInitializers(basisKVectors, null)}}
                    };
                }
            
                public static {{recordSymbol.Name}} WedgeProduct({{recordSymbol.Name}} left, {{recordSymbol.Name}} right)
                {
                    return new {{recordSymbol.Name}}
                    {
            {{ProductInitializers(basisKVectors, false)}}
                    };
                }
            
                public static {{recordSymbol.Name}} DotProduct({{recordSymbol.Name}} left, {{recordSymbol.Name}} right)
                {
                    return new {{recordSymbol.Name}}
                    {
            {{ProductInitializers(basisKVectors, true)}}
                    };
                }

                public static {{recordSymbol.Name}} Sum({{recordSymbol.Name}} left, {{recordSymbol.Name}} right)
                {
                    return new {{recordSymbol.Name}}
                    {
            {{SumInitializers(basisKVectors)}}
                    };
                }

                public static {{recordSymbol.Name}} Invert({{recordSymbol.Name}} value)
                {
                    var conjugate = Conjugate(value);

                    return Product(conjugate, {{componentType}}.ReciprocalEstimate(Product(value, conjugate).{{KVector.S}}));
                }

                public static {{recordSymbol.Name}} Normalize({{recordSymbol.Name}} value)
                {
                    var conjugate = Conjugate(value);

                    return Product(value, {{componentType}}.ReciprocalSqrtEstimate(Product(value, conjugate).{{KVector.S}}));
                }
                            
                public static {{recordSymbol.Name}} operator ^({{recordSymbol.Name}} value, int power)
                {
                    if (power == 0)
                    {
                        throw new NotSupportedException("Power must not be zero (undefined behavior)");
                    }

                    if (power == -1)
                    {
                        power = 0 - power;
                        value = Invert(value);
                    }

                    for (var i = 1; i < power; i++)
                    {
                        value *= value;
                    }

                    return value;
                }
                        
                public static {{recordSymbol.Name}} operator ~({{recordSymbol.Name}} value)
                {
                    return Conjugate(value);
                }

                public static {{recordSymbol.Name}} operator -({{recordSymbol.Name}} value)
                {
                    return Product(value, -ComponentMultiplicativeIdentity);
                }

                public static {{recordSymbol.Name}} operator *({{recordSymbol.Name}} left, {{recordSymbol.Name}} right)
                {
                    return Product(left, right);
                }
            
                public static {{recordSymbol.Name}} operator +({{recordSymbol.Name}} left, {{recordSymbol.Name}} right)
                {
                    return Sum(left, right);
                }

                public static {{recordSymbol.Name}} operator -({{recordSymbol.Name}} left, {{recordSymbol.Name}} right)
                {
                    return Sum(left, -right);
                }
                            
                public override string ToString()
                {
                    var components = new List<string>();
            
                    string GetComponent({{componentType}} scalar, string suffix = null)
                    {                            
                        if (scalar == ComponentMultiplicativeIdentity)
                        {
                            return suffix == null
                                ? "1"
                                : components.Count == 0
                                    ? suffix
                                    : $"+ {suffix}";
                        }
            
                        if (scalar == -ComponentMultiplicativeIdentity)
                        {
                            return suffix == null
                                ? "-1"
                                : components.Count == 0
                                    ? $"-{suffix}"
                                    : $"- {suffix}";
                        }
            
                        return suffix == null
                            ? $"{scalar}"
                            : components.Count == 0
                                ? $"{scalar:0.#####;-0.#####;0} {suffix}"
                                : $"{scalar:+ 0.#####;- 0.#####;+ 0} {suffix}";
                    }

                    void AddComponent({{componentType}} scalar, string suffix = null)
                    {
                        if (scalar == ComponentAdditiveIdentity)
                        {
                            return;
                        }

                        components.Add(GetComponent(scalar, suffix));
                    }

            {{ToStringAddComponents(basisKVectors)}}

                    return components.Count > 0 ? string.Join(" ", components) : "0";
                }
            }
            """);

            return source.ToString();
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var attributeTypeSymbol = context.Compilation
                .GetTypeByMetadataName("GeometricAlgebra.Attributes.GeometricAlgebraAttribute");

            var syntaxTrees = context.Compilation.SyntaxTrees
                .Where(syntaxTree => syntaxTree
                    .GetRoot()
                    .DescendantNodes()
                    .OfType<RecordDeclarationSyntax>()
                    .Any(recordDeclaration => recordDeclaration
                        .DescendantNodes()
                        .OfType<AttributeSyntax>()
                        .Any()
                    )
                );

            foreach (var syntaxTree in syntaxTrees)
            {
                var semanticModel = context.Compilation.GetSemanticModel(syntaxTree);

                var recordDeclarations = syntaxTree
                    .GetRoot()
                    .DescendantNodes()
                    .OfType<RecordDeclarationSyntax>()
                    .Where(recordDeclaration => recordDeclaration
                        .DescendantNodes()
                        .OfType<AttributeSyntax>()
                        .Any()
                    );

                foreach (var recordDeclaration in recordDeclarations)
                {
                    var recordSymbol = semanticModel.GetDeclaredSymbol(recordDeclaration);

                    (byte p, byte n, byte z, string componentType)? metricSignature = null;

                    foreach (var attributeData in recordSymbol.GetAttributes())
                    {
                        if (!attributeTypeSymbol.Equals(attributeData.AttributeClass, SymbolEqualityComparer.Default))
                        {
                            // This isn't the [GeometricNumber(...)] attribute
                            continue;
                        }

                        var namedArguments = attributeData.NamedArguments.ToDictionary(x => x.Key, x => x.Value);

                        byte p = 0;
                        byte n = 0;
                        byte z = 0;
                        string componentType = "System.Single";

                        if (namedArguments.TryGetValue("P", out var pArgument))
                        {
                            p = Convert.ToByte(pArgument.Value);
                        }

                        if (namedArguments.TryGetValue("N", out var nArgument))
                        {
                            n = Convert.ToByte(nArgument.Value);
                        }

                        if (namedArguments.TryGetValue("Z", out var zArgument))
                        {
                            z = Convert.ToByte(zArgument.Value);
                        }

                        if (namedArguments.TryGetValue("ComponentType", out var componentTypeArgument))
                        {
                            componentType = componentTypeArgument.Value.ToString();
                        }

                        metricSignature = (p, n, z, componentType);

                        break;
                    }

                    if (!metricSignature.HasValue)
                    {
                        continue;
                    }

                    var source = GenerateSource(recordSymbol, metricSignature.Value);

                    context.AddSource($"{recordSymbol.Name}.g.cs", SourceText.From(source, Encoding.UTF8));
                }
            }

        }

        public void Initialize(GeneratorInitializationContext context)
        {
#if DEBUG
            if (!Debugger.IsAttached)
            {
                //Debugger.Launch();
            }
#endif
        }
    }
}