using UnityEngine;
using System.Collections;

public enum UIScrollDirection
{
	Vertical,
	Horizontal
}

public class UIScroll : MonoBehaviour
{
	public UIScrollDirection Direction;
	public GameObject items;
	public float hideDuration = 1.5f;
	public float startMargin = 0;
	bool pressed = false;
	Vector3 dragDelta;
	Vector3 mouseLastPosition;
	Bounds itemsBounds;
	Vector3 boundsCenterFix;
	BoxCollider2D box2D;

	// Use this for initialization
	void Start ()
	{
		itemsBounds = CalculateBounds (items.transform);
		boundsCenterFix = itemsBounds.center - items.transform.position;
		box2D = GetComponent<BoxCollider2D> ();

	}
	
	// Update is called once per frame
	void Update ()
	{
		updateDragDelta ();

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

		//Debug.DrawLine(box2D.bounds.min,box2D.bounds.max);
		Debug.DrawLine (itemsBounds.min, itemsBounds.max, Color.red);

	}

	void HideItemsFX ()
	{

		foreach (Transform item in items.transform) {
			var components = item.gameObject.GetComponentsInChildren<Component> (false);
		
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
		if (Direction == UIScrollDirection.Vertical) {
			if (item.position.y > box2D.bounds.max.y) {
			
				var alpha = 1 - (Easing (1, Mathf.Abs (item.position.y - box2D.bounds.max.y), hideDuration) - 1);
			
				newColor = new Color (itemColor.r, itemColor.g, itemColor.b, alpha);

			} else if (item.position.y < box2D.bounds.min.y) {
				var alpha = 1 - (Easing (1, Mathf.Abs (item.position.y - box2D.bounds.min.y), hideDuration) - 1);
			
				newColor = new Color (itemColor.r, itemColor.g, itemColor.b, alpha);

			} else {
			
				newColor = new Color (itemColor.r, itemColor.g, itemColor.b, 1);
			}
		} else {
			if (item.position.x > box2D.bounds.max.x) {
					
				var alpha = 1 - (Easing (1, Mathf.Abs (item.position.x - box2D.bounds.max.x), hideDuration) - 1);

				newColor = new Color (itemColor.r, itemColor.g, itemColor.b, alpha);

			} else if (item.position.x < box2D.bounds.min.x) {
				var alpha = 1 - (Easing (1, Mathf.Abs (item.position.x - box2D.bounds.min.x), hideDuration) - 1);
					
				newColor = new Color (itemColor.r, itemColor.g, itemColor.b, alpha);
					
			} else {
					
				newColor = new Color (itemColor.r, itemColor.g, itemColor.b, 1);
			}
		}

		return newColor;
	}

	void OnMouseDown ()
	{
		pressed = true;
		mouseLastPosition = Input.mousePosition;
	}

	void OnMouseUp ()
	{
		pressed = false;
		dragDelta = Vector3.zero;
	}

	void updateDragDelta ()
	{
		if (pressed) {

			dragDelta = Input.mousePosition - mouseLastPosition;
			//Debug.Log (dragDelta);
			
			mouseLastPosition = Input.mousePosition;
		}
	}

	bool NeedScroll ()
	{
		var needScroll = false;
		if (Direction == UIScrollDirection.Vertical) {
			
			needScroll = (itemsBounds.size.y > box2D.size.y - startMargin) 
				|| (itemsBounds.max.y > box2D.bounds.max.y)
				|| (itemsBounds.min.y < box2D.bounds.min.y);
		} else {
			needScroll = (itemsBounds.size.x > box2D.size.x - startMargin) 
				|| (itemsBounds.max.x > box2D.bounds.max.x)
				|| (itemsBounds.min.x < box2D.bounds.min.x);
			
		}
		return needScroll;
	}

	void scrollContent ()
	{

		if (NeedScroll ()) {

			//Movimenta os items de acordo com o drag
			var itemPos = items.transform.position;

			if (Direction == UIScrollDirection.Vertical) {
				itemPos.y += dragDelta.y * Time.deltaTime;
			} else {
				itemPos.x += dragDelta.x * Time.deltaTime;
			}

			items.transform.position = itemPos;
		}
		itemsBounds.center = items.transform.position + boundsCenterFix;
		
		//Calcula a posiçao da parte inferior (onde termina os items)
		//var itemsBottom = items.transform.position  - (itemsBounds.size / 2);// + boundsCenterFix
			
		if (Direction == UIScrollDirection.Vertical) {

			//Limita os items ao final do scroll
			if (itemsBounds.min.y > box2D.bounds.min.y
				&& itemsBounds.size.y > box2D.size.y - startMargin) {
				var newPos = box2D.bounds.min + (itemsBounds.size / 2) - boundsCenterFix;
				items.transform.position = new Vector3 (items.transform.position.x,
			                                        newPos.y,
				                                        items.transform.position.z);
			}

			//Limita os items ao início do scroll
			if (itemsBounds.max.y < box2D.bounds.max.y - startMargin) {
				var newPos = box2D.bounds.max - (itemsBounds.size / 2) - boundsCenterFix;
				items.transform.position = new Vector3 (items.transform.position.x,
				                                        newPos.y - startMargin,
				                                        items.transform.position.z);
			}
		} else {
			//Limita os items ao início do scroll (Direita)
			if (itemsBounds.max.x < box2D.bounds.max.y
				&& itemsBounds.size.x > box2D.size.x - startMargin) {
				var newPos = box2D.bounds.max - (itemsBounds.size / 2) - boundsCenterFix;
				items.transform.position = new Vector3 (newPos.x,
				                                        items.transform.position.y,
				                                        items.transform.position.z);
				Debug.Log("a");
			}

			//Limita os items ao final do scroll (Esquerda)
			if (itemsBounds.min.x > box2D.bounds.min.x + startMargin) {
				var newPos = box2D.bounds.min + (itemsBounds.size / 2) - boundsCenterFix;
				items.transform.position = new Vector3 (newPos.x + startMargin,
					                                        items.transform.position.y,
				                                        items.transform.position.z);
				Debug.Log("b");
			}
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
			if (child.gameObject.activeSelf) {
				if (child.gameObject.renderer != null) {
					bounds.Encapsulate (child.gameObject.renderer.bounds);   
				} else {
					bounds.Encapsulate (CalculateBounds (child));
				}
			}
		}
		
		return bounds;
	}
}
