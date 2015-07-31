//----------------------------------------------
//            NJG MiniMap (NGUI)
// Copyright Â© 2014 Ninjutsu Games LTD.
//----------------------------------------------

using UnityEngine;

/// <summary>
/// This script can be used to anchor an object to the side or corner of the screen.
/// </summary>

[ExecuteInEditMode]
[AddComponentMenu("NJG MiniMap/OnGUI/Anchor")]
public class GUIAnchor : MonoBehaviour
{
	public enum Side
	{
		BottomLeft,
		Left,
		TopLeft,
		Top,
		TopRight,
		Right,
		BottomRight,
		Bottom,
		Center,
	}

	bool mNeedsHalfPixelOffset = false;

	/// <summary>
	/// Camera used to determine the anchor bounds. Set automatically if none was specified.
	/// </summary>

	public Camera uiCamera = null;

	/// <summary>
	/// Transform used to determine the container's bounds. Overwrites the camera-based anchoring if the value was specified.
	/// </summary>

	public Transform transformContainer = null;
	
	/// <summary>
	/// Side or corner to anchor to.
	/// </summary>

	public Side side = Side.Center;

	/// <summary>
	/// Whether a half-pixel offset will be applied on windows machines. Most of the time you'll want to leave this as 'true'.
	/// This value is only used if the widget and panel containers were not specified.
	/// </summary>

	public bool halfPixelOffset = true;

	/// <summary>
	/// Relative offset value, if any. For example "0.25" with 'side' set to Left, means 25% from the left side.
	/// </summary>

	public Vector2 relativeOffset = Vector2.zero;

	Transform mTrans;
	Animation mAnim;
	Rect mRect;
	
	void Awake () 
	{
		mTrans = transform;
		mAnim = GetComponent<Animation>(); 
		mRect = new Rect();
	}

	/// <summary>
	/// Automatically find the camera responsible for drawing the widgets under this object.
	/// </summary>

	void Start ()
	{
		mNeedsHalfPixelOffset = (Application.platform == RuntimePlatform.WindowsPlayer ||
			Application.platform == RuntimePlatform.XBOX360 ||
			Application.platform == RuntimePlatform.WindowsWebPlayer ||
			Application.platform == RuntimePlatform.WindowsEditor);

		// Only DirectX 9 needs the half-pixel offset
		if (mNeedsHalfPixelOffset) mNeedsHalfPixelOffset = (SystemInfo.graphicsShaderLevel < 40);

		if (uiCamera == null) uiCamera = NJGTools.FindCameraForLayer(gameObject.layer);
		Update();
	}

	/// <summary>
	/// Anchor the object to the appropriate point.
	/// </summary>

	void Update ()
	{
		if (mAnim != null && mAnim.enabled && mAnim.isPlaying) return;

		bool useCamera = false;

		if (transformContainer != null)
		{
			// Widget is used -- use its bounds as the container's bounds
			Transform t = transformContainer;
			Vector3 ls = t.localScale;
			Vector3 lp = t.localPosition;

			Vector3 size = transformContainer.localScale;
			Vector3 offset = Vector3.zero;// transformContainer.pivotOffset;
			offset.y -= 1f;

			//offset.x *= (transformContainer.localScale.x);
			//offset.y *= (transformContainer.localScale.y);
			
			mRect.x = lp.x + offset.x;
			mRect.y = lp.y + offset.y;
			
			mRect.width = size.x * ls.x;
			mRect.height = size.y * ls.y;
		} 
		else if (uiCamera != null)
		{	
			useCamera = true;
			mRect = uiCamera.pixelRect;	
		}
		else return;

		float cx = (mRect.xMin + mRect.xMax) * 0.5f;
		float cy = (mRect.yMin + mRect.yMax) * 0.5f;
		Vector3 v = new Vector3(cx, cy, 0f);

		if (side != Side.Center)
		{
			if (side == Side.Right || side == Side.TopRight || side == Side.BottomRight) v.x = mRect.xMax;
			else if (side == Side.Top || side == Side.Center || side == Side.Bottom) v.x = cx;
			else v.x = mRect.xMin;

			if (side == Side.Top || side == Side.TopRight || side == Side.TopLeft) v.y = mRect.yMax;
			else if (side == Side.Left || side == Side.Center || side == Side.Right) v.y = cy;
			else v.y = mRect.yMin;
		}

		float width  = mRect.width;
		float height = mRect.height;

		v.x += relativeOffset.x * width;
		v.y += relativeOffset.y * height;

		if (useCamera)
		{
			if (uiCamera.orthographic)
			{
				v.x = Mathf.Round(v.x);
				v.y = Mathf.Round(v.y);

				if (halfPixelOffset && mNeedsHalfPixelOffset)
				{
					v.x -= 0.5f;
					v.y += 0.5f;
				}
			}
			v.z = uiCamera.WorldToScreenPoint(mTrans.position).z;
			v = uiCamera.ScreenToWorldPoint(v);
		}
		else
		{
			v.x = Mathf.Round(v.x);
			v.y = Mathf.Round(v.y);

			if (transformContainer != null)
			{
				v = transformContainer.TransformPoint(v);
			}
			else if (transformContainer != null)
			{
				Transform t = transformContainer.parent;
				if (t != null) v = t.TransformPoint(v);
			}
			v.z = mTrans.position.z;
		}
		
		// Wrapped in an 'if' so the scene doesn't get marked as 'edited' every frame
		if (mTrans.position != v) mTrans.position = v;
	}
}
