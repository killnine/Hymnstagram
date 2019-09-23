using System.ComponentModel;

namespace Hymnstagram.Model.Domain
{
    public enum CreativeType
    {
        [Description("Composer")]
        Composer = 1,
        [Description("Writer")]
        Writer = 2,
        [Description("Arranger")]
        Arranger = 3,
        [Description("Editor")]
        Editor = 4,
        [Description("Technical Editor")]
        TechnicalEditor = 5,
        [Description("Associate Editor")]
        AssociateEditor = 6,
    }
}
