using SETestEnv;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VRageMath;
using ConsoleClassLibrary;

namespace console
{

    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = new System.Globalization.CultureInfo("en-US");

            TestProgrammableBlock owner = new TestProgrammableBlock();
            owner.CustomName = "DebugSrv";

            owner.CustomData = @"";

            Script.Program.FutureOwner = owner;
  
            var test = new Script.Program();

            TestTextPanel panel1 = new TestTextPanel
            {
                IsWorking = true,
                CustomName = "TestLCD",
                DetailedInfo = "Type: LCD Panel\nMax Required Input: 100 W\nCurrent Input: 100 W",

                PrivateTitle = "Working;Time Global Time: ;Cargo;Power;Inventory;Echo;Center << Damage >>;Damage;BlockCount",

                BuildIntegrity = 7200,
                MaxIntegrity = 7200
            };


            panel1.SetProperty(new TestProp<float>("FontSize", 0.8f));
            panel1.SetProperty(new TestProp<Color>("FontColor", new Color(255, 255, 255)));
            panel1.SetProperty(new TestProp<Color>("BackgroundColor", new Color(0, 0, 0)));

            test.TestGridTerminalSystem.CubeGrid.RegisterBlock(panel1);

            TestTimerBlock timer = new TestTimerBlock
            {
                CustomName = "Sequenser Timer"
            };

            test.TestGridTerminalSystem.CubeGrid.RegisterBlock(timer);


            TestBatteryBlock battery = new TestBatteryBlock
            {
                CustomName = "Battery",
                DetailedInfo =
                    "Type: Battery\n" +
                    "Max Output: 12.00 MW\n" +
                    "Max Required Input: 12.00 MW\n" +
                    "Max Stored Power: 3.00 MWh\n" +
                    "Current Input: 0 W\n" +
                    "Current Output: 1.43 MWh\n" +
                    "Stored power: 2.47 MWh\n" +
                    "Fully depleted in: 1 days",

                BuildIntegrity = 7200,
                MaxIntegrity = 7200,
                CurrentDamage = 200
            };
            test.TestGridTerminalSystem.CubeGrid.RegisterBlock(battery);


            //Console2.CreateBuffers();

            Console2.ForegroundColor = ConsoleColor.Yellow;
            string arg = Console2.ReadLine();
            while (arg != "q")
            {
                test.RunMain(arg);
                Console2.ForegroundColor = ConsoleColor.Yellow;
                Console2.WriteLine("execution finished");
                arg = Console2.ReadLine();
                // Console2.SwitchBuffer();
            }

            //test.Save();
            Console2.ReadLine();
        }

    }
}
