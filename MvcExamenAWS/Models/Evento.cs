﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcExamenAWS.Models
{
    [Table("eventos")]
    public class Evento
    {
        [Key]
        [Column("idevento")]
        public int Id { get; set; }
        [Column("nombre")]
        public string Nombre { get; set; }
        [Column("artista")]
        public string Artista { get; set; }
        [Column("idcategoria")]
        public int IdCategoria { get; set; }
        [Column("imagen")]
        public string Imagen { get; set; }
    }
}
