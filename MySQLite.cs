using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Drawing;
using System.Data;
using System.Windows.Forms.VisualStyles;
using System.Windows.Forms;

namespace Movie
{
    internal class MySQLite
    {
        public readonly SQLiteConnection conn;
        public readonly SQLiteConnection con;
        
        public string Name { get;}
        public string DbPath { get;}        
        public string Error { get; }
        public string Msg { get; }

        public SQLiteDataAdapter Adapt;
        
        public DataTable Data;
        public int MainTableDataCount = 0;

        public List<string> TableNames = new List<string>();
        public Dictionary<string, List<string>> ColNames = new Dictionary<string, List<string>>();
        public Dictionary<string, List<string>> TypeNames = new Dictionary<string, List<string>>();
        public Dictionary<string, List<string>> NullableValues = new Dictionary<string, List<string>>();
        public Dictionary<string, string> PKNames = new Dictionary<string,string>();

        private readonly List<string> selectOperands =  new List<string> {"=",">=",">","<=","<" ,"<>","!=","like","glob"};
        private readonly List<string> andOr = new List<string> { "and", "AND", "or", "OR" };

        //コンストラクタ
        public MySQLite(string dbPath)
        {            
            Name = Path.GetFileNameWithoutExtension(dbPath);
            DbPath = dbPath;
            conn = new SQLiteConnection("Data Source=" + DbPath + ";");
            conn.Open();
            getTableInfo();
            Data = GetAllData(Name);
            MainTableDataCount = Data.Rows.Count;
        }

        #region 基本操作       
        //接続解除
        public void Close()
        {
            conn.Close();
            conn.Dispose();
        }

        //テーブル情報取得
        //１：テーブル一覧を取得しテーブル名をTableNamesにLIstとして格納
        //２：テーブル名をループし各テーブル毎の情報を取得
        private bool getTableInfo()
        {
            //テーブル名一覧を取得
            SQLiteCommand cmd = new SQLiteCommand($"SELECT name FROM sqlite_master WHERE type = 'table'", conn);
            try
            {
                SQLiteDataReader reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    Error = $"テーブル名の情報取得に失敗しました。\r\n";
                    return false;
                }

                DataTable dt = new DataTable();
                dt.Load(reader);

                TableNames = new List<string>(dt.AsEnumerable().Select(row => row[0].ToString()).ToList());                
            }
            catch (Exception ex)
            {
                Error = $"テーブル名の情報取得に失敗しました\r\n{ex.Message}\r\n";
                return false;
            }

            //テーブル毎に列名、型、NULL、プライマリーキー情報を取得
            foreach (string tblName in TableNames)
            {
                cmd = new SQLiteCommand($"PRAGMA TABLE_INFO ('{tblName}')", conn);
                try
                {
                    SQLiteDataReader reader = cmd.ExecuteReader(); 
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    ColNames[tblName] = new List<string>();
                    TypeNames[tblName] = new List<string>();
                    NullableValues[tblName] = new List<string>();
                    //0:rcid 1:colname 2:type 3:null 4:? 5:primaryley(1=pk)
                    foreach (DataRow row in dt.Rows)
                    {
                        ColNames[tblName].Add(row[1].ToString());
                        TypeNames[tblName].Add(row[2].ToString());
                        NullableValues[tblName].Add(row[3].ToString());
                        if (row[5].ToString() == "1")
                        {
                            PKNames[tblName] = row[1].ToString();
                        }                        
                    }

                }
                catch (Exception ex)
                {
                    Error = $"{tblName}テーブルの情報取得に失敗しました\r\n{ex.Message}\r\n";
                    return false;
                }
            }            
            return true;
        }

        //テーブル作成
        public bool CreateTable(string name, List<string> colNames, List<string> types, List<string> nullables, string pkCol)
        {
            Error = Msg = "";
            string sqlStr = "";

            //colNames、types、nullables、aliasesのデータ数が同一ではない場合エラーとする
            if (colNames.Count != types.Count || colNames.Count != nullables.Count)
            {
                Error = $"列に関する情報リストの要素数が異なります。";
                return false;
            }

            sqlStr = $"CREATE TABLE {name} (";

            for (int i = 0; i < colNames.Count; i++)
            {
                string nullable = (nullables[i] == "NULL") ? "" : nullables[i];
                sqlStr += $" {colNames[i]} {types[i]} {nullable} ";

                if (pkCol == colNames[i])
                {
                    sqlStr += "PRIMARY KEY ";
                }
                sqlStr += ",";
            }

            sqlStr = sqlStr.Remove(sqlStr.Length - 1);

            sqlStr += " )";

            SQLiteCommand cmd = new SQLiteCommand(sqlStr, conn);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Error = $"{name}テーブル作成に失敗しました。\r\n{ex.Message}";
                return false;
            }

            Msg = $"{Name}データベースに '{name}' テーブルを作成しました！。\r\n{sqlStr}";
            return true;
        }
                
        //テーブルデータを全件取得
        public DataTable GetAllData(string tblName,string col = "id",string sortOrder = "asc")
        {
            Msg = Error = "";
            string error = $"{tblName}テーブルのデータを取得出来ませんでした。\r\n";

            DataTable dt = null;

            //テーブルのチェック
            if (!TableNames.Contains(tblName))
            {
                Error += error;
                return dt;
            }

            try
            {
                Adapt = new SQLiteDataAdapter($"select * from {tblName} order by {col} {sortOrder}", conn);
                dt = new DataTable(tblName);
                Adapt.Fill(dt);

            }
            catch (Exception ex)
            {
                Error = $"{error}{ex.Message}";
            }

            return dt;
        }

        //データ挿入
        public bool InsertDataFromCSV(string tblName, string csvPath)
        {
            Msg = Error = "";

            string error = $"{tblName}テーブルにデータを登録出来ませんでした。\r\n";

            //まずテーブルの列名リストを取得
            if (!TableNames.Contains(tblName))
            {
                Error += error;
                return false;
            }

            var data = new List<List<string>>();
            if (!File.Exists(csvPath))
            {
                Error += error + $"{csvPath}は存在しません。";
                return false;
            }

            using (StreamReader sr = new StreamReader(csvPath, Encoding.GetEncoding("UTF-8")))
            {
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    var buff = line.Split(',');
                    data.Add(buff.ToList());
                }
            }

            int count = 0;
            foreach (List<string> row in data)
            {
                string sql = $"insert into {tblName} ({string.Join(",", ColNames[tblName])}) values (";
                count++;

                foreach (string val in row)
                {
                    sql += $"'{val}',";
                }

                sql = sql.Remove(sql.Length - 1) + ");";

                try
                {
                    SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Error += "id" + count.ToString() + " → " + ex.Message + "\r\n";
                }


            }

            Msg = $"{count}件のデータを挿入しました。\r\n";
            if (Error != "")
            {
                Msg += "挿入できなかったデータ以下の通りです。\r\n" + Error;
            }
            return true;
        }

        //データ挿入（新規登録）
        public bool Insert(string tblName,List<string> newData)
        {
            string error = $"{tblName}テーブル・データ登録エラー : MySQL.Insert()\r\n";
            string msg = $"{tblName}データベーステーブル・データ登録成功 : MySQL.Insert()\r\n";
            Error = "";
            Msg = "";
            //データ検証
            if (!ValidateNewData(tblName,newData))
            {
                Error = error + Error;
                return false;
            }

            string sql = $"insert into {tblName} ({string.Join(",", ColNames[tblName])}) values (";
            foreach (string dt in newData)//'のエスｋ－プ処理必要
            {
                sql += $"'{dt}',";
            }
            sql = sql.Remove(sql.Length - 1) + ");";

            try
            {
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Error = error+ex.Message;
                return false;
            }
            return true;
        }

        //データ更新（編集）
        public bool Update(string tblName, List<string> newData)
        {
            return true;
        }

        //データ削除
        public bool Delete(string tblName,string pk)
        {
            return true;
        }

        #endregion

        public bool ValidateNewData(string tblName, List<string> newData)
        {
            string err = $">>データ整合性エラー : MySQLite.ValidateNewData()\r\n";

            //データ要素数チェック
            if (newData.Count != ColNames[tblName].Count)
            {
                Error += $"{err}>>新規データの要素数:{newData.Count}が不正です。\r\n";
                return false;
            }

            string valiStr = "";
            int tyrInt;
            DateTime tryDate;
            for (int i = 0; i < ColNames[tblName].Count; i++)
            {
                if (TypeNames[tblName][i] == "INTEGER" && !int.TryParse(newData[i], out tyrInt))
                {
                    valiStr += $">>型エラー : {ColNames[tblName][i]}列は整数値を設定してください。\r\n";
                }
                else if (ColNames[tblName][i] == "date" && !DateTime.TryParse(newData[i], out tryDate))
                {
                    valiStr += $">>型エラー : date列は日付値(yyyy/MM/dd)を設定してください。\r\n";
                }
                //'1'の場合はNOT NULL
                if (NullableValues[tblName][i] == "1" && newData[i].Replace(" ", "").Replace("　", "") == "")
                {
                    valiStr += $">>空白値許容エラー : {ColNames[tblName][i]}列は空白値不許容です、値を設定してください。\r\n";
                }
            }


            if (valiStr != "")
            {
                Error += err + valiStr;
                return false;
            }

            return true;
        }


        public DataTable Search(string tblName,string col,string ope, string word)
        {
            Error = "";
            string error = "検索に失敗しました。\r\n";

            DataTable dt = new DataTable();
            string sql = CreateSelectSql(tblName, new List<string> { col }, new List<string> { ope }, new List<string> { word });
            if (sql == null)
            {
                Error = error + Error;
                return null;
            }

            //検索
            try
            {
                SQLiteDataAdapter adpt = new SQLiteDataAdapter(sql, conn);
                dt = new DataTable();
                adpt.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                Error = $"{error}{ex.Message}";
                return null;
            }
        }

        public string CreateSelectSql(string tblName, List<string> cols, List<string> opes, List<string> words, string andOr = "or")
        {
            string error = "検索sql文字列の作成に失敗しました。\r\n";
            string sql = $"select * from {tblName} where";

            //テーブル名のチェック
            if (!TableNames.Contains(tblName))
            {
                Error += error + $"{tblName}テーブルは存在しません。\r\n";
                return null;
            }
            //引数のcols、opes、wordsの要素数が一致しているか？
            if (cols.Count != opes.Count || cols.Count != words.Count)
            {
                Error += error + $"引数に指定された要素の数が一致しません。\r\n";
                return null;
            }
            //引数の要素の内容をチェック
            for (int i = 0; i < cols.Count; i++)
            {
                if (!ColNames[tblName].Contains(cols[i]) && cols[i] != "")
                {
                    Error += error + $"{cols[i]}という列は存在しません。\r\n";
                    return null;
                }
                if (!selectOperands.Contains(opes[i]))
                {
                    Error += error + $"'{opes[i]}' は無効な比較演算子です。\r\n";
                    return null;
                }
            }
            //andorチェック
            if (!this.andOr.Contains(andOr))
            {
                Error += error + $"'{andOr}' は無効な条件比較子です。\r\n";
                return null;
            }


            //生成

            List<string> sqlList = new List<string>();
            for (int i = 0; i < cols.Count; i++)
            {
                string wd = words[i];
                if (opes[i] == "glob" || opes[i] == "GLOB")
                {
                    wd = $"*{words[i]}*";
                }
                else if (opes[i] == "like" || opes[i] == "LIKE")
                {
                    wd = $"%{words[i]}%";
                }

                //colsが""の場合全列対象
                if (cols[i] == "")
                {
                    string allColSql = " (";
                    foreach (string col in ColNames[tblName])
                    {
                        allColSql += $" {col} {opes[i]} '{wd}' or";
                    }
                    sqlList.Add(allColSql.Remove(allColSql.Length - 2) + ") ");
                }
                else
                {
                    sqlList.Add($" ({cols[i]} {opes[i]} '{wd}') ");
                }

            }
            sql += sqlList.Count == 1 ? sqlList[0] : string.Join(andOr, sqlList);
            return sql;
        }

        
        public string CreateNewPK(string tblName,int addCount = 1)
        {
            string error = $"{tblName}テーブルの主キーの生成に失敗しました。\r\n";
                        
            DataTable dt = new DataTable();
            try
            {
                Adapt = new SQLiteDataAdapter($"select * from {tblName}", conn);
                Adapt.Fill(dt);
            }
            catch (Exception ex)
            {
                Error = $"{error}{ex.Message}";
                return null;
            }

            string newPK = tblName + "_" + (dt.Rows.Count + addCount).ToString("000000");
            try
            {
                Adapt = new SQLiteDataAdapter($"select * from {tblName} where {PKNames[tblName]} = '{newPK}'", conn);
                dt = new DataTable();
                Adapt.Fill(dt);
            }
            catch (Exception ex)
            {
                Error = $"{error}{ex.Message}";
                return null;
            }

            if (dt.Rows.Count > 0)
            {                
                CreateNewPK(tblName,addCount+1);
            }
            
            return newPK;
        }
        //////////////////////////////////////////////////////////////////////////////////////////
        public int GetDataCount(string tblName)
        {
            try
            {
                DataTable dt = new DataTable();
                Adapt = new SQLiteDataAdapter($"select * from {tblName}", conn);
                Adapt.Fill(dt);
                return dt.Rows.Count;
            }
            catch (Exception ex)
            {
                Error = $"{ex.Message}";
                return -1;
            }
        }
        //////////////////////////////////////////////////////////////////////////////////////////
        public void ExecuteNonQuery(string sql)
        {
            SQLiteCommand sqlCom = new SQLiteCommand(sql, conn);
            sqlCom.ExecuteNonQuery();
        }

        public SQLiteDataAdapter ExecuteQueryAdapter(string sql)
        {
            SQLiteDataAdapter Adapter = new SQLiteDataAdapter(sql, conn);
            return Adapter;
        }

        public SQLiteDataReader ExecuteQueryReader(string sql)
        {
            SQLiteCommand sqlCom = new SQLiteCommand(sql, conn);
            SQLiteDataReader reader = sqlCom.ExecuteReader();

            return reader;
        }
    }
}
