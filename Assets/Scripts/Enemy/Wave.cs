using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Wave")]
public class Wave : ScriptableObject
{
    public EnemiesInformation[] enemies;
    public int startLevel;
    [Space]
    public int times;
    public float timeRange;
}