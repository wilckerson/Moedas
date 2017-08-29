using UnityEngine;
using System.Collections;

public class GameOverManager : MonoBehaviour
{

		public static float lastGameRecord = 0;
		float screenTime = 0;
	int record = 0;
		public TextMesh pointsText;
		public TextMesh recordText;


		// Use this for initialization
		void Start ()
		{
	
		Repository.Instance.Load();
		record = Repository.Instance.GameData.Record;
				

		if (lastGameRecord > record) {
			record = (int)lastGameRecord;

			Repository.Instance.GameData.Record = record;
			Repository.Instance.Save();
						
			
						recordText.text = "Novo record, parabéns!";
				} else {
			
						recordText.text = string.Format ("Melhor pontuação: {0}s", record);
				}
		pointsText.text = string.Format ("Sua pontuação: {0}s", (int)lastGameRecord);
		}
	
		// Update is called once per frame
		void Update ()
		{
				screenTime += Time.deltaTime;
				if (screenTime > 3 && Input.GetMouseButtonDown (0)) {
						//Recomeçando o jogo
						Application.LoadLevel ("Game");
				}
		}
    
    
}
