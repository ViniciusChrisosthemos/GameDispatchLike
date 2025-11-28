using System;

[Serializable]
public class ItemHolder<T>
{
    public T Item;
    public int Amount;
    
    public ItemHolder(T item, int amount)
    {
        Item = item;
        Amount = amount;
    }
}
