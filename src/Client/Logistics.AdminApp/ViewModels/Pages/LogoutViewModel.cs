﻿namespace Logistics.AdminApp.ViewModels.Pages;

public class LogoutViewModel : PageBaseViewModel
{
    private readonly NavigationManager _navigationManager;

    public LogoutViewModel(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
    } 

    public void SignOut()
    {
        _navigationManager.NavigateTo("/Account/Logout", true);
    }
}
