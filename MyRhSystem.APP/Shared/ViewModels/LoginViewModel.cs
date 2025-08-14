using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.Components;
using MyRhSystem.APP.Shared.Services;
using MyRhSystem.Contracts.Auth;

namespace MyRhSystem.APP.Shared.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly AuthApiService _authApiService;
    private readonly NavigationManager _navigationService;

    [ObservableProperty] private bool isLoading;
    [ObservableProperty] private bool hasAnyUser;
    [ObservableProperty] private string email = "";
    [ObservableProperty] private string password = "";
    [ObservableProperty] private string? error;

    public LoginViewModel(AuthApiService authApiService, NavigationManager navigationService)
    {
        _authApiService = authApiService;
        _navigationService = navigationService;
    }

    [RelayCommand]
    
    public async Task InitializeAsync()
    {
        IsLoading = true;
        var res = await _authApiService.HasAnyUserAsync();
        HasAnyUser = res?.HasAnyUser ?? false;
        IsLoading = false;
    }

    [RelayCommand]
    public async Task LoginAsync()
    {
        Error = null;
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            Error = "Informe e-mail e senha.";
            return;
        }

        IsLoading = true;
        var resp = await _authApiService.LoginAsync(new LoginRequest { Email = Email.Trim(), Password = Password });
        IsLoading = false;

        if (resp is null)
        {
            Error = "E-mail ou senha inválidos.";
            return;
        }

        // TODO: guardar token/usuário (SecureStorage/Preferências)
        // SecureStorage.SetAsync("auth_token", resp.Token ?? "");
        // Preferências simples p/ demo:
        // Preferences.Default.Set("user_email", resp.User.Email);

        // Redirecionar para a home do app (ex.: /users)
        _navigationService.NavigateTo("/users", replace: true);
    }

    [RelayCommand] public void GoToRegister() => _navigationService.NavigateTo("/users/edit");
}

