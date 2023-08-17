using Abacus.Fixed64Precision;
using System;
using System.Collections.Generic;

namespace GeckoPhysics
{
    public class Actor : IEquatable<Actor>
    {
        private bool _active;
        private List<Collider> _colliders;
        private ActorType _actorType;
        private Int32 _collisionMask, _resolutionMask;

        public Transform Transform;

        public Actor(ActorType actor = ActorType.Static, bool enabled = true, Int32 collisionMask = 0, Int32 resolutionMask = 0) {
            _colliders = new List<Collider>();
            // amount it can hold without resizing
            _colliders.Capacity = 10;

            _resolutionMask = resolutionMask;
            _collisionMask = collisionMask;

            _actorType = actor;
            _active = enabled;
        }

        public ref List<Collider> GetColliders()
        {
            return ref _colliders;
        }

        public void AddCollider(Collider collider)
        {
            _colliders.Add(collider);
        }

        public void SetActive(bool active)
        {
            _active = active;
        }

        public bool IsActive()
        {
            return _active;
        }

        public void SetActorType(ActorType actor)
        {
            _actorType = actor;
        }

        public ActorType GetActorType()
        {
            return _actorType;
        }

        public ref Int32 CollisionMask() => ref _collisionMask;

        public ref Int32 ResolutionMask() => ref _resolutionMask; 

        public bool IsMaskSet(int index, Int32 mask)
        {
            if (index < 0 || index > 31)
                return false;

            return (mask & (1 << index)) != 0;
        }

        public void SetMask(int index, ref Int32 mask)
        {
            if (index < 0 || index > 31)
                return;

            mask |= 1 << index;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Actor);
        }

        public bool Equals(Actor other)
        {
            return other is not null &&
                   _active == other._active &&
                   EqualityComparer<List<Collider>>.Default.Equals(_colliders, other._colliders) &&
                   _actorType == other._actorType &&
                   _collisionMask == other._collisionMask &&
                   EqualityComparer<Transform>.Default.Equals(Transform, other.Transform);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_active, _colliders, _actorType, _collisionMask, Transform);
        }

        public static bool operator ==(Actor left, Actor right)
        {
            return EqualityComparer<Actor>.Default.Equals(left, right);
        }

        public static bool operator !=(Actor left, Actor right)
        {
            return !(left == right);
        }
    }
}
