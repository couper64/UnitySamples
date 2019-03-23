using UnityEngine;

public class Toggler : MonoBehaviour {

	[SerializeField]
	private Rotator rotator;

	private void Reset() {

		rotator = GetComponentInChildren<Rotator>();
	}

	public void Toggle(int foodIndex) {

		for (int i = 0; i < rotator.food.Length; i++) {

			// disable everything except chosen food;
			if (i != foodIndex) {
				rotator.food[i].gameObject.SetActive(false);
			}

		}

		// because we toggle it instead;
		rotator.food[foodIndex].gameObject.SetActive(!rotator.food[foodIndex].gameObject.activeSelf);
	}

}
