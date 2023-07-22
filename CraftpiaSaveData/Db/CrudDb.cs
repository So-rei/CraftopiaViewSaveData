using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using static System.Data.Entity.Infrastructure.Design.Executor;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;

namespace CraftpiaViewSaveData
{
    public static class CrudDb
    {

        public static List<ClassDb> Read(string dbpath = "")
        {
            // コネクション  
            var ret = new List<ClassDb>();
            using (var con = new SQLiteConnection("Data Source=" + dbpath))
            {
                con.Open();
                using (var command = con.CreateCommand())
                {
                    // usersテーブルの作成
                    //command.CommandText = @"create table users(id int, name varchar(10), age int)";
                    //command.ExecuteNonQuery();

                    // データの挿入
                    //command.CommandText = @"
                    //INSERT INTO users(id, name, age) VALUES(1, 'Mike', 30);
                    //INSERT INTO users(id, name, age) VALUES(2, 'Lisa', 24);
                    //INSERT INTO users(id, name, age) VALUES(3, 'Taro', 35);";
                    //command.ExecuteNonQuery();

                    // SELECT文の実行
                    command.CommandText = "select * from " + CommonConst.CraftpiaTableName + ";";
                    using (var reader = command.ExecuteReader())
                    {
                        // 1行ずつデータを取得
                        while (reader.Read())
                        {
                            ret.Add(new ClassDb(reader["id"].ToString(), reader["value"].ToString()));
                        }
                    }
                }

                con.Close();
            }
            return ret;
        }

        public static bool Update(string dbpath, List<ClassDb> rows)
        {
            try
            {
                // コネクション  
                using (var con = new SQLiteConnection("Data Source=" + dbpath))
                {
                    con.Open();
                    foreach (var row in rows)
                    {
                        //各Update
                        using (var command = con.CreateCommand())
                        {
                            // データの挿入
                            command.CommandText = @" UPDATE " + CommonConst.CraftpiaTableName + " SET " +
                                                   " value = " + row.value +
                                                   " WHERE " + "id = " + row.id + ";";

                            command.ExecuteNonQuery();
                        }
                    }
                    con.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
