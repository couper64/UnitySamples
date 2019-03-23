using UnityEngine;

public class MaterialSwitcher : MonoBehaviour {

	[SerializeField]
	private Rotator rotator;

	[SerializeField]
	private Material[] materials;

	private void Reset() {

		rotator = GetComponent<Rotator>();

		// we need to assign them manualy;
		// from the editor;
		materials = new Material[3];
	}

	public void SwitchFresnel() {

		Material material = null;

		for (int i = 0; i < materials.Length; i++) {

			if (materials[i].name.StartsWith("Food_Fresnel")) {

				material = materials[i];
			}
		}

		for (int i = 0; i < rotator.food.Length; i++) {

			rotator.food[i].GetComponent<MeshRenderer>().material = material;
		}
	}

	public void SwitchDissolve() {

		Material material = null;

		for (int i = 0; i < materials.Length; i++)
		{

			if (materials[i].name.StartsWith("Food_Dissolve")) {
				material = materials[i];
			}
		}

		for (int i = 0; i < rotator.food.Length; i++) {
			rotator.food[i].GetComponent<MeshRenderer>().material = material;
		}
	}

	public void SwitchHologram() {

		Material material = null;

		for (int i = 0; i < materials.Length; i++) {

			if (materials[i].name.StartsWith("Food_Hologram")) {
				material = materials[i];
			}
		}

		for (int i = 0; i < rotator.food.Length; i++) {
			rotator.food[i].GetComponent<MeshRenderer>().material = material;
		}
	}

}
