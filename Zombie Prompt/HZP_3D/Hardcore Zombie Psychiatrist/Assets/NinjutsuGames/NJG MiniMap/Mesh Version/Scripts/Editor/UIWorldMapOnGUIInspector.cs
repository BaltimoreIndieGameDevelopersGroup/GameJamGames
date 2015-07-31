//----------------------------------------------
//            NJG MiniMap (NGUI)
// Copyright © 2014 Ninjutsu Games LTD.
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(UIWorldMapOnGUI))]
public class UIWorldMapOnGUIInspector : NJG.UIWorldMapInspectorBase
{
	protected override void DrawNotFound()
	{
		UIWorldMapOnGUI mp = (UIWorldMapOnGUI)m;
		if (mp != null)
		{
			if (mp.planeRenderer == null)
			{
				mp.planeRenderer = mp.GetComponentInChildren<MeshRenderer>();

				if (mp.planeRenderer == null)
				{
					GUI.backgroundColor = Color.red;
					EditorGUILayout.HelpBox("No MeshRenderer found.", MessageType.Error);
					GUI.backgroundColor = Color.white;

					/*if (GUILayout.Button("Create UITexture"))
					{
						NJGEditorTools.CreateUIMapTexture(m);
					}*/
					EditorGUILayout.Separator();
					return;
				}
			}
		}
	}
}
