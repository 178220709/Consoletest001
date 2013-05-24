 using System;
 using System.Collections.Generic;
 using System.Reflection;

using System.Linq;

class Item
{
    public Item(int id)
    {
        ID = id;
        IsEnabled = true;
    }
 
    public int ID { get; private set; }
 
    static Item()
    {
        var type = typeof(Item);
        D = new PropertyInfo[6];
        D[0] = type.GetProperty("D1");
        D[1] = type.GetProperty("D2");
        D[2] = type.GetProperty("D3");
        D[3] = type.GetProperty("D4");
        D[4] = type.GetProperty("D5");
        D[5] = type.GetProperty("D6");
    }
 
    private static PropertyInfo[] D;
    public Item D1 { get; private set; }
    public Item D2 { get; private set; }
    public Item D3 { get; private set; }
    public Item D4 { get; private set; }
    public Item D5 { get; private set; }
    public Item D6 { get; private set; }
 
    public void SetItem(Item item, int d)
    {
        var property = D[d - 1];
        property.SetValue(this, item);
        if (D[GetDirection(d + 3) - 1].GetValue(item) == null)
        {
            D[GetDirection(d + 3) - 1].SetValue(item, this);
        }
    }
 
    private int GetDirection(int d)
        {
            if (d > 6)
                d -= 6;
            return d;
        }
 
    public bool IsEnabled { get; set; }
 
    public int GetTriangle()
    {
        int count = 0;
        for (int i = 1; i < 7; i++)
        {
            int d1 = i - 1;
            int d2 = GetDirection(i + 1) - 1;
            int d3 = GetDirection(i + 2) - 1;
            int d4 = GetDirection(i + 3) - 1;
            Item next1 = D[d1].GetValue(this) as Item;
            Item next2 = D[d2].GetValue(this) as Item;
            while (next1 != null && next2 != null)
            {
                if (next1.IsEnabled && next2.IsEnabled)
                    count++;
                Item next3 = next1;
                Item next4 = next2;
                while (D[d3].GetValue(next3) != next2)
                {
                    next3 = D[d3].GetValue(next3) as Item;
                    next4 = D[d4].GetValue(next4) as Item;
                    if (next3 == null || next4 == null)
                        break;
                    if (next3.IsEnabled && next4.IsEnabled)
                        count++;
                }
                next1 = D[d1].GetValue(next1) as Item;
                next2 = D[d2].GetValue(next2) as Item;
            }
        }
        return count;
    }


    static void Mainds(string[] args)
    {
        List<Item> items = new List<Item>();
        Item item1 = new Item(1);
        Item item2 = new Item(2);
        Item item3 = new Item(3);
        Item item4 = new Item(4);
        Item item5 = new Item(5);
        Item item6 = new Item(6);
        Item item7 = new Item(7);
        Item item8 = new Item(8);
        Item item9 = new Item(9);
        Item item10 = new Item(10);

        item1.SetItem(item2, 6);
        item1.SetItem(item3, 5);

        item2.SetItem(item4, 6);
        item2.SetItem(item5, 5);
        item2.SetItem(item3, 4);

        item3.SetItem(item5, 6);
        item3.SetItem(item6, 5);

        item4.SetItem(item7, 6);
        item4.SetItem(item8, 5);
        item4.SetItem(item5, 4);

        item5.SetItem(item8, 6);
        item5.SetItem(item9, 5);
        item5.SetItem(item6, 4);

        item6.SetItem(item9, 6);
        item6.SetItem(item10, 5);

        item7.SetItem(item8, 4);
        item8.SetItem(item9, 4);
        item9.SetItem(item10, 4);

        items.Add(item1);
        items.Add(item2);
        items.Add(item3);
        items.Add(item4);
        items.Add(item5);
        items.Add(item6);
        items.Add(item7);
        items.Add(item8);
        items.Add(item9);
        items.Add(item10);

        List<Item> result = new List<Item>();

        Item pick = items.Where(t => t.IsEnabled).OrderByDescending(t => t.GetTriangle()).FirstOrDefault();
        int count = pick.GetTriangle();
        while (count > 0)
        {
            result.Add(pick);
            pick.IsEnabled = false;
            pick = items.Where(t => t.IsEnabled).OrderByDescending(t => t.GetTriangle()).FirstOrDefault();
            if (pick == null)
                count = 0;
            else
                count = pick.GetTriangle();
        }

        foreach (Item item in result)
        {
            Console.WriteLine(item.ID);
        }

        Console.ReadLine();
    }

}