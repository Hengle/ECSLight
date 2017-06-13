﻿// Copyright (C) 2017 Robert A. Wallis, All Rights Reserved.

using System.Collections;
using ECSLight;
using NUnit.Framework;
using Tests.Stubs;

namespace Tests
{
	[TestFixture]
	public class EntityTests
	{
		[Test]
		public void Components()
		{
			var context = new Context();

			// init
			var e = context.CreateEntity();
			Assert.IsFalse(e.Contains<AComponent>());

			// add
			var aComponent = new AComponent("a1");
			e.Add(aComponent);
			Assert.IsTrue(e.Contains<AComponent>());
			Assert.AreSame(aComponent, e.Get<AComponent>());
			var bComponent = new BComponent();
			Assert.IsFalse(e.Contains<BComponent>());
			e.Add(bComponent);
			Assert.AreSame(bComponent, e.Get<BComponent>());
			Assert.AreSame(aComponent, e.Get<AComponent>());

			// replace
			var aComponent2 = new AComponent("a2");
			Assert.AreNotSame(aComponent2, e.Get<AComponent>());
			e.Add(aComponent2);
			Assert.AreSame(aComponent2, e.Get<AComponent>());

			// remove
			e.Remove<AComponent>();
			Assert.IsFalse(e.Contains<AComponent>());
		}

		[Test]
		public void CoverIEnumerable()
		{
			var entity = new Entity(new StubComponentManager());
			var enumerable = entity as IEnumerable;
			enumerable.GetEnumerator();
			// didn't crash
		}
	}
}