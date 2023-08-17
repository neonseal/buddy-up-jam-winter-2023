using UnityEngine;

[CreateAssetMenu(fileName = "Client Card", menuName = "Scriptable Objects/Client Card")]
public class ClientCardScriptableObject : ScriptableObject {
    public ClientCard clientCardPrefab;
    public string clintName;
    [TextArea(3, 10)]
    public string[] sentences;
}