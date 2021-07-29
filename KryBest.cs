using ExileCore;
using ExileCore.Shared;
using ExileCore.Shared.Enums;
using ImGuiNET;
using SharpDX;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Globalization;
using System.IO;
using System.Linq;
using ExileCore.PoEMemory;
using ExileCore.PoEMemory.Components;
using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Cache;
using System.Runtime.InteropServices;
using ExileCore.Shared.Abstract;
using ExileCore.Shared.Helpers;
using JM.LinqFaster;


namespace KryBest
{
    public class KryBest : BaseSettingsPlugin<KryBestSettings>
    {
        private Random Random { get; } = new Random();
        private Vector2 ClickWindowOffset => GameController.Window.GetWindowRectangle().TopLeft;

        private static bool IsRunning { get; set; } = false;


        public override bool Initialise()
        {
            Input.RegisterKey(Settings.MathKey.Value);
            return true;
        }


        public override Job Tick()
        {
            if (!Input.GetKeyState(Settings.MathKey.Value)) return null;
            //MathWork();

            return null;
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
            if (!Input.GetKeyState(Settings.MathKey.Value)) return false;
            if (!GameController.Window.IsForeground()) return false;

            return true;
        }

        private IEnumerator MathWork()
        {
            var items = GameController.Game.IngameState.IngameUi.InventoryPanel[InventoryIndex.PlayerInventory]?.VisibleInventoryItems;
            if (items == null)
            {
                DebugWindow.LogError("KryBest -> null.");
                yield break;
            }

            try
            {
                DebugWindow.LogError("Kry -> Entrou.");
                var playerPos = GameController.Player.GetComponent<Positioned>().GridPos;
                var ExpeditionStuff = GameController.EntityListWrapper.OnlyValidEntities
                    .SelectWhereF(x => x.GetHudComponent<BaseIcon>(), icon => icon != null).ToList();

                DebugWindow.LogError(string.Format("Kry -> Count. {0}",ExpeditionStuff.Count));
                DebugWindow.LogError(string.Format("Kry --------------------------------------------"));
                foreach (var stuff in ExpeditionStuff)
                   {
                    /* Vector2 p1 = new Vector2(stuff.GridPosition().X, stuff.GridPosition().Y);

                     RectangleF rec = new RectangleF(p1.X, p1.Y, 5, 5);
                     Graphics.DrawBox(rec, Color.Blue);*/

                    if (stuff.Entity.Metadata.Contains("Expedition"))
                    {
                        DebugWindow.LogError($"MetaData: {stuff.Entity.Metadata}  ---  {stuff.GridPosition} ---- {stuff.Entity.Rarity}");
                       
                    }
                   }
                

                //   DebugWindow.LogError($"Ignored entities file does not exist. Path: {pPos}");


                //DrawLine(new Vector2(pPos.X, pPos.Y), new Vector2( mPos.X, mPos.Y));
            }
            finally
            {

            }
        }


        public override void DrawSettings()
        {
            base.DrawSettings();
        }


        private void DrawLine(Vector2 pos1 , Vector2 pos2)
        {
            Graphics.DrawLine(pos1, pos2, 2, Color.Red);
        }


    }
}
