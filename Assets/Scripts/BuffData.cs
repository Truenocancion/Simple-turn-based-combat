using UnityEngine;

[CreateAssetMenu(fileName = "New Buff", menuName = "Game/Buff Data")]
public class BuffData : ScriptableObject
{
    public string name;
    [Range(1, 3)]
    public int turns;
    [Header("Damage")]
    public bool doubleDamageActive;
    [Header("Armor")]
    [Range(-50, 50)]
    public int playerArmorValueChange;
    [Range(-50, 50)]
    public int enemyArmorValueChange;
    [Header("Vampirism")]
    [Range(-50, 50)]
    public int playerVampirismValueChange;
    [Range(-50, 50)]
    public int enemyVampirismValueChange;
}
