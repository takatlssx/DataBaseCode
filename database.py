class DataBase:
    
    def __init__(self,name,ini_file_path):
        self.name = name
        self.ini_file_path = ini_file_path
        self.ini_section_strs_list = []
    
    
    def initialize():
        #iniを読み込み、文字列をIniSectionStrsList(List<List<string>>)に格納
        with open(ini_file_path,"r",encoding = "utf-8") as f:
            section_strs = []
            section_cnt = -1
            for line in f:
                line = line.replace(" ","").replace("　","").replace("\n","")
                
                if line == "[database]" or line == "[table]":
                    section_cnt += 1
                    ini_section_strs_list.append([])
                
                ini_section_strs_list[section_cnt].append(line)
                
        #ini_section_strs_listをループし、このデータベースの情報を取得
        for section_strs in ini_section_strs_list:
            #このデータベースのセクションか確認
            if "[database]" in section_strs and f"name={self.name}" in section_strs:
                pass
            #このデータベースに所属するテーブルのセクションか確認
            if "[table]" in section_strs and f"owner={self.name}" in section_strs:
                pass
                    
                
                
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
                        
                        string tblName = "";
                        List<string> colmuns;
                        List<string> types;
                        List<bool> isNullables;
                        List<string> aliases;
                        string pk;
                        
                        //キーがテーブル名ならテーブルオブジェクト生成し
                        //テーブルディクショナリに追加
                        if(ky == "name")
                        {
                            tblName = vl;
                        }
                        else if(ky == "db_file_path")
                        {
                            Table[tblName] = new Table(tblName,vl);
                        }
                        else if(ky == "column")
                        {
                            columns = vl.Split(',').ToList();
                        }
                        else if(ky == "type")
                        {
                            types = vl.Split(',').ToList();
                        }
                        else if(ky == "is_nullable")
                        {
                            var buff = vl.Split(',');
                            foreach(string bf in buff)
                            {
                                isNullables.Add(bf == "true")
                            }
                        }
                        else if(ky == "alias")
                        {
                            aliases = vl.Split(',').ToList();
                        }
                        else if(ky == "primary_key")
                        {
                            Table[tblName].SetColumns(columns,types,isNullables,aliases,vl);
                            Table[tblName].SetData();
                        }
                    }
                }
            }
            
        }
