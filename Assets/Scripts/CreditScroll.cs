
using UnityEngine;
using UnityEngine.UI;

public class CreditScroll : MonoBehaviour {

	[SerializeField]
	private float scrollSpeed = 150f;
	[SerializeField]
	private Text creditText;
	public MainMenu mainMenu;

	private Vector3 originalCreditsPos;

	void Start() {
		originalCreditsPos = creditText.transform.position;
	}

	void Update() {
		creditText.transform.position += new Vector3(0, scrollSpeed * Time.deltaTime, 0);

		if (Input.GetButton("Cancel") || Input.GetButton("Select") || Input.GetButton("AltSelect")) {
			mainMenu.hideCredits();
			creditText.transform.position = originalCreditsPos;
		}
	}
}