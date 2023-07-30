using System;

namespace GeometricAlgebra.Analyzers
{
    internal class Vector
    {
        public float MagnitudeSquared { get; }
        public char Symbol { get; }
        public byte SymbolOrder { get; }
        public byte Index { get; }

        public Vector(float squared, char symbol, byte symbolOrder, byte index)
        {
            MagnitudeSquared = squared;
            Symbol = symbol;
            SymbolOrder = symbolOrder;
            Index = index;
        }

        public static bool operator ==(Vector left, Vector right)
        {
            return left.SymbolOrder == right.SymbolOrder && left.Index == right.Index;
        }

        public static bool operator !=(Vector left, Vector right)
        {
            throw new NotImplementedException();
        }

        public static bool operator <(Vector left, Vector right)
        {
            if (left.SymbolOrder > right.SymbolOrder)
            {
                return false;
            }

            if (left.SymbolOrder < right.SymbolOrder)
            {
                return true;
            }

            return left.Index < right.Index;
        }

        public static bool operator >(Vector left, Vector right)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"{Symbol}{Index}";
        }
    }
}