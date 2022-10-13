using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/ZombieData", fileName = "Zombie Data")]
public class ZombieData : ScriptableObject
{
    public float health = 100;
    public float damage = 100;
    public float speed = 10f;
    public Color skinColor = Color.white;
}
