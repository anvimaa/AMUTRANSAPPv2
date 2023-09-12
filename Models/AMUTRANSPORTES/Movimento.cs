using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMUTRANSAPP.Models.AMUTRANSPORTES
{
    [Table("Movimento", Schema = "dbo")]
    public partial class Movimento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime DataEntrada { get; set; }

        [Required]
        public string Assunto { get; set; }

        [Required]
        public string Proveniencia { get; set; }

        public string Despacho { get; set; }

        public string Oficio { get; set; }

        public DateTime? DataResposta { get; set; }

        public string TipoMovimento { get; set; }

        public string OBS { get; set; }

    }
}