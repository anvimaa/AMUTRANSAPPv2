using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMUTRANSAPP.Models.AMUTRANSPORTES
{
    [Table("Empresa", Schema = "dbo")]
    public partial class Empresa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Designacao { get; set; }

        [Required]
        public string NIF { get; set; }

        [Required]
        public string Sede { get; set; }

        [Required]
        public string Responsavel { get; set; }

        [Required]
        public string Contacto { get; set; }

        [Required]
        public string Atividade { get; set; }

        public DateTime? DataCadastro { get; set; }

        public ICollection<LicencaLoja> LicencaLojas { get; set; }

    }
}