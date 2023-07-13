using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
/* User-defined Namespaces */
using PlayArea;

namespace MendingGames {
    public class Dash : MonoBehaviour {
        private bool enabled;
        private bool triggered;
        private ToolType requiredToolType;
        private SpriteRenderer spriteRenderer;

        public static event Action<Dash> OnDashTriggered;
        public static event Action<Dash> OnDashedLineReleased;

        private void Awake() {
            DOTween.Init();

            spriteRenderer = this.GetComponent<SpriteRenderer>();

            enabled = false;
            triggered = false;
        }

        public void EnableDash(ToolType requiredToolType) {
            spriteRenderer.color = Color.blue;
            enabled = true;
        }

        public void Reset(bool active) {
            triggered = false;
            enabled = active;
            spriteRenderer.color = Color.black;
        }

        private void OnMouseOver() {
            if (Input.GetMouseButton(0) &&
                enabled &&
                !triggered 
                //&&
                //CanvasManager.toolType == requiredToolType
            ) {
                spriteRenderer.color = Color.yellow;
                triggered = true;
                Sequence sequence = DOTween.Sequence();
                sequence.Append(this.gameObject.transform.DOScale(.15f, 0.25f));
                sequence.SetLoops(2, LoopType.Yoyo);
            }
        }

        /* Public Properties */
        public bool Enabled { get => enabled; }
        public bool Triggered { get => triggered; }

    }
}
