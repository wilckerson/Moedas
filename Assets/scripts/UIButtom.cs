using UnityEngine;
using System.Collections;

public class UIButtom : MonoBehaviour
{

	public Sprite activeSprite;
	private Sprite currentSprite;
	private SpriteRenderer spriteRenderer;
	private bool pressed = false;

	// Use this for initialization
	void Start ()
	{
		spriteRenderer = GetComponent<SpriteRenderer> ();
		currentSprite = spriteRenderer.sprite;
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	void OnMouseDown ()
	{
		pressed = true;
		spriteRenderer.sprite = activeSprite;
	}

	void OnMouseUp ()
	{
		pressed = false;
		spriteRenderer.sprite = currentSprite;
	}
}
