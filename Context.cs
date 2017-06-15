﻿// Copyright (C) 2017 Robert A. Wallis, All Rights Reserved.

using System;
using System.Collections;
using System.Collections.Generic;

namespace ECSLight
{
	/// <summary>
	/// A context manages a set of entities and matchers.
	/// For example, a game could have a 'board' context with 'piece' entities.
	/// And a multiplayer game could have multiple 'board' contexts.
	/// </summary>
	public class Context : IEnumerable<Entity>
	{
		private readonly Dictionary<Entity, Dictionary<Type, IComponent>> _allEntities;
		private readonly SetManager _setManager;
		private readonly EntityManager _entityManager;

		public Context(int capacity = 128)
		{
			_allEntities = new Dictionary<Entity, Dictionary<Type, IComponent>>(capacity);
			_setManager = new SetManager(_allEntities);
			var componentManager = new ComponentManager(_allEntities, _setManager);
			_entityManager = new EntityManager(_allEntities, componentManager);
		}

		/// <summary>
		/// Make a new entity, or recycle an unused entity.
		/// </summary>
		/// <returns>new empty entity</returns>
		public Entity CreateEntity(string name = "")
		{
			return _entityManager.CreateEntity(name);
		}

		/// <summary>
		/// Release the entity back to be reused later.
		/// </summary>
		/// <param name="entity">Entity to be released.</param>
		public void ReleaseEntity(Entity entity)
		{
			_entityManager.ReleaseEntity(entity);
		}

		/// <summary>
		/// Returns all entities that have the specified components.
		/// </summary>
		/// <returns>An enumerable list of entities, that will update automatically.</returns>
		public HashSet<Entity> SetContaining(params Type[] types)
		{
			return _setManager.SetContaining(types);
		}

		/// <summary>
		/// Enumerator for all the entities.
		/// </summary>
		public IEnumerator<Entity> GetEnumerator()
		{
			return _allEntities.Keys.GetEnumerator();
		}

		/// <summary>
		/// Enumerator for all the entities.
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}