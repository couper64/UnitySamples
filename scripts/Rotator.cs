using Unity.Entities;
using UnityEngine;

namespace peasantsLogic
{
	// one of the components making up Rotator entity;
	public class Rotator : MonoBehaviour
	{
		public Vector3 angularVelocity = Vector3.zero;
	}

	// a system which will operate on a set of entities;
	public class RotatorSystem : ComponentSystem
	{
		// component "filter" list specifying our entity;
		struct RotatorComponents
		{
			public Rotator rotator;
			public Transform transform;
		}

		protected override void OnUpdate()
		{
			// prepare container of entities with specified components;
			ComponentGroupArray<RotatorComponents> entities;

			// retrieve all available entities matching our search filter;
			entities = GetEntities<RotatorComponents>();

			// execute logic on each of them;
			foreach (var e in entities)
			{
				// we want to rotate root object using angularVelocity around itself;
				e.transform.Rotate(e.rotator.angularVelocity * Time.deltaTime, Space.Self);
			}
		}
	}
}

