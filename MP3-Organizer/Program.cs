using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace MP3_Organizer
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            List<string> allowed = new List<string>();

            allowed.Add(".flac");
            allowed.Add(".mp3");
            allowed.Add(".m4a");

            string startIn;
            string destination = @"D:\Music\";

            Console.WriteLine("Input start directory: ");
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "Select the folder to extract from";
            dialog.ShowNewFolderButton = false;
            dialog.RootFolder = Environment.SpecialFolder.MyComputer;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                startIn = dialog.SelectedPath;

                TrackOrganizer organizer = new TrackOrganizer(startIn, destination, allowed);

                organizer.Index();
                organizer.Organize();

                Console.WriteLine("Done! Press any key to continue...");
                Console.ReadKey();
            }
            else
            {
                Environment.Exit(0);
            }
        }
    }
}
