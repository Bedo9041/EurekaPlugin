using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EurekaPlugin.Models
{
    public enum EurekaWeather
    {
        Gales,
        Showers,
        Fair_Skies,
        Snow,
        Heat_Waves,
        Thunder,
        Blizzards,
        Fog,
        Umbral_Wind,
        Thunderstorms,
        Gloom,
        None
    }

    public static class EurekaWeatherExtensions
    {
        public static string ToFriendlyString(this EurekaWeather weather)
        {
            switch(weather)
            {
                case EurekaWeather.Gales:
                    return "Gales";
                case EurekaWeather.Showers:
                    return "Showers";
                case EurekaWeather.Fair_Skies:
                    return "Fair Skies";
                case EurekaWeather.Snow:
                    return "Snow";
                case EurekaWeather.Heat_Waves:
                    return "Heat Waves";
                case EurekaWeather.Thunder:
                    return "Thunder";
                case EurekaWeather.Blizzards:
                    return "Blizzards";
                case EurekaWeather.Fog:
                    return "Fog";
                case EurekaWeather.Umbral_Wind:
                    return "Umbral Wind";
                case EurekaWeather.Thunderstorms:
                    return "Thunderstorms";
                case EurekaWeather.Gloom:
                    return "Gloom";
                case EurekaWeather.None:
                default:
                    return "None";
            }
        }
    }
}
