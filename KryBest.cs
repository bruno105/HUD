using ExileCore;
using ExileCore.Shared;
using ExileCore.Shared.Enums;
using ImGuiNET;
using SharpDX;
using System;
using System.Collections;
using System.Windows.Forms;

namespace KryBest
{
    public class KryBest : BaseSettingsPlugin<KryBestSettings>
    {
        private Random Random { get; } = new Random();
        private Vector2 ClickWindowOffset => GameController.Window.GetWindowRectangle().TopLeft;

        private static bool IsRunning { get; set; } = false;


        public override bool Initialise()
        {
            Input.RegisterKey(Keys.LControlKey);
            return true;
        }

        public override void Render()
        {
            if (!IsRunConditionMet()) return;
            IsRunning = true;

            var coroutineWorker = new Coroutine(MathWork(), this, "KryBest.MathExpedition");
            Core.ParallelRunner.Run(coroutineWorker);
        }

        private bool IsRunConditionMet()
        {
            if (IsRunning) return false;
            if (!Input.GetKeyState(Settings.MathKey.Value)) return false;
            if (!GameController.Window.IsForeground()) return false;
            if (!GameController.Game.IngameState.IngameUi.InventoryPanel.IsVisible) return false;

            if (GameController.Game.IngameState.IngameUi.StashElement.IsVisibleLocal) return true;
            if (GameController.Game.IngameState.IngameUi.SellWindow.IsVisibleLocal) return true;
            if (GameController.Game.IngameState.IngameUi.TradeWindow.IsVisibleLocal) return true;

            return false;
        }

        private IEnumerator MathWork()
        {
            var items = GameController.Game.IngameState.IngameUi.InventoryPanel[InventoryIndex.PlayerInventory]?.VisibleInventoryItems;
            if (items == null)
            {
                DebugWindow.LogError("KryBest -> Items in inventory is null.");
                yield break;
            }

            try
            {
                Input.KeyDown(Keys.LControlKey);
                foreach (var item in items)
                {   
                    var centerOfItem = item.GetClientRect().Center
                        + ClickWindowOffset
                        + new Vector2(Random.Next(0, 5), Random.Next(0, 5));

                    Input.SetCursorPos(centerOfItem);
                    yield return new WaitTime(3);
                    Input.Click(MouseButtons.Left);
                    yield return new WaitTime(3);
                    Input.Click(MouseButtons.Left);

                    var waitTime = Math.Max(3, Settings.ExtraDelayInMs - 6 - 8 + Random.Next(0, 16));
                    yield return new WaitTime(waitTime);
                }
            }
            finally
            {
                Input.KeyUp(Keys.LControlKey);
                IsRunning = false;
            }
        }


        public override void DrawSettings()
        {
            base.DrawSettings();
        }


    }
}
