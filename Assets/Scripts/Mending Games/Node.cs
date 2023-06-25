using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
/* User-defined Namespaces */
using PlayArea;

namespace MendingGames {
    public class Node : MonoBehaviour {
        private bool targetNode;
        private bool activated;
        private ToolType requiredToolType;
        private SpriteRenderer spriteRenderer;

        private void Awake() {
            DOTween.Init();

            spriteRenderer = this.GetComponent<SpriteRenderer>();
        }

        public Node(ToolType requiredToolType, bool isTarget = false) {
            this.requiredToolType = requiredToolType;
            this.targetNode = isTarget;
        }

        public bool IsTargetNode() {
            return targetNode;
        }


        /* Public Properties */
        public bool Activated { get => activated; }
    }

}
