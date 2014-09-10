using UnityEngine;
using System.Collections;

public class UIScroll : MonoBehaviour
{

	bool pressed = false;
	Vector3 dragDelta;
	Vector3 mouseLastPosition;
	public GameObject items;
	public GameObject start;
	public GameObject end;

	public float hideDuration = 0.5f;
	Bounds itemsBounds;
	Vector3 boundsCenterFix;

	// Use this for initialization
	void Start ()
	{
		itemsBounds = CalculateBounds (items.transform);
		boundsCenterFix = itemsBounds.center - items.transform.position;

	}
	
	// Update is called once per frame
	void Update ()
	{
		scrollContent ();

		HideItemsFX ();
		//ScaleHide
//			if (item.position.y > start.transform.position.y) {
//				float dist = item.position.y - start.transform.position.y;
//				item.localScale = new Vector3(1,1-(Mathf.Clamp(dist,0,1)*2));
//
//				if(item.localScale.y <= 0)
//				{
//					item.gameObject.SetActive(false);
//				}
//			}
//			else
//			{
//				item.localScale = Vector3.one;
//				item.gameObject.SetActive(true);
//			}



	}

	void HideItemsFX ()
	{

		foreach (Transform item in items.transform) {
			var components = item.gameObject.GetComponentsInChildren<Component> ();
		
			foreach (var component in components) {
				if (component is SpriteRenderer) {
					var render = (SpriteRenderer)component;
					render.color = HideItemColor (item, render.color);
				} else if (component is TextMesh) {
					var txMesh = (TextMesh)component;
					txMesh.color = HideItemColor (item, txMesh.color);
				}
			}
		}
	}

	Color HideItemColor (Transform item, Color itemColor)
	{
		Color newColor = itemColor;

		if (item.position.y > start.transform.position.y) {
			
			var alpha = 1 - (Easing (1, Mathf.Abs (item.position.y - start.transform.position.y), hideDuration) - 1);
			
			newColor = new Color (
				itemColor.r,
				itemColor.g,
				itemColor.b,
				alpha
			);
//			if (alpha <= 0.1f) {
//				item.gameObject.SetActive (false);
//			}
		} else if (item.position.y < end.transform.position.y) {
			var alpha = 1 - (Easing (1, Mathf.Abs (item.position.y - end.transform.position.y), hideDuration) - 1);
			
			newColor = new Color (
				itemColor.r,
				itemColor.g,
				itemColor.b,
				alpha
			);
//			if (alpha <= 0.1f) {
//				item.gameObject.SetActive (false);
//			}
		} else {
			
			newColor = new Color (
				itemColor.r,
				itemColor.g,
				itemColor.b,
				1
			);
			
			//item.gameObject.SetActive(true);
		}

		return newColor;
	}

	void updateDragDelta ()
	{
		if (Input.GetMouseButtonDown (0)) {
			pressed = true;
			mouseLastPosition = Input.mousePosition;
			dragDelta = Vector3.zero;
		} else if (Input.GetMouseButtonUp (0)) {
			pressed = false;
			dragDelta = Vector3.zero;
		}
		
		if (pressed) {

			dragDelta = Input.mousePosition - mouseLastPosition;
			//Debug.Log (dragDelta);
			
			mouseLastPosition = Input.mousePosition;
		}
	}

	void scrollContent ()
	{
		updateDragDelta ();
			
		//Movimenta os items de acordo com o drag
		var itemPos = items.transform.position;
		itemPos.y += dragDelta.y * Time.deltaTime;
		items.transform.position = itemPos;

		//Calcula a posiçao da parte inferior (onde termina os items)
		var itemsBottom = items.transform.position + boundsCenterFix - (itemsBounds.size / 2);
			
		//Limita os items ao final do scroll
		if (itemsBottom.y > end.transform.position.y) {
				
			items.transform.position = new Vector3 (items.transform.position.x,
				                                        end.transform.position.y + itemsBounds.size.y,
				                                        items.transform.position.z);
		}

		//Limita os items ao início do scroll
		if (items.transform.position.y < start.transform.position.y) {

			items.transform.position = new Vector3 (items.transform.position.x,
				                                        start.transform.position.y,
				                                        items.transform.position.z);
		}
	}

	float Easing (float startValue, float time, float duration)
	{
		var c = 1;

		//Linear
		//return c*time/duration + startValue;

		//ExpoOut
		return c * (-Mathf.Pow (2, -10 * time / duration) + 1) + startValue;
	}

	Bounds CalculateBounds (Transform transform)
	{
		Bounds bounds = new Bounds (transform.position, Vector3.zero); 
		
		foreach (Transform child in transform) {
			if (child.gameObject.renderer != null) {
				bounds.Encapsulate (child.gameObject.renderer.bounds);   
			} else {
				bounds.Encapsulate (CalculateBounds (child));
			}
		}
		
		return bounds;
	}
}
