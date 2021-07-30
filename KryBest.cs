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




        public override void Render()
        {
            //   if (!IsRunConditionMet()) return;


            /* var coroutineWorker = new Coroutine(MathWork(), this, "KryBest.MathExpedition");
             Core.ParallelRunner.Run(coroutineWorker);*/

            // MathWork();
            IsRunConditionMet();
            try
            {

                if (Calculed == false)
                {
                    entList.Clear();
                    DebugWindow.LogError("Kry -> Entrou.");

                    var playerPos = GameController.Player.GetComponent<Positioned>().GridPos;

                    var ExpeditionStuff = GameController.Entities.Where(item => item != null && item.Metadata.Contains("Expedition") && !item.Metadata.Contains("ExpeditionRelic")).ToList();
                    var ReclicChests = GameController.Entities.Where(item => item != null && item.Metadata.Contains("ExpeditionRelic")).ToList();

                    DebugWindow.LogError(string.Format("Kry -> Count. {0}", ExpeditionStuff.Count));
                    DebugWindow.LogError(string.Format("Kry --------------------------------------------"));
                    foreach (var entity in ReclicChests)
                    {
                        entList.Add(entity);

                        DebugWindow.LogError($"MetaData: {entity.Metadata}  ---  {entity.GridPos.X} , {entity.GridPos.Y} ---- {entity.Rarity} ---- ");
                        DebugWindow.LogError($"Mods on Relic:");
                        foreach (var mod in entity.GetComponent<ObjectMagicProperties>().Mods)
                        {
                            DebugWindow.LogError($"Mod: {mod}");

                        }

                        DebugWindow.LogError(string.Format("Kry -----------------------{0}---------------------", entList.Count));

                    }






                    Calculed = true;

                }



                if (entList.Count > 0)
                {
                    var camera = GameController.IngameState.Camera;

                    foreach (var i in entList)
                    {
                        var Start = camera.WorldToScreen(i.Pos);
                        Vector3 aux = new Vector3(i.Pos.X+50,i.Pos.Y,i.Pos.Z);
                        var End =   camera.WorldToScreen(i.Pos);

                        var Start2 = new System.Numerics.Vector2(i.GridPos.X, i.GridPos.Y);
                        var End2 = new System.Numerics.Vector2(i.GridPos.X+50, i.GridPos.Y);
                        // var pickButtonRect =  new SharpDX.RectangleF(i.ItemOnGround.GridPos.X, i.ItemOnGround.GridPos.Y, 50, 50);
                        DebugWindow.LogError($"Pos: {Start},{End}  --- ");

                        Graphics.DrawBox(Start.TranslateToNum(0, 0), End.TranslateToNum(20, 20), Color.BlueViolet);
                        Graphics.DrawLine(Start.TranslateToNum(0, 0), End.TranslateToNum(20, 10),2.0f, Color.Red);
                        Graphics.DrawLine(Start2, End2, 2.0f, Color.Blue);
                    }

                }
                else
                {
                    Calculed = false;
                }




            }
            finally
            {

            }


        }

        private bool IsRunConditionMet()
        {
            if (!Input.GetKeyState(Settings.MathKey.Value) && once == true) return false;
            if (!GameController.Window.IsForeground()) return false;
            if (Input.GetKeyState(Settings.ResetKey.Value)) {

                once = true;
                Calculed = false;
                return true; 
            
            }

            return true;
        }

        private readonly List<Entity> entList = new List<Entity>();
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
                    /* var ExpeditionStuff = GameController.Game.IngameState.IngameUi.ItemsOnGroundLabels
                         .Where(item => item != null && item.ItemOnGround.Metadata.Contains("ExpeditionRelic")).ToList();*/
                    var ExpeditionStuff = GameController.Entities.Where(item => item != null && item.Metadata.Contains("ExpeditionRelic")).ToList();

                    DebugWindow.LogError(string.Format("Kry -> Count. {0}", ExpeditionStuff.Count));
                    DebugWindow.LogError(string.Format("Kry --------------------------------------------"));
                    foreach (var stuff in ExpeditionStuff)
                    {
                        entList.Add(stuff);

                        DebugWindow.LogError($"MetaData: {stuff.Metadata}  ---  {stuff.GridPos.X} , {stuff.GridPos.Y} ---- {stuff.Rarity} ---- ");
                        DebugWindow.LogError($"Mods on Relic:");
                        foreach (var mod in stuff.GetComponent<ObjectMagicProperties>().Mods)
                        {
                            DebugWindow.LogError($"Mod: {mod}");

                        }

                        DebugWindow.LogError(string.Format("Kry --------------------------------------------"));

                    }






                    Calculed = true;

                }



                if (entList.Count > 0)
                {
                    var camera = GameController.IngameState.Camera;

                    foreach (var i in entList)
                    {
                        var worldtoscreen = camera.WorldToScreen(i.Pos);

                        DebugWindow.LogError($"WorldToScreen1: {worldtoscreen.X},{worldtoscreen.Y}");

                        Graphics.DrawBox(worldtoscreen.TranslateToNum(0, 0), worldtoscreen.TranslateToNum(10, 20), Color.BlueViolet);
                        Graphics.DrawLine(worldtoscreen.TranslateToNum(-300,0), worldtoscreen.TranslateToNum(300, 0),1.0f,Color.Blue);
                    }

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






    }
}
