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
using ExileCore.PoEMemory.Elements;

namespace KryBest
{
    public class KryBest : BaseSettingsPlugin<KryBestSettings>
    {
        private Random Random { get; } = new Random();
        private Vector2 ClickWindowOffset => GameController.Window.GetWindowRectangle().TopLeft;

        private static bool once { get; set; } = true;
        private static bool Calculed { get; set; } = false;


        public override bool Initialise()
        {
            Input.RegisterKey(Settings.MathKey.Value);
            Input.RegisterKey(Settings.ResetKey.Value);
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


            var coroutineWorker = new Coroutine(MathWork(), this, "KryBest.MathExpedition");
            Core.ParallelRunner.Run(coroutineWorker);


        }

        private bool IsRunConditionMet()
        {
            if (!Input.GetKeyState(Settings.MathKey.Value) && once == true) return false;
            if (!GameController.Window.IsForeground()) return false;
            if (Input.GetKeyState(Settings.ResetKey.Value) && once == false) {

                once = true;
                Calculed = false;
                return true; 
            
            }

            return true;
        }

        private readonly List<LabelOnGround> entList = new List<LabelOnGround>();
        private IEnumerator MathWork()
        {
            once = false;
            var items = GameController.Game.IngameState.IngameUi.InventoryPanel[InventoryIndex.PlayerInventory]?.VisibleInventoryItems;
            if (items == null)
            {
                DebugWindow.LogError("KryBest -> null.");
                yield break;
            }

            try
            {

                if (Calculed == false)
                {
                    entList.Clear();
                    DebugWindow.LogError("Kry -> Entrou.");

                    var playerPos = GameController.Player.GetComponent<Positioned>().GridPos;
                    var ExpeditionStuff = GameController.Game.IngameState.IngameUi.ItemsOnGroundLabels
                        .Where(item => item != null && item.ItemOnGround.Metadata.Contains("ExpeditionRelic")).ToList();

                    DebugWindow.LogError(string.Format("Kry -> Count. {0}", ExpeditionStuff.Count));
                    DebugWindow.LogError(string.Format("Kry --------------------------------------------"));
                    foreach (var stuff in ExpeditionStuff)
                    {
                        entList.Add(stuff);

                        DebugWindow.LogError($"MetaData: {stuff.ItemOnGround.Metadata}  ---  {stuff.ItemOnGround.GridPos.X} , {stuff.ItemOnGround.GridPos.Y} ---- {stuff.ItemOnGround.Rarity} ---- ");
                        DebugWindow.LogError($"Mods on Relic:");
                        foreach (var mod in stuff.ItemOnGround.GetComponent<ObjectMagicProperties>().Mods)
                        {
                            DebugWindow.LogError($"Mod: {mod}");

                        }

                        DebugWindow.LogError(string.Format("Kry --------------------------------------------"));

                    }



                    if(entList.Count > 0)
                    {

                        foreach(var i in entList)
                        {
                            var pickButtonRect =  new SharpDX.RectangleF(i.ItemOnGround.GridPos.X, i.ItemOnGround.GridPos.X+ 10, 50, 50);
                            Graphics.DrawImage("pick.png", pickButtonRect);
                        }

                    }


                    Calculed = true;

                }

                


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
