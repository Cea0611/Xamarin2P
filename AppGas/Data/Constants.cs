using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AppGas.Data
{
    public class Constants
    {
        //Constante para abrir o crear archivo SQLite en modo lectura-escritura
        public const SQLite.SQLiteOpenFlags Flags = SQLite.SQLiteOpenFlags.ReadWrite | SQLite.SQLiteOpenFlags.Create | SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath
        {
            get
            {
                string basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath, "AppGas.db3");
            }
        }
    }
}
