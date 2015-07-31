//----------------------------------------------
//            NJG MiniMap (NGUI)
// Copyright © 2014 Ninjutsu Games LTD.
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Game map can have icons on it -- this class takes care of animating them when needed.
/// </summary>

public class UIMapArrowOnGUI : NJG.UIMapArrowBase
{
	public NJGAtlas.Sprite sprite
	{
		set
		{
			if (mSprite != value)
			{
				mSprite = value;
				if (mMesh != null)
				{
					NJGMapOnGUI.instance.atlas.ChangeSprite(mMesh, mSprite.uvs);
				}
			}
		}
		get { return mSprite; }
	}

	public Color color
	{
		set
		{
			if (mColor != value)
			{
				mColor = value;
				if (mMesh != null)
				{
					NJGMapOnGUI.instance.atlas.ChangeColor(mMesh, mColor);
				}
			}
		}
		get { return mColor; }
	}

	public Transform planeRenderer
	{
		get
		{
			if (mPlaneTrans == null && NJGMapOnGUI.instance.atlas != null)
			{
				mPlane = new GameObject("_Arrow");
				mPlane.layer = gameObject.layer;
				NJGMapOnGUI.instance.atlas.CreateSprite(mPlane, sprite.uvs, color);
				mMesh = mPlane.GetComponent<MeshFilter>().sharedMesh;
				mPlaneTrans = mPlane.transform;
				mPlaneTrans.parent = transform;
				mPlaneTrans.localPosition = new Vector3(0, 0, -(item.depth + 1));
				mPlaneTrans.localEulerAngles = Vector3.zero;
			}
			return mPlaneTrans;
		}
	}

	Mesh mMesh;
	Color mColor = Color.white;
	GameObject mPlane;
	Transform mPlaneTrans;
	
	NJGAtlas.Sprite mSprite;
}