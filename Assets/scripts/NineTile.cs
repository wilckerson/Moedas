using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;

public class TileSprite
{
	public GameObject Holder {get;set;}
	public Sprite CroppedSprite {get;set;}
	public SpriteRenderer Renderer {get;set;}

	public TileSprite(string name, Sprite baseSprite,Transform parent, Rect cropRect, Vector2 origin,Vector3 posFix){

		CroppedSprite = Sprite.Create(baseSprite.texture,cropRect,origin,100);

		Holder = GameObject.Find (name) ?? new GameObject (name);
		
		Holder.transform.parent = parent;
		Holder.transform.position = parent.position + posFix;
		Renderer = Holder.GetComponents<SpriteRenderer>().FirstOrDefault() ?? Holder.AddComponent<SpriteRenderer>();
		Renderer.sprite = CroppedSprite;
	}
}

[ExecuteInEditMode]
public class NineTile : MonoBehaviour
{

	public float Margin = 10;
	public float Width = 0;
	public float Height = 0;
	public Sprite baseSprite;

	private TileSprite tileTopLeft;
	private TileSprite tileLeft;
	private TileSprite tileBottomLeft;
	private TileSprite tileTop;
	private TileSprite tileCenter;
	private TileSprite tileBottom;
	private TileSprite tileTopRight;
	private TileSprite tileRight;
	private TileSprite tileBottomRight;

	private bool setup = false;

	void Start ()
	{
		
		//Debug.Log ("Start");
	}


	// Update is called once per frame
	void Update ()
	{
	
		if(!IsSetuped())
		{
			CleanUp();
			Setup();
			setup = true;
		}
	}

	void OnDestroy ()
	{
		//Debug.Log ("OnDestroy");


		CleanUp ();
	}

	public void CleanUp ()
	{

		if (IsSetuped()) {
			DestroyImmediate (tileTopLeft.Holder);
			DestroyImmediate(tileLeft.Holder);
			DestroyImmediate (tileBottomLeft.Holder);
			DestroyImmediate (tileTop.Holder);
			DestroyImmediate (tileTopRight.Holder);
			DestroyImmediate (tileRight.Holder);
			DestroyImmediate (tileBottomRight.Holder);
			DestroyImmediate (tileBottom.Holder);
			DestroyImmediate (tileCenter.Holder);

			tileTopLeft = null;
			tileLeft = null;
			tileBottomLeft = null;
			tileTop = null;
			tileTopRight = null;
			tileRight = null;
			tileBottomRight = null;
			tileBottom = null;
			tileCenter = null;
		}
	}

	public bool IsSetuped()
	{
		return (tileTopLeft != null && tileTopLeft.Holder != null
		        && tileLeft != null && tileLeft.Holder != null
		        && tileBottomLeft != null && tileBottomLeft.Holder != null
		        && tileTop != null && tileTop.Holder != null
		        && tileTopRight != null && tileTopRight.Holder != null
		        && tileRight != null && tileRight.Holder != null
		        && tileBottomRight != null && tileBottomRight.Holder != null
		        && tileBottom != null && tileBottom.Holder != null
		        && tileCenter != null && tileCenter.Holder != null);
	}

	public void Setup ()
	{

		//Debug.Log ("Setup");
		if(baseSprite == null)
		{
			throw new UnityException("You must set the baseSprite");
		}

		var heightFix = Height/(100f * 2);
		var widthFix = Width/(100f * 2);

		tileTopLeft = new TileSprite(
			"tileTopLeft",
			baseSprite,
			this.transform,
			new Rect(0,baseSprite.textureRect.height-this.Margin,this.Margin,this.Margin),
			new Vector2(1,0),
			new Vector3(-widthFix,heightFix,0));
			
		tileLeft = new TileSprite(
			"tileLeft",
			baseSprite,
			this.transform,
			new Rect(0,this.Margin,this.Margin,baseSprite.textureRect.height - this.Margin - this.Margin),
			new Vector2(1,0.5f),
			new Vector3(-widthFix,0,0));
		tileLeft.Holder.transform.localScale = new Vector3(1,Height/tileLeft.CroppedSprite.textureRect.height,1);

		tileBottomLeft = new TileSprite(
			"tileBottomLeft",
			baseSprite,
			this.transform,
			new Rect(0,0,this.Margin,this.Margin),
			new Vector2(1,1),
			new Vector3(-widthFix,-heightFix,0));

		tileTop = new TileSprite(
			"tileTop",
			baseSprite,
			this.transform,
			new Rect(this.Margin,baseSprite.textureRect.height - this.Margin,baseSprite.textureRect.width - this.Margin - this.Margin,this.Margin),
			new Vector2(0.5f,0),
			new Vector3(0,heightFix,0));
		tileTop.Holder.transform.localScale = new Vector3(Width/tileTop.CroppedSprite.textureRect.width,1,1);

		tileTopRight = new TileSprite(
			"tileTopRight",
			baseSprite,
			this.transform,
			new Rect(baseSprite.textureRect.width - this.Margin,baseSprite.textureRect.height-this.Margin,this.Margin,this.Margin),
			new Vector2(0,0),
			new Vector3(widthFix,heightFix,0));

		tileRight = new TileSprite(
			"tileRight",
			baseSprite,
			this.transform,
			new Rect(baseSprite.textureRect.width - this.Margin,this.Margin,this.Margin,baseSprite.textureRect.height - this.Margin - this.Margin),
			new Vector2(0,0.5f),
			new Vector3(widthFix,0,0));
		tileRight.Holder.transform.localScale = new Vector3(1,Height/tileRight.CroppedSprite.textureRect.height,1);

		tileBottomRight = new TileSprite(
			"tileBottomRight",
			baseSprite,
			this.transform,
			new Rect(baseSprite.textureRect.width - this.Margin,0,this.Margin,this.Margin),
			new Vector2(0,1),
			new Vector3(widthFix,-heightFix,0));

		tileBottom = new TileSprite(
			"tileBottom",
			baseSprite,
			this.transform,
			new Rect(this.Margin,0,baseSprite.textureRect.width - this.Margin - this.Margin,this.Margin),
			new Vector2(0.5f,1),
			new Vector3(0,-heightFix,0));
		tileBottom.Holder.transform.localScale = new Vector3(Width/tileBottom.CroppedSprite.textureRect.width,1,1);

		tileCenter = new TileSprite(
			"tileCenter",
			baseSprite,
			this.transform,
			new Rect(this.Margin,this.Margin,baseSprite.textureRect.width - this.Margin - this.Margin,baseSprite.textureRect.height - this.Margin - this.Margin),
			new Vector2(0.5f,0.5f),
			Vector3.zero);
		tileCenter.Holder.transform.localScale = new Vector3(
			Width/tileCenter.CroppedSprite.textureRect.width,
			Height/tileCenter.CroppedSprite.textureRect.height,1);


	}

}
