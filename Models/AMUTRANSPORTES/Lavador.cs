using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMUTRANSAPP.Models.AMUTRANSPORTES
{
    [Table("Lavador", Schema = "dbo")]
    public partial class Lavador
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int EstacaoServicoId { get; set; }

        public string Nome { get; set; }

        public string BI { get; set; }

        public string Telefone { get; set; }

        public string Funcao { get; set; }

        public EstacaoServico EstacaoServico { get; set; }

    }
}