using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* User-defined Namespaces */
using PlayArea;

namespace Scriptables.DamageInstructions {
    [CreateAssetMenu(fileName = "Damage Instructions", menuName = "Scriptable Objects/Damage Instructions")]
    public class DamageInstructrionsScriptableObject : ScriptableObject {
        [SerializeField] private string title;
        [SerializeField] private PlushieDamageType plushieDamageType;
        [SerializeField] private ToolType requiredToolType;
        [SerializeField] private Vector2[] targetLocations;
        [SerializeField] private Sprite damageSprite;
        [SerializeField] private int rotationZValue;

        /* Public Properties */
        public PlushieDamageType PlushieDamageType { get => plushieDamageType; }
        public ToolType RequiredToolType { get => requiredToolType; }
        public Vector2[] TargetLocations { get => targetLocations; }
        public Sprite DamageSprite { get => damageSprite; }
        public int RotationZValue { get => rotationZValue; }
    }


}