using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBlueprint", menuName = "ScriptableObjects/EnemyBlueprint")]
public class EnemyBlueprint : ScriptableObject
{
    public float speed = 10f;
    public float health = 50f;
    public float givesMoney = 10f;
    public int givesExperience = 5;
}