/* User-defined Namespaces */
using PlayArea;
using UnityEngine;

namespace Scriptables {
    [CreateAssetMenu(fileName = "Tool", menuName = "Scriptable Objects/Tool")]
    public class ToolScriptableObject : ScriptableObject {
        [SerializeField] private Sprite toolSlotSprite;
        [SerializeField] private Texture2D toolCursorTexture;
        [SerializeField] private Sprite standardToolImage;
        [SerializeField] private Sprite selectedToolImage;
        [SerializeField] private ToolType toolType;

        /* Public Properties */
        public Sprite ToolSlotSprite { get => toolSlotSprite; }
        public Texture2D ToolCursorTexture { get => toolCursorTexture; }
        public Sprite StandardToolImage { get => standardToolImage; }
        public Sprite SelectedToolImage { get => selectedToolImage; }
        public ToolType ToolType { get => toolType; }
    }
}


