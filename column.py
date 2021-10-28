class Column:
    
    def __init__(self,name,type,is_nullable,alias,index):
        self.name = name
        self.type = type
        self.is_nullable = is_nullable
        self.alias = alias
        self.index = index
