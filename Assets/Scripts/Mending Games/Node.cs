using DG.Tweening;

using PlayArea;
using System;
using UnityEngine;

namespace MendingGames {
    public class Node : MonoBehaviour {
        private ToolType requiredToolType;
        private SpriteRenderer spriteRenderer;
        private bool animationPlaying;

        [Header("Reset Tweening Elements")]
        [SerializeField] float duration;
        [SerializeField] float strength;
        [SerializeField] int vibrato;
        [SerializeField] float randomness;
        [SerializeField] bool snapping;
        [SerializeField] bool fadeOut;

        /* Public Properties */
        public bool StartingNode { get; set; }
        public bool ActiveNode { get; set; }
        public bool TargetNode { get; set; }
        public bool Triggered { get; private set; }
        public PlayAreaCanvasManager CanvasManager { get; set; }

        public static event Action<Node> OnNodeTriggered;
        public static event Action<Node> OnActiveNodeReleased;

        private void Awake() {
            DOTween.Init();

            spriteRenderer = this.GetComponent<SpriteRenderer>();

            Triggered = false;
            TargetNode = false;
        }

        private void Update() {
            // Only send the released command for the active node
            if (Input.GetMouseButtonUp(0) && this.ActiveNode && this.Triggered) {
                ActivateOrDeactivateNode(false);
            }
        }

        private void OnMouseDown() {
            if (TargetNode && CanvasManager.CurrentToolType == requiredToolType) {
                ActivateOrDeactivateNode(true);
            }
        }

        private void OnMouseOver() {
            if (!Triggered && Input.GetMouseButton(0) && CanvasManager.CurrentToolType == requiredToolType && TargetNode) {
                ActivateOrDeactivateNode(true);
            }
        }

        private void ActivateOrDeactivateNode(bool Triggered) {
            // Activate target node if clicked or moused over
            if (Triggered) {
                spriteRenderer.color = Color.green;
                OnNodeTriggered?.Invoke(this);
                if (!animationPlaying) {
                    animationPlaying = true;
                    Sequence sequence = DOTween.Sequence();
                    sequence.Append(this.gameObject.transform.DOScale(.15f, 0.25f));
                    sequence.SetLoops(2, LoopType.Yoyo);
                    animationPlaying = false;
                }

                this.Triggered = Triggered;
                this.ActiveNode = true;
            } else {
                // Or release control of target node if mouse button is released
                this.Triggered = false;
                spriteRenderer.color = Color.blue;
                OnActiveNodeReleased?.Invoke(this);
                if (!animationPlaying) {
                    animationPlaying = true;
                    this.gameObject.transform.DOShakePosition(duration, strength, vibrato, randomness, snapping, fadeOut);
                    animationPlaying = false;
                }
            }
        }

        public void SetColor(Color color) {
            this.spriteRenderer.color = color;
        }

        public void SetToolType(ToolType requiredToolType) {
            this.requiredToolType = requiredToolType;
        }
    }

}
