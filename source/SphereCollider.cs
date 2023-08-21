using Abacus.Fixed64Precision;

namespace GeckoPhysics
{
    public class SphereCollider : Collider, IEquatable<SphereCollider>
    {
        public Fixed64 Radius;

        public SphereCollider(Vector3 position, Fixed64 radius, bool active = true)
        {
            LocalPosition = position;
            Radius = radius;
            Console.WriteLine(LocalPosition);
            SetActive(active);
        }

        public override bool CheckCollision(ref Transform thisActor, ref Transform otherActor, ICollider other, out CollisionInfo info)
        {
            info = default;

            return other.GetColliderType() switch
            {
                ColliderType.Sphere => Algo.SphereSphere(thisActor, otherActor, this, (SphereCollider)other, out info),
                ColliderType.AABB => Algo.AABBSphere(otherActor, thisActor, (AABBCollider)other, this, out info),
                _ => false,
            };
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SphereCollider);
        }

        public bool Equals(SphereCollider other)
        {
            return other is not null &&
                   base.Equals(other) &&
                   LocalPosition.Equals(other.LocalPosition) &&
                   Radius.Equals(other.Radius);
        }

        public override ColliderType GetColliderType()
        {
            return ColliderType.Sphere;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), LocalPosition, Radius);
        }

        public static bool operator ==(SphereCollider left, SphereCollider right)
        {
            return EqualityComparer<SphereCollider>.Default.Equals(left, right);
        }

        public static bool operator !=(SphereCollider left, SphereCollider right)
        {
            return !(left == right);
        }
    }
}
