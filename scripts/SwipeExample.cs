using System;
using UnityEngine;

/// <summary>
/// SwipeExample and SwipeInput are separated to have at least 
/// some level of abstraction from Unity.
/// Although, Input, Vector2 are still coupled with SwipeInput.
/// </summary>
public class SwipeExample : MonoBehaviour
{
	public C64.SwipeInput swipeInput = new C64.SwipeInput();

	private void Reset()
	{
		swipeInput.OnReset();
	}

	private void Update()
	{
		swipeInput.OnUpdate();
	}
}

/// <summary>
/// C64 - couper64.
/// </summary>
namespace C64
{
	[Serializable]
	public class SwipeInput
	{
		public enum SwipeState
		{
			NULL = 0,
			TAP,
			LEFT,
			RIGHT,
			UP,
			DOWN
		}

		#region Properties, Getters/Setters

		public SwipeState State
		{
			get
			{
				return state;
			}
		}

		public Vector2 TouchStart
		{
			get
			{
				return touchStart;
			}
		}

		public float SwipeDeadZone
		{
			get
			{
				return swipeDeadZone;
			}
		}

		public bool IsTouchEnabled
		{
			get
			{
				return isTouchEnabled;
			}
		}

		#endregion

		#region Public Events

		public event EventHandler OnSwipeLeftHandler;
		public event EventHandler OnSwipeRightHandler;
		public event EventHandler OnSwipeUpHandler;
		public event EventHandler OnSwipeDownHandler;
		public event EventHandler OnTapHandler;

		#endregion

		#region Private Member Variables

		[SerializeField]
		private SwipeState state;

		[SerializeField]
		private Vector2 touchStart;

		[SerializeField]
		private float swipeDeadZone;

		[SerializeField]
		private bool isTouchEnabled;

		#endregion

		#region Public Member Methods

		public void OnReset()
		{
			// Reset swipe state.
			state = SwipeState.NULL;

			// Reset start position.
			touchStart = Vector2.zero;

			// Reset to default swipe dead zone.
			swipeDeadZone = 100.00f;

			// Disable touch and mouse inputs.
			isTouchEnabled = false;
		}

		public void OnUpdate()
		{
			// Only if required.
			if (isTouchEnabled)
			{
				// Must be called at the beginning of the frame
				// because it registers inputs for the frame.
				TouchBegin();
			}

			// Set state.
			switch (state)
			{
				// Raise events.
				case SwipeState.LEFT: OnSwipeLeft(EventArgs.Empty); break;
				case SwipeState.RIGHT: OnSwipeRight(EventArgs.Empty); break;
				case SwipeState.UP: OnSwipeUp(EventArgs.Empty); break;
				case SwipeState.DOWN: OnSwipeDown(EventArgs.Empty); break;
				case SwipeState.TAP: OnTap(EventArgs.Empty); break;
				case SwipeState.NULL: break;
			}

			// Only if required.
			if (isTouchEnabled)
			{
				// Must be called at the end of the frame
				// because it resets all the settings back
				// for the next frame.
				TouchEnd();
			}
		}

		#endregion

		#region Private Member Methods

		private void TouchBegin()
		{
			// Track touches for debugging on phones.
			if (Input.touchCount > 0)
			{
				if (Input.touches[0].phase == TouchPhase.Began)
				{
					touchStart = Input.touches[0].position;
				}
			}
			// Track mouse buttons for debugging on PC.
			else if (Input.GetMouseButtonDown(0))
			{
				touchStart = Input.mousePosition;
			}
		}

		private void TouchEnd()
		{
			// Untrack touches for debugging on phones.
			if (Input.touchCount > 0)
			{
				if (
					Input.touches[0].phase == TouchPhase.Ended ||
					Input.touches[0].phase == TouchPhase.Canceled
				)
				{
					// Process results.
					ProcessInput();

					// Reset globals for the next frame.
					touchStart = Vector2.zero;
				}
			}
			// Untrack mouse buttons for debugging on PC.
			else if (Input.GetMouseButtonUp(0))
			{
				// Process results.
				ProcessInput();

				// Reset globals for the next frame.
				touchStart = Vector2.zero;
			}
		}

		private void ProcessInput()
		{
			// Local variables.
			Vector2 touchDelta = Vector2.zero;

			// Process inputs.
			if (Input.touchCount > 0)
			{
				touchDelta = Input.touches[0].position - touchStart;
			}
			else
			{
				touchDelta = (Vector2)Input.mousePosition - touchStart;
			}

			// Whether we consider the gesture to be a swipe or not.
			if (touchDelta.magnitude > swipeDeadZone)
			{
				// Local variables.
				float x = touchDelta.x;
				float y = touchDelta.y;

				// Reset to default state from previous frame.
				state = SwipeState.NULL;

				// Decide major direction of a swipe.
				if (Mathf.Abs(x) > Mathf.Abs(y))
				{
					// Left or right.
					if (x < 0)
					{
						// Left.
						state = SwipeState.LEFT;
					}
					else
					{
						// Right.
						state = SwipeState.RIGHT;
					}
				}
				else
				{
					// Up or down.
					if (y > 0)
					{
						// Up.
						state = SwipeState.UP;
					}
					else
					{
						// Down.
						state = SwipeState.DOWN;
					}
				}
			}
			else
			{
				// Otherwise, we assume it is a tap.
				state = SwipeState.TAP;
			}

			// Only during development.
			if (Debug.isDebugBuild)
			{
				Debug.Log(state.ToString());
			}
		}

		private void OnSwipeLeft(EventArgs e)
		{
			OnSwipeLeftHandler?.Invoke(this, e);

			// Reset.
			state = SwipeState.NULL;
		}

		private void OnSwipeRight(EventArgs e)
		{
			OnSwipeRightHandler?.Invoke(this, e);

			// Reset.
			state = SwipeState.NULL;
		}

		private void OnSwipeUp(EventArgs e)
		{
			OnSwipeUpHandler?.Invoke(this, e);

			// Reset.
			state = SwipeState.NULL;
		}

		private void OnSwipeDown(EventArgs e)
		{
			OnSwipeDownHandler?.Invoke(this, e);

			// Reset.
			state = SwipeState.NULL;
		}

		private void OnTap(EventArgs e)
		{
			OnTapHandler?.Invoke(this, e);

			// Reset.
			state = SwipeState.NULL;
		}

		#endregion
	}
}
