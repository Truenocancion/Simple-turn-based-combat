using System;

public class Unit
{
    private int _health;
    private int _armor;
    private int _vampirism;
    public int Health
    {
        get { return _health; }
        set { _health = Math.Max(0, Math.Min(value, 100)); }
    }
    public int Armor
    {
        get { return _armor; }
        set { _armor = Math.Max(0, Math.Min(value, 100)); }
    }
    public int Vampirism
    {
        get { return _vampirism; }
        set { _vampirism = Math.Max(0, Math.Min(value, 100)); }
    }

    public int baseAttackDamage;
    public Buff firstBuff;
    public Buff secondBuff;

    public Unit()
    {
        Health = 100;
        Armor = 0;
        Vampirism = 0;
        baseAttackDamage = 15;
    }

    public void Attack(Unit targetUnit)
    {
        UpdateBuff(ref firstBuff, this, targetUnit);
        UpdateBuff(ref secondBuff, this, targetUnit);
        UpdateBuff(ref targetUnit.firstBuff, targetUnit, this);
        UpdateBuff(ref targetUnit.secondBuff, targetUnit, this);

        int damageReductionFromArmor = (int)(baseAttackDamage * targetUnit.Armor / 100);
        int damageApplied = baseAttackDamage - damageReductionFromArmor;
        if ((firstBuff != null && firstBuff.buffData.doubleDamageActive == true) || (secondBuff != null && secondBuff.buffData.doubleDamageActive == true))
        {
            damageApplied *= 2;
        }
        // as unit shouldn't be able to heal the target when attacking
        if (damageApplied > 0)
        {
            targetUnit.Health -= damageApplied;
        }

        int healthStolen = (int)(damageApplied * Vampirism / 100);
        Health += healthStolen;
    }

    private void UpdateBuff(ref Buff buff, Unit thisUnit, Unit otherUnit)
    {
        if (buff != null)
        {
            if (buff.turns > 0)
            {
                buff.turns--;

                if (buff.isApplied == false)
                {
                    ApplyBuff(buff, thisUnit, otherUnit);
                }
            }
            else
            {
                RemoveBuff(ref buff, thisUnit, otherUnit);
            }
        }
    }

    private void ApplyBuff(Buff buff, Unit thisUnit, Unit otherUnit)
    {
        thisUnit.Armor += buff.buffData.playerArmorValueChange;
        otherUnit.Armor += buff.buffData.enemyArmorValueChange;
        thisUnit.Vampirism += buff.buffData.playerVampirismValueChange;
        otherUnit.Vampirism += buff.buffData.enemyVampirismValueChange;
        buff.isApplied = true;
    }

    private void RemoveBuff(ref Buff buff, Unit thisUnit, Unit otherUnit)
    {  
        if ((buff.buffData.playerArmorValueChange < 0 && Armor > 0) || buff.buffData.playerArmorValueChange > 0)
        {
            thisUnit.Armor -= buff.buffData.playerArmorValueChange;
        }
        if ((buff.buffData.enemyArmorValueChange < 0 && otherUnit.Armor > 0) || buff.buffData.enemyArmorValueChange > 0)
        {
            otherUnit.Armor -= buff.buffData.enemyArmorValueChange;
        }
        if ((buff.buffData.playerVampirismValueChange < 0 && Vampirism > 0) || buff.buffData.playerVampirismValueChange > 0)
        {
            thisUnit.Vampirism -= buff.buffData.playerVampirismValueChange;
        }
        if ((buff.buffData.enemyVampirismValueChange < 0 && otherUnit.Vampirism > 0) || buff.buffData.enemyVampirismValueChange > 0)
        {
            otherUnit.Vampirism -= buff.buffData.enemyVampirismValueChange;
        } 
        buff = null;
    }
}
