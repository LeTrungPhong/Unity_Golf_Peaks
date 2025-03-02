using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PathLevel", menuName = "ScriptableObjects/PathLevelScriptableObject", order = 2)]
public class PathLevelScriptableObject : ScriptableObject
{
    public List<string> pathLevel;
}
