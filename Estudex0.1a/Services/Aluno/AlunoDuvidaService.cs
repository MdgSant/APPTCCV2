using EstudeX.Services;
using Estudex0._1a.Constants;
using Estudex0._1a.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Estudex0._1a.Services.Aluno
{
    public class AlunoDuvidaService : Request
    {
        private readonly Request _request;
        private const string apiUrlBase = "http://localhost:8080/duvidas";
        private string _token;

        public AlunoDuvidaService(string token)
        {
            _request = new Request();
            _token = token;
        }

        public async Task<ObservableCollection<Duvida>> GetDuvidasAsync()
        {
            return await _request.GetAsync<ObservableCollection<Duvida>>(URLsAPI.Duvidas, _token);
        }

        public async Task<Duvida> GetDuvidaAsync(int idDuvida)
        {
            return await _request.GetAsync<Duvida>($"{URLsAPI.Duvidas}/{idDuvida}", _token);
        }

        public async Task<Duvida> PostDuvidaAsync(Duvida d)
        {
            return await _request.PostAsync<Duvida>(URLsAPI.Duvidas, d, _token);
        }

        public async Task<Duvida> GetDuvidaAlunoAsync(int idDuvida)
        {
            return await _request.GetAsync<Duvida>($"{URLsAPI.Duvidas}/aluno/{idDuvida}", _token);
        }

        public async Task<ObservableCollection<Duvida>> GetMinhasDuvidasAsync(int idUtilizador)
        {
            return await _request.GetAsync<ObservableCollection<Duvida>>($"{URLsAPI.Duvidas}/aluno/{idUtilizador}", _token);
        }

        public async Task<ObservableCollection<RespostaDuvida>> GetRespostasDuvidaAsync(int idDuvida)
        {
            return await _request.GetAsync<ObservableCollection<RespostaDuvida>>(
                $"{URLsAPI.RespostasDuvidas}/duvida/{idDuvida}", _token);
        }
    }
}
