using Abacus.Fixed64Precision;

namespace GeckoPhysics
{
    public class AABBCollider : Collider
    {
        public Vector3 HalfExtents;

        public AABBCollider(Vector3 position, Vector3 halfextents, bool active = true)
        {
            LocalPosition = position;
            HalfExtents = halfextents;

            SetActive(active);
        }

        public override bool CheckCollision(ref Transform thisActor, ref Transform otherActor, ICollider other, out CollisionInfo info)
        {
            info = default;

            return other.GetColliderType() switch
            {
                ColliderType.Sphere => Algo.AABBSphere(thisActor, otherActor, this, (SphereCollider)other, out info),
                ColliderType.AABB => Algo.AABBAABB(thisActor, otherActor, this, (AABBCollider)other, out info),
                _ => false,
            };
        }

        public override ColliderType GetColliderType()
        {
            return ColliderType.AABB;
        }

        public Vector3 GetHalfExtends()
        {
            return HalfExtents;
        }
    }
}
