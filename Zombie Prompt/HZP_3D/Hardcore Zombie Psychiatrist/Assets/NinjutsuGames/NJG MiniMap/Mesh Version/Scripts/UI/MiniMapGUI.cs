//----------------------------------------------
//            NJG MiniMap (NGUI)
// Copyright © 2014 Ninjutsu Games LTD.
//----------------------------------------------

using NJG;
using UnityEngine;
using System.Collections;

/// <summary>
/// Simple class that handles OnGUI styles for MiniMap. Feel free to modify it to your needs.
/// </summary>

[ExecuteInEditMode]
[RequireComponent(typeof(GUIAnchor))]
public class MiniMapGUI : MonoBehaviour 
{
	[System.Serializable]
	public class ToolTipGUISettings
	{
		public Texture2D background;
		public Color color;
		public Font font;
		public RectOffset border;
		public RectOffset padding;
	}

	[System.Serializable]
	public class MiniMapGUISettings
	{
		public Texture2D frame;
		public int frameBorder;
		public Vector2 margin;
		public Texture2D button;
		public Texture2D buttonHover;
		public Texture2D buttonDown;
		public Texture2D zoomInIcon;
		public Texture2D zoomOutIcon;
		public Texture2D lockIcon;
		public Texture2D worldMapIcon;
	}

	public MiniMapGUISettings minimap;
	public ToolTipGUISettings tooltip;

	//public float width = 100;
	//public float height = 100;

	float width { get { return UIMiniMapOnGUI.instance.mapScale.x; } }
	float height { get { return UIMiniMapOnGUI.instance.mapScale.y; } }

	GUIStyle frameStyle;
	GUIStyle buttonStyle;

	Rect mainRect;
	GUIAnchor mAnchor;

	void Awake()
	{
		mAnchor = GetComponent<GUIAnchor>();
	}

	/// <summary>
	/// Setup the styles.
	/// </summary>
	
	void Start () {
		frameStyle = new GUIStyle();
		frameStyle.normal.background = minimap.frame;

		buttonStyle = new GUIStyle();
		buttonStyle.normal.background = minimap.button;
		buttonStyle.hover.background = minimap.buttonHover;
		buttonStyle.active.background = minimap.buttonDown;

		/*buttonStyleFullscreen = new GUIStyle();
		buttonStyleFullscreen.normal.background = itsDataModuleMinimap.itsAppearanceMap.itsButton;
		buttonStyleFullscreen.hover.background = itsDataModuleMinimap.itsAppearanceMap.itsButtonHover;
		buttonStyleFullscreen.active.background = itsDataModuleMinimap.itsAppearanceMap.itsButtonDown;*/
	}

	/// <summary>
	/// Update MainRect according to the GUIAnchor side.
	/// </summary>
	
	void UpdateRect()
	{
		//minimap.margin = new Vector2(Mathf.Abs(UIMiniMapOnGUI.instance.rendererTransform.localPosition.x) / UIMiniMapOnGUI.instance.mapScale.x / 10 / 2 - 0.003f,
		//							Mathf.Abs(UIMiniMapOnGUI.instance.rendererTransform.localPosition.y) / UIMiniMapOnGUI.instance.mapScale.y / 10 / 2 - 0.003f);

		switch (mAnchor.side)
		{
			case GUIAnchor.Side.Left:
				mainRect.x = 0;
				mainRect.x += (Screen.width - width) * minimap.margin.x;
				mainRect.y = (Screen.height - height) / 2;
				break;
			case GUIAnchor.Side.Center:
				mainRect.x = (Screen.width - width) / 2;
				mainRect.y = (Screen.height - height) / 2;
				break;
			case GUIAnchor.Side.Right:
				mainRect.x = Screen.width - width;
				mainRect.x -= (Screen.width - width) * minimap.margin.x;
				mainRect.y = (Screen.height - height) / 2;
				break;
			case GUIAnchor.Side.Top:
				mainRect.y = 0;
				mainRect.y += (Screen.height - height) * minimap.margin.y;
				mainRect.x = (Screen.width - width) / 2;
				break;
			case GUIAnchor.Side.Bottom:
				mainRect.x = (Screen.width - width) / 2;
				mainRect.y = Screen.height - height;
				mainRect.y -= (Screen.height - height) * minimap.margin.y;
				break;
			case GUIAnchor.Side.TopLeft:
				mainRect.y = 0;
				mainRect.y += (Screen.height - height) * minimap.margin.y;
				mainRect.x = 0;
				mainRect.x += (Screen.width - width) * minimap.margin.x;
				break;
			case GUIAnchor.Side.TopRight:
				mainRect.y = 0;
				mainRect.y += (Screen.height - height) * minimap.margin.y;
				mainRect.x = Screen.width - width;
				mainRect.x -= (Screen.width - width) * minimap.margin.x;
				break;
			case GUIAnchor.Side.BottomLeft:
				mainRect.y = Screen.height - height;
				mainRect.y -= (Screen.height - height) * minimap.margin.y;
				mainRect.x = 0;
				mainRect.x += (Screen.width - width) * minimap.margin.x;
				break;
			case GUIAnchor.Side.BottomRight:
				mainRect.y = 0;
				mainRect.y += (Screen.height - height) * minimap.margin.y;
				mainRect.x = Screen.width - width;
				mainRect.x -= (Screen.width - width) * minimap.margin.x;
				break;
		}

		mainRect.width = width;
		mainRect.height = height;
	}

	void ToolTip(string content)
	{
		if (string.IsNullOrEmpty(content))
			return;

		GUIStyle tooltipStyle = new GUIStyle();
		tooltipStyle.normal.background = tooltip.background;
		tooltipStyle.normal.textColor = tooltip.color;
		tooltipStyle.font = tooltip.font;
		tooltipStyle.border = tooltip.border;
		tooltipStyle.padding = tooltip.padding;

		Vector2 mPos = Input.mousePosition;
		Vector2 mSize = tooltipStyle.CalcSize(new GUIContent(content)) + new Vector2(tooltip.padding.left + tooltip.padding.right, tooltip.padding.top + tooltip.padding.bottom);

		GUI.Label(new Rect(mPos.x - mSize.x / 2, Screen.height - mPos.y + 15, mSize.x, mSize.y), content, tooltipStyle);
	}

	void OnGUI()
	{
		UpdateRect();

		frameStyle.normal.background = minimap.frame;
		frameStyle.border = new RectOffset(minimap.frameBorder, minimap.frameBorder, minimap.frameBorder, minimap.frameBorder);

		GUI.Box(mainRect, "", frameStyle);

		// left top button
		/*itsRectZoomIn = new Rect(mainRect.x + aButtonPadding,
								 mainRect.y + aButtonPadding,
								 aButtonSize, aButtonSize);

		// left bottom button
		itsRectZoomOut = new Rect(mainRect.x + aButtonPadding,
								  mainRect.y + mainRect.height - aButtonSize - aButtonPadding,
								  aButtonSize, aButtonSize);

		// right top button
		itsRectStatic = new Rect(mainRect.x + mainRect.width - aButtonSize - aButtonPadding,
								 mainRect.y + aButtonPadding,
								 aButtonSize, aButtonSize);

		// right bottom button
		itsRectFullscreen = new Rect(mainRect.x + mainRect.width - aButtonSize - aButtonPadding,
									 mainRect.y + mainRect.height - aButtonSize - aButtonPadding,
									 aButtonSize, aButtonSize);*/

		/*if (!zoomInButton)
		{
			Debug.LogError("Please assign a texture on the inspector");
			return;
		}
		if (GUI.Button(new Rect(10, 10, 50, 50), zoomInButton))
			Debug.Log("Clicked the button with an image");

		if (GUI.Button(new Rect(10, 70, 50, 30), "Click"))
			Debug.Log("Clicked the button with text");*/

	}
}
