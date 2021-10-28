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
        self.relational_tables = {}
        self.main_table = table.Table
    
    
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
                        self.root_dir = vl
                    elif ky == "file_dir":
                        self.file_dir = vl
                    elif ky == "backup_dir":
                        self.backup_dir = vl
                    elif ky == "member":
                        self.table_names = vl.split(",")
                    elif ky =="main":
                        self.main_table_name = vl
                    elif ky == "relatinal":
                        self.relational_table_names = vl.split(",")
                        
            #このデータベース所属のテーブルセクションか確認     
            if "[table]" in section_strs and f"owner={self.name}" in section_strs:
                for line in section_strs:
                    ky = "" if line == "[table]" else line.split("=")[0]
                    vl = "" if line == "[table]" else line.split("=")[1]
                    
                    tbl_name = ""
                    colmuns = []
                    types = []
                    is_nullables = []
                    aliases = []
                    pk = ""
                    
                    if ky == "name":
                        tbl_name = vl
                    elif ky == "db_file_path":
                        self.tables[tbl_name] = table.Table(tbl_name,vl)
                    elif ky == "column":
                        columns = vl.split(",")
                    elif ky == "type":
                        types = vl.split(",")
                    elif ky =="is_nullable":
                        buff = vl.split(",")
                        for bf in buff:
                            is_nullables.append(bf == "true")
                    elif ky == "alias":
                        aliases = vl.split(",")
                    elif ky == "primary_key":
                        self.tables[tbl_name].set_columns(columns,types,isNullables,aliases,vl)
                        self.tables[tbl_name].set_data()
                    
                    if tbl_name == self.main_table_name:
                        self.main_table = self.tables[tbl_name]
                    elif tbl_name in self.relational_table_names:
                        self.relational_tables[tbl_name] = self.tables[tbl_name]
                        
                        
