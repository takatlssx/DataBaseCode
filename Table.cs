using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DataBaseManager
{
    public class Table
    {
        public string Error = "";
        public string Msg = "";
        public string SearchMsg = "";


        public string Name { get; private set; }
        public string DbFilePath { get; private set; }
        
        public Dictionary<string, Column> Columns { get; private set; }

        public string PrimaryKey { get; private set; }
        
        public List<List<string>> Data { get; private set; }


        public Table(string name, string dbFilePath)
        {
            Name = name;
            DbFilePath = dbFilePath;
        }


        //列情報（Columns）を設定
        public bool SetColumns(string[] names, string[] types, bool[] isNullables, string[] alias, string primaryKey)
        {
            Columns = new Dictionary<string, Column>();

            //各要素の数チェック
            if ((names.Count() != types.Count()) || (names.Count() != isNullables.Count()) || (names.Count() != alias.Count()))
            {
                Error = "列を設定するための各要素の数が一致しません。\r\n";
                return false;
            }
            //typesチェック
            foreach (string type in types)
            {
                if (!(new List<string> { "int","string","date" }).Contains(type))
                {
                    Error = $"型名:{type}は有効でない値です。\r\n";
                    return false;
                }
            }
            //primaryKeyチェック
            if (!names.Contains(primaryKey))
            {
                Error = $"プライマリーキー:{primaryKey}は有効でない値です。\r\n";
                return false;
            }

            //設定
            try
            {
                for (int i = 0; i < names.Count(); i++)
                {
                    Columns[names[i]] = new Column(names[i], types[i], isNullables[i], alias[i], i);
                }

                PrimaryKey = primaryKey;
            }
            catch (Exception ex)
            {
                Error = $"列設定に失敗しました。\r\n{ex.ToString()}\r\n";
                return false;
            }
            return true;
        }


        public bool SetData()
        {            
            Data = new List<List<string>>();

            if (!File.Exists(DbFilePath))
            {
                return false;
            }

            using (StreamReader sr = new StreamReader(DbFilePath))
            {
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    var buff = line.Split(',');
                    Data.Add(buff.ToList());
                }
            }
            return true;
        }
    }
}
