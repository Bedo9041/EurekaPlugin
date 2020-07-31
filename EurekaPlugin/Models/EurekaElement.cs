using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EurekaPlugin.Models
{
    public enum EurekaElement
    {
        Wind,
        Water,
        Earth,
        Lightning,
        Fire,
        Ice,
        Unknown
    }

    public static class EurekaElementExtensions
    {
        public static string ToFriendlyString(this EurekaElement element)
        {
            switch(element)
            {
                case EurekaElement.Earth:
                    return "Earth";
                case EurekaElement.Fire:
                    return "Fire";
                case EurekaElement.Ice:
                    return "Ice";
                case EurekaElement.Lightning:
                    return "Lightning";
                case EurekaElement.Water:
                    return "Water";
                case EurekaElement.Wind:
                    return "Wind";
                case EurekaElement.Unknown:
                default:
                    return "Unknown";
            }
        }
    }
}
