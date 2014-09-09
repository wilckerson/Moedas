using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Repository
{ //: MonoBehaviour {

	#region Singleton
	private static Repository instance;

	public static Repository Instance {
		get {
			if (instance == null) {
				instance = new Repository ();
			}
			return instance;
		}
	}
	#endregion

	public GameData GameData;

	private string fileName = "gameData.dat";

	private string FilePath{
		get {
			return string.Format ("{0}/{1}", Application.persistentDataPath, fileName);
		}
	}

	public void Save ()
	{
		FileStream file = File.Open (FilePath, FileMode.OpenOrCreate);
		
		BinaryFormatter bf = new BinaryFormatter();
		bf.Serialize(file,this.GameData);
		
		file.Close();
	}

	public void Load ()
	{
		if (File.Exists (this.FilePath)) {
			FileStream file = File.Open (this.FilePath, FileMode.Open);

			BinaryFormatter bf = new BinaryFormatter();
			this.GameData = (GameData)bf.Deserialize (file);

			file.Close();
		}
		else
		{
			this.GameData = new GameData();
		}
	}

}
