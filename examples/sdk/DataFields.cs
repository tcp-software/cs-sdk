using System;
using System.Collections.Generic;
using System.Text;

public class DataFields : Dictionary<string, DataItem> { }

public class DataItem
{
    private string _value = null;
    private DataFields _item = null;

    public DataItem()
    {
    }

    public DataItem(string Value, DataFields Item)
    {
        _value = Value;
        _item = Item;
    }

    public string Value
    {
        get { return _value; }
        set { _value = value; }
    }

    public DataFields Item
    {
        get { return _item; }
        set { _item = value; }
    }
}
