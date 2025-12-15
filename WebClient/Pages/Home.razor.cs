using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shares.DTOs;
using Shares.Results;
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
    private Color _infoColor = Color.Warning;
    private bool _stockLoaded = false;
    private bool _isLoading = false;

    // --------------------- METHODS ---------------------
    private async Task GetAssetAsync() {
        _isLoading = true;
        _stockLoaded = false;
        _infoColor = Color.Warning;

        try {
            if (!IsTickerValid()) return;

            _ticker = await ApiService.GetTickerAsync(_symbol);
            _stockLoaded = true;
            _infoMsg = "Nothing to show...";
        } 
        catch(ApiException ex) { 
            _infoMsg = ex.StatusCode switch {
                404 => $"Asset with symbol ({_symbol}) not found",
                500 => $"Internal error in API ({ex.Message})",
                _ => ex.Message
            };
        }
        catch(Exception ex) { _infoMsg = ex.Message; }
        finally { _isLoading = false; }
    }
    private async Task UpdateAssetAsync() {
        _isLoading = true;
        _stockLoaded = false;
        _infoColor = Color.Warning;

        try {
            if (!IsTickerValid()) return;

            Result result = await ApiService.RefreshTickerAsync(_symbol);

            if (result.Success) {
                _infoMsg = result.Description;
                _infoColor = Color.Success;
            }
        }
        catch(ApiException ex) { 
            _infoMsg = ex.StatusCode switch {
                404 => $"Asset with symbol ({_symbol}) does not exist in Database",
                500 => $"Internal error in API ({ex.Message})",
                _ => ex.Message
            };

            // Lanza excepcion incluso en exito por serializacion de Result (solucion temporal)
            if (ex.StatusCode == 200) {
                _infoMsg = $"Asset ({_symbol}) has been updated successfully";
                _infoColor = Color.Success;
            }
        }
        catch(Exception ex) { _infoMsg = ex.Message; }
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