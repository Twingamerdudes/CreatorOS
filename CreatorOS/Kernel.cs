using Sys = Cosmos.System;
using SipaaKernelV3.Graphics;
using Mos.Assets;
using Mos.UI;
using Cosmos.Core.Memory;
using Mos.Applications;
using CosmosTTF;
using Mos.Tools;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Mos
{
    public class Kernel : Sys.Kernel
    {
        SipaVGA vga;
        Taskbar taskbar;
        public int delta, frames, fps;
        public static uint mousex, mousey;
        TextRenderer TextRenderer = new TextRenderer();
        Sys.FileSystem.CosmosVFS fs = new Sys.FileSystem.CosmosVFS();
        public void NewWindow(Button button)
        {
            WindowManager.AddWindow(new Terminal(vga, "Terminal", 400, 300));
        }
        public void NewNotepadWindow(Button button)
        {
            WindowManager.AddWindow(new Notepad(vga, "Notepad", 400, 300, ""));
        }
        protected override void BeforeRun()
        {
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);
            if(!File.Exists(@"0:\root\config.cs"))
            {
                Console.Clear();
                Console.WriteLine("CreatorOS Setup");
                Console.WriteLine("It looks like you have not setup CreatorOS yet. Completing setup.");
                Console.WriteLine("Username:");
                string name = Console.ReadLine();
                Console.WriteLine("Password:");
                string pass = Console.ReadLine();
                Console.WriteLine("Confirm Password:");
                string passConfirm = "";
                while(pass != passConfirm)
                {
                    passConfirm = Console.ReadLine();
                    if(pass != passConfirm)
                    {
                        Console.WriteLine("Passwords do not match. Try again.");
                    }
                }
                Console.WriteLine("Creating Directories");
                Directory.CreateDirectory(@"0:\home");
                Directory.CreateDirectory(@"0:\root");
                File.Create(@"0:\root\config.cs");
                File.Create(@"0:\home\welcome.txt");
                Console.WriteLine("Writing Config");
                File.WriteAllText(@"0:\root\config.cs", "Username: " + name + "\nPassword: " + pass);
                File.WriteAllText(@"0:\home\welcome.txt", "Welcome to CreatorOS!");
                Console.WriteLine("OS Setup complete");
                Console.WriteLine("Press any key to contiune installation of utilities");
                Console.ReadKey();
            }
            if (!Directory.Exists(@"0:\Applications"))
            {
                Console.WriteLine("Applications Directory does not exist, creating directory");
                Directory.CreateDirectory(@"0:\Applications");
            }
            vga = new SipaVGA(new SVGAMode(800, 600));
            fps = 0;
            TTFManager.RegisterFont("Roboto", Data.Roboto_ttf);
            taskbar = new Taskbar(vga, 50);
            Sys.MouseManager.ScreenWidth = 800;
            Sys.MouseManager.ScreenHeight = 600;
            Button terminal = new(vga, "Terminal", 50, NewWindow);
            taskbar.AddButton(terminal);
            Button notepad = new(vga, "Notepad", 50, NewNotepadWindow);
            taskbar.AddButton(notepad);
        }

        protected override void Run()
        {
            vga.Clear();
            vga.DrawFilledRectangle(0, 0, 800, 600, (uint)Color.MakeArgb(255, 150, 40, 40));
            taskbar.Render();
            TextRenderer.DrawTTFString(6, 6, "FPS: " + fps, vga, "Roboto", System.Drawing.Color.White);
            WindowManager.Update();
            mousex = Sys.MouseManager.X;
            mousey = Sys.MouseManager.Y;
            Alpha.DrawImageAlpha(Data.Cursor, mousex, mousey, (uint)Color.MakeArgb(0, 0, 0, 0), vga);
            if (delta != Cosmos.HAL.RTC.Second)
            {
                delta = Cosmos.HAL.RTC.Second;
                fps = frames;
                frames = 0;
            }
            frames++;
            vga.Update();
            Heap.Collect();
        }
    }
}
