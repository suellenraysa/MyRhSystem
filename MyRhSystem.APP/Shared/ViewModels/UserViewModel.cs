using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyRhSystem.Infrastructure.Seed;

namespace MyRhSystem.APP.Shared.ViewModels;

public class UserViewModel : ObservableObject
{
    private readonly DataSeeder _seeder;


    public UserViewModel(DataSeeder seeder)
    {
        _seeder = seeder;
        SeedCommand = new AsyncRelayCommand(ExecuteSeedAsync);
    }

    public IAsyncRelayCommand SeedCommand { get; }

    private async Task ExecuteSeedAsync()
    {
        await _seeder.SeedAsync();
        // sinalize na UI que terminou, ex:
        // IsSeeded = true;
    }
}
