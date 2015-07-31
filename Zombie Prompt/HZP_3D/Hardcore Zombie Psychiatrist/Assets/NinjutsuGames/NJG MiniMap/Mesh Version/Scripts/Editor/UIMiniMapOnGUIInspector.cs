//----------------------------------------------
//            NJG MiniMap (NGUI)
// Copyright © 2014 Ninjutsu Games LTD.
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(UIMiniMapOnGUI))]
public class UIMiniMapOnGUIInspector : NJG.UIMiniMapInspectorBase
{
	protected override void DrawNotFound()
	{
		UIMiniMapOnGUI mp = (UIMiniMapOnGUI)m;
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
			else
			{
				if (mp.material == null) mp.material = NJGEditorTools.GetMaterial(m, true);
			}
		}
	}
}
