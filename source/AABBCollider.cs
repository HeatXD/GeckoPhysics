using Abacus.Fixed64Precision;

namespace GeckoPhysics
{
    internal class AABBCollider : Collider
    {
        public Vector3 Min, Max;

        public AABBCollider(Vector3 position, Vector3 min, Vector3 max, bool active = true)
        {
            LocalPosition = position;
            Min = min;
            Max = max;

            SetActive(active);
        }

        public override bool CheckCollision(ref Transform thisActor, ref Transform otherActor, ICollider other)
        {
            return other.GetColliderType() switch
            {
                ColliderType.Sphere => Algo.AABBSphere(thisActor, otherActor, this, (SphereCollider)other),
                ColliderType.AABB => Algo.AABBAABB(thisActor, otherActor, this, (AABBCollider)other),
                _ => false,
            };
        }

        public override ColliderType GetColliderType()
        {
            return ColliderType.AABB;
        }
    }
}
