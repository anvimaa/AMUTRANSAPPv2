using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMUTRANSAPP.Models.AMUTRANSPORTES
{
    [Table("Fiscal", Schema = "dbo")]
    public partial class Fiscal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ParagemId { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string BI { get; set; }

        public string Telefone { get; set; }

        public string Funcao { get; set; }

        public Paragem Paragem { get; set; }

    }
}