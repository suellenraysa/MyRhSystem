using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyRhSystem.APP.Shared.Services;
using MyRhSystem.Contracts.Users;
using Microsoft.AspNetCore.Components;

namespace MyRhSystem.APP.ViewModels;

public partial class UserEditViewModel : ObservableObject
{
    private readonly UsersApiService _usersApiService;
    private readonly NavigationManager _navigationManager;

    [ObservableProperty] private bool isCreate = true;
    [ObservableProperty] private UpdateUserRequest form = new();
    [ObservableProperty] private CreateUserRequest createForm = new();

    public UserEditViewModel(UsersApiService usersApiService, NavigationManager navigationManager)
    {
        _usersApiService = usersApiService;
        _navigationManager = navigationManager;
    }

    [RelayCommand]
    public async Task InitializeAsync(int? id)
    {
        IsCreate = !id.HasValue;
        if (IsCreate) return;

        var dto = await _usersApiService.GetByIdAsync(id!.Value);
        if (dto is null) { Back(); return; }

        Form = new UpdateUserRequest
        {
            Id = dto.Id,
            Nome = dto.Nome,
            Email = dto.Email
        };
    }

    [RelayCommand]
    public async Task SaveAsync()
    {
        if (IsCreate)
        {
            var created = await _usersApiService.CreateAsync(CreateForm);
            if (created is not null) Back();
        }
        else
        {
            await _usersApiService.UpdateAsync(Form.Id, Form);
            Back();
        }
    }

    [RelayCommand] public void Back() => _navigationManager.NavigateTo("/users");
}
