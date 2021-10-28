import column

class Table:
    
    def __init__(self,name,db_file_path):
        self.error = ""
        self.msg = ""
        
        self.name = name
        self.db_file_path = db_file_path
        
        self.columns = {}
        self.data = []
        
        
    #列情報を設定する
    def set_columns(self,names,types,is_nullables,aliases,primary_key):
        #各要素の数チェック
        if len(names) != len(types) or len(names) != len(isNullables) or len(names) != len(alias):
            self.error = "列を設定するための各要素の数が一致しません。\n"
            return False
        #typesチェック
        for tp in types:
            if tp not in ["int","string","date"]:
                self.error = f"型名:{tp}は有効でない値です。\n"
                return False
        #pkチェック
        if primary_key not in names:
            self.error = f"プライマリーキー:{primary_key}は有効でない値です。\n"
            return False
        
        #列設定
        //設定
        try:
            for i in range(len(names)):
                columns[names[i]] = column.Column(names[i],types[i],is_nullables[i],aliases[i],i)                
            return True
        except Exception as ex:
            self.error = "列の設定に失敗しました。\n"+str(ex)+"\n"
            return False
    
    #データを設定する   
    def set_data():
        if os.path.exists(self.db_file_path):
            return False
        
        try:
            with open(self.db_file_path,"r",encoding="utf-8") as f:
                for line in f:
                    data.append(line.replace("\n","").split(","))
        except Exception as ex:
            self.error = f"データファイル:{self.db_file_path}の読み込みに失敗しました。\n{ex}\n"
            return False
