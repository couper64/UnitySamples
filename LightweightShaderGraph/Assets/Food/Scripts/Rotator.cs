using UnityEngine;

public class Rotator : MonoBehaviour {

	[SerializeField]
	public Transform[] food;

	private void Reset() {

		food = new Transform[transform.childCount];

		for (int i = 0; i < transform.childCount; i++) {
			food[i] = transform.GetChild(i);
		}

	}

	private void Update() {

		transform.Rotate(Vector3.right * Time.deltaTime * 45.00f, Space.Self);
		transform.Rotate(Vector3.up * Time.deltaTime * 45.00f, Space.Self);
		transform.Rotate(Vector3.forward * Time.deltaTime * 45.00f, Space.Self);
	}

}
