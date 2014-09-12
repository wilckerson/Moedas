using UnityEngine;
using System.Collections;

public class UIButtom : MonoBehaviour
{

	//public Sprite activeSprite;
	//private Sprite currentSprite;
	//private SpriteRenderer spriteRenderer;
	public float scaleFactor = 1.1f;
	private bool pressed = false;

	// Use this for initialization
	void Start ()
	{
		//spriteRenderer = GetComponent<SpriteRenderer> ();
		//currentSprite = spriteRenderer.sprite;
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	void OnMouseDown ()
	{
		Debug.Log("Down");
		pressed = true;
		transform.localScale = new Vector3(
			transform.localScale.x * scaleFactor,
			transform.localScale.y * scaleFactor,
			transform.localScale.z
			);
		//spriteRenderer.sprite = activeSprite;
	}

	void OnMouseUp ()
	{
		Debug.Log("Up");
		pressed = false;
		transform.localScale = new Vector3(
			transform.localScale.x / scaleFactor,
			transform.localScale.y / scaleFactor,
			transform.localScale.z
			);
		//spriteRenderer.sprite = currentSprite;
	}

	public bool IsPressed(){
		return pressed;
	}
}
