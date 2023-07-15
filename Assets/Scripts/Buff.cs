public class Buff
{
    public BuffData buffData;
    public int turns;
    public bool isApplied;

    public Buff(BuffData buffData)
    {
        this.buffData = buffData;
        this.turns = buffData.turns;
        isApplied = false;
    }
}
