using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppGas.Models
{
    [Table("Task")]
    public class GasModel
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Company { get; set; }

        public string Name { get; set; }

        public double pRoja { get; set; }

        public double pVerde { get; set; }

        public double pDiesel { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        public string ImageBase64 { get; set; }

        //public bool Done { get; set; }
    }
}
