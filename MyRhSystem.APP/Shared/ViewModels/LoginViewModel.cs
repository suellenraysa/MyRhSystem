using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.Components;
using MyRhSystem.APP.Shared.Services;
using MyRhSystem.Contracts.Auth;

public partial class LoginViewModel : ObservableObject
{
    private readonly AuthApiService _authApiService;
    private readonly NavigationManager _navigation;
    private CancellationTokenSource? _cts;

    private bool isLoading;
    public bool IsLoading { get => isLoading; set => SetProperty(ref isLoading, value); }

    private bool hasAnyUser;
    public bool HasAnyUser { get => hasAnyUser; set => SetProperty(ref hasAnyUser, value); }

    private string? email;
    public string? Email { get => email; set => SetProperty(ref email, value); }

    private string? password;
    public string? Password { get => password; set => SetProperty(ref password, value); }

    private string? error;
    public string? Error { get => error; set => SetProperty(ref error, value); }

    public LoginViewModel(AuthApiService authApiService, NavigationManager navigation)
    {
        _authApiService = authApiService;
        _navigation = navigation;
    }

    [RelayCommand]
    public async Task InitializeAsync()
    {
        _cts?.Cancel();
        _cts = new CancellationTokenSource();
        var ct = _cts.Token;

        try
        {
            IsLoading = true;
            var res = await _authApiService.HasAnyUserAsync(ct);
            HasAnyUser = res?.HasAnyUser ?? false;
            Error = null;
        }
        catch (OperationCanceledException) { /* ignorar */ }
        catch (HttpRequestException ex)
        {
            System.Diagnostics.Debug.WriteLine($"[Login] HTTP: {ex}");
            Error = "Não foi possível conectar ao servidor.";
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[Login] Init error: {ex}");
            Error = "Falha ao carregar informações iniciais.";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    public async Task LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            Error = "Informe e-mail e senha.";
            return;
        }

        try
        {
            IsLoading = true;
            Error = null;

            var resp = await _authApiService.LoginAsync(
                new LoginRequest { Email = Email.Trim(), Password = Password });

            if (resp is null)
            {
                Error = "E-mail ou senha inválidos.";
                return;
            }

            // TODO: salvar token/usuário
            _navigation.NavigateTo("/company/list", replace: true);
        }
        catch (HttpRequestException ex)
        {
            System.Diagnostics.Debug.WriteLine($"[Login] HTTP: {ex}");
            Error = "Servidor indisponível ou endereço incorreto.";
        }
        catch (NotSupportedException ex) // JSON inesperado
        {
            System.Diagnostics.Debug.WriteLine($"[Login] JSON: {ex}");
            Error = "Resposta inesperada do servidor.";
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[Login] Error: {ex}");
            Error = "Falha ao entrar. Tente novamente.";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand] public void GoToRegister() => _navigation.NavigateTo("/users/edit");
}
