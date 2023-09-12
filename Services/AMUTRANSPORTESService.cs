using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Radzen;

using AMUTRANSAPP.Data;

namespace AMUTRANSAPP
{
    public partial class AMUTRANSPORTESService
    {
        AMUTRANSPORTESContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly AMUTRANSPORTESContext context;
        private readonly NavigationManager navigationManager;

        public AMUTRANSPORTESService(AMUTRANSPORTESContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public void ApplyQuery<T>(ref IQueryable<T> items, Query query = null)
        {
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }
        }


        public async Task ExportEmpresasToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/amutransportes/empresas/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/amutransportes/empresas/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportEmpresasToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/amutransportes/empresas/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/amutransportes/empresas/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnEmpresasRead(ref IQueryable<AMUTRANSAPP.Models.AMUTRANSPORTES.Empresa> items);

        public async Task<IQueryable<AMUTRANSAPP.Models.AMUTRANSPORTES.Empresa>> GetEmpresas(Query query = null)
        {
            var items = Context.Empresas.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnEmpresasRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnEmpresaGet(AMUTRANSAPP.Models.AMUTRANSPORTES.Empresa item);
        partial void OnGetEmpresaById(ref IQueryable<AMUTRANSAPP.Models.AMUTRANSPORTES.Empresa> items);


        public async Task<AMUTRANSAPP.Models.AMUTRANSPORTES.Empresa> GetEmpresaById(int id)
        {
            var items = Context.Empresas
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetEmpresaById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnEmpresaGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnEmpresaCreated(AMUTRANSAPP.Models.AMUTRANSPORTES.Empresa item);
        partial void OnAfterEmpresaCreated(AMUTRANSAPP.Models.AMUTRANSPORTES.Empresa item);

        public async Task<AMUTRANSAPP.Models.AMUTRANSPORTES.Empresa> CreateEmpresa(AMUTRANSAPP.Models.AMUTRANSPORTES.Empresa empresa)
        {
            OnEmpresaCreated(empresa);

            var existingItem = Context.Empresas
                              .Where(i => i.Id == empresa.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Empresas.Add(empresa);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(empresa).State = EntityState.Detached;
                throw;
            }

            OnAfterEmpresaCreated(empresa);

            return empresa;
        }

        public async Task<AMUTRANSAPP.Models.AMUTRANSPORTES.Empresa> CancelEmpresaChanges(AMUTRANSAPP.Models.AMUTRANSPORTES.Empresa item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnEmpresaUpdated(AMUTRANSAPP.Models.AMUTRANSPORTES.Empresa item);
        partial void OnAfterEmpresaUpdated(AMUTRANSAPP.Models.AMUTRANSPORTES.Empresa item);

        public async Task<AMUTRANSAPP.Models.AMUTRANSPORTES.Empresa> UpdateEmpresa(int id, AMUTRANSAPP.Models.AMUTRANSPORTES.Empresa empresa)
        {
            OnEmpresaUpdated(empresa);

            var itemToUpdate = Context.Empresas
                              .Where(i => i.Id == empresa.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(empresa);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterEmpresaUpdated(empresa);

            return empresa;
        }

        partial void OnEmpresaDeleted(AMUTRANSAPP.Models.AMUTRANSPORTES.Empresa item);
        partial void OnAfterEmpresaDeleted(AMUTRANSAPP.Models.AMUTRANSPORTES.Empresa item);

        public async Task<AMUTRANSAPP.Models.AMUTRANSPORTES.Empresa> DeleteEmpresa(int id)
        {
            var itemToDelete = Context.Empresas
                              .Where(i => i.Id == id)
                              .Include(i => i.LicencaLojas)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnEmpresaDeleted(itemToDelete);


            Context.Empresas.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterEmpresaDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportEstacaoServicosToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/amutransportes/estacaoservicos/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/amutransportes/estacaoservicos/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportEstacaoServicosToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/amutransportes/estacaoservicos/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/amutransportes/estacaoservicos/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnEstacaoServicosRead(ref IQueryable<AMUTRANSAPP.Models.AMUTRANSPORTES.EstacaoServico> items);

        public async Task<IQueryable<AMUTRANSAPP.Models.AMUTRANSPORTES.EstacaoServico>> GetEstacaoServicos(Query query = null)
        {
            var items = Context.EstacaoServicos.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnEstacaoServicosRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnEstacaoServicoGet(AMUTRANSAPP.Models.AMUTRANSPORTES.EstacaoServico item);
        partial void OnGetEstacaoServicoById(ref IQueryable<AMUTRANSAPP.Models.AMUTRANSPORTES.EstacaoServico> items);


        public async Task<AMUTRANSAPP.Models.AMUTRANSPORTES.EstacaoServico> GetEstacaoServicoById(int id)
        {
            var items = Context.EstacaoServicos
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetEstacaoServicoById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnEstacaoServicoGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnEstacaoServicoCreated(AMUTRANSAPP.Models.AMUTRANSPORTES.EstacaoServico item);
        partial void OnAfterEstacaoServicoCreated(AMUTRANSAPP.Models.AMUTRANSPORTES.EstacaoServico item);

        public async Task<AMUTRANSAPP.Models.AMUTRANSPORTES.EstacaoServico> CreateEstacaoServico(AMUTRANSAPP.Models.AMUTRANSPORTES.EstacaoServico estacaoservico)
        {
            OnEstacaoServicoCreated(estacaoservico);

            var existingItem = Context.EstacaoServicos
                              .Where(i => i.Id == estacaoservico.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.EstacaoServicos.Add(estacaoservico);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(estacaoservico).State = EntityState.Detached;
                throw;
            }

            OnAfterEstacaoServicoCreated(estacaoservico);

            return estacaoservico;
        }

        public async Task<AMUTRANSAPP.Models.AMUTRANSPORTES.EstacaoServico> CancelEstacaoServicoChanges(AMUTRANSAPP.Models.AMUTRANSPORTES.EstacaoServico item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnEstacaoServicoUpdated(AMUTRANSAPP.Models.AMUTRANSPORTES.EstacaoServico item);
        partial void OnAfterEstacaoServicoUpdated(AMUTRANSAPP.Models.AMUTRANSPORTES.EstacaoServico item);

        public async Task<AMUTRANSAPP.Models.AMUTRANSPORTES.EstacaoServico> UpdateEstacaoServico(int id, AMUTRANSAPP.Models.AMUTRANSPORTES.EstacaoServico estacaoservico)
        {
            OnEstacaoServicoUpdated(estacaoservico);

            var itemToUpdate = Context.EstacaoServicos
                              .Where(i => i.Id == estacaoservico.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(estacaoservico);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterEstacaoServicoUpdated(estacaoservico);

            return estacaoservico;
        }

        partial void OnEstacaoServicoDeleted(AMUTRANSAPP.Models.AMUTRANSPORTES.EstacaoServico item);
        partial void OnAfterEstacaoServicoDeleted(AMUTRANSAPP.Models.AMUTRANSPORTES.EstacaoServico item);

        public async Task<AMUTRANSAPP.Models.AMUTRANSPORTES.EstacaoServico> DeleteEstacaoServico(int id)
        {
            var itemToDelete = Context.EstacaoServicos
                              .Where(i => i.Id == id)
                              .Include(i => i.Lavadors)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnEstacaoServicoDeleted(itemToDelete);


            Context.EstacaoServicos.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterEstacaoServicoDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportFiscalsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/amutransportes/fiscals/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/amutransportes/fiscals/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportFiscalsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/amutransportes/fiscals/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/amutransportes/fiscals/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnFiscalsRead(ref IQueryable<AMUTRANSAPP.Models.AMUTRANSPORTES.Fiscal> items);

        public async Task<IQueryable<AMUTRANSAPP.Models.AMUTRANSPORTES.Fiscal>> GetFiscals(Query query = null)
        {
            var items = Context.Fiscals.AsQueryable();

            items = items.Include(i => i.Paragem);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnFiscalsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnFiscalGet(AMUTRANSAPP.Models.AMUTRANSPORTES.Fiscal item);
        partial void OnGetFiscalById(ref IQueryable<AMUTRANSAPP.Models.AMUTRANSPORTES.Fiscal> items);


        public async Task<AMUTRANSAPP.Models.AMUTRANSPORTES.Fiscal> GetFiscalById(int id)
        {
            var items = Context.Fiscals
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            items = items.Include(i => i.Paragem);
 
            OnGetFiscalById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnFiscalGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnFiscalCreated(AMUTRANSAPP.Models.AMUTRANSPORTES.Fiscal item);
        partial void OnAfterFiscalCreated(AMUTRANSAPP.Models.AMUTRANSPORTES.Fiscal item);

        public async Task<AMUTRANSAPP.Models.AMUTRANSPORTES.Fiscal> CreateFiscal(AMUTRANSAPP.Models.AMUTRANSPORTES.Fiscal fiscal)
        {
            OnFiscalCreated(fiscal);

            var existingItem = Context.Fiscals
                              .Where(i => i.Id == fiscal.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Fiscals.Add(fiscal);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(fiscal).State = EntityState.Detached;
                throw;
            }

            OnAfterFiscalCreated(fiscal);

            return fiscal;
        }

        public async Task<AMUTRANSAPP.Models.AMUTRANSPORTES.Fiscal> CancelFiscalChanges(AMUTRANSAPP.Models.AMUTRANSPORTES.Fiscal item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnFiscalUpdated(AMUTRANSAPP.Models.AMUTRANSPORTES.Fiscal item);
        partial void OnAfterFiscalUpdated(AMUTRANSAPP.Models.AMUTRANSPORTES.Fiscal item);

        public async Task<AMUTRANSAPP.Models.AMUTRANSPORTES.Fiscal> UpdateFiscal(int id, AMUTRANSAPP.Models.AMUTRANSPORTES.Fiscal fiscal)
        {
            OnFiscalUpdated(fiscal);

            var itemToUpdate = Context.Fiscals
                              .Where(i => i.Id == fiscal.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(fiscal);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterFiscalUpdated(fiscal);

            return fiscal;
        }

        partial void OnFiscalDeleted(AMUTRANSAPP.Models.AMUTRANSPORTES.Fiscal item);
        partial void OnAfterFiscalDeleted(AMUTRANSAPP.Models.AMUTRANSPORTES.Fiscal item);

        public async Task<AMUTRANSAPP.Models.AMUTRANSPORTES.Fiscal> DeleteFiscal(int id)
        {
            var itemToDelete = Context.Fiscals
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnFiscalDeleted(itemToDelete);


            Context.Fiscals.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterFiscalDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportLavadorsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/amutransportes/lavadors/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/amutransportes/lavadors/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportLavadorsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/amutransportes/lavadors/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/amutransportes/lavadors/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnLavadorsRead(ref IQueryable<AMUTRANSAPP.Models.AMUTRANSPORTES.Lavador> items);

        public async Task<IQueryable<AMUTRANSAPP.Models.AMUTRANSPORTES.Lavador>> GetLavadors(Query query = null)
        {
            var items = Context.Lavadors.AsQueryable();

            items = items.Include(i => i.EstacaoServico);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnLavadorsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnLavadorGet(AMUTRANSAPP.Models.AMUTRANSPORTES.Lavador item);
        partial void OnGetLavadorById(ref IQueryable<AMUTRANSAPP.Models.AMUTRANSPORTES.Lavador> items);


        public async Task<AMUTRANSAPP.Models.AMUTRANSPORTES.Lavador> GetLavadorById(int id)
        {
            var items = Context.Lavadors
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            items = items.Include(i => i.EstacaoServico);
 
            OnGetLavadorById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnLavadorGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnLavadorCreated(AMUTRANSAPP.Models.AMUTRANSPORTES.Lavador item);
        partial void OnAfterLavadorCreated(AMUTRANSAPP.Models.AMUTRANSPORTES.Lavador item);

        public async Task<AMUTRANSAPP.Models.AMUTRANSPORTES.Lavador> CreateLavador(AMUTRANSAPP.Models.AMUTRANSPORTES.Lavador lavador)
        {
            OnLavadorCreated(lavador);

            var existingItem = Context.Lavadors
                              .Where(i => i.Id == lavador.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Lavadors.Add(lavador);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(lavador).State = EntityState.Detached;
                throw;
            }

            OnAfterLavadorCreated(lavador);

            return lavador;
        }

        public async Task<AMUTRANSAPP.Models.AMUTRANSPORTES.Lavador> CancelLavadorChanges(AMUTRANSAPP.Models.AMUTRANSPORTES.Lavador item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnLavadorUpdated(AMUTRANSAPP.Models.AMUTRANSPORTES.Lavador item);
        partial void OnAfterLavadorUpdated(AMUTRANSAPP.Models.AMUTRANSPORTES.Lavador item);

        public async Task<AMUTRANSAPP.Models.AMUTRANSPORTES.Lavador> UpdateLavador(int id, AMUTRANSAPP.Models.AMUTRANSPORTES.Lavador lavador)
        {
            OnLavadorUpdated(lavador);

            var itemToUpdate = Context.Lavadors
                              .Where(i => i.Id == lavador.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(lavador);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterLavadorUpdated(lavador);

            return lavador;
        }

        partial void OnLavadorDeleted(AMUTRANSAPP.Models.AMUTRANSPORTES.Lavador item);
        partial void OnAfterLavadorDeleted(AMUTRANSAPP.Models.AMUTRANSPORTES.Lavador item);

        public async Task<AMUTRANSAPP.Models.AMUTRANSPORTES.Lavador> DeleteLavador(int id)
        {
            var itemToDelete = Context.Lavadors
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnLavadorDeleted(itemToDelete);


            Context.Lavadors.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterLavadorDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportLicencaLojasToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/amutransportes/licencalojas/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/amutransportes/licencalojas/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportLicencaLojasToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/amutransportes/licencalojas/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/amutransportes/licencalojas/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnLicencaLojasRead(ref IQueryable<AMUTRANSAPP.Models.AMUTRANSPORTES.LicencaLoja> items);

        public async Task<IQueryable<AMUTRANSAPP.Models.AMUTRANSPORTES.LicencaLoja>> GetLicencaLojas(Query query = null)
        {
            var items = Context.LicencaLojas.AsQueryable();

            items = items.Include(i => i.Empresa);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnLicencaLojasRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnLicencaLojaGet(AMUTRANSAPP.Models.AMUTRANSPORTES.LicencaLoja item);
        partial void OnGetLicencaLojaById(ref IQueryable<AMUTRANSAPP.Models.AMUTRANSPORTES.LicencaLoja> items);


        public async Task<AMUTRANSAPP.Models.AMUTRANSPORTES.LicencaLoja> GetLicencaLojaById(int id)
        {
            var items = Context.LicencaLojas
                              .AsNoTracking()
                              .Where(i => i.Id == id);

            items = items.Include(i => i.Empresa);
 
            OnGetLicencaLojaById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnLicencaLojaGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnLicencaLojaCreated(AMUTRANSAPP.Models.AMUTRANSPORTES.LicencaLoja item);
        partial void OnAfterLicencaLojaCreated(AMUTRANSAPP.Models.AMUTRANSPORTES.LicencaLoja item);

        public async Task<AMUTRANSAPP.Models.AMUTRANSPORTES.LicencaLoja> CreateLicencaLoja(AMUTRANSAPP.Models.AMUTRANSPORTES.LicencaLoja licencaloja)
        {
            OnLicencaLojaCreated(licencaloja);

            var existingItem = Context.LicencaLojas
                              .Where(i => i.Id == licencaloja.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }

            #region MeuCodigo
            int numlicenca = 0;
            switch (licencaloja.Tipo)
            {
                case 0:
                    numlicenca = Context.LicencaLojas.Where(i => i.Tipo == 0).ToList().Count() + 1;
                    break;
                case 1:
                    numlicenca = Context.LicencaLojas.Where(i => i.Tipo == 1).ToList().Count() + 1;
                    break;
                case 2:
                    numlicenca = Context.LicencaLojas.Where(i => i.Tipo == 2).ToList().Count() + 1;
                    break;
                case 3:
                    numlicenca = Context.LicencaLojas.Where(i => i.Tipo == 3).ToList().Count() + 1;
                    break;
                case 4:
                    numlicenca = Context.LicencaLojas.Where(i => i.Tipo == 4).ToList().Count() + 1;
                    break;
                default:
                    break;
            }
            licencaloja.NumLicenca = numlicenca;
            #endregion

            try
            {
                Context.LicencaLojas.Add(licencaloja);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(licencaloja).State = EntityState.Detached;
                throw;
            }

            OnAfterLicencaLojaCreated(licencaloja);

            return licencaloja;
        }

        public async Task<AMUTRANSAPP.Models.AMUTRANSPORTES.LicencaLoja> CancelLicencaLojaChanges(AMUTRANSAPP.Models.AMUTRANSPORTES.LicencaLoja item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnLicencaLojaUpdated(AMUTRANSAPP.Models.AMUTRANSPORTES.LicencaLoja item);
        partial void OnAfterLicencaLojaUpdated(AMUTRANSAPP.Models.AMUTRANSPORTES.LicencaLoja item);

        public async Task<AMUTRANSAPP.Models.AMUTRANSPORTES.LicencaLoja> UpdateLicencaLoja(int id, AMUTRANSAPP.Models.AMUTRANSPORTES.LicencaLoja licencaloja)
        {
            OnLicencaLojaUpdated(licencaloja);

            var itemToUpdate = Context.LicencaLojas
                              .Where(i => i.Id == licencaloja.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(licencaloja);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterLicencaLojaUpdated(licencaloja);

            return licencaloja;
        }

        partial void OnLicencaLojaDeleted(AMUTRANSAPP.Models.AMUTRANSPORTES.LicencaLoja item);
        partial void OnAfterLicencaLojaDeleted(AMUTRANSAPP.Models.AMUTRANSPORTES.LicencaLoja item);

        public async Task<AMUTRANSAPP.Models.AMUTRANSPORTES.LicencaLoja> DeleteLicencaLoja(int id)
        {
            var itemToDelete = Context.LicencaLojas
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnLicencaLojaDeleted(itemToDelete);


            Context.LicencaLojas.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterLicencaLojaDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportMovimentosToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/amutransportes/movimentos/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/amutransportes/movimentos/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportMovimentosToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/amutransportes/movimentos/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/amutransportes/movimentos/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnMovimentosRead(ref IQueryable<AMUTRANSAPP.Models.AMUTRANSPORTES.Movimento> items);

        public async Task<IQueryable<AMUTRANSAPP.Models.AMUTRANSPORTES.Movimento>> GetMovimentos(Query query = null)
        {
            var items = Context.Movimentos.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnMovimentosRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnMovimentoGet(AMUTRANSAPP.Models.AMUTRANSPORTES.Movimento item);
        partial void OnGetMovimentoById(ref IQueryable<AMUTRANSAPP.Models.AMUTRANSPORTES.Movimento> items);


        public async Task<AMUTRANSAPP.Models.AMUTRANSPORTES.Movimento> GetMovimentoById(int id)
        {
            var items = Context.Movimentos
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetMovimentoById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnMovimentoGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnMovimentoCreated(AMUTRANSAPP.Models.AMUTRANSPORTES.Movimento item);
        partial void OnAfterMovimentoCreated(AMUTRANSAPP.Models.AMUTRANSPORTES.Movimento item);

        public async Task<AMUTRANSAPP.Models.AMUTRANSPORTES.Movimento> CreateMovimento(AMUTRANSAPP.Models.AMUTRANSPORTES.Movimento movimento)
        {
            OnMovimentoCreated(movimento);

            var existingItem = Context.Movimentos
                              .Where(i => i.Id == movimento.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Movimentos.Add(movimento);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(movimento).State = EntityState.Detached;
                throw;
            }

            OnAfterMovimentoCreated(movimento);

            return movimento;
        }

        public async Task<AMUTRANSAPP.Models.AMUTRANSPORTES.Movimento> CancelMovimentoChanges(AMUTRANSAPP.Models.AMUTRANSPORTES.Movimento item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnMovimentoUpdated(AMUTRANSAPP.Models.AMUTRANSPORTES.Movimento item);
        partial void OnAfterMovimentoUpdated(AMUTRANSAPP.Models.AMUTRANSPORTES.Movimento item);

        public async Task<AMUTRANSAPP.Models.AMUTRANSPORTES.Movimento> UpdateMovimento(int id, AMUTRANSAPP.Models.AMUTRANSPORTES.Movimento movimento)
        {
            OnMovimentoUpdated(movimento);

            var itemToUpdate = Context.Movimentos
                              .Where(i => i.Id == movimento.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(movimento);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterMovimentoUpdated(movimento);

            return movimento;
        }

        partial void OnMovimentoDeleted(AMUTRANSAPP.Models.AMUTRANSPORTES.Movimento item);
        partial void OnAfterMovimentoDeleted(AMUTRANSAPP.Models.AMUTRANSPORTES.Movimento item);

        public async Task<AMUTRANSAPP.Models.AMUTRANSPORTES.Movimento> DeleteMovimento(int id)
        {
            var itemToDelete = Context.Movimentos
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnMovimentoDeleted(itemToDelete);


            Context.Movimentos.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterMovimentoDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportParagemsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/amutransportes/paragems/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/amutransportes/paragems/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportParagemsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/amutransportes/paragems/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/amutransportes/paragems/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnParagemsRead(ref IQueryable<AMUTRANSAPP.Models.AMUTRANSPORTES.Paragem> items);

        public async Task<IQueryable<AMUTRANSAPP.Models.AMUTRANSPORTES.Paragem>> GetParagems(Query query = null)
        {
            var items = Context.Paragems.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnParagemsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnParagemGet(AMUTRANSAPP.Models.AMUTRANSPORTES.Paragem item);
        partial void OnGetParagemById(ref IQueryable<AMUTRANSAPP.Models.AMUTRANSPORTES.Paragem> items);


        public async Task<AMUTRANSAPP.Models.AMUTRANSPORTES.Paragem> GetParagemById(int id)
        {
            var items = Context.Paragems
                              .AsNoTracking()
                              .Where(i => i.Id == id);

 
            OnGetParagemById(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnParagemGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnParagemCreated(AMUTRANSAPP.Models.AMUTRANSPORTES.Paragem item);
        partial void OnAfterParagemCreated(AMUTRANSAPP.Models.AMUTRANSPORTES.Paragem item);

        public async Task<AMUTRANSAPP.Models.AMUTRANSPORTES.Paragem> CreateParagem(AMUTRANSAPP.Models.AMUTRANSPORTES.Paragem paragem)
        {
            OnParagemCreated(paragem);

            var existingItem = Context.Paragems
                              .Where(i => i.Id == paragem.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Paragems.Add(paragem);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(paragem).State = EntityState.Detached;
                throw;
            }

            OnAfterParagemCreated(paragem);

            return paragem;
        }

        public async Task<AMUTRANSAPP.Models.AMUTRANSPORTES.Paragem> CancelParagemChanges(AMUTRANSAPP.Models.AMUTRANSPORTES.Paragem item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnParagemUpdated(AMUTRANSAPP.Models.AMUTRANSPORTES.Paragem item);
        partial void OnAfterParagemUpdated(AMUTRANSAPP.Models.AMUTRANSPORTES.Paragem item);

        public async Task<AMUTRANSAPP.Models.AMUTRANSPORTES.Paragem> UpdateParagem(int id, AMUTRANSAPP.Models.AMUTRANSPORTES.Paragem paragem)
        {
            OnParagemUpdated(paragem);

            var itemToUpdate = Context.Paragems
                              .Where(i => i.Id == paragem.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(paragem);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterParagemUpdated(paragem);

            return paragem;
        }

        partial void OnParagemDeleted(AMUTRANSAPP.Models.AMUTRANSPORTES.Paragem item);
        partial void OnAfterParagemDeleted(AMUTRANSAPP.Models.AMUTRANSPORTES.Paragem item);

        public async Task<AMUTRANSAPP.Models.AMUTRANSPORTES.Paragem> DeleteParagem(int id)
        {
            var itemToDelete = Context.Paragems
                              .Where(i => i.Id == id)
                              .Include(i => i.Fiscals)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnParagemDeleted(itemToDelete);


            Context.Paragems.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterParagemDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}