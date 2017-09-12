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

        #region Output

        class Text
        {
            List<IMyTextPanel> lcds = null;
            StringBuilder outputBuffer = new StringBuilder();

            public Text(IMyGridTerminalSystem grid, string tag, float fontSize, uint fontHash)
            {
                lcds = new List<IMyTextPanel>();
                Grid.ObjectsWithTag<IMyTextPanel>(grid, lcds, tag);
                foreach (var lcd in lcds)
                {
                    lcd.ApplyAction("OnOff_On");
                    lcd.SetValue<float>("FontSize", fontSize);
                    lcd.SetValue<long>("Font", fontHash);
                }
            }

            public void NewFrame()
            {
                outputBuffer.Clear();
            }

            public void Write(string str)
            {
                outputBuffer.Append(str);
            }

            public void Line(string str)
            {
                outputBuffer.AppendLine(str);
            }

            public void Line()
            {
                outputBuffer.AppendLine();
            }

            public void Flush()
            {
                for (int i = 0, imax = lcds.Count; i < imax; i++)
                {
                    IMyTextPanel lcd = lcds[i];
                    lcd.WritePublicText(outputBuffer.ToString());
                    lcd.ShowTextureOnScreen();
                    lcd.ShowPublicTextOnScreen();
                }

            }
        }

        #endregion

        #endregion // ingame script end
    }
}