using DG.Tweening;

using PlayArea;
using UnityEngine;

namespace MendingGames {
    public class Dash : MonoBehaviour {
        private ToolType requiredToolType;
        private SpriteRenderer spriteRenderer;

        [Header("Reset Tweening Elements")]
        [SerializeField] float duration;
        [SerializeField] float strength;
        [SerializeField] int vibrato;
        [SerializeField] float randomness;
        [SerializeField] bool snapping;
        [SerializeField] bool fadeOut;

        public bool Enabled { get; private set; }
        public bool Triggered { get; private set; }
        public Node ParentNode { get; set; }
        public int DashSetIndex { get; set; }
        public PlayAreaCanvasManager CanvasManager { get; set; }

        private void Awake() {
            DOTween.Init();

            spriteRenderer = this.GetComponent<SpriteRenderer>();

            Enabled = false;
            Triggered = false;
        }

        public void EnableDash(ToolType requiredToolType) {
            spriteRenderer.color = Color.blue;
            this.requiredToolType = requiredToolType;
            Enabled = true;
        }

        public void ResetDash(bool active) {
            Triggered = false;
            Enabled = active;
            spriteRenderer.color = Color.blue;
            this.gameObject.transform.DOShakePosition(duration, strength, vibrato, randomness, snapping, fadeOut);
        }

        private void OnMouseOver() {

            if (Input.GetMouseButton(0) &&
                Enabled &&
                !Triggered &&
                ParentNode.Triggered &&
                CanvasManager.CurrentToolType == requiredToolType
            ) {
                spriteRenderer.color = Color.green;
                Triggered = true;
                Sequence sequence = DOTween.Sequence();
                sequence.Append(this.gameObject.transform.DOScale(.15f, 0.25f));
                sequence.SetLoops(2, LoopType.Yoyo);
            }
        }

    }
}
