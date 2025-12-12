using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shares.DTOs;
using WebClient.Services.API.ApiService;

namespace WebClient.Pages;

public partial class Home {
    // --------------------- INITIALIZATION ---------------------
    [Inject] public ISnackbar Snackbar { get; private set; } = default!;
    [Inject] public IApiService ApiService { get; private set; } = default!;

    // --------------------- PROPERTIES ---------------------
    // Aux.Fields
    private TickerDto? _ticker;
    private string? _symbol = string.Empty;
    private string _infoMsg = "Nothing to show...";
    private bool _stockLoaded = false;
    private bool _isLoading = false;

    // --------------------- METHODS ---------------------
    private async Task GetAssetAsync() {
        _isLoading = true;
        _stockLoaded = false;

        try {
            if (!IsTickerValid()) return;

            _ticker = await ApiService.GetTickerAsync(_symbol);
            _stockLoaded = true;
        } 
        catch(Exception ex) { _infoMsg = ex.Message; }
        finally { _isLoading = false; }
    }
    private async Task UpdateAssetAsync() {
        _isLoading = true;

        try {
            if (!IsTickerValid()) return;
            await Task.Delay(1000);
        }
        finally { _isLoading = false; }
    }

    // Aux.Meths
    private bool IsTickerValid() {
        bool valid = !string.IsNullOrWhiteSpace(_symbol) && _symbol?.Length <= 10;
        if (!valid) 
            Snackbar.Add("Ticker neither can be null nor has more than 10 chars", Severity.Warning);

        return valid;
    }
}