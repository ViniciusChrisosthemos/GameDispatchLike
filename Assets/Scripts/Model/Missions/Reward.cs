public class Reward
{
    private int _money;
    private int _experience;

    public Reward(int money, int experience)
    {
        _money = money;
        _experience = experience;
    }

    public int Money => _money;

    public int Experience => _experience;
}
