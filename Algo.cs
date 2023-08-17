using Abacus.Fixed64Precision;

namespace GeckoPhysics
{
    // References: 
    // https://developer.mozilla.org/en-US/docs/Games/Techniques/3D_collision_detection
    // https://www.jeffreythompson.org/collision-detection/circle-circle.php

    internal static class Algo
    {
        internal static bool AABBAABB(in Transform thisActor, in Transform otherActor, AABBCollider thisColl, AABBCollider otherColl)
        {
            var thisPos = thisActor.Position + Rotate(thisColl.LocalPosition, thisActor.Rotation);
            var otherPos = otherActor.Position + Rotate(otherColl.LocalPosition, otherActor.Rotation);

            var thisMax = thisPos + thisColl.Max;
            var thisMin = thisPos + thisColl.Min;
            var otherMax = otherPos + otherColl.Max;
            var otherMin = otherPos + otherColl.Min;

            if (thisMin.X > otherMax.X || thisMax.X < otherMin.X) return false; // No overlap along X-axis
            if (thisMin.Y > otherMax.Y || thisMax.Y < otherMin.Y) return false; // No overlap along Y-axis
            if (thisMin.Z > otherMax.Z || thisMax.Z < otherMin.Z) return false; // No overlap along Z-axis

            return true;
        }

        private static Vector3 Rotate(Vector3 point, Quaternion rotation)
        {
            var newPoint = rotation * Quaternion.CreateFromYawPitchRoll(point.Y, point.X, point.Z) * Quaternion.Conjugate(rotation);
            return newPoint.ToYawPitchRoll();
        }

        internal static bool AABBSphere(in Transform thisActor, in Transform otherActor, AABBCollider thisColl, SphereCollider otherColl)
        {
            var thisPos = thisActor.Position + Rotate(thisColl.LocalPosition, thisActor.Rotation);
            var otherPos = otherActor.Position + Rotate(otherColl.LocalPosition, otherActor.Rotation);

            var thisMax = thisPos + thisColl.Max;
            var thisMin = thisPos + thisColl.Min;

            Fixed64 sqrDist = 0;

            Fixed64 distX = Max(0, Max(thisMin.X - otherPos.X, otherPos.X - thisMax.X));
            sqrDist += distX * distX;

            Fixed64 distY = Max(0, Max(thisMin.Y - otherPos.Y, otherPos.Y - thisMax.Y));
            sqrDist += distY * distY;

            Fixed64 distZ = Max(0, Max(thisMin.Z - otherPos.Z, otherPos.Z - thisMax.Z));
            sqrDist += distZ * distZ;

            return sqrDist < (otherColl.Radius * otherColl.Radius);
        }

        private static Fixed64 Max(Fixed64 a, Fixed64 b)
        {
            return a >= b ? a : b;
        }

        internal static bool SphereSphere(in Transform thisActor, in Transform otherActor, SphereCollider thisColl, SphereCollider otherColl)
        {
            var thisPos = thisActor.Position + Rotate(thisColl.LocalPosition, thisActor.Rotation);
            var otherPos = otherActor.Position + Rotate(otherColl.LocalPosition, otherActor.Rotation);

            var distX = thisPos.X - otherPos.X;
            var distY = thisPos.Y - otherPos.Y;
            var distZ = thisPos.Z - otherPos.Z;

            // Instead of calculating the square root of the squared distance,
            // you can compare the squared distance directly to the sum of the squared radii.
            // This can save computational resources since square root calculations are relatively expensive.
            var sqrDistance = (distX * distX) + (distY * distY) + (distZ * distZ);
            var sqrRadiiSum = (thisColl.Radius + otherColl.Radius) * (thisColl.Radius + otherColl.Radius);

            return sqrDistance < sqrRadiiSum;
        }
    }


}