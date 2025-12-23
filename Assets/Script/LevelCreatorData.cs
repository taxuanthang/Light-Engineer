using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData2D", menuName = "Tools/2D Level Data")]
public class LevelData2D : ScriptableObject
{
    public List<GameObject> prefabs = new List<GameObject>();
}
