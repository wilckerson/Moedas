using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour {

	public static int lastGamePoints = -1;
	int record = 0;
	public TextMesh pointsText;
	public TextMesh recordText;
	public GameObject flare;

	// Use this for initialization
	void Start () {

		Repository.Instance.Load();
		record = Repository.Instance.GameData.Record;

		if (lastGamePoints > record) {
			record = lastGamePoints;

			Repository.Instance.GameData.Record = record;
			Repository.Instance.Save();

			string[] recordMsg = { "Uau! Novo record!", "Mandou bem! Novo record!", 
				"Parabéns! Novo record!", ":) Novo record!", "Arrasou! Novo record!" };

			int idx = Random.Range (0, recordMsg.Length-1);

			recordText.text = recordMsg[idx];
			flare.SetActive (true);
		} else {
			flare.SetActive (false);
			recordText.text = string.Format ("Melhor pontuação: {0}", record);
		}
		pointsText.text = string.Format ("Sua pontuação: {0}", (int)lastGamePoints);

		if (lastGamePoints == -1) {
			pointsText.gameObject.SetActive (false);
		}

		if (record == 0) {
			recordText.gameObject.SetActive (false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
