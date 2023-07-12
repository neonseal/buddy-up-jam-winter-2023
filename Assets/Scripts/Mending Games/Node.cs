using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
/* User-defined Namespaces */
using PlayArea;

namespace MendingGames {
    public class Node : MonoBehaviour {
        private bool startingNode;
        private bool targetNode;
        private bool activated;
        private ToolType requiredToolType;
        private SpriteRenderer spriteRenderer;

        public static event Action<Node> OnTargetNodeTriggered;
        public static event Action<Node> OnTargetNodeReleased;

        private void Awake() {
            DOTween.Init();

            spriteRenderer = this.GetComponent<SpriteRenderer>();

            activated = false;
        }

        private void OnMouseDown() {
            if (this.startingNode) {
                UpdateTargetNode(true);
            }
        }

        private void OnMouseOver() {
            if (this.targetNode) {
                UpdateTargetNode(true);
            }
        }

        private void OnMouseUp() {
            if (this.targetNode) {
                UpdateTargetNode(false);
            }
        }

        private void UpdateTargetNode(bool triggered) {
            // Activate target node if clicked or moused over
            if (triggered) {
                spriteRenderer.color = Color.red;
                OnTargetNodeTriggered?.Invoke(this);
                Sequence sequence = DOTween.Sequence();
                sequence.Append(this.gameObject.transform.DOScale(.15f, 0.25f));
                sequence.SetLoops(1, LoopType.Yoyo);
                this.activated = true;
            } else {
                // Or release control of target node if mouse button is released
                spriteRenderer.color = Color.blue;
                OnTargetNodeReleased?.Invoke(this);
                this.activated = false;
            }
        }

        public void SetNodeProperties(ToolType requiredToolType, bool isStartingNode = false) {
            this.requiredToolType = requiredToolType;
            this.startingNode = isStartingNode;

            if (isStartingNode) {
                spriteRenderer.color = Color.blue;
                this.targetNode = true;
            }
        }

        public bool IsTargetNode() {
            return targetNode;
        }


        /* Public Properties */
        public bool Activated { get => activated; }
    }

}
