using System;

namespace ShalicoUtils
{
    public partial struct FractionInt
    {
        public static FractionInt operator +(FractionInt a, FractionInt b)
        {
            if (a._denominator == b._denominator)
                return new FractionInt(a._numerator + b._numerator, a._denominator);

            return new FractionInt(a._numerator * b._denominator + b._numerator * a._denominator,
                a._denominator * b._denominator);
        }

        public static FractionInt operator -(FractionInt a, FractionInt b)
        {
            if (a._denominator == b._denominator)
                return new FractionInt(a._numerator - b._numerator, a._denominator);

            return new FractionInt(a._numerator * b._denominator - b._numerator * a._denominator,
                a._denominator * b._denominator);
        }

        public static FractionInt operator *(FractionInt a, FractionInt b)
        {
            return new FractionInt(a._numerator * b._numerator, a._denominator * b._denominator);
        }

        public static FractionInt operator /(FractionInt a, FractionInt b)
        {
            return new FractionInt(a._numerator * b._denominator, a._denominator * b._numerator);
        }

        public static FractionInt operator +(FractionInt a, int b)
        {
            return new FractionInt(a._numerator + b * a._denominator, a._denominator);
        }

        public static FractionInt operator -(FractionInt a, int b)
        {
            return new FractionInt(a._numerator - b * a._denominator, a._denominator);
        }

        public static FractionInt operator *(FractionInt a, int b)
        {
            return new FractionInt(a._numerator * b, a._denominator);
        }

        public static FractionInt operator /(FractionInt a, int b)
        {
            return new FractionInt(a._numerator, a._denominator * b);
        }

        public static FractionInt operator +(int a, FractionInt b)
        {
            return new FractionInt(a * b._denominator + b._numerator, b._denominator);
        }

        public static FractionInt operator -(int a, FractionInt b)
        {
            return new FractionInt(a * b._denominator - b._numerator, b._denominator);
        }

        public static FractionInt operator *(int a, FractionInt b)
        {
            return new FractionInt(a * b._numerator, b._denominator);
        }

        public static FractionInt operator /(int a, FractionInt b)
        {
            return new FractionInt(a * b._denominator, b._numerator);
        }

        public static bool operator ==(FractionInt a, FractionInt b)
        {
            return a._numerator * b._denominator == b._numerator * a._denominator;
        }

        public static bool operator !=(FractionInt a, FractionInt b)
        {
            return a._numerator * b._denominator != b._numerator * a._denominator;
        }

        public static bool operator <(FractionInt a, FractionInt b)
        {
            return a._numerator * b._denominator < b._numerator * a._denominator;
        }

        public static bool operator >(FractionInt a, FractionInt b)
        {
            return a._numerator * b._denominator > b._numerator * a._denominator;
        }

        public static bool operator <=(FractionInt a, FractionInt b)
        {
            return a._numerator * b._denominator <= b._numerator * a._denominator;
        }

        public static bool operator >=(FractionInt a, FractionInt b)
        {
            return a._numerator * b._denominator >= b._numerator * a._denominator;
        }

        public static bool operator ==(FractionInt a, int b)
        {
            return a._numerator == b * a._denominator;
        }

        public static bool operator !=(FractionInt a, int b)
        {
            return a._numerator != b * a._denominator;
        }

        public static bool operator <(FractionInt a, int b)
        {
            return a._numerator < b * a._denominator;
        }

        public static bool operator >(FractionInt a, int b)
        {
            return a._numerator > b * a._denominator;
        }

        public static bool operator <=(FractionInt a, int b)
        {
            return a._numerator <= b * a._denominator;
        }

        public static bool operator >=(FractionInt a, int b)
        {
            return a._numerator >= b * a._denominator;
        }

        public static bool operator ==(int a, FractionInt b)
        {
            return a * b._denominator == b._numerator;
        }

        public static bool operator !=(int a, FractionInt b)
        {
            return a * b._denominator != b._numerator;
        }

        public static bool operator <(int a, FractionInt b)
        {
            return a * b._denominator < b._numerator;
        }

        public static bool operator >(int a, FractionInt b)
        {
            return a * b._denominator > b._numerator;
        }

        public static bool operator <=(int a, FractionInt b)
        {
            return a * b._denominator <= b._numerator;
        }

        public static bool operator >=(int a, FractionInt b)
        {
            return a * b._denominator >= b._numerator;
        }

        public override bool Equals(object obj)
        {
            return obj is FractionInt other && Equals(other);
        }

        public readonly override int GetHashCode()
        {
            return HashCode.Combine(_numerator, _denominator);
        }

        public readonly TypeCode GetTypeCode()
        {
            return TypeCode.Object;
        }
    }
}