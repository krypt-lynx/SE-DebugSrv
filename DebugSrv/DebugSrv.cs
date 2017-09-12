#define NotUgly
#define Simulation

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
using System.Linq;

namespace Script
{
    partial class Program : TestGridProgram
    {
        public override void RunMain(string argument)
        {
            (this.Runtime as TestGridProgramRuntimeInfo).SetInstructionCount(0);
            Main(argument);
        }


        /* #override
         * InsertFileName : false
         * TrimComments : false
         */

        #region ingame script start



        #region settings

        // DebugSrv will work without a timer, but it this case it will consume operations of block caller.
        // So, if you want to debug complex script I'm recommend to use a timer for DebugSrv

        string lcdsTag = "debuglog";
        string timerTag = "debugtimer";

        // If true, DebugSrv will log any argument is as.
        // It have two applications: 
        // - compitability mode with DebugSrv v1 
        // - log all messages, recieved by antenna.
        static bool flatMode = false;

        // LCD font size. Font size of LCD will be overwrited by this value
        static float fontSize = 0.6f;
        // 
        static bool UseMonospaced = true;

        // Word Wrap. If auto will use word wrap only if DebugSrv have dedicated timer
        static WordWrap UseWordWrap = WordWrap.Auto;

        // you can setup number of lines manualy
        // in case if you using modded LCDs with non standard height
        static int numberOfLines = (int)(655 / (37 * fontSize));


        #endregion

        #region DebugSrv

        bool initialized = false;

        Text text;
        IMyTimerBlock timer = null;
        bool usingTimer = false;
        bool usingWordWrap = false;

        Queue<string> cmdQueue = null;
        List<string> log;
        int head = 0;
        Action<string> handleCommand = null;

        public static Program Current = null;

        const uint Font_Debug = 151057691;
        const uint Font_Monospace = 1147350002;

        public Program()
        {
            Current = this;

            Initialize();
            UpdateObjects();
            Message();
        }

        void Message()
        {
            UpdateObjects();

            text.Line();
            text.Line("DebugSrv v2 ready");
            text.Line("no log yet");
            if (usingTimer)
            {
                text.Line();
                text.Line(string.Format("[i] using timer \"{0}\"", timer.CustomName));
                if (!timer.IsWorking)
                    text.Line(string.Format("/!\\ timer \"{0}\"\n    is not working", timer.CustomName));
                if (!timer.IsFunctional)
                    text.Line(string.Format("/!\\ timer \"{0}\"\n    is not functional", timer.CustomName));
            }
            text.Flush();
        }

        private bool Pong(string arg)
        {
            if (arg.StartsWith("ping "))
            {
                arg = arg.Substring(5);
            }
            else
            {
                return false;
            }

            long id;

            if (long.TryParse(arg, out id))
            {
                var block = GridTerminalSystem.GetBlockWithId(id);
                if (block != null)
                {
                    block.CustomData = "pong";
                }

            }
            return true;
        }

        void Main(string argument)
        {
            if (Pong(argument))
            {
                return;
            }

            if (!initialized)
            {
                UpdateObjects();
                initialized = true;
            }

            if (!usingTimer || !timer.IsWorking)
            {
                handleCommand(argument);
            }
            else
            {
                if (argument.Length != 0)
                {
                    cmdQueue.Enqueue(argument);
                    timer.ApplyAction("TriggerNow");
                }
                else
                {
                    while ((cmdQueue.Count > 0) &&
                               (this.Runtime.MaxInstructionCount - this.Runtime.CurrentInstructionCount > 10000))
                    {
                        var cmd = cmdQueue.Dequeue();
                        handleCommand(cmd);
                    }
                }
            }
        }

        void handleCommandFlat(string cmd)
        {
            SplitAndLog(cmd);
            PrintLog();
        }

        void handleCommandFull(string cmd)
        {
            if (string.IsNullOrEmpty(cmd))
            {
                return;
            }

            switch (char.ToUpper(cmd[0]))
            {
                case 'L':
                    SplitAndLog(cmd.Substring(1, cmd.Length - 1));
                    PrintLog();
                    break;
                case 'R':
                    Initialize();
                    UpdateObjects();
                    Message();
                    break;
            }
        }

        private void SplitAndLog(string argument)
        {
            string[] parts = argument.Split('\n');
            foreach (string part in parts)
            {
                if (usingWordWrap)
                {
                    // todo:
                }
                else
                {
                    LogLine(part);
                }
            }
        }

        void LogLine(string line)
        {
            log[head] = line;
            head = (head + 1) % numberOfLines;
        }

        void UpdateObjects()
        {
            text = new Text(GridTerminalSystem, lcdsTag, fontSize, UseMonospaced ? Font_Monospace : Font_Debug);

            timer = Grid.ObjectWithTag<IMyTimerBlock>(GridTerminalSystem, timerTag);
            usingTimer = timer != null;

            switch (UseWordWrap)
            {
                case WordWrap.Yes:
                    usingWordWrap = true;
                    break;
                case WordWrap.No:
                    usingWordWrap = false;
                    break;
                case WordWrap.Auto:
                    usingWordWrap = usingTimer;
                    break;
            }
        }

        void Initialize()
        {
            handleCommand = flatMode ? (Action<string>)handleCommandFlat : handleCommandFull;

            cmdQueue = new Queue<string>();
            log = new List<string>(numberOfLines);
            for (int i = 0; i < numberOfLines; i++)
            {
                log.Add(null);
            }
        }

        void PrintLog()
        {
            text.NewFrame();

            for (int i = 0, imax = log.Count; i < imax; i++)
            {
                string line = log[(i + head) % numberOfLines];
                if (line != null)
                {
                    text.Line(line);
                }
                else
                {
                    text.Line();
                }
            }

            text.Flush();
        }

        #endregion

        #endregion // ingame script end
    }

}




