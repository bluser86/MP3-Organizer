using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace MP3_Organizer
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Define some locals
            List<string> allowed = new List<string>();

            allowed.Add(".flac");
            allowed.Add(".mp3");
            allowed.Add(".m4a");

            string startIn = "", destination = "";
            bool errors = false;
            
            // Print welcome message
            Console.WriteLine("Welcome to the song indexing tool. This tool indexes your music " + Environment.NewLine +
                              "specified by fileformat. Everything is based on your IDv3 tags. " + Environment.NewLine +
                              "If your tags don't match, the tool will mess up the copying. Be " + Environment.NewLine +
                              "very careful with your indexing scope!");
            Console.WriteLine(Environment.NewLine);

            // Print allowed formats
            Console.WriteLine("Allowed file formats supported are:");

            foreach (string format in allowed)
            {
                Console.WriteLine(string.Format(@" - {0}", format));
            }

            Console.Write(Environment.NewLine);

            // Display target location folder dialog
            Console.WriteLine("Define a target location for the renamed track...");
            FolderBrowserDialog targetLocationDialog = new FolderBrowserDialog();
            targetLocationDialog.Description = "Select the folder to copy the renamed tracks to";
            targetLocationDialog.ShowNewFolderButton = true;
            targetLocationDialog.RootFolder = Environment.SpecialFolder.MyComputer;

            if (targetLocationDialog.ShowDialog() != DialogResult.OK)
            {
                Console.WriteLine("User abort. Exiting...");
                Environment.Exit(0);
            }

            destination = targetLocationDialog.SelectedPath;

            Console.WriteLine(string.Format(@"Chosen target path is {0}", destination));
            Console.Write(Environment.NewLine);
            
            // Display start location folder dialog
            Console.WriteLine("Input start directory...");
            FolderBrowserDialog startInDialog = new FolderBrowserDialog();
            startInDialog.Description = "Select the folder to extract from";
            startInDialog.ShowNewFolderButton = false;
            startInDialog.RootFolder = Environment.SpecialFolder.MyComputer;

            if (startInDialog.ShowDialog() != DialogResult.OK)
            {
                Console.WriteLine("User abort. Exiting...");
                Environment.Exit(0);
            }

            startIn = startInDialog.SelectedPath;

            Console.WriteLine(string.Format(@"Chosen start path is {0}", startIn));
            Console.Write(Environment.NewLine);

            // Start organizing crap
            TrackOrganizer organizer = new TrackOrganizer(startIn, destination, allowed);
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            Console.WriteLine("Start indexing and organizing procedure...");
            Console.Write(Environment.NewLine);
            try
            {
                organizer.Index();
                organizer.Organize();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                errors = true;
            }

            Console.Write(Environment.NewLine);

            stopwatch.Stop();

            // Print some nice stats
            if (!errors)
            {
                Console.WriteLine(string.Format(@"{0} tracks were renamed and moved to the target location.", organizer.Files.Count));
                Console.WriteLine(string.Format(@"All tracks were indexed and organized in {0}.", stopwatch.Elapsed));
                Console.Write(Environment.NewLine);
            }

            // And wait for last user input
            Console.WriteLine("Done! Press any key to continue...");
            Console.ReadKey();
        }
    }
}
