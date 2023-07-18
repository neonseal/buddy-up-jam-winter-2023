using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Plushie Set", menuName = "Scriptable Objects/Plushie Set")]
public class PlushieSetScriptableObject : ScriptableObject
{
    public List<PlushieScriptableObject> plushieList;
}
