using MendingGames;
/* User-defined Namespaces */
using PlayArea;
using UnityEngine;

namespace Scriptables.DamageInstructions {
    [CreateAssetMenu(fileName = "Damage Instructions", menuName = "Scriptable Objects/Damage Instructions")]
    public class DamageInstructrionsScriptableObject : ScriptableObject {
        public PlushieDamageType PlushieDamageType;
        public MendingGameType MendingGameType;
        public ToolType RequiredToolType;
        public Vector2[] TargetLocations;
        public Sprite DamageSprite;
        public Sprite RepairedDamageSprite;
        public int RotationZValue;
    }


}