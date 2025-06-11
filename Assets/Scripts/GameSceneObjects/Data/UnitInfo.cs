using UnityEngine;

[CreateAssetMenu(fileName = "NewUnitCharacteristics", menuName = "GameData/Unit Characteristics")]
public class UnitInfo : ScriptableObject
{
    [SerializeField] private float health = 100f;
    [SerializeField] private float armor = 0f; // 0 means no armor, 1 means full armor
    [SerializeField] private float attackPower = 10f;
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float movementSpeed = 5f;
    
    public float Health => health;
    public float Armor => armor;
    public float AttackPower => attackPower;
    public float AttackSpeed => attackSpeed;
    public float AttackRange => attackRange;
    public float MovementSpeed => movementSpeed;
}
