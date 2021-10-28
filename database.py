import table

class DataBase:
    
    def __init__(self,name,ini_file_path):
        self.name = name
        self.ini_file_path = ini_file_path
        self.ini_section_strs_list = []
        
        self.root_dir = ""
        self.file_dir = ""
        self.backup_dir = ""
        
        self.table_names = []
        self.main_table_name = ""
        self.relational_table_names = []
        
        self.tables = {}
    
    
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
                for line in section_strs:
                    ky = "" if line == "[database]" else line.split("=")[0]
                    vl = "" if line == "[database]" else line.split("=")[1]
                    
                    if ky == "root_dir":
                        pass
                    elif ky == "file_dir":
                        pass
                    elif ky == "backup_dir":
                        pass
                    elif ky == "member":
                        pass
                    elif ky =="main":
                        pass
                    elif ky == "relatinal":
                        pass
            #このデータベース所属のテーブルセクションか確認     
            if "[table]" in section_strs and f"owner={self.name}" in section_strs:
                for line in section_strs:
                    ky = "" if line == "[table]" else line.split("=")[0]
                    vl = "" if line == "[table]" else line.split("=")[1]
                    
                    tblName = ""
                    colmuns = []
                    types = []
                    is_nullables = []
                    aliases = []
                    pk = ""
                    
                    if ky == "name":
                        pass
                    elif ky == "db_file_path":
                        pass
                    elif ky == "column":
                        pass
                    elif ky == "type":
                        pass
                    elif ky =="is_nullable":
                        pass
                    elif ky == "alias":
                        pass
                    elif ky == "primary_key":
                        pass
                    
                    
            #このデータベースに所属するテーブルのセクションか確認
            if "[table]" in section_strs and f"owner={self.name}" in section_strs:
                pass
                    
                
                
      
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
