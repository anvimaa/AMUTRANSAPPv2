using AMUTRANSAPP.Data;
using AMUTRANSAPP.Services;
using AspNetCore.Reporting;
using BlazorAppReports.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace AMUTRANSAPP.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LojasController : ControllerBase
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly AMUTRANSPORTESContext _context;
    private readonly AMUTRANSPORTESService _service;
    private readonly ApplicationServices _applicationServices;

    LojaServices lojaServices = new LojaServices();
    public LojasController(
        IWebHostEnvironment webHostEnvironment,
        AMUTRANSPORTESContext context,
        AMUTRANSPORTESService service,
        ApplicationServices applicationServices)
    {
        _webHostEnvironment = webHostEnvironment;
        _context = context;
        _service = service;
        _applicationServices = applicationServices;
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    [HttpGet]
    [Route("LojaReport/{id:int}")]
    public async Task<IActionResult> LojaReport(int id)
    {
        var licenca = await _service.GetLicencaLojaById(id);

        var mimetype = "";
        int extension = 1;
        var reportFile = "LojaReport.rdlc";

        switch (licenca.Tipo)
        {
            case 0:
                reportFile = "LojaReport.rdlc";
                break;
            case 1:
                reportFile = "EscolaReport.rdlc";
                break;
            case 2:
                reportFile = "OficinaReport.rdlc";
                break;
            case 3:
                reportFile = "EstacaoReport.rdlc";
                break;
            case 4:
                reportFile = "RecachotagemReport.rdlc";
                break;
            default:
                break;
        }

        var path = Path.Combine(_webHostEnvironment.WebRootPath, "Reports", reportFile);

        var dtHoje = licenca.DataValidade.AddYears(-1);
        var hoje = $"{dtHoje.Day} de {_applicationServices.GetMonths()[dtHoje.Month]} de {dtHoje.Year}";

        var dtValidade = licenca.DataValidade;
        var validade = $"{dtValidade.Day} de {_applicationServices.GetMonths()[dtValidade.Month]} de {dtValidade.Year}";

        string numlicenca = licenca.NumLicenca.ToString().PadLeft(3, '0');

        Dictionary<string, string> parameters = new Dictionary<string, string>
        {
            { "param", "Loja Reports" },
            { "tipolicenca", licenca.TipoLicenca.ToUpper() },
            {"num", numlicenca},
            {"ano", dtHoje.Year.ToString()},
            {"empresa", licenca.Empresa.Designacao.ToUpper()},
            {"nif", licenca.Empresa.NIF.ToUpper()},
            {"sede", licenca.LocalAtividade.ToUpper()},
            {"atividade", _applicationServices.GetAtividades()[licenca.Tipo.Value].ToUpper()},
            {"validade", validade},
            {"hoje", hoje},
            {"tipo", licenca.Tipo.ToString()}
        };

        LocalReport localReport = new LocalReport(path);

        var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimetype);

        return File(result.MainStream, "application/pdf");
    }
}
