using UnityEngine;
using System.Collections;

public class CalculateBounds : MonoBehaviour {

	Bounds bounds;

	// Use this for initialization
	void Start () {
	
		bounds = Calculate(this.transform);
	}
	
	// Update is called once per frame
	void Update () {
	
		Debug.DrawLine(bounds.center - (bounds.size / 2), bounds.center + (bounds.size / 2),Color.red);
	}

	Bounds Calculate(Transform transform){
		Bounds bounds = new Bounds(transform.position,Vector3.zero); 
		
		foreach (Transform child in transform)
		{
			if(child.gameObject.renderer != null){
				bounds.Encapsulate(child.gameObject.renderer.bounds);   
			}
			else
			{
				bounds.Encapsulate(Calculate(child));
			}
		}

		return bounds;
	}
}
