using UnityEngine;

/// <summary>
/// Dialogue Set
/// 
/// This class represents the data structure informing a particular client's 
/// body of dialogue for an interaction. This will be defined via the inspector
/// </summary>
namespace Dialogue {
    [System.Serializable]
    public class ClientDialogueSet {
        [SerializeField]
        private string name;
        [SerializeField, TextArea(3, 10)]
        private string[] sentences;

        /* Public Properties */
        public string Name { get => name; }
        public string[] Sentences { get => sentences; }
    }

}

