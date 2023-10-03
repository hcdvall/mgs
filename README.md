# mgs
Expanding my Snake project from the last course in data structures and algorithms.
Recently an enemy with pathfinding was added.

Patterns:
- Singleton (barely): in `ObjectPooler.cs` in `class ObjectPooler`
  Used it to make an instance of the ObjectPooler.

- Pooling: in `ObjectPooler.cs` and `FoodSpawn.cs` in the method `FoodSpawner`
  Used to create a pool of food for spawning. 
  Can also be used to create additional pools for power ups and tails.
  Probably a boilerplate use case for object pooling.

- Observer: in `Event.cs` in `class Event`, `EventListener.cs` in `class EventListener`, `EnemyMovement.cs` in `method FoodDropped`, `FoodSpawn.cs`
  Used the pattern to observe when it was time to calculate a new path for the enemy.
  I created the event foodDrop that observe when the method FoodSpawner is called.
  When food is spawned, the method FoodDropped() will receive the position of the food and find a new path.
  Initially I calculated new paths every Update() and figured I only need to do this when new food is spawning.
  The observer pattern might be good to use in this case. 
