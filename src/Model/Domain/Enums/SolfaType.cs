using System.ComponentModel;

namespace Model
{
    public enum SolfaType
    {
        [Description("Unknown")]
        Unknown = 0,
        [Description("Do")]
        Do = 1,
        [Description("Re")]
        Re = 2,
        [Description("Mi")]
        Mi = 3,
        [Description("Fa")]
        Fa = 4,
        [Description("Sol")]
        Sol = 5,
        [Description("La")]
        La = 6,
        [Description("Ti")]
        Ti = 7
    }
}
