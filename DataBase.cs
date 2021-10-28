namespace DataBase
{
    public class DataBase
    {
        public string Name {get; private set;}
        public string IniFilePath; private set;}
    
        
    
        public DataBase(string name, string iniFilePath)
        {
            Name = name;
            IniFilePath = iniFilePath;
        }
    
        //設定取得
        private void _initialize()
        {
            //iniを読み込み、List<List<string>>に格納
        }
    }
}
