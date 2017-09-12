using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using VRageMath;
using VRage.Game;
using Sandbox.ModAPI.Interfaces;
using Sandbox.ModAPI.Ingame;
using Sandbox.Game.EntityComponents;
using VRage.Game.Components;
using VRage.Collections;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI.Ingame;
using SETestEnv;

namespace Script
{
    partial class Program
    {

        #region ingame script start

        #region Objects requests

        public class Grid
        {
            static public void ObjectsWithTag<T>(IMyGridTerminalSystem grid, List<T> objects, string tag) where T : class, IMyTerminalBlock
            {
                var blocks = new List<IMyTerminalBlock>();
                grid.GetBlocksOfType<T>(blocks, block =>
                {
                    var parts = new List<string>(block.CustomName.Split(new char[] { '|' }));
                    return parts.Contains(tag);
                });

                objects.Clear();
                for (int i = 0, imax = blocks.Count; i < imax; i++)
                {
                    objects.Add((T)blocks[i]);
                }
            }

            static public T ObjectWithTag<T>(IMyGridTerminalSystem grid, string tag) where T : class, IMyTerminalBlock
            {
                var blocks = new List<IMyTerminalBlock>();
                bool found = false;
                grid.GetBlocksOfType<T>(blocks, block =>
                {
                    if (found)
                        return false;
                    var parts = new List<string>(block.CustomName.Split('|'));
                    found = parts.Contains(tag);
                    return found;
                });

                return found ? blocks[0] as T : null;
            }

        }

        #endregion

        #endregion // ingame script end
    }
}
