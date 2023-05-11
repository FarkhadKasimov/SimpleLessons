using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Battle : MonoBehaviour
{
    private List<Warrior> _warriorList = new List<Warrior>() 
    {
        new Knight(100, 50, DamageType.Melee, 20),
        new Archer(100, 20, 0.5f),
        new Dragon(100, 70, DamageType.Range)
    };

    [SerializeField] private List<Text> _warriorTexts;

    private void Start()
    {
        StartCoroutine(Fight());
    }
    private IEnumerator Fight()
    {
        for (int i = 0; i < _warriorTexts.Count; i++)
        {
            _warriorTexts[i].text = $"{_warriorList[i].GetType()}\n {_warriorList[i].GetInfo()}";
        }

        yield return new WaitForSeconds(2);

        _warriorList[0].TakeDamage(_warriorList[1].GetDamage());

        for (int i = 0; i < _warriorTexts.Count; i++)
        {
            _warriorTexts[i].text = $"{_warriorList[i].GetType()}\n {_warriorList[i].GetInfo()}";
        }
    }
}

public enum DamageType { Melee, Range, Magic};

public class Warrior
{
    protected int Health;
    protected int Damage;
    protected DamageType DmgType;

    public Warrior(int health, int damage, DamageType dmgType)
    {
        Health = health;
        Damage = damage;
        DmgType = dmgType;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
    }

    public int GetDamage()
    {
        return Damage;
    }

    public string GetInfo()
    {
        return $"HP: {Health}\n Dmg: {Damage}\n DamageType: {DmgType}";
    }

    public bool IsAlive()
    {
        return Health > 0;
    }
}

public class Knight : Warrior
{
    private int _armor;

    public Knight(int health, int damage, DamageType dmgType, int armor) : base(health, damage, dmgType) 
    {
        _armor = armor;
    }
}

public class Archer : Warrior
{
    private float _criticalChanse;

    public Archer(int health, int damage, float criticalChanse) : base(health, damage, DamageType.Range)
    {
        _criticalChanse = criticalChanse;
    }
}

public class Dragon : Warrior
{
    public Dragon(int health, int damage, DamageType dmgType) : base(health, damage, dmgType)
    {
        
    }

    public void Heal(int heal)
    {
        Health += heal;
    }
}
