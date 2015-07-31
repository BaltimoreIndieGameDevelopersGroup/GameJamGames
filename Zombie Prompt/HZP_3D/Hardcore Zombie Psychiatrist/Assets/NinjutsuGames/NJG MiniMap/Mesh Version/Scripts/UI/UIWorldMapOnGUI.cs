//----------------------------------------------
//            NJG MiniMap (NGUI)
// Copyright © 2014 Ninjutsu Games LTD.
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NJG;

/// <summary>
/// A game mini map that display icons and scroll UITexture when target moves.
/// </summary>

[ExecuteInEditMode]
[AddComponentMenu("NJG MiniMap/OnGUI/World Map")]
public class UIWorldMapOnGUI : NJG.UIWorldMapBase
{
	static UIWorldMapOnGUI mInst;
	static public UIWorldMapOnGUI instance { get { if (mInst == null) mInst = GameObject.FindObjectOfType(typeof(UIWorldMapOnGUI)) as UIWorldMapOnGUI; return mInst; } }

	public override Vector2 mapScale { get { return new Vector2(rendererTransform.localScale.x * 10, rendererTransform.localScale.z * 10); } }
	//public override Vector3 iconScale { get { return new Vector3((map.iconSize / 500f), (map.iconSize / 500f), 1); } }

	NJGMapOnGUI mapOnGUI;
	BoxCollider mCol;
	Rect mRect;

	public override bool isMouseOver
	{
		get
		{
			Vector2 mp = Input.mousePosition;
			return mRect.Contains(mp);
		}
		set
		{
			base.isMouseOver = value;
		}
	}

	protected override void Awake()
	{
		mapOnGUI = NJGMapOnGUI.instance;
		if(iconRoot != null) iconRoot.localEulerAngles = Vector3.zero;
		base.Awake();
	}

	protected override void OnStart()
	{
		mCol = planeRenderer.GetComponent<BoxCollider>();
		if (mCol == null)
		{
			mCol = NJGTools.AddWidgetCollider(planeRenderer.gameObject);
			if(mCol.size != new Vector3(10, 0, 10)) mCol.size = new Vector3(10, 0, 10);
		}

		if (NJG.NJGMapBase.instance == null) return;		

		if (planeRenderer != null)
		{
			if (material == null)
			{
				Debug.LogWarning("The UITexture does not have a material assigned", this);
			}
			else
			{
				if (NJG.NJGMapBase.instance.generateMapTexture)
				{
					material.mainTexture = mapOnGUI.mapTexture;
				}
				//else
				//	material.mainTexture = NJG.NJGMapBase.instance.userMapTexture;

				//if (maskTexture != null) material.SetTexture("_Mask", maskTexture);
			}

			material.color = mapColor;
		}
		base.OnStart();

		if (iconRoot != null)
		{
			iconRoot.localEulerAngles = Vector3.zero;
			iconRoot.localPosition = new Vector3(rendererTransform.localPosition.x, rendererTransform.localPosition.y, -1);
		}
	}

	const float mul = 0.0045f;

	public Vector3 GetIconScale(NJGMapItem item)
	{
		Vector3 s = item.iconScale * mul;
		s.z = 1;
		return s; 
	}

	/// <summary>
	/// Get the map icon entry associated with the specified unit.
	/// </summary>

	protected override UIMapIconBase GetEntry(NJGMapItem item)
	{
		// Try to find an existing entry
		for (int i = 0, imax = mList.Count; i < imax; ++i)
		{
			UIMapIconOnGUI ic = (UIMapIconOnGUI)mList[i];
			if (ic.item == item)
			{
				/*ic.item = item;
				ic.sprite = mapOnGUI.GetSprite(item.type);
				if (ic.color != item.color) ic.color = item.color;
				if (ic.planeRenderer.localScale != GetIconScale(item)) ic.planeRenderer.localScale = GetIconScale(item);*/
				return ic;
			}
		}

		// See if an unused entry can be reused
		if (mUnused.Count > 0)
		{
			UIMapIconOnGUI ent = (UIMapIconOnGUI)mUnused[mUnused.Count - 1];
			ent.item = item;
			ent.sprite = mapOnGUI.GetSprite(item.type);
			ent.color = item.color;
			ent.planeRenderer.localScale = GetIconScale(item);
			mUnused.RemoveAt(mUnused.Count - 1);
			NJGTools.SetActive(ent.gameObject, true);
			mList.Add(ent);
			return ent;
		}

		// Create this new icon
		GameObject go = NJGTools.AddChild(iconRoot.gameObject);
		go.name = "Icon" + mCount;

		UIMapIconOnGUI mi = go.AddComponent<UIMapIconOnGUI>();
		mi.item = item;
		mi.sprite = mapOnGUI.GetSprite(item.type);
		mi.color = item.color;
		mi.planeRenderer.localScale = GetIconScale(item);

		if (mi == null)
		{
			Debug.LogError("Expected to find a Game Map Icon on the prefab to work with", this);
			Destroy(go);
		}
		else
		{
			mCount++;
			mi.item = item;
			mList.Add(mi);
		}
		return mi;
	}

	protected override void Update()
	{
#if UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5
		if (rendererTransform.hasChanged)
		{
			rendererTransform.hasChanged = false;
#else
		if (mLastScale != rendererTransform.localScale)
		{
			mLastScale = rendererTransform.localScale;
#endif
			mRect.x = Screen.width / 2 - mapHalfScale.x;
			mRect.y = Screen.height / 2 - mapHalfScale.y;
			mRect.width = mapScale.x;
			mRect.height = mapScale.y;
		}

		base.Update();
	}
}