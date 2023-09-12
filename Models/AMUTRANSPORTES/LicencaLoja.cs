using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMUTRANSAPP.Models.AMUTRANSPORTES
{
    [Table("LicencaLoja", Schema = "dbo")]
    public partial class LicencaLoja
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int EmpresaId { get; set; }

        [Required]
        public DateTime DataValidade { get; set; }

        [Required]
        public DateTime DataCadastro { get; set; }

        [Required]
        public decimal Valor { get; set; }

        public string Situacao { get; set; }

        [Required]
        public int NumLicenca { get; set; }

        public int? Tipo { get; set; }

        public string TipoLicenca { get; set; }

        public string LocalAtividade { get; set; }

        public Empresa Empresa { get; set; }

    }
}