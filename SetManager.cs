// Copyright (C) 2017 Robert A. Wallis, All Rights Reserved.

using System;
using System.Collections.Generic;

namespace ECSLight
{
	/// <summary>
	/// Keeps track of all the entity sets.
	/// Adds and removes entities to the appropriate sets.
	/// </summary>
	public class SetManager : ISetManager
	{
		private readonly IEnumerable<IEntity> _entities;
		private readonly List<EntitySet> _entitySets;

		public SetManager(IEnumerable<IEntity> entities)
		{
			_entities = entities;
			_entitySets = new List<EntitySet>();
		}

		/// <summary>
		/// Makes a new set and registers it for updating membership later.
		/// </summary>
		/// <returns>An enumerable list of entitySet, that will update automatically.</returns>
		public EntitySet CreateSet(EntitySet.IncludeInSet predicate)
		{
			var entitySet = new EntitySet(predicate);
			_entitySets.Add(entitySet);
			foreach (var entity in _entities) {
				if (!entitySet.Matches(entity))
					continue;
				entitySet.Add(entity);
			}
			return entitySet;
		}

		/// <summary>
		/// Unregisters a set so it will no longer get membership updates.
		/// </summary>
		public void RemoveSet(EntitySet set)
		{
			_entitySets.Remove(set);
		}

		/// <summary>
		/// Add entity to all matching sets, remove from any unmatching sets.
		/// </summary>
		public void UpdateSets(IEntity entity)
		{
			foreach (var set in _entitySets) {
				if (set.Matches(entity))
					set.Add(entity);
				else
					set.Remove(entity);
			}
		}

		/// <summary>
		/// Checks if the entity should be in the types list.
		/// </summary>
		/// <returns>true if the entity matches</returns>
		public static bool EntityMatchesTypes(IEntity entity, params Type[] types)
		{
			var all = false;
			foreach (var type in types) {
				if (entity.Contains(type)) {
					all = true;
				} else {
					all = false;
					break;
				}
			}
			return all;
		}
	}
}