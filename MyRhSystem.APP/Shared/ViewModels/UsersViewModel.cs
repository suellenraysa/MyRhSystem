using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.Components;
using MyRhSystem.APP.Shared.Services;
using MyRhSystem.Contracts.Common;
using MyRhSystem.Contracts.Users;

namespace MyRhSystem.APP.Shared.ViewModels;

public partial class UsersViewModel : ObservableObject
{
    private readonly UsersApiService _usersApiService;
    private readonly NavigationManager _navigationManager;
    private CancellationTokenSource? _cts;

    [ObservableProperty] private PagedResponse<UserDto> data = new(new List<UserDto>(), 1, 10, 0);
    [ObservableProperty] private bool isLoading;
    [ObservableProperty] private string? search;
    [ObservableProperty] private int page = 1;
    [ObservableProperty] private int pageSize = 10;
    [ObservableProperty] private int? deleteId;

    public int TotalPages => (int)Math.Ceiling((double)Data.TotalCount / PageSize);
    public bool CanPrev => Page > 1;
    public bool CanNext => Page < TotalPages;

    public UsersViewModel(UsersApiService usersApiService, NavigationManager navigationManager)
    {
        _usersApiService = usersApiService;
        _navigationManager = navigationManager;
    }

    [RelayCommand]
    public async Task InitializeAsync() => await LoadAsync();

    [RelayCommand]
    public async Task LoadAsync()
    {
        // cancela requisição anterior (debounce)
        _cts?.Cancel();
        _cts = new CancellationTokenSource();
        var ct = _cts.Token;

        IsLoading = true;
        try
        {
            var resp = await _usersApiService.GetAsync(new PagedRequest(Page, PageSize, Search), ct);
            Data = resp ?? new PagedResponse<UserDto>(new List<UserDto>(), Page, PageSize, 0);
        }
        catch (OperationCanceledException)
        {
            // ignorado — outra busca começou
        }
        catch (Exception ex)
        {
            // Logue e “amorteça” o erro para não quebrar a UI
            System.Diagnostics.Debug.WriteLine($"Users LoadAsync error: {ex}");
            Data = new PagedResponse<UserDto>(new List<UserDto>(), Page, PageSize, 0);
        }
        finally
        {
            IsLoading = false;
            OnPropertyChanged(nameof(TotalPages));
            OnPropertyChanged(nameof(CanPrev));
            OnPropertyChanged(nameof(CanNext));
        }
    }

    partial void OnSearchChanged(string? value)
    {
        Page = 1;
        _ = LoadAsync(); // fire-and-forget protegido pelo try/catch
    }

    [RelayCommand]
    public async Task PrevAsync()
    {
        if (!CanPrev) return;
        Page--;
        await LoadAsync();
    }

    [RelayCommand]
    public async Task NextAsync()
    {
        if (!CanNext) return;
        Page++;
        await LoadAsync();
    }

    [RelayCommand] public void GoCreate() => _navigationManager.NavigateTo("/users/edit");
    [RelayCommand] public void GoEdit(int id) => _navigationManager.NavigateTo($"/users/edit/{id}");

    [RelayCommand] public void ConfirmDelete(int id) => DeleteId = id;
    [RelayCommand] public void CancelDelete() => DeleteId = null;

    [RelayCommand]
    public async Task DeleteAsync()
    {
        if (!DeleteId.HasValue) return;
        await _usersApiService.DeleteAsync(DeleteId.Value);
        DeleteId = null;
        await LoadAsync();
    }

   
}
