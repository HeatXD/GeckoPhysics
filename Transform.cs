using Abacus.Fixed64Precision;
using System;

namespace GeckoPhysics
{
    public struct Transform : IEquatable<Transform>
    {
        public Vector3 Position, OldPosition;
        public Vector3 Velocity;
        public Vector3 Acceleration;
        public Quaternion Rotation;

        public override bool Equals(object obj)
        {
            return obj is Transform transform && Equals(transform);
        }

        public bool Equals(Transform other)
        {
            return Acceleration.Equals(other.Acceleration);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Acceleration);
        }

        public static bool operator ==(Transform left, Transform right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Transform left, Transform right)
        {
            return !(left == right);
        }
    }
}