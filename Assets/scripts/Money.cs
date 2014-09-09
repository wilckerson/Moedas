using UnityEngine;
using System.Collections;

public class Money : MonoBehaviour
{

		public int Value = 0;
		public bool selected = false;
		
		Color selectedColor = Color.green;
		Color defaultColor;
		SpriteRenderer spriteRenderer;
		// Use this for initialization
		void Start ()
		{
				spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
				defaultColor = spriteRenderer.color;
		}
	
		// Update is called once per frame
		void Update ()
		{
		

				spriteRenderer.color = selected ? selectedColor : defaultColor;
		var topY = transform.position.y + (renderer.bounds.size.y / 2);

		if (rigidbody2D.IsSleeping() && topY >= GameManager.screenTopY) {
						GameManager.Lose = true;

				}
		}

//		void OnCollisionEnter2D (Collision2D coll)
//		{
//
//		}

		void OnMouseDown ()
		{
				//Destroy (gameObject);
				selected = !selected;

				//Informando ao manager
				if (selected) {
						GameManager.sum += Value;
				} else {
						GameManager.sum -= Value;
				}
		}
}
