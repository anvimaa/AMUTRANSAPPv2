using AMUTRANSAPP.Data;
using AMUTRANSAPP.Models.AMUTRANSPORTES;
using System.Data;

namespace BlazorAppReports.Data;

public class LojaServices
{
    public DataTable GetLojasInfo(List<Paragem> paragems)
    {
        var dt = new DataTable();
        dt.Columns.Add("Id");
        dt.Columns.Add("Nome");
        DataRow dr;
        

        foreach (var paragem in paragems)
        {
            dr = dt.NewRow();
            dr["Id"] = paragem.Id;
            dr["Nome"] = paragem.Nome;
            dt.Rows.Add(dr);
        }

        //for (int i = 1; i <= 50; i++)
        //{
        //    dr = dt.NewRow();
        //    dr["Id"] = i;
        //    dr["Nome"] = $"Loja {i}";
        //    dt.Rows.Add(dr);
        //}
        return dt;
    }
}
