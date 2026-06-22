using EstudeX.Services;
using Estudex0._1a.Models;
using Estudex0._1a.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Estudex0._1a.Constants;

namespace Estudex0._1a.Services.Professor
{
    public class ProfessorDuvidasService : Request
    {
        private readonly Request _request;
        private const string apiUrlBase = "http://localhost:8080/duvidas";
        
        private string _token;


        public ProfessorDuvidasService(string token)
        {
            _request = new Request();
            _token = token;
        }

        public async Task<ObservableCollection<Duvida>> GetDuvidasAsync()
        {
            return await _request.GetAsync<ObservableCollection<Duvida>>(URLsAPI.Duvidas, _token);
        }

        public async Task<Duvida> GetDuvidaProfessorAsync(int idDuvida)
        {
            return await _request.GetAsync<Duvida>($"{URLsAPI.Duvidas}/professor/{idDuvida}", _token);
        }

        public async Task<bool> VerificarDuvidaRespondidaAsync(int idDuvida)
        {
            return await _request.GetAsync<bool>(
                $"{URLsAPI.RespostasDuvidas}/verificar/{idDuvida}", _token);
        }

        public async Task<bool> PostRespostaDuvidaAsync(RespostaDuvida resposta)
        {
            try
            {
                await _request.PostAsync<RespostaDuvida>(URLsAPI.RespostasDuvidas, resposta, _token);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<int> PutDuvidaAsync(Duvida d)
        {
            return await _request.PutAsync(URLsAPI.Duvidas, d, _token);
        }

        public async Task<int> DeleteDuvidaAsync(int idDuvida)
        {
            return await _request.DeleteAsync($"{URLsAPI.Duvidas}/{idDuvida}", _token);
        }
    }
}
