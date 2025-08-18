//using Microsoft.AspNetCore.Components;
//using MyRhSystem.APP.Shared.Models;
//using MyRhSystem.APP.Shared;
//using MyRhSystem.APP.Shared.Services;

//namespace MyRhSystem.APP.Components.Pages;

//public class CompanyRegisterBase : ComponentBase
//{
//    protected CompanyRegisterModel Model { get; set; } = new();
//    protected bool IsLoading = false;

//    [Inject] protected NavigationManager Navigation { get; set; } = default!;
//    [Inject] protected CompanyRegisterApiService CompanyService { get; set; } = default!;

//    protected async Task SalvarEmpresa()
//    {
//        IsLoading = true;

//        try
//        {
//            await CompanyService.RegisterCompanyAsync(Model);
//            Navigation.NavigateTo("/dashboard");
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Erro ao salvar empresa: {ex.Message}");
//        }
//        finally
//        {
//            IsLoading = false;
//        }
//    }
//}
