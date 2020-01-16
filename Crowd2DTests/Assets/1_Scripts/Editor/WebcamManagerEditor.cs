using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CrowdProject
{
    [CustomEditor(typeof(WebCamManager))]
    public class WebcamManagerEditor : Editor
    {
        WebCamManager webcamManager;
        Editor noiseEditor;

        public override void OnInspectorGUI()
        {

            using (var check = new EditorGUI.ChangeCheckScope())
            {
                base.OnInspectorGUI();
            }

            DrawSettingsEditor(webcamManager.settings, ref webcamManager.foldout, ref noiseEditor);

            if (GUILayout.Button("Apply Testing Values"))
            {
                webcamManager.ApplyTest();
            }
        }

        void DrawSettingsEditor(UnityEngine.Object settings, ref bool foldout, ref Editor editor)
        {
            if (settings != null)
            {
                foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);

                using (var check = new EditorGUI.ChangeCheckScope())
                {
                    if (foldout)
                    {
                        CreateCachedEditor(settings, null, ref editor);
                        editor.OnInspectorGUI();
                    }
                }
            }
        }

        private void OnEnable()
        {
            webcamManager = (WebCamManager)target;
        }
    }
}