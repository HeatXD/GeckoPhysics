using Abacus.Fixed64Precision;
using System;
using System.Collections.Generic;

namespace GeckoPhysics
{
	public class PhysicsWorld
	{
		private List<Actor> _actors;
		private Dictionary<int, CollisionPair> _lastCollisions;

		public PhysicsWorld()
		{
			_actors = new List<Actor>();
			_lastCollisions = new Dictionary<int, CollisionPair>();
			// amount it can hold without resizing
			_actors.Capacity = 25;
		}

		public void Step(Fixed64 delta)
		{
			ApplyMovement(delta);
			CollectCollisions();
			ResolveCollisions();
			NotifyActors();
		}

		private void NotifyActors()
		{
			if (_lastCollisions.Count > 0)
                Console.WriteLine("notifying-actors");
		}

		private void ResolveCollisions()
		{
			if (_lastCollisions.Count > 0)
				Console.WriteLine("num-collisions: " + _lastCollisions.Count);
		}

		private void CollectCollisions()
		{
			// clear last collisions
			_lastCollisions.Clear();

			// handle collisions
			int len = _actors.Count;
			for (int I = 0; I < len; I++)
			{
				var actorA = _actors[I];

				if (!actorA.IsActive())
					continue;

				var aColliders = actorA.GetColliders();
				for (int J = 0; J < len; J++)
				{
					var actorB = _actors[J];

					if (!actorB.IsActive() || actorA == actorB)
						continue;

					// check if the collision pair already seems to exist because if it does we can skip it
					var collPair = new CollisionPair(ref actorA, ref actorB);
					// GD.Print(collPair.GetHashCode());
					if (_lastCollisions.ContainsKey(collPair.GetHashCode()))
						continue;

					// check If they have a collision mask in common 
					bool hasCommonMask = false;
					for (int K = 0; K < 32; K++)
					{
						hasCommonMask = actorA.IsMaskSet(K, actorA.CollisionMask()) && actorB.IsMaskSet(K, actorB.CollisionMask());
						if (hasCommonMask)
							break;
					}

					// ignore static <-> static collisions
					bool bothStatic = actorA.GetActorType() == ActorType.Static && actorB.GetActorType() == ActorType.Static;
					if (bothStatic || !hasCommonMask)
						continue;

					for (int L = 0; L < aColliders.Count; L++)
					{
						var aColl = aColliders[L];
						if (!aColl.IsActive())
							continue;

						var bColliders = actorB.GetColliders();

						bool collisionFound = false;
						for (int M = 0; M < bColliders.Count; M++)
						{
							var bColl = bColliders[M];
							if (!bColl.IsActive())
								continue;

							// check if actors collided
							collisionFound = HasCollided(ref actorA, ref actorB, ref aColl, ref bColl);
							if (collisionFound)
							{
								_lastCollisions.Add(collPair.GetHashCode(), collPair);
								break;
							}
						}
						// collision found? break since we already found a collision between these actors.
						if (collisionFound)
							break;
					}
				}
			}
		}

		private bool HasCollided(ref Actor actorA, ref Actor actorB, ref Collider aColl, ref Collider bColl)
		{
			return aColl.CheckCollision(ref actorA.Transform, ref actorB.Transform, bColl);
		}

		private void ApplyMovement(Fixed64 delta)
		{
			foreach (var actor in _actors)
			{
				// keep track of the old position might come in helpfull later while resolving collisions
				actor.Transform.OldPosition = actor.Transform.Position;
				// skip them if they are inactive or they are not dynamic 
				if (actor.GetActorType() != ActorType.Dynamic || !actor.IsActive())
					continue;
				// update to new position
				actor.Transform.Velocity += actor.Transform.Acceleration * delta;
				actor.Transform.Position += actor.Transform.Velocity * delta;
			}
		}

		public Actor CreateActor(ActorType actType = ActorType.Static, Int32 collMask = 0, Int32 resMask = 0, bool enabled = true)
		{
			_actors.Add(new Actor(actType, enabled, collMask, resMask));
			return _actors[_actors.Count - 1];
		}

		public List<Actor> GetActors()
		{
			return _actors;
		}
	}
}
