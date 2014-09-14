using UnityEngine;
using System.Collections;

public class PulseAnimation : MonoBehaviour {

	public float velocity = 4;//4;
	public float pulse = 0.03f;//70;

	Vector3 initialScale;
	float rndX = 0;
	float rndY = 0;
	// Use this for initialization
	void Start () {
		initialScale = transform.localScale;

		rndX = Random.Range(-1,1f);
		rndY = Random.Range(-1,1f);
	}
	
	// Update is called once per frame
	void Update () {
//		var modifX = 1 - (Mathf.Sin((rndX + Time.time) * velocity) / pulse);
//		var modifY = 1 - (Mathf.Cos((rndY + Time.time) * velocity) / pulse);
//
//		transform.localScale = new Vector3(
//			initialScale.x * modifX,
//			initialScale.y * modifY,
//			initialScale.z);

		var modifX = Mathf.Sign(Mathf.Sin((rndX + Time.time) * velocity)) * pulse * Time.deltaTime;
		var modifY = Mathf.Sign(Mathf.Sin((rndY + Time.time) * velocity)) * pulse * Time.deltaTime;
		transform.localScale += new Vector3(modifX,modifY,0);
//		transform.localScale = new Vector3(
//			transform.localScale.x * modif,
//			transform.localScale.y * modif,
//			transform.localScale.z);


	}

	public void Reset(){
		transform.localScale = initialScale;
	}
}
