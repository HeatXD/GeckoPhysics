using Abacus.Fixed64Precision;
using System;
using System.Collections.Generic;

namespace GeckoPhysics
{
    public interface ICollider
    {
        public bool CheckCollision(ref Transform thisActor, ref Transform otherActorPos, ICollider other);

        public void SetActive(bool active);

        public bool IsActive();

        public ColliderType GetColliderType();
    }

    public enum ColliderType
    {
        Sphere = 0,
        AABB,
    }

    public abstract class Collider : ICollider, IEquatable<Collider>
    {
        public Vector3 LocalPosition;
        
        private bool _active;

        public abstract ColliderType GetColliderType();

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return Equals(obj as Collider);
        }

        public bool Equals(Collider other)
        {
            return other is not null &&
                   LocalPosition.Equals(other.LocalPosition) &&
                   _active == other._active;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(LocalPosition, _active);
        }

        public bool IsActive()
        {
            return _active;
        }

        public void SetActive(bool active)
        {
            _active = active;
        }

        public abstract bool CheckCollision(ref Transform thisActorPos, ref Transform otherActorPos, ICollider other);

        public static bool operator ==(Collider left, Collider right)
        {
            return EqualityComparer<Collider>.Default.Equals(left, right);
        }

        public static bool operator !=(Collider left, Collider right)
        {
            return !(left == right);
        }
    }
}
