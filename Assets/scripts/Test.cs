using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	TextMesh txMesh;
	//float alpha = 0;

	// Use this for initialization
	void Start () {

		txMesh = GetComponent<TextMesh>();
		//alpha = txMesh.color.a;
		//Debug.Log(alpha);
	}
	
	// Update is called once per frame
	void Update () {
	
		var dist = transform.position;


		float c = 1; //change over time
		float t = dist.y; //time
		float d = 10; //duration
		float b = 1; //start value
		//Linear
		//var newAlpha = c*t/d + b;
		//ExpoOut
		var newAlpha = c * ( -Mathf.Pow( 2, -10 * t/d ) + 1 ) + b;
		//Debug.Log(newAlpha);

		txMesh.color = new Color(
			txMesh.color.r,
			txMesh.color.g,
			txMesh.color.b,
			newAlpha
			);
	}
}
