namespace DataBase
{
    public class DataBase
    {
        public string Name {get; private set;}
        public string IniFilePath {get; private set;}
    
        public List<List<string>> IniLineStrLists {get; private set;}
    
        public DataBase(string name, string iniFilePath)
        {
            Name = name;
            IniFilePath = iniFilePath;
        }
    
        //設定取得
        private void _initialize()
        {
            //iniを読み込み、List<List<string>>に格納
            using(StreamReader sr = new StreamReader(IniFilePath))
            {
                string line = "";
                while((line = sr.ReadLine()) != null)
                {
                    line = line.Replace(" ","").Replace("　","");
                    //section行か確認
                    if(line == "[database]" || line == "[table]")
                    {
                    }
                }
            }
        }
    }
}
