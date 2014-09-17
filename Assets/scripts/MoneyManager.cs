using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoneyManager : MonoBehaviour
{
//	public enum CoinName{
//		coin1,
//		coin5,
//		coin10,
//		coin25,
//		coin50,
//		coin1r
//	}
		//Dictionary<CoinName,GameObject> prefabMoney;
		GameObject[] prefabs;
		public int initialSpan = 10;
		public float spawRange = 3;
		public static List<Money> lstMoney;
		static float nextSpanTime = 0;
		// Use this for initialization
		void Start ()
		{

				lstMoney = new List<Money> ();
				prefabs = Resources.LoadAll<GameObject> ("prefabs/money");
				//prefabMoney = new Dictionary<CoinName, GameObject> ();
				//prefabMoney.Add (CoinName.coin1, Resources.Load ("prefabs/money"));

				//Debug.Log (prefabs.Length);

				//Initial Spawn
				for (int i = 0; i < initialSpan; i++) {
						Spawn ();
				}

		}
	
		// Update is called once per frame
		void Update ()
		{
	
				if (Input.GetKeyUp (KeyCode.Space)) {
						//Debug.Log ("Spawn");
						Spawn ();

				}


				AutomaticSpawnLogic ();


		}

		void AutomaticSpawnLogic ()
		{
				var time = GameManager.gameTime;

				
				if (time > nextSpanTime) {
			
						//var randomTime = Random.Range (0.2f, 2f) + (8f / (1 + time));
			var randomTime = Random.Range (0f, 2f) + (8f / (1 + (time/2f)));
						nextSpanTime = time + randomTime;
						
			Debug.Log ("Next: " + nextSpanTime);


						//Extra spawn
						if (Random.Range (0, 100) > 70) {
								Spawn ();
						}

						//Se tiver poucas moedas
						if (lstMoney.Count < 5) {
								Debug.Log ("Poucas moedas?");
								var rnd = Random.Range (2, 5);
								for (int i = 0; i < rnd; i++) {
										Spawn ();
								}
						}

						//Normal Spawn
						Spawn ();
				}
		}

		void Spawn ()
		{
//				List<int> moneyIdx = new List<int> ();
//
//				var porc = Random.Range (0, 100);
//				if (porc > 20) {
//					moneyIdx.Add(1);
//				}

				var coinIdx = Random.Range (1, prefabs.Length);
				var rndRange = Random.Range (-spawRange, spawRange);
				Vector3 pos = this.gameObject.transform.position + new Vector3 (rndRange, rndRange, 0);
		
				var instance = Instantiate (prefabs [coinIdx], pos, Quaternion.identity) as GameObject;
				var coin = instance.GetComponent<Money> ();
				lstMoney.Add (coin);
		}

		public static void DeselectMoney ()
		{
				foreach (var coin in lstMoney) {
				
						Money coinObj = coin.GetComponent<Money> ();
						coinObj.selected = false;
				}
		}

		public static void RemoveSelectedMoney ()
		{
				for (int i = 0; i < lstMoney.Count; i++) {
						var coin = lstMoney [i];

						if (coin.selected) {
								Destroy (coin.gameObject);
								lstMoney.RemoveAt (i);
								i--;
						}
				}
		}

	public static void ForceSpawn(){
		nextSpanTime = 0;
	}
}
