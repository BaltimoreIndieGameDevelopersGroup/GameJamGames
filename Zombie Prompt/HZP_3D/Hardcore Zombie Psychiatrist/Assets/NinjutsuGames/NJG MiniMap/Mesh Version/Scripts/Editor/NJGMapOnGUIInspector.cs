//----------------------------------------------
//            NJG MiniMap (NGUI)
// Copyright © 2014 Ninjutsu Games LTD.
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NJG;

[CustomEditor(typeof(NJGMapOnGUI))]
public class NJGMapOnGUIInspector : NJG.NJGMapInspectorBase
{
	static NJGMapOnGUI m;
	UnityEngine.Object mFolderObject;
	string mFolder;

	public override void OnInspectorGUI()
	{
		m = target as NJGMapOnGUI;
		base.OnInspectorGUI();

		Save(false);
		serializedObject.ApplyModifiedProperties();	
	}

	protected override void DrawCustomIconsUI()
	{
		EditorGUILayout.LabelField("Atlas Settings", EditorStyles.boldLabel);
		m.atlas = (NJGAtlas)EditorGUILayout.ObjectField("Atlas", m.atlas, typeof(NJGAtlas), false);

		mFolderObject = EditorGUILayout.ObjectField("Sprites Folder", AssetDatabase.LoadAssetAtPath(m.iconFolder, typeof(UnityEngine.Object)), typeof(UnityEngine.Object), false);
		if (mFolderObject != null)
		{
			mFolder = AssetDatabase.GetAssetPath(mFolderObject);

			bool valid = !mFolder.Contains(".");

			string folderName = mFolder.Substring(mFolder.LastIndexOf('/')).Replace('/', ' ').Trim();			

			GUI.backgroundColor = valid ? Color.cyan : Color.red;
			if (valid)
			{
				if (m.iconFolder != mFolder)
				{
					m.iconFolder = mFolder;
					Save(true);
				}
				GUI.backgroundColor = m.atlas != null ? Color.green : Color.cyan;
				if (GUILayout.Button(m.atlas != null ? "Update Atlas" : "Create Atlas"))
				{
					if (m.atlas != null)
					{
						NJGEditorTools.UpdateAtlas(m.atlas, m.iconFolder + "/");
					}
					else
					{
						m.atlas = NJGEditorTools.CreateAtlas(m.iconFolder + "/", folderName);
						Save(true);
					}
				}
				GUI.backgroundColor = Color.white;
				EditorGUILayout.HelpBox(m.iconFolder, MessageType.Info);				
			}
			else
			{
				EditorGUILayout.HelpBox("INVALID! You must select a folder", MessageType.Error);
			}
			GUI.backgroundColor = Color.white;
		}
		else
		{
			GUI.backgroundColor = Color.yellow;
			EditorGUILayout.HelpBox("Drag your icon folder here", MessageType.Warning);
			GUI.backgroundColor = Color.white;
		}
		//GUILayout.EndHorizontal();
		NJGEditorTools.DrawSeparator();
	}

	protected override void DrawEditButtons()
	{
		if (UIMiniMapOnGUI.instance != null)
		{
			GUI.backgroundColor = UIMiniMapOnGUI.instance != null ? Color.cyan : Color.green;
			if (GUILayout.Button(new GUIContent(UIMiniMapOnGUI.instance != null ? "Edit Mini Map" : "Create Mini Map", "Click to edit the Mini Map")))
			{
				if (UIMiniMapOnGUI.instance != null)
				{
					Selection.activeGameObject = UIMiniMapOnGUI.instance.gameObject;
				}
				/*else
				{
					NJGEditorTools.CreateMap(true);
				}*/
			}
		}

		if (UIWorldMapOnGUI.instance != null)
		{
			GUI.backgroundColor = UIWorldMapOnGUI.instance != null ? Color.cyan : Color.green;

			if (GUILayout.Button(new GUIContent(UIWorldMapOnGUI.instance != null ? "Edit World Map" : "Create World Map", "Click to edit the World Map")))
			{
				if (UIWorldMapOnGUI.instance != null)
				{
					Selection.activeGameObject = UIWorldMapOnGUI.instance.gameObject;
				}
				/*else
				{
					NJGEditorTools.CreateMap(false);
				}*/
			}
		}
	}

	protected override void DrawMapNotFound()
	{
		if (UIMiniMapOnGUI.instance == null && UIWorldMapOnGUI.instance == null)
		{
			GUI.backgroundColor = Color.red;
			EditorGUILayout.HelpBox("Could not found any UIMiniMapOnGUI or UIWorldMapOnGUI instance.", MessageType.Error);
			GUI.backgroundColor = Color.white;
		}
	}

	protected override void DrawComponentSelector()
	{
		/*ComponentSelector.Draw<UIAtlas>(m.atlas, OnSelectAtlas);
		if (m.atlas == null)
		{
			EditorGUILayout.HelpBox("You need to select an atlas first", MessageType.Warning);
		}*/
	}

	Material mMat;
	protected override void DrawIconSpriteUI(NJGMapBase.MapItemType mapItem)
	{
		if (m.atlas != null)
		{
			string spr = mapItem.sprite;

			IconField("Icon Sprite", spr, delegate(string sp)
			{
				mapItem.sprite = sp;
				Save(true);
			}, GUILayout.Width(120f));

			float extraSpace = 0;

			// Draw sprite preview.					
			if (mMat == null) mMat = m.atlas.spriteMaterial;

			if (mMat != null)
			{
				Texture2D tex = mMat.mainTexture as Texture2D;

				if (tex != null)
				{
					NJGAtlas.Sprite sprite = m.atlas.GetSprite(spr);

					if (sprite != null)
					{
						int size = mapItem.useCustomSize ? mapItem.size : m.iconSize;
						GUILayout.Space(4f);
						GUILayout.BeginHorizontal();
						{
							GUILayout.Space((Screen.width - 100) - size);
							GUI.color = mMat.color = mapItem.color;

							DrawSprite(tex, sprite.uvs, null, false, size, new Vector2(0, 18));
							GUI.color = Color.white;
						}
						GUILayout.EndHorizontal();

						extraSpace = size * (float)sprite.height / sprite.width;
					}
				}
				extraSpace = Mathf.Max(0f, extraSpace - 24f);
				GUILayout.Space(extraSpace);
			}
		}
	}

	protected override void DrawArrowSpriteUI(NJGMapBase.MapItemType mapItem)
	{
		if (m.atlas != null && mapItem.haveArrow)
		{
			GUILayout.BeginVertical("Box");

			//if (string.IsNullOrEmpty(mSpriteName)) mSpriteName = m.atlas.spriteList[0].name;
			//string spr = string.IsNullOrEmpty(mapItem.arrowSprite) ? mSpriteName : mapItem.arrowSprite;
			string spr = mapItem.arrowSprite;

			mapItem.arrowOffset = EditorGUILayout.IntField("Arrow Offset", mapItem.arrowOffset);
			mapItem.arrowRotate = EditorGUILayout.Toggle("Arrow Rotate", mapItem.arrowRotate);

			IconField("Arrow Sprite", spr, delegate(string sp)
			{
				mapItem.arrowSprite = sp;
				Save(true);
			}, GUILayout.Width(120f));

			float extraSpace = 0;

			// Draw sprite preview.					
			if (mMat == null) mMat = m.atlas.spriteMaterial;

			if (mMat != null)
			{
				Texture2D tex = mMat.mainTexture as Texture2D;

				if (tex != null)
				{
					NJGAtlas.Sprite sprite = m.atlas.GetSprite(spr);

					if (sprite != null)
					{
						GUILayout.Space(4f);
						GUILayout.BeginHorizontal();
						{
							GUILayout.Space((Screen.width - 100) - m.arrowSize);
							GUI.color = mMat.color = mapItem.color;
							DrawSprite(tex, sprite.uvs, null, false, m.arrowSize, new Vector2(0, 18));
							GUI.color = Color.white;
						}
						GUILayout.EndHorizontal();

						extraSpace = m.arrowSize * (float)tex.height / tex.width;
					}
				}
				extraSpace = Mathf.Max(0f, extraSpace - 20f);
				GUILayout.Space(extraSpace);
			}

			// Depth
			GUILayout.BeginHorizontal();
			{
				EditorGUILayout.PrefixLabel("Arrow Depth");

				int depth = mapItem.arrowDepth;
				if (GUILayout.Button("Back", GUILayout.Width(60f))) --depth;
				depth = EditorGUILayout.IntField(depth);
				if (GUILayout.Button("Forward", GUILayout.Width(60f))) ++depth;

				if (mapItem.arrowDepth != depth)
				{
					mapItem.arrowDepth = depth;
				}
			}
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
		}
	}

	#region Sprite Field
	/// <summary>
	/// Draw a sprite selection field.
	/// </summary>

	static public void IconField(string fieldName, string sprite,
		IconSelector.Callback callback, params GUILayoutOption[] options)
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label(fieldName, GUILayout.Width(116f));

		if (GUILayout.Button(string.IsNullOrEmpty(sprite) ? "Select Icon" : sprite, "MiniPullDown", options))
		{
			IconSelector.Show(NJGMapOnGUI.instance.atlas, sprite, callback);
		}
		GUILayout.EndHorizontal();
	}

	/// <summary>
	/// Draw a sprite selection field.
	/// </summary>

	/*static public void IconField(string fieldName, string caption, string sprite, IconSelector.Callback callback)
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label(fieldName, GUILayout.Width(116f));

		if (GUILayout.Button(sprite, "MiniPullDown", GUILayout.Width(120f)))
		{
			IconSelector.Show(NJGMapOnGUI.instance.atlas, sprite, callback);
		}

		if (!string.IsNullOrEmpty(caption))
		{
			GUILayout.Space(20f);
			GUILayout.Label(caption);
		}
		
		GUILayout.EndHorizontal();
		GUILayout.Space(-4f);
		
	}*/
	#endregion
}
