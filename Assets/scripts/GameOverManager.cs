using UnityEngine;
using System.Collections;

public class GameOverManager : MonoBehaviour
{

		public static float lastGameTime = 0;
	int record = 0;
		float screenTime = 0;
		public TextMesh pointsText;
		public TextMesh recordText;


		// Use this for initialization
		void Start ()
		{
	
		Repository.Instance.Load();
		record = Repository.Instance.GameData.Record;
				

				if (lastGameTime > record) {
						record = (int)lastGameTime;

			Repository.Instance.GameData.Record = record;
			Repository.Instance.Save();
						
			
						recordText.text = "Parabens! Voce fez o maior tempo!";
				} else {
			
						recordText.text = string.Format ("Maior tempo: {0}s", record);
				}
				pointsText.text = string.Format ("Voce ficou: {0}s", (int)lastGameTime);
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
