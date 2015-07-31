//----------------------------------------------
//            NJG MiniMap (NGUI)
// Copyright © 2014 Ninjutsu Games LTD.
//----------------------------------------------

using NJG;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A game mini map that display icons and scroll UITexture when target moves.
/// </summary>

[ExecuteInEditMode]
[AddComponentMenu("NJG MiniMap/OnGUI/Minimap")]
public class UIMiniMapOnGUI : NJG.UIMiniMapBase
{
	static UIMiniMapOnGUI mInst;
	static public UIMiniMapOnGUI instance { get { if (mInst == null) mInst = GameObject.FindObjectOfType(typeof(UIMiniMapOnGUI)) as UIMiniMapOnGUI; return mInst; } }
	
	public override Vector2 mapScale { 
		get
		{
#if UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5
			if (rendererTransform.hasChanged)
			{
				rendererTransform.hasChanged = false; 
#else
			if(mLastScale != rendererTransform.localScale)
			{
				mLastScale = rendererTransform.localScale;
#endif
				mMapScale = new Vector2(rendererTransform.localScale.x * 10, rendererTransform.localScale.z * 10);
			} 
			return mMapScale;
		}
		set
		{
			mMapScale = value / 10;
			rendererTransform.localScale = new Vector3(mMapScale.x, 1, mMapScale.y);
		}
	}
	//public override Vector3 iconScale { get { if (mIconScale == Vector2.zero) mIconScale = new Vector3((map.iconSize / 500f), (map.iconSize / 500f), 1); return mIconScale; } }
	public override Vector3 arrowScale { get { if (mArrowScale == Vector2.zero) mArrowScale = new Vector3((map.arrowSize * mul), (map.arrowSize * mul), 1); return mArrowScale; } }	

	NJGMapOnGUI mapOnGUI;

#if UNITY_3_5
	Vector3 mLastScale;
	Vector3 mLastPos;
#endif

	GUIAnchor mAnchor;
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
		base.Awake();

		mapOnGUI = NJGMapOnGUI.instance;

		/*if (iconRoot != null)
		{
			if (iconRoot.GetComponent<GUIAnchor>() == null)
			{
				GUIAnchor a = iconRoot.gameObject.AddComponent<GUIAnchor>();
				a.transformContainer = rendererTransform;
				a.side = GUIAnchor.Side.Center;
			}
		}*/
	}

	protected override void Start()
	{
		if (map == null) return;

		mAnchor = GetComponent<GUIAnchor>();

		if (planeRenderer != null)
		{
			if (material == null)
			{
				//Debug.LogWarning("The UITexture does not have a material assigned", this);
			}
			else
			{
				if (map.generateMapTexture)
					material.mainTexture = map.mapTexture;
				//elsematerial
				//	material.mainTexture = map.userMapTexture;

				//if (maskTexture != null) material.SetTexture("_Mask", maskTexture);
				material.color = mapColor;
			}			
		}
		base.Start();
	}

	protected override void OnStart()
	{		
		base.OnStart();
		mCol = planeRenderer.GetComponent<BoxCollider>();
		if (mCol == null)
		{
			mCol = NJGTools.AddWidgetCollider(planeRenderer.gameObject);
			if(mCol.size != new Vector3(10, 0, 10)) mCol.size = new Vector3(10, 0, 10);
		}

		if(iconRoot != null) iconRoot.localEulerAngles = Vector3.zero;		
	}

	/// <summary>
	/// Update the map's alignment.
	/// </summary>

	public override void UpdateAlignment()
	{
		base.UpdateAlignment();
		if (mCol == null) mCol = planeRenderer.GetComponent<BoxCollider>();

		if (mAnchor == null) mAnchor = GetComponent<GUIAnchor>();
		mAnchor.side = (GUIAnchor.Side)pivot;

//[BIG]#if UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5
		if (rendererTransform.hasChanged)
		{
			rendererTransform.hasChanged = false;
//#else
//		if(mLastPos != rendererTransform.localPosition)
//		{
//			mLastPos = rendererTransform.localPosition;
//#endif

			if (iconRoot != null) iconRoot.localPosition = new Vector3(rendererTransform.localPosition.x, rendererTransform.localPosition.y, -1);

			mRect.x = Screen.width / 2 - mapHalfScale.x;
			mRect.y = Screen.height / 2 - mapHalfScale.y;			

			switch (pivot)
			{
				case Pivot.Top:
					mRect.y = (Screen.height - margin.y) - mapScale.y;
					break;
				case Pivot.TopLeft:
					mRect.x = margin.x;
					mRect.y = (Screen.height - margin.y) - mapScale.y;
					break;
				case Pivot.TopRight:
					mRect.x = (Screen.width - margin.x) - mapScale.x;
					mRect.y = (Screen.height - margin.y) - mapScale.y;
					break;
				case Pivot.Left:
					mRect.x = margin.x;
					break;
				case Pivot.Right:
					mRect.x = (Screen.width - margin.x) - mapScale.x;
					break;
				case Pivot.Bottom:
					mRect.y = margin.y;
					break;
				case Pivot.BottomLeft:
					mRect.x = margin.x;
					mRect.y = margin.y;
					break;
				case Pivot.BottomRight:
					mRect.x = (Screen.width - margin.x) - mapScale.x;
					mRect.y = margin.y;
					break;
			}

			
			mRect.width = mapScale.x;
			mRect.height = mapScale.y;
		}	
	}

	#region MapIcon Entry

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
			if (ic.item.Equals(item))
			{
				ic.item = item;
				ic.sprite = mapOnGUI.GetSprite(item.type);
				if(ic.color != item.color) ic.color = item.color;
				if (ic.planeRenderer.localScale != GetIconScale(item)) ic.planeRenderer.localScale = GetIconScale(item);
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
	#endregion

	#region Arrows

	/// <summary>
	/// Get the map icon entry associated with the specified unit.
	/// </summary>

	protected override UIMapArrowBase GetArrow(Object o)
	{
		NJGMapItem item = (NJGMapItem)o;
		// Try to find an existing entry
		for (int i = 0, imax = mListArrow.Count; i < imax; ++i) if (mListArrow[i].item == item)
			{
				UIMapArrowOnGUI ic = (UIMapArrowOnGUI)mListArrow[i];
				//ic.item = item;
				ic.sprite = mapOnGUI.GetArrowSprite(item.type);
                /*ic.color = item.color;
                ic.planeRenderer.localScale = iconScale;
                ic.child = ent.planeRenderer;*/
				return ic;
			}

		// See if an unused entry can be reused
		if (mUnusedArrow.Count > 0)
		{
			UIMapArrowOnGUI ent = (UIMapArrowOnGUI)mUnusedArrow[mUnusedArrow.Count - 1];
			ent.item = item;			
			ent.color = item.color;
			ent.sprite = mapOnGUI.GetArrowSprite(item.type);
			ent.planeRenderer.localScale = arrowScale;
			ent.planeRenderer.localPosition = new Vector3(0, mapHalfScale.y - item.arrowOffset, -(item.arrowDepth + 1));
			ent.child = ent.planeRenderer;
			mUnusedArrow.RemoveAt(mUnusedArrow.Count - 1);
			NJGTools.SetActive(ent.gameObject, true);
			mListArrow.Add(ent);
			return ent;
		}

		// Create this new icon
		GameObject go = NJGTools.AddChild(UIMiniMapOnGUI.instance.rendererTransform.parent.gameObject);
		go.name = "Arrow" + mArrowCount;
		go.transform.parent = UIMiniMapOnGUI.instance.arrowRoot.transform;
		go.transform.localPosition = Vector3.zero;
		go.transform.localScale = Vector3.one;

		UIMapArrowOnGUI mi = go.AddComponent<UIMapArrowOnGUI>();
		mi.item = item;
		mi.color = item.color;
		mi.sprite = mapOnGUI.GetArrowSprite(item.type);		
		mi.planeRenderer.localScale = arrowScale;
		mi.planeRenderer.localPosition = new Vector3(0, mapHalfScale.y - item.arrowOffset, -(item.arrowDepth + 1));
		mi.child = mi.planeRenderer;

		if (mi == null)
		{
			Debug.LogError("Expected to find a UIMapArrowOnGUI on the prefab to work with");
			Destroy(go);
		}
		else
		{
			mArrowCount++;
			mi.item = item;
			mListArrow.Add(mi);
		}
		return mi;
	}
	
	#endregion
}