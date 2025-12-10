using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace WebClient.Pages;

public partial class Home {
    // --------------------- INITIALIZATION ---------------------
    [Inject] public ISnackbar Snackbar { get; private set; } = default!;

    // --------------------- PROPERTIES ---------------------
    // Aux.Fields
    private string? _ticker = string.Empty;
    private string _infoMsg = "Nothing to show...";
    private bool _stockLoaded = false;
    private bool _isLoading = false;

    // --------------------- METHODS ---------------------
    private async Task GetAssetAsync() {
        _isLoading = true;

        try {
            if (!IsTickerValid()) return;
            await Task.Delay(1000);
        }
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
        bool valid = !string.IsNullOrWhiteSpace(_ticker) && _ticker?.Length <= 10;
        if (!valid) 
            Snackbar.Add("Ticker neither can be null nor has more than 10 chars", Severity.Warning);

        return valid;
    }
}