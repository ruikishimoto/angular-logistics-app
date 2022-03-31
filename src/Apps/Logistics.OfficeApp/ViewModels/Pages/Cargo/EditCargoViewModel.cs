﻿using Microsoft.AspNetCore.Components.Authorization;
using Logistics.WebApi.Client.Exceptions;

namespace Logistics.OfficeApp.ViewModels.Pages.Cargo;

public class EditCargoViewModel : PageViewModelBase
{
    private readonly AuthenticationStateProvider authStateProvider;

    public EditCargoViewModel(
        AuthenticationStateProvider authStateProvider,
        IApiClient apiClient)
        : base(apiClient)
    {
        this.authStateProvider = authStateProvider;
        Trucks = new List<TruckDto>();
        Cargo = new CargoDto();
    }

    [Parameter]
    public string? Id { get; set; }

    [CascadingParameter]
    public Toast? Toast { get; set; }

    
    #region Binding properties

    public CargoDto Cargo { get; set; }
    public IEnumerable<TruckDto> Trucks { get; set; }
    public bool EditMode => !string.IsNullOrEmpty(Id);
    public string Error { get; set; } = string.Empty;

    #endregion


    public override async Task OnInitializedAsync()
    {
        Error = string.Empty;

        if (EditMode)
        {
            IsBusy = true;
            var cargo = await FetchCargoAsync(Id!);

            if (cargo != null)
                Cargo = cargo;

            IsBusy = false;
        }  
    }

    public override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
            return;

        if (!EditMode)
        {
            await LoadCurrentDispatcherAsync();
        }
    }

    public async Task UpdateAsync()
    {
        IsBusy = true;
        Error = string.Empty;
        try
        {
            if (EditMode)
            {
                await apiClient.UpdateCargoAsync(Cargo!);
                Toast?.Show("Cargo has been saved successfully.", "Notification");
            }
            else
            {
                await apiClient.CreateCargoAsync(Cargo!);
                Toast?.Show("A new cargo has been created successfully.", "Notification");
                ResetData();
            }
            IsBusy = false;
        }
        catch (ApiException ex)
        {
            Error = ex.Message;
            IsBusy = false;
        }
    }

    private void ResetData()
    {
        Cargo.AssignedTruckId = null;
        Cargo.PricePerMile = 0;
        Cargo.TotalTripMiles = 0;
        Cargo.Source = string.Empty;
        Cargo.Destination = string.Empty;
    }

    private async Task LoadCurrentDispatcherAsync()
    {
        var authState = await authStateProvider.GetAuthenticationStateAsync();
        var externalId = authState.User.GetId();

        if (!string.IsNullOrEmpty(externalId) && Cargo != null)
        {
            var user = await Task.Run(async () => await apiClient.GetUserAsync(externalId));
            Cargo.AssignedDispatcherId = user?.Id;
            Cargo.AssignedDispatcherName = user?.GetFullName();
            StateHasChanged();
        }
    }

    public async Task<IEnumerable<DataListItem>> SearchTruck(string value)
    {
        var pagedList = await apiClient.GetTrucksAsync(value);
        var dataListItems = new List<DataListItem>();

        if (pagedList.Items != null)
        {
            foreach (var item in pagedList.Items)
            {
                dataListItems.Add(new DataListItem(item.Id!, item.DriverName!));
            }
        }

        return dataListItems;
    }

    private Task<CargoDto?> FetchCargoAsync(string id)
    {
        return Task.Run(async () =>
        {
            return await apiClient.GetCargoAsync(id);
        });
    }
}