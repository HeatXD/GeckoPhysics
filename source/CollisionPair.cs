namespace GeckoPhysics
{
    internal class CollisionPair : IEquatable<CollisionPair>
    {
        public Actor ActorA;
        public Actor ActorB;
        public CollisionInfo Info;

        public CollisionPair(ref Actor actorA, ref Actor actorB)
        {
            ActorA = actorA ?? throw new ArgumentNullException(nameof(actorA));
            ActorB = actorB ?? throw new ArgumentNullException(nameof(actorB));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as CollisionPair);
        }

        public bool Equals(CollisionPair other)
        {
            return other is not null &&
                   (EqualityComparer<Actor>.Default.Equals(ActorA, other.ActorA) &&
                   EqualityComparer<Actor>.Default.Equals(ActorB, other.ActorB) ||
                   EqualityComparer<Actor>.Default.Equals(ActorA, other.ActorB) &&
                   EqualityComparer<Actor>.Default.Equals(ActorB, other.ActorA));
        }

        public override int GetHashCode()
        {
            return ActorA.GetHashCode() + ActorB.GetHashCode();
        }

        public static bool operator ==(CollisionPair left, CollisionPair right)
        {
            return EqualityComparer<CollisionPair>.Default.Equals(left, right);
        }

        public static bool operator !=(CollisionPair left, CollisionPair right)
        {
            return !(left == right);
        }
    }
}