using UnityEngine;
using UnityEditor;
using RubicksCubeCreator;

namespace RubicksCubeCreatorEditor
{
    [CustomEditor(typeof(CubePreset))]
    [CanEditMultipleObjects]
    public class CubePresetEditor : Editor
    {
        GameObject m_PreviewObject;
        public override bool HasPreviewGUI() => m_PreviewObject != null;
        private PreviewRenderUtility _previewRenderUtility;
        Editor gameObjectEditor;
        private void OnEnable()
        {
            m_PreviewObject = (target as CubePreset).BlockPrefab?.gameObject;
            m_PreviewObject.GetComponent<MeshRenderer>().materials = (target as CubePreset).Materials.ToArray();
        }
        private void OnDisable()
        {
            if (_previewRenderUtility != null)
            {
                _previewRenderUtility.Cleanup();
                _previewRenderUtility = null;
            }
        }
        public override void OnPreviewGUI(Rect r, GUIStyle background)
        {
            if (m_PreviewObject)
            {
                if (gameObjectEditor == null)
                    gameObjectEditor = Editor.CreateEditor(m_PreviewObject);
                gameObjectEditor.OnPreviewGUI(r, background);
            }
        }
    }
}
