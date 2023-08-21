using Abacus.Fixed64Precision;

namespace GeckoPhysics
{
    // References: 
    // https://developer.mozilla.org/en-US/docs/Games/Techniques/3D_collision_detection
    // https://www.jeffreythompson.org/collision-detection/circle-circle.php

    public static class Algo
    {
        private static Fixed64 epsilon = 0.0001;

        internal static bool AABBAABB(in Transform thisActor, in Transform otherActor, AABBCollider thisColl, AABBCollider otherColl, out CollisionInfo info)
        {
            var thisPos = thisActor.Position + Rotate(thisColl.LocalPosition, thisActor.Rotation);
            var otherPos = otherActor.Position + Rotate(otherColl.LocalPosition, otherActor.Rotation);

            var thisMin = thisPos - thisColl.HalfExtents;
            var thisMax = thisPos + thisColl.HalfExtents;
            var otherMin = otherPos - otherColl.HalfExtents;
            var otherMax = otherPos + otherColl.HalfExtents;

            if (thisMin.X <= otherMax.X && thisMax.X >= otherMin.X &&
                thisMin.Y <= otherMax.Y && thisMax.Y >= otherMin.Y &&
                thisMin.Z <= otherMax.Z && thisMax.Z >= otherMin.Z)
            {
                // AABB collision occurred
                info.Depth = CalculateOverlapDepth(thisMin, thisMax, otherMin, otherMax) + epsilon;
                info.Normal = CalculateCollisionNormal(thisPos, otherPos);
                return true;
            }

            info.Depth = 0;
            info.Normal = Vector3.Zero;
            return false;
        }

        public static Vector3 Rotate(Vector3 point, Quaternion rotation)
        {
            var newPoint = rotation * Quaternion.CreateFromYawPitchRoll(point.Y, point.X, point.Z) * Quaternion.Conjugate(rotation);
            return rotation != Quaternion.Identity ? newPoint.ToYawPitchRoll() : point;
        }

        internal static bool AABBSphere(in Transform thisActor, in Transform otherActor, AABBCollider thisColl, SphereCollider otherColl, out CollisionInfo info)
        {
            var thisPos = thisActor.Position + Rotate(thisColl.LocalPosition, thisActor.Rotation);
            var otherPos = otherActor.Position + Rotate(otherColl.LocalPosition, otherActor.Rotation);

            var closestPoint = Vector3.Clamp(otherPos, thisPos - thisColl.HalfExtents, thisPos + thisColl.HalfExtents);
            var delta = otherPos - closestPoint;
            var distanceSquared = Vector3.Dot(delta, delta);

            if (distanceSquared <= otherColl.Radius * otherColl.Radius)
            {
                // AABB-Sphere collision occurred
                info.Depth = otherColl.Radius - Fixed64.Sqrt(distanceSquared) + epsilon;
                info.Normal = CalculateCollisionNormal(thisPos, otherPos);
                return true;
            }

            info.Depth = 0;
            info.Normal = Vector3.Zero;
            return false;
        }

        private static Fixed64 Max(Fixed64 a, Fixed64 b)
        {
            return a >= b ? a : b;
        }

        private static Fixed64 Min(Fixed64 a, Fixed64 b)
        {
            return a <= b ? a : b;
        }

        internal static bool SphereSphere(in Transform thisActor, in Transform otherActor, SphereCollider thisColl, SphereCollider otherColl, out CollisionInfo info)
        {
            var thisPos = thisActor.Position + Rotate(thisColl.LocalPosition, thisActor.Rotation);
            var otherPos = otherActor.Position + Rotate(otherColl.LocalPosition, otherActor.Rotation);

            var delta = otherPos - thisPos;
            var distanceSquared = Vector3.Dot(delta, delta);
            var sumRadii = thisColl.Radius + otherColl.Radius;

            if (distanceSquared <= sumRadii * sumRadii)
            {
                // Sphere-Sphere collision occurred
                info.Depth = sumRadii - Fixed64.Sqrt(distanceSquared) + epsilon;
                info.Normal = CalculateCollisionNormal(thisPos, otherPos);
                return true;
            }

            info.Depth = 0;
            info.Normal = Vector3.Zero;
            return false;
        }

        private static Fixed64 CalculateOverlapDepth(Vector3 thisMin, Vector3 thisMax, Vector3 otherMin, Vector3 otherMax)
        {
            var xOverlap = Max(0, Min(thisMax.X - otherMin.X, otherMax.X - thisMin.X));
            var yOverlap = Max(0, Min(thisMax.Y - otherMin.Y, otherMax.Y - thisMin.Y));
            var zOverlap = Max(0, Min(thisMax.Z - otherMin.Z, otherMax.Z - thisMin.Z));
            return Min(xOverlap, Min(yOverlap, zOverlap));
        }

        private static Vector3 CalculateCollisionNormal(Vector3 thisPos, Vector3 otherPos)
        {
            var delta = otherPos - thisPos;
            var distanceSquared = Vector3.Dot(delta, delta);

            // Define a small threshold to avoid division by very small numbers

            if (distanceSquared < epsilon * epsilon)
                return Vector3.Up;
            else
                return Vector3.Normalise(delta);
        }
    }
}