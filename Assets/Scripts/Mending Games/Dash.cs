using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
/* User-defined Namespaces */
using PlayArea;

namespace MendingGames {
    public class Dash : MonoBehaviour {
        private bool activated;
        private ToolType requiredToolType;
        private SpriteRenderer spriteRenderer;

        public static event Action<Dash> OnDashTriggered;
        public static event Action<Dash> OnDashedLineReleased;

        private void Awake() {
            DOTween.Init();

            spriteRenderer = this.GetComponent<SpriteRenderer>();

            activated = false;
        }

        /* Public Properties */
        public bool Activated { get => activated; }
    }
}
