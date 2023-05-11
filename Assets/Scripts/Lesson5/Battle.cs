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

        _warriorList[0].TakeDamage(_warriorList[1].Damage);

        for (int i = 0; i < _warriorTexts.Count; i++)
        {
            _warriorTexts[i].text = $"{_warriorList[i].GetType()}\n {_warriorList[i].GetInfo()}";
        }
    }
}

public enum DamageType { Melee, Range, Magic};

public class Warrior
{
    protected int _health;
    protected DamageType _dmgType;

    public virtual int Damage { get; set; }
    public bool IsAlive { get { return _health > 0; } }

    public Warrior(int health, int damage, DamageType dmgType)
    {
        _health = health;
        Damage = damage;
        _dmgType = dmgType;
    }

    public virtual void TakeDamage(int damage)
    {
        _health -= damage;
    }

    public virtual string GetInfo()
    {
        return $"HP: {_health}\n Dmg: {Damage}\n DamageType: {_dmgType}";
    }
}

public class Knight : Warrior
{
    private int _armor;

    public Knight(int health, int damage, DamageType dmgType, int armor) : base(health, damage, dmgType) 
    {
        _armor = armor;
    }

    public override void TakeDamage(int damage)
    {
        damage -= _armor/4;

        if (damage > 0)
        {
            _health -= damage;
        }
        else
        {
            _health--;
        }
    }

    public override string GetInfo()
    {
        return $"HP: {_health}\n Dmg: {Damage}\n Damage Type: {_dmgType}\n Armor: {_armor}";
    }
}

public class Archer : Warrior
{
    private float _criticalChanse;

    public override int Damage 
    {
        get
        {
            float rand = Random.Range(0f, 1f);
            if (rand <= _criticalChanse)
                return base.Damage * 2;
            else
                return base.Damage;
        }
    }

    public Archer(int health, int damage, float criticalChanse) : base(health, damage, DamageType.Range)
    {
        _criticalChanse = criticalChanse;
    }

    public override string GetInfo()
    {
        return $"HP: {_health}\n Dmg: {Damage}\n Damage Type: {_dmgType}\n Critical Chanse: {_criticalChanse}";
    }
}

public class Dragon : Warrior
{
    public Dragon(int health, int damage, DamageType dmgType) : base(health, damage, dmgType)
    {
        
    }

    public void Heal(int heal)
    {
        _health += heal;
    }
}
