using AMUTRANSAPP.Data;
using AMUTRANSAPP.Models;
using AMUTRANSAPP.Models.AMUTRANSPORTES;
using Microsoft.EntityFrameworkCore;

namespace AMUTRANSAPP.Services;

public class ApplicationServices
{
    private readonly AMUTRANSPORTESContext _context;

    public ApplicationServices(AMUTRANSPORTESContext context)
    {
        _context = context;
    }

    public async Task<List<HomeInfoModel>> GetInfoModels()
    {
        var licencas = await _context.LicencaLojas.ToListAsync();

        int licLoja = 0, licEscola = 0, licOficina = 0, licEstacao = 0, licReca = 0,
            licLojaLic = 0, licEscolaLic = 0, licOficinaLic = 0, licEstacaoLic = 0, licRecaLic = 0;

        foreach (var licenca in licencas)
        {
            if (licenca.Tipo == 0)
            {
                licLoja++;
                if (licenca.Situacao == "Emitido")
                    licLojaLic++;
            }
            else if (licenca.Tipo == 1)
            {
                licEscola++;
                if (licenca.Situacao == "Emitido")
                    licEscolaLic++;
            }
            else if (licenca.Tipo == 2)
            {
                licOficina++;
                if (licenca.Situacao == "Emitido")
                    licOficinaLic++;
            }
            else if (licenca.Tipo == 3)
            {
                licEstacao++;
                if (licenca.Situacao == "Emitido")
                    licEstacaoLic++;
            }
            else if (licenca.Tipo == 4)
            {
                licReca++;
                if (licenca.Situacao == "Emitido")
                    licRecaLic++;
            }
        }

        return new()
        {
            new HomeInfoModel(){Title="Loja de Peças", TotalGeral=licLoja,TotalValido=licLoja,TotalInvalido=licLojaLic},
            new HomeInfoModel(){Title="Escola de Condução", TotalGeral=licEscola,TotalValido=licEscola,TotalInvalido=licEscolaLic},
            new HomeInfoModel(){Title="Oficinas", TotalGeral=licOficina,TotalValido=licOficina,TotalInvalido=licOficinaLic},
            new HomeInfoModel(){Title="Estação de Serviço", TotalGeral=licEstacao,TotalValido=licEstacao,TotalInvalido=licEstacaoLic},
            new HomeInfoModel(){Title="Recauchotagem", TotalGeral=licReca,TotalValido=licReca,TotalInvalido=licRecaLic},
        };
    }

    public async Task<List<LicencaLoja>> GetLicencasExpiradas()
    {
        var licencas = await _context.LicencaLojas.Where(i =>
        i.DataValidade < DateTime.Now).ToListAsync();
        return licencas;
    }

    public List<string> GetLicencas()
    {
        return new List<string>() { "Loja de Venda de Peças", "Escola de Condução", "Oficina Mecânica", "Estação de Serviço", "Recauchutagem", "Socateiras" };
    }

    public List<string> GetLicencasTipo()
    {
        return new List<string>() { "Todos", "Loja de Venda de Peças", "Escola de Condução", "Oficina Mecânica", "Estação de Serviço", "Recauchutagem", "Socateiras" };
    }

    public List<string> GetEstados()
    {
        return new List<string>() { "Todos", "Em Espera", "Emitido", "Expirado" };
    }

    public Dictionary<int, string> GetMonths()
    {
        return new Dictionary<int, string>() {
            { 1, "Janeiro" },
            { 2, "Fevereiro" },
            { 3, "Março" },
            { 4, "Abril" },
            { 5, "Maio" },
            { 6, "Junho" },
            { 7, "Julho" },
            { 8, "Agosto" },
            { 9, "Setembro" },
            { 10, "Outubro" },
            { 11, "Novembro" },
            { 12, "Dezembro" },
        };
    }

    public Dictionary<int, string> GetAtividades()
    {
        return new Dictionary<int, string>() {
            { 0, "Venda de Peças Sobressalentes" },
            { 1, "Escola de Condução" },
            { 2, "Oficina Mecânica" },
            { 3, "Estação de Serviço" },
            { 4, "Recauchutagem" },
            { 5, "Recolha de Peças Sobressalentes" },
        };
    }
}
