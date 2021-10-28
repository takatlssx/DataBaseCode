namespace DataBase
{
    public class Column
    {
        public string Name {get;}
        public string Type {get;}
        public bool InNullable {get;}
        public string Alias {get;}
        public int Index {get;}
        
        public Column(string name, string type, bool isNullable, string alias, int index)
        {
            Name = name;
            Type = type;
            IsNullable = isNullable;
            Alias = alias;
            Index = index;
        }        
    }
}
