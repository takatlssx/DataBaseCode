namespace DataBase
{
    public class DataBase
    {
        public string Name {get; private set;}
        public string IniFilePath {get; private set;}    
        public List<List<string>> IniSectionStrsList {get; private set;} = new List<List<string>>();
        
        public string RootDir {get; private set;}
        public string FileDir {get; private set;}
        public string BackupDir {get; private set;}
        
        public List<string> TableNames {get; private set;}
        public string MainTableName {get; private set;}
        public List<string> RelationalTableNames {get; private set;}
        
        public Dictionay<string,Table> Tables {get; private set;} = new Dictionay<string,Table>();
        
        public Table MainTable {get; private set;}
        
        public Dictionay<string,Table> RelationalTables {get; private set;} = new Dictionay<string,Table>();
        
        public DataBase(string name, string iniFilePath)
        {
            Name = name;
            IniFilePath = iniFilePath;
        }
    
        //設定取得
        private void _initialize()
        {
            //iniを読み込み、文字列をIniSectionStrsList(List<List<string>>)に格納
            using(StreamReader sr = new StreamReader(IniFilePath))
            {
                string line = "";
                
                //セクションの文字行を格納するリスト
                List<string> sectionStrs = new List<string>();
                
                //現在処理中のセクション番号のカウンター
                //IniSectionStrsListのインデックスとして使用
                int sectionCount = -1;
                
                while((line = sr.ReadLine()) != null)
                {                    
                    //lineから空白文字を除去
                    line = line.Replace(" ","").Replace("　","");
                    
                    //sectionラベル行なら、この前処理していたsectionCountをカウントアップし
                    //IniSectionStrsListに新しいList<string>を追加
                    if(line == "[database]" || line == "[table]")
                    {
                        sectionCount++;
                        IniSectionStrsList.Add(new List<string>())
                    }
                    
                    //1行の文字列を追加
                    IniSectionStrsList[sectionCount].Add(line);
                }
            }
            
            //IniSectionStrsListをループし、このデータベースの情報を取得
            foreach(List<string> sectionStrs in IniSectionStrsList)
            {
                //このデータベースのセクションか確認
                if(sectionStrs.Contains("[database]") && sectionStrs.Contains($"name={Name}"))
                {
                    foreach(string st in sectionStrs)
                    {
                        string ky = "";
                        string vl = "";
                        
                        if(st != "[database]")
                        {
                            ky = st.Split('=')[0];
                            vl = st.Split('=')[1];
                        }
                        
                        if(ky == "root_dir")
                        {
                            RootDir = vl;
                        }
                        else if(ky == "file_dir")
                        {
                            FileDir = vl;
                        }
                        else if(ky == "backup_dir")
                        {
                            BackupDir = vl;
                        }
                        else if(ky == "member")
                        {
                            TableNames = vl.Split(',').ToList();
                        }
                        else if(ky == "main")
                        {
                            MainTableName = vl;
                        }
                        else if(ky == "relational")
                        {
                            RelationalTableNames = vl.Split(',').ToList();
                        }
                    }
                }
                
                //このデータベースに所属するテーブルのセクションか確認
                if(sectionStrs.Contains("[table]") && sectionStrs.Contains($"owner={Name}"))
                {
                    foreach(string st in sectionStrs)
                    {
                        string ky = "";
                        string vl = "";
                        
                        if(st != "[database]")
                        {
                            ky = st.Split('=')[0];
                            vl = st.Split('=')[1];
                        }
                        //キーがテーブル名ならテーブルオブジェクト生成し
                        //テーブルディクショナリに追加
                        if(ky == "name")
                        {
                            Tables[vl] = new Table(vl);
                        }
                        else if(ky == "owner")
                        {
                            Tables[vl].Owner = vl;
                        }
                        
                    }
                }
            }
            
        }
    }
}
