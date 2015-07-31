//----------------------------------------------
//            NJG MiniMap (NGUI)
// Copyright © 2014 Ninjutsu Games LTD.
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
[AddComponentMenu("NJG MiniMap/OnGUI/Map")]
public class NJGMapOnGUI : NJG.NJGMapBase 
{	
	/// <summary>
	/// Get instance.
	/// </summary>

	static NJGMapOnGUI mInst;
	new static public NJGMapOnGUI instance { get { if (mInst == null) mInst = GameObject.FindObjectOfType(typeof(NJGMapOnGUI)) as NJGMapOnGUI; return mInst; } }

	/// <summary>
	/// Default sprite.
	/// </summary>

	public NJGAtlas.Sprite defaultSprite;

	[SerializeField]
	public string iconFolder = "";

	/// <summary>
	/// Atlas we are going to use for icon sprites.
	/// </summary>
	
	public NJGAtlas atlas;

	#region Getters	

	/// <summary>
	/// Get sprite from type.
	/// </summary>

	public NJGAtlas.Sprite GetSprite(int type)
	{
		if (atlas == null)
		{
			Debug.LogError("You need to assign an atlas", this);
			return null;
		}
		return Get(type) == null ? defaultSprite : atlas.GetSprite(Get(type).sprite);
	}

	/// <summary>
	/// Get arrow sprite from type.
	/// </summary>

	public NJGAtlas.Sprite GetArrowSprite(int type)
	{
		if (atlas == null)
		{
			Debug.LogWarning("You need to assign an atlas", this);
			return null;
		}
		return Get(type) == null ? defaultSprite : atlas.GetSprite(Get(type).arrowSprite);
	}
	#endregion

	/// <summary>
	/// Clean up.
	/// </summary>

	void OnDestroy()
	{
		//if (mapRenderer != null)
		//	if (mapRenderer.gameObject != null)
		//		NJGTools.Destroy(mapRenderer.gameObject);

		if (UIMiniMapOnGUI.instance != null && Application.isPlaying) UIMiniMapOnGUI.instance.planeRenderer.material.mainTexture = null;

		if (mapTexture != null) NJGTools.Destroy(mapTexture);
		mapTexture = null;

		//base.OnDestroy();
	}

}
