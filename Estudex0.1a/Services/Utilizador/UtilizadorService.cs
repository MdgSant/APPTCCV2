using EstudeX.Services;
using Estudex0._1a.Constants;
using Estudex0._1a.Models;
using Estudex0._1a.Models.Login;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace EstudeX.Services;

public class UtilizadorService : Request
{
    private readonly Request _request;
    private const string ApiUrlBase = URLsAPI.Utilizadores;
    private readonly string _token;

    public UtilizadorService(string token = "")
    {
        _request = new Request();
        _token = token;
    }

    public async Task<ObservableCollection<Utilizador>> GetAllAsync()
    {
        return await _request.GetAsync<ObservableCollection<Utilizador>>(ApiUrlBase, _token);
    }

    public async Task<Utilizador> GetByIdAsync(int id)
    {
        return await _request.GetAsync<Utilizador>($"{ApiUrlBase}/{id}", _token);
    }

    public async Task<Utilizador> CreateAsync(Utilizador utilizador)
{
    return await _request.PostAsync<Utilizador>(ApiUrlBase, utilizador, _token);
}

    public async Task<int> UpdateAsync(Utilizador utilizador)
    {
        return await _request.PutAsync($"{ApiUrlBase}/{utilizador.IdUtilizador}", utilizador, _token);
    }

    public async Task<int> DeleteAsync(int id)
    {
        return await _request.DeleteAsync($"{ApiUrlBase}/{id}", _token);
    }

    // LOGIN
    public async Task<LoginResponse> PostAutenticarUtilizadorAsync(LoginRequest u)
    {
        return await _request.PostAsync<LoginResponse, LoginRequest>(URLsAPI.Autenticar, u, string.Empty);
    }

    // CADASTRO
    public async Task<Utilizador> PostRegistrarUtilizadorAsync(Utilizador u)
    {
        return await _request.PostAsync<Utilizador>(URLsAPI.Registrar, u, string.Empty);
    }
}
