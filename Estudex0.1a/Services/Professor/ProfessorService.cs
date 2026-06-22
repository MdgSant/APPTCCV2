using EstudeX.Services;
using Estudex0._1a.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Estudex0._1a.Constants;

namespace Estudex0._1a.Services.Professor
{
    public class ProfessorService : Request
    {
        private readonly Request _request;
        public async Task<ObservableCollection<NivelDificuldade>> GetNiveisDificuldadeAsync()
        {
            return await _request.GetAsync<ObservableCollection<NivelDificuldade>>(URLsAPI.NivelDificuldade, _token);
        }
        public async Task<ObservableCollection<Utilizador>> GetOrientadoresAsync()
        {
            return await _request.GetAsync<ObservableCollection<Utilizador>>(URLsAPI.Utilizadores, _token);
        }
        public async Task<ObservableCollection<Disciplina>> GetDisciplinasAsync()
        {
            return await _request.GetAsync<ObservableCollection<Disciplina>>(URLsAPI.Disciplinas, _token);
        }

        public async Task<ObservableCollection<AtividadeResposta>> GetRespostasPorAtividadeAsync(int idAtividade)
        {
            return await _request.GetAsync<ObservableCollection<AtividadeResposta>>(
                $"{URLsAPI.AtividadesRespostas}/atividade/{idAtividade}", _token);
        }

        private string _token;
        public ProfessorService(string token)
        {
            _request = new Request();
            _token = "";
        }
        public async Task<Atividade> PostAtividadeAsync(Atividade a)
        {
            return await _request.PostAsync<Atividade>(URLsAPI.Atividades, a, _token);
        }
        #region Métodos Post
        public async Task<ObservableCollection<Atividade>> GetAtividadesAsync()
        {
            ObservableCollection<Models.Atividade> listaAtividades = await
                _request.GetAsync<ObservableCollection<Models.Atividade>>(URLsAPI.Atividades, _token);
            return listaAtividades;
        }

        public async Task<AtividadePergunta> PostPerguntaAsync(AtividadePergunta p)
        {
            return await _request.PostAsync<AtividadePergunta>(URLsAPI.AtividadesPergunta, p, _token);
        }
        #endregion
        public async Task<Atividade> GetAtividadeAsync(int atividadeId)
        {
            string url = $"{URLsAPI.Atividades}/{atividadeId}";
            return await _request.GetAsync<Atividade>(url, _token);
        }

        public async Task<PaginaResposta<Atividade>> GetAtividadesPaginadasAsync(int pagina, int tamanho = 10)
        {
            return await _request.GetAsync<PaginaResposta<Atividade>>(
                $"{URLsAPI.Atividades}?page={pagina}&size={tamanho}", _token);
        }

        public async Task<int> PutAtividadeAsync(Atividade a)
        {
            return await _request.PutAsync(URLsAPI.Atividades, a, _token);
        }

        public async Task<int> DeleteAtividadeAsync(int atividadeId)
        {
            string url = $"{URLsAPI.Atividades}/{atividadeId}";
            return await _request.DeleteAsync(url, _token);
        }

    }
}
