using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ShalicoUtils
{
    [Serializable]
    public partial struct FractionInt : IEquatable<FractionInt>, IComparable<FractionInt>, IEquatable<int>,
        IComparable<int>
    {
        public static readonly FractionInt Zero = new(0);
        public static readonly FractionInt Half = new(1, 2);
        public static readonly FractionInt Quarter = new(1, 4);
        public static readonly FractionInt One = new(1);
        [SerializeField] private int _numerator;
        [SerializeField] private int _denominator;

        public FractionInt(int numerator, int denominator)
        {
            if (denominator == 0) throw new DivideByZeroException("Denominator must not be zero");
            _numerator = numerator;
            _denominator = denominator;
        }

        public FractionInt(int numerator)
        {
            _numerator = numerator;
            _denominator = 1;
        }

        public FractionInt(double value, double accuracy = 1E-6)
        {
            var sign = Math.Sign(value);
            value = Math.Abs(value);

            if (Math.Abs(value % 1) < accuracy) // もし完全な整数であれば
            {
                _numerator = (int)value * sign;
                _denominator = 1;
                return;
            }

            var integerPart = (int)value;
            var fractionalPart = value - integerPart;
            var previousDenominator = 0;
            var denominator = 1;

            while (true)
            {
                var reciprocal = 1.0 / fractionalPart;
                var integralPartOfReciprocal = (int)reciprocal;
                fractionalPart = reciprocal - integralPartOfReciprocal;

                var temp = denominator;
                denominator = denominator * integralPartOfReciprocal + previousDenominator;
                previousDenominator = temp;

                var numerator = (int)Math.Round(value * denominator);

                if (Math.Abs(value - (double)numerator / denominator) < accuracy || fractionalPart < accuracy)
                {
                    _numerator = numerator * sign;
                    _denominator = denominator;
                    return;
                }
            }
        }

        public FractionInt(FractionInt other)
        {
            _numerator = other._numerator;
            _denominator = other._denominator;
        }

        public int Sign => Math.Sign(_numerator) * Math.Sign(_denominator);

        public int Numerator => _numerator;
        public int Denominator => _denominator;

        public readonly int CompareTo(FractionInt other)
        {
            var numeratorComparison = _numerator.CompareTo(other._numerator);
            if (numeratorComparison != 0) return numeratorComparison;
            return _denominator.CompareTo(other._denominator);
        }

        public readonly int CompareTo(int other)
        {
            return _numerator.CompareTo(other * _denominator);
        }

        public readonly bool Equals(FractionInt other)
        {
            return _numerator == other._numerator && _denominator == other._denominator;
        }

        public readonly bool Equals(int other)
        {
            return _numerator == other * _denominator;
        }

        public readonly FractionInt Reduce()
        {
            return Reduce(this);
        }

        public readonly override string ToString()
        {
            return $"{_numerator}/{_denominator}";
        }

        public readonly void Deconstruct(out int numerator, out int denominator)
        {
            numerator = _numerator;
            denominator = _denominator;
        }

        public readonly FractionInt Inverse()
        {
            return new FractionInt(_denominator, _numerator);
        }

        public readonly float ToFloat()
        {
            return (float)_numerator / _denominator;
        }

        public readonly int ToInt()
        {
            return _numerator / _denominator;
        }

        public readonly int ToIntByCeil()
        {
            return (_numerator + _denominator - 1) / _denominator;
        }

        public readonly int ToIntByFloor()
        {
            return (_numerator - _denominator + 1) / _denominator;
        }

        public readonly int ToIntByRound()
        {
            return (_numerator >= 0 ? _numerator + _denominator / 2 : _numerator - _denominator / 2) / _denominator;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Gcd(int a, int b)
        {
            while (b != 0)
            {
                var r = a % b;
                a = b;
                b = r;
            }

            return a;
        }

        public static FractionInt Reduce(FractionInt fraction)
        {
            var gcd = Gcd(fraction._numerator, fraction._denominator);
            return new FractionInt(fraction._numerator / gcd, fraction._denominator / gcd);
        }
    }
}