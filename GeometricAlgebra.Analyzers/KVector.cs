using Microsoft.CodeAnalysis;
using System.Linq;
using System;
using System.Collections.Generic;

namespace GeometricAlgebra.Analyzers
{
    internal class KVector
    {
        public static readonly KVector S = new(Array.Empty<Vector>());
        private IReadOnlyCollection<Vector> Vectors { get; }
        public int K => Vectors.Count;
        public float Sign { get; }

        public KVector(IReadOnlyCollection<Vector> vectors)
        {
            Vectors = vectors;
        }

        public static (KVector, float, bool parallel) GetBasisAndSign(KVector left, KVector right)
        {
            float sign = 1;
            var parallel = false;

            var leftVectors = new List<Vector>(left.Vectors);
            var rightVectors = new List<Vector>(right.Vectors);
            var mergedVectors = new List<Vector>();

            while (leftVectors.Any() && rightVectors.Any())
            {
                var leftVector = leftVectors.First();
                var rightVector = rightVectors.First();

                if (leftVector == rightVector)
                {
                    parallel = true;

                    sign *= leftVector.MagnitudeSquared;

                    if (leftVectors.Count % 2 == 0)
                    {
                        sign *= -1;
                    }

                    leftVectors.Remove(leftVector);
                    rightVectors.Remove(rightVector);
                }
                else if (leftVector < rightVector)
                {
                    leftVectors.Remove(leftVector);
                    mergedVectors.Add(leftVector);
                }
                else
                {
                    if (leftVectors.Count % 2 == 1)
                    {
                        sign *= -1;
                    }

                    rightVectors.Remove(rightVector);
                    mergedVectors.Add(rightVector);
                }
            }

            if (leftVectors.Any())
            {
                mergedVectors.AddRange(leftVectors);
            }
            else if (rightVectors.Any())
            {
                mergedVectors.AddRange(rightVectors);
            }

            return (new KVector(mergedVectors), sign, parallel);
        }

        public KVector AppendUnorderedBasisVector(Vector vector)
        {
            var orderedVectors = Vectors
                .Append(vector)
                .OrderBy(x => x.SymbolOrder)
                .ThenBy(x => x.Index)
                .ToArray();

            return new KVector(orderedVectors);
        }

        public static KVector FromBasisVector(Vector vector)
        {
            return new KVector(new Vector[] { vector });
        }

        public override string ToString()
        {
            if (Vectors.Count == 0)
            {
                return "S";
            }

            return string.Join("", Vectors.Select(vector => vector.ToString()));
        }
    }
}