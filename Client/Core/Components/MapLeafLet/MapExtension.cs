using BlazorLeaflet;
using BlazorLeaflet.Models;
using Microsoft.JSInterop;

namespace Client.Core.Components.MapLeafLet
{
    public static class MapExtension
    {
        public static void AddRouting(this Map map, IJSRuntime jsRuntime, List<LatLng> routes, List<LatLngDouble>? gpsroutes)
        {
            jsRuntime.InvokeVoidAsync("window.leafletBlazor" + ".addRoute", map.Id, routes, gpsroutes);
        }
        public static void AddRoutingWithoutGPS(this Map map, IJSRuntime jsRuntime, List<LatLng> routes)
        {
            jsRuntime.InvokeVoidAsync("window.leafletBlazor" + ".addRoute", map.Id, routes);
        }
        public static void ZoomToLocation(this Map map, IJSRuntime jsRuntime, float lat, float lng, float ZoomLevel, string text, bool iscar)
        {
            jsRuntime.InvokeVoidAsync("window.leafletBlazor" + ".zoomToLocation", map.Id, lat, lng, ZoomLevel, text, iscar);
        }
        public static async ValueTask<string> GetGeoadress(this Map map, IJSRuntime jsRuntime, LatLng Latlng)
        {
            return await jsRuntime.InvokeAsync<string>("window.leafletBlazor" + ".getAdress", Latlng);
        }
    }
}
