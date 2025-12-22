using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Tools/Level Creator Data")]
public class LevelCreatorData : ScriptableObject
{
    public List<GameObject> prefabs = new List<GameObject>();
}
