using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LevelDataScriptableObject", order = 0)]
public class LevelData : ScriptableObject
{
    public List<LoadCapturableData> targets;
}
