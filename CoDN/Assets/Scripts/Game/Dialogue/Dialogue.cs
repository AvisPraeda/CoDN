using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Dialogue", menuName = "Scriptable/Dialogue")]

[System.Serializable]
public class Dialogue : ScriptableObject
{
    public string header;

    [TextArea(1, 6)]
    public string[] sentences;
}
