using UnityEditor;
using UnityEngine;

namespace Demo
{
	[CustomEditor(typeof(TrackBuilder))]
	public class TrackBuilderEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			TrackBuilder builder = (TrackBuilder)target;

			//draw button
			EditorGUILayout.BeginHorizontal(GUILayout.Height(EditorGUIUtility.singleLineHeight + 4f * EditorGUIUtility.standardVerticalSpacing));
				EditorGUILayout.Space();
					
					if(GUILayout.Button("Build"))
						builder.Build();

					if(GUILayout.Button("Clear"))
						builder.Clear();

				EditorGUILayout.Space();
			EditorGUILayout.EndHorizontal();
		}
	}
}