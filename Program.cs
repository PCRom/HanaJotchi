using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace HanaJotchi
{
    public static class Program
    {
        public static VPScreen VirtualPet;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Enable visual styles and set compatible text rendering for the application.
            Application.EnableVisualStyles();

            // Probably should disable this for better performance, but it can cause some weird font rendering issues if not enabled, so we'll just leave it on.
            Application.SetCompatibleTextRenderingDefault(false);

            // Sleep for a short time to allow the application to initialize properly before we start doing file operations, this is mostly to prevent any potential issues with the application not being fully ready when we try to create or edit the config file.
            Thread.Sleep(500);

            // Define the path to the configuration file that will be used to determine if we should use a private server or not. This file will be created or edited when the user holds shift while starting the application, if the file exists and has content we will assume the user wants to use a private server, if the file doesn't exist or is empty we will assume the user wants to use the official servers.
            string path = Path.Combine(Application.StartupPath, "HanaJotchiPrivate.cfg");

            //This is used to make it easier for an end user to change servers, most people just edit the config directly.
            if ((Control.ModifierKeys & Keys.Shift) != 0)
            {
                // Define the lines that will be written to the configuration file, these lines will be used to provide information to the user about the private server and how to use it, as well as the default API endpoint that will be used if the user doesn't change it. The user can edit this file to change the API endpoint or to disable the private server by clearing out the contents of the file.
                string[] lines = new string[]
                {
                    "// Enable HanaJotchi Private Server",
                    "// This is a private server version of the HanaJotchi game.",
                    "// It allows you to play the game without connecting to the official servers.",
                    "// Please note that this version may not receive updates or support. Use at your own risk!",
                    "// Hold-Shift Startup; Leave Default or Empty all lines and save if you want to disable this feature!",
                    "server=http://api.67..domain.com/api-link-dir/example/api/some_folder/etc"
                };

                //Output to file, if the file already exists it will be overwritten, if not it will be created.
                File.WriteAllLines(path, lines);

                // Get the initial length of the file before the user edits it.
                FileInfo fileInfo = new FileInfo(path);

                // If the file doesn't exist or is empty, we won't use a private server, if the file exists and has content we will assume the user wants to use a private server, if the file exists and has content but is not changed we will assume the user doesn't want to use a private server.
                long initialLength = fileInfo.Length;

                //Start a text editor with the contents
                Process notepad = Process.Start("notepad.exe", path);
                notepad.WaitForExit();
                
                //Refresh to get a new size,
                fileInfo.Refresh();

                // Check if the file is empty (if it was cleared out on purpose lets not use a private server)
                if ((fileInfo.Exists && (fileInfo.Length == 0)) || (fileInfo.Exists && (fileInfo.Length == initialLength)))
                {
                    File.Delete(path);
                }
            }

            // Boot the main game screen, should we use a private server or not is determined by the existence of the config file, if it exists we will use a private server, if it doesn't exist we will use the official servers.
            VirtualPet = new VPScreen(File.Exists(path));

            // Start the main game loop.
            Application.Run(VirtualPet);
        }
    }
}
