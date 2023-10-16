using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMUTRANSAPP.Models.AMUTRANSPORTES
{
    [Table("CarroPreso", Schema = "dbo")]
    public partial class CarroPreso
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Matricula { get; set; }

        public string Marca { get; set; }

        public string Modelo { get; set; }

        public string Cor { get; set; }

        public string LocalEncontrado { get; set; }

        public string LocalColocado { get; set; }

        public string Estado { get; set; }

        public string Obs { get; set; }

        public DateTime? DataEntrada { get; set; }

        public DateTime? DataSaida { get; set; }

    }
}