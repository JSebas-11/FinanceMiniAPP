using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shares.DTOs;
using Shares.Enums;

namespace WebClient.Components;

public partial class StockView {
    //--------------------------INITIALIZATIION--------------------------
    [Parameter]
    public TickerDto? Ticker { get; set; }

    //--------------------------aux.Fields--------------------------
    private Color _marketStateColor = Color.Surface;
    private string? _quoteTypeImgSrc;
    private string _updatedIcon = Icons.Material.Outlined.BrowserUpdated;
    private readonly Dictionary<string, object?> _financialDict = [];
    private readonly Dictionary<string, object?> _fundamentalsDict = [];

    //--------------------------METHODS--------------------------
    protected override void OnInitialized() {
        if (Ticker is null) return;
        
        DefineGraphicItems();
        DefineMetricDicts();
    }

    private void DefineGraphicItems() {
        // Definicion de elementos graficos que seran dinamicos de acuerdo a ciertas condiciones
        _updatedIcon = Ticker?.LastUpdated.ToLocalTime().Date == DateTime.Today.Date 
            ? Icons.Material.Outlined.EventAvailable 
            : Icons.Material.Outlined.BrowserUpdated;

        _marketStateColor = Ticker?.MarketState switch {
            MarketState.Regular => Color.Success,
            MarketState.Closed => Color.Error,
            MarketState.Post or MarketState.Pre or MarketState.Unknown => Color.Warning,
            _ => Color.Warning
        };

        _quoteTypeImgSrc = Ticker?.QuoteType switch {
            QuoteType.Equity => "imgs/stock_icon.png",
            QuoteType.ETF => "imgs/etf_icon.png",
            QuoteType.CryptoCurrency => "imgs/crypto_icon.webp",
            QuoteType.Fund => "imgs/fund_icon.png",
            QuoteType.Index => "imgs/index_icon.png",
            QuoteType.Unknown => "imgs/generic_icon.png",
            _ => "imgs/generic_icon.png"
        };
    }

    private void DefineMetricDicts() {
        // FINANCIAL DICTIONARY
        _financialDict["Volume"] = Ticker?.RegularMarketVolume;
        _financialDict["Capitalization"] = Ticker?.MarketCap;
        _financialDict["52 weeks highest"] = Ticker?.FiftyTwoWeekHigh;
        _financialDict["52 weeks lowest"] = Ticker?.FiftyTwoWeekLow;
        
        // FINANCIAL DICTIONARY
        _fundamentalsDict["EPS TTM"] = Ticker?.EpsTtm;
        _fundamentalsDict["EPS Forward"] = Ticker?.EpsForward;
        _fundamentalsDict["PE Forward"] = Ticker?.ForwardPE;
        _fundamentalsDict["Books to Price"] = Ticker?.Price2Book;
        _fundamentalsDict["Book Value"] = Ticker?.BookValue;
        _fundamentalsDict["Shares Outstanding"] = Ticker?.SharesOutstanding;
    }
}