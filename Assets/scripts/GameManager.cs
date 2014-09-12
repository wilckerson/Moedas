﻿using UnityEngine;
using System.Collections;
using System.Linq;

public class GameManager : MonoBehaviour
{

		public static float gameTime;
		public static int sum;
		public TextMesh targetText;
	public TextMesh gameTimeText;
	public GameObject screenTop;
	public static float screenTopY;
		int target;
		int points;
	public static bool Lose = false;

		// Use this for initialization
		void Start ()
		{
		Debug.Log(Application.persistentDataPath);
		screenTopY = screenTop.transform.position.y;
				sum = 0;
				target = 0;
				gameTime = 0;
				points = 0;
		Lose = false;
				newTarget ();
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		if (Lose == false) {
						gameTime += Time.deltaTime;
						gameTimeText.text = gameTime.ToString ("00:00");

						if (sum > target) {
								//Errou, desmarca as moedas selecionadas
								MoneyManager.DeselectMoney ();
								//Debug.Log ("Errou");
								sum = 0;
						} else if (sum == target) {
								//Debug.Log ("Acertou");
								sum = 0;
								//Acertou, remove as moedas selecionadas
								MoneyManager.RemoveSelectedMoney ();
								//Gera novo target
								newTarget ();
								//Conta os pontos
								points++;
						}
				} else {
			//Debug.Log("Perdeu");
			GameOverManager.lastGameTime = gameTime;
			//Application.LoadLevel("GameOver");
			Application.LoadLevel("Levels");
				}
		}

		void newTarget ()
		{

				//var oldTarget = target;

				//do {
				target = 0;
				//target = Random.Range (10, 200);
				var moneyGroup = MoneyManager.lstMoney.GroupBy (g => g.Value);
				float porcentage = 40 - (gameTime / 3);
				if (porcentage < 0) {
						porcentage = 0;
				}
				foreach (var group in moneyGroup) {
						if (group.Key != 1) {
								if (Random.Range (0, 100) > porcentage) {

										int count = (int)Random.Range (1f, group.Count () * (100 - porcentage) / 100);
										target += (group.Key * count);

								}
						}
				}

				if (target == 0) {
						if (MoneyManager.lstMoney.Count > 0) {
								var idx = Random.Range (0, MoneyManager.lstMoney.Count);
								target = MoneyManager.lstMoney [idx].Value;
						} else {
								MoneyManager.ForceSpawn ();
								//break;
						}
				}
				//Debug.Log ("Loop");

				//} while(oldTarget == target && MoneyManager.lstMoney.Count > 1);// || target % 5 != 0);


				//Debug.Log (target);
				targetText.text = (target / 100f).ToString ("C");
		}
}
