using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using AMUTRANSAPP.Data;

namespace AMUTRANSAPP.Controllers
{
    public partial class ExportAMUTRANSPORTESController : ExportController
    {
        private readonly AMUTRANSPORTESContext context;
        private readonly AMUTRANSPORTESService service;

        public ExportAMUTRANSPORTESController(AMUTRANSPORTESContext context, AMUTRANSPORTESService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/AMUTRANSPORTES/empresas/csv")]
        [HttpGet("/export/AMUTRANSPORTES/empresas/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportEmpresasToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetEmpresas(), Request.Query), fileName);
        }

        [HttpGet("/export/AMUTRANSPORTES/empresas/excel")]
        [HttpGet("/export/AMUTRANSPORTES/empresas/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportEmpresasToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetEmpresas(), Request.Query), fileName);
        }

        [HttpGet("/export/AMUTRANSPORTES/estacaoservicos/csv")]
        [HttpGet("/export/AMUTRANSPORTES/estacaoservicos/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportEstacaoServicosToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetEstacaoServicos(), Request.Query), fileName);
        }

        [HttpGet("/export/AMUTRANSPORTES/estacaoservicos/excel")]
        [HttpGet("/export/AMUTRANSPORTES/estacaoservicos/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportEstacaoServicosToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetEstacaoServicos(), Request.Query), fileName);
        }

        [HttpGet("/export/AMUTRANSPORTES/fiscals/csv")]
        [HttpGet("/export/AMUTRANSPORTES/fiscals/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportFiscalsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetFiscals(), Request.Query), fileName);
        }

        [HttpGet("/export/AMUTRANSPORTES/fiscals/excel")]
        [HttpGet("/export/AMUTRANSPORTES/fiscals/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportFiscalsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetFiscals(), Request.Query), fileName);
        }

        [HttpGet("/export/AMUTRANSPORTES/lavadors/csv")]
        [HttpGet("/export/AMUTRANSPORTES/lavadors/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLavadorsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetLavadors(), Request.Query), fileName);
        }

        [HttpGet("/export/AMUTRANSPORTES/lavadors/excel")]
        [HttpGet("/export/AMUTRANSPORTES/lavadors/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLavadorsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetLavadors(), Request.Query), fileName);
        }

        [HttpGet("/export/AMUTRANSPORTES/licencalojas/csv")]
        [HttpGet("/export/AMUTRANSPORTES/licencalojas/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLicencaLojasToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetLicencaLojas(), Request.Query), fileName);
        }

        [HttpGet("/export/AMUTRANSPORTES/licencalojas/excel")]
        [HttpGet("/export/AMUTRANSPORTES/licencalojas/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportLicencaLojasToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetLicencaLojas(), Request.Query), fileName);
        }

        [HttpGet("/export/AMUTRANSPORTES/movimentos/csv")]
        [HttpGet("/export/AMUTRANSPORTES/movimentos/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportMovimentosToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetMovimentos(), Request.Query), fileName);
        }

        [HttpGet("/export/AMUTRANSPORTES/movimentos/excel")]
        [HttpGet("/export/AMUTRANSPORTES/movimentos/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportMovimentosToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetMovimentos(), Request.Query), fileName);
        }

        [HttpGet("/export/AMUTRANSPORTES/paragems/csv")]
        [HttpGet("/export/AMUTRANSPORTES/paragems/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportParagemsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetParagems(), Request.Query), fileName);
        }

        [HttpGet("/export/AMUTRANSPORTES/paragems/excel")]
        [HttpGet("/export/AMUTRANSPORTES/paragems/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportParagemsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetParagems(), Request.Query), fileName);
        }

        [HttpGet("/export/AMUTRANSPORTES/carropresos/csv")]
        [HttpGet("/export/AMUTRANSPORTES/carropresos/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCarroPresosToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetCarroPresos(), Request.Query), fileName);
        }

        [HttpGet("/export/AMUTRANSPORTES/carropresos/excel")]
        [HttpGet("/export/AMUTRANSPORTES/carropresos/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportCarroPresosToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetCarroPresos(), Request.Query), fileName);
        }
    }
}
