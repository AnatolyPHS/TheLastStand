using GameSceneObjects.Units;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUnitInfo", menuName = "GameData/Unit Info")]
public class UnitInfo : ScriptableObject
{
    [SerializeField] private float health = 100f;
    [SerializeField][Range(0f,1f)] 
    private float armor = 0f;
    [SerializeField] private float attackPower = 10f;
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float searchRange = 5f;
    [SerializeField] private float unitStopDistance = 1f;
    [SerializeField] private UnitFaction faction = UnitFaction.None;
    [SerializeField] private int unitCost = 100;
    
    public float Health => health;
    public float Armor => armor;
    public float AttackPower => attackPower;
    public float AttackSpeed => attackSpeed;
    public float AttackRange => attackRange;
    public float MovementSpeed => movementSpeed;
    public float SearchRange => searchRange;
    public float UnitStopDistance => unitStopDistance;
    public UnitFaction Faction => faction;
    public int UnitCost => unitCost;
}
