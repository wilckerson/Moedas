using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoinsManager : MonoBehaviour
{
//	public enum CoinName{
//		coin1,
//		coin5,
//		coin10,
//		coin25,
//		coin50,
//		coin1r
//	}
	//Dictionary<CoinName,GameObject> prefabCoins;
	GameObject[] prefabs;
		public int initialSpan = 10;
		public float spawRange = 3;
		public static List<Coin> lstCoins;
		float nextSpanTime = 0;
		// Use this for initialization
		void Start ()
		{

				lstCoins = new List<Coin> ();
				prefabs = Resources.LoadAll<GameObject> ("prefabs/coins");
				//prefabCoins = new Dictionary<CoinName, GameObject> ();
				//prefabCoins.Add (CoinName.coin1, Resources.Load ("prefabs/coins"));

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

		//Debug.Log ("Time: "+ time + " Next: " + nextSpanTime);

				if (time > nextSpanTime) {
			
						var randomTime = Random.Range (0.2f, 2f)  + (8f / (1 + time));
						nextSpanTime = time + randomTime;
						

						//Extra spawn
			if(Random.Range(0,100) > 70){
						Spawn ();
			}

			//Se tiver poucas moedas
			if(lstCoins.Count < 10){
				Debug.Log("Poucas moedas?");
				var rnd = Random.Range(3,10);
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
//				List<int> coinsIdx = new List<int> ();
//
//				var porc = Random.Range (0, 100);
//				if (porc > 20) {
//					coinsIdx.Add(1);
//				}

				var coinIdx = Random.Range (1, prefabs.Length);
				var rndRange = Random.Range (-spawRange, spawRange);
				Vector3 pos = this.gameObject.transform.position + new Vector3 (rndRange, rndRange, 0);
		
				var instance = Instantiate (prefabs [coinIdx], pos, Quaternion.identity) as GameObject;
				var coin = instance.GetComponent<Coin> ();
				lstCoins.Add (coin);
		}

		public static void DeselectCoins ()
		{
				foreach (var coin in lstCoins) {
				
						Coin coinObj = coin.GetComponent<Coin> ();
						coinObj.selected = false;
				}
		}

		public static void RemoveSelectedCoins ()
		{
				for (int i = 0; i < lstCoins.Count; i++) {
						var coin = lstCoins [i];

						if (coin.selected) {
								Destroy (coin.gameObject);
								lstCoins.RemoveAt (i);
								i--;
						}
				}
		}
}
