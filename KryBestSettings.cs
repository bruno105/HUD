using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;
using ExileCore.Shared.Attributes;
using System.Windows.Forms;

namespace KryBest
{
    public class KryBestSettings : ISettings
    {
        public ToggleNode Enable { get; set; } = new ToggleNode(true);

        [Menu("Math Best Choice Hotkey")]
        public HotkeyNode MathKey { get; set; } = new HotkeyNode(Keys.F5);

        [Menu("Reset Choice Hotkey")]
        public HotkeyNode ResetKey { get; set; } = new HotkeyNode(Keys.F7);

        [Menu("Time between Clicks in Milliseconds")]
        public RangeNode<int> ExtraDelayInMs { get; set; } = new RangeNode<int>(40, 10, 100);

    }
}
