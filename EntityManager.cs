using System;
using System.Collections.Generic;

namespace ECSLight
{
	public class EntityManager
	{
		private readonly Dictionary<Entity, Dictionary<Type, IComponent>> _entities;
		private readonly IComponentManager _componentManager;

		public EntityManager(Dictionary<Entity, Dictionary<Type, IComponent>> entities, IComponentManager componentManager)
		{
			_entities = entities;
			_componentManager = componentManager;
		}

		/// <summary>
		/// Make a new entity, or recycle an unused entity.
		/// </summary>
		/// <returns>new empty entity</returns>
		public Entity CreateEntity()
		{
			var entity = new Entity(_componentManager);
			_entities.Add(entity, new Dictionary<Type, IComponent>());
			return entity;
		}

		/// <summary>
		/// Release the entity back to be reused later.
		/// </summary>
		/// <param name="entity">Entity to be released.</param>
		public void ReleaseEntity(Entity entity)
		{
			var types = new List<Type>();
			foreach (var component in entity) {
				types.Add(component.GetType());
			}
			foreach (var type in types) {
				_componentManager.RemoveComponent(entity, type);
			}
			_entities.Remove(entity);
		}
	}
}