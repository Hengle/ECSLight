// Copyright (C) 2017 Robert A. Wallis, All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;

namespace ECSLight
{
	public class ComponentManager : IComponentManager
	{
		private readonly Dictionary<IEntity, Dictionary<Type, object>> _components;
		private readonly ISetManager _setManager;
		private static readonly IEnumerator<object> EmptyComponents = new List<object>(0).GetEnumerator();

		public ComponentManager(ISetManager setManager)
		{
			_components = new Dictionary<IEntity, Dictionary<Type, object>>();
			_setManager = setManager;
		}

		/// <summary>
		/// Attach a component, or replace a component, with the new component.
		/// </summary>
		/// <typeparam name="TComponent">Type of component to attach.</typeparam>
		/// <param name="entity">IEntity to which the component should be attached.</param>
		/// <param name="component">Component to attach to the entity.</param>
		public void AddComponent<TComponent>(IEntity entity, TComponent component) where TComponent : class
		{
			var type = typeof(TComponent);

			// add component to entity
			if (!_components.ContainsKey(entity))
				_components[entity] = new Dictionary<Type, object>(1);

			_components[entity][type] = component;
			_setManager.UpdateSets(entity);
		}

		/// <summary>
		/// Check if an entity has a component.
		/// </summary>
		/// <typeparam name="TComponent">Type of component that may be attached to the entity.</typeparam>
		/// <param name="entity">IEntity to check if component is attached.</param>
		/// <returns>`true` if the entity has a component of that type attached</returns>
		public bool ContainsComponent<TComponent>(IEntity entity)
		{
			var type = typeof(TComponent);
			return ContainsComponent(entity, type);
		}

		/// <summary>
		/// Check if an entity has a component.
		/// </summary>
		/// <param name="entity">IEntity to check if component is attached.</param>
		/// <param name="type">Type of component that may be attached to the entity.</param>
		/// <returns>`true` if the entity has a component of that type attached</returns>
		public bool ContainsComponent(IEntity entity, Type type)
		{
			if (!_components.ContainsKey(entity))
				return false;
			var entityComponents = _components[entity];
			return entityComponents.ContainsKey(type);
		}

		/// <summary>
		/// Gets the component attached to the entity.
		/// </summary>
		/// <typeparam name="TComponent">Type of component to remove.</typeparam>
		/// <param name="entity">Which entity owns the component?</param>
		/// <returns>`null` if no component is attached</returns>
		public TComponent ComponentFrom<TComponent>(IEntity entity) where TComponent : class
		{
			if (!_components.ContainsKey(entity))
				return null;
			if (!_components[entity].ContainsKey(typeof(TComponent)))
				return null;
			return _components[entity][typeof(TComponent)] as TComponent;
		}

		/// <summary>
		/// Removes the component from the entity.
		/// </summary>
		/// <typeparam name="TComponent">Type of component to remove from entitiy.</typeparam>
		/// <param name="entity">IEntity from which component is removed.</param>
		public void RemoveComponent<TComponent>(IEntity entity) where TComponent : class
		{
			var type = typeof(TComponent);
			RemoveComponent(entity, type);
		}

		/// <summary>
		/// Removes the component from the entity.
		/// </summary>
		/// <param name="entity">IEntity from which component is removed.</param>
		/// <param name="type">Type of component to remove from entitiy.</param>
		public void RemoveComponent(IEntity entity, Type type)
		{
			if (!_components.ContainsKey(entity))
				return;
			var components = _components[entity];
			if (!components.ContainsKey(type))
				return;
			var disposable = components[type] as IDisposable;
			if (disposable != null)
				disposable.Dispose();
			components.Remove(type);
			_setManager.UpdateSets(entity);
		}

		/// <summary>
		/// Enumerate through the components in an entity.
		/// </summary>
		public IEnumerator<object> GetEnumerator(IEntity entity)
		{
			if (!_components.ContainsKey(entity))
				return EmptyComponents;
			return _components[entity].Values.ToList().GetEnumerator();
		}
	}
}