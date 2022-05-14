﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShrineFox.IO
{
    public class Output
    {
        /// <summary>
        /// The path of the file to output log text to.
        /// Must be set in order for text to be logged.
        /// </summary>
        public static string LogPath { get; set; } = "";

        /// <summary>
        /// Whether to show extra info in the log.
        /// </summary>
        public static bool VerboseLogging { get; set; } = false;

        /// <summary>
        /// The form control to output text to.
        /// Must be set in order for log text to appear in form.
        /// </summary>
        public static RichTextBox LogControl { get; set; } = new RichTextBox();

        /// <summary>
        /// Logs text with a timestamp to the directory specified by LogPath.
        /// </summary>
        /// <param name="text">The text to log.</param>
        /// <param name="color">The color of the text in the console/form.</param>
        /// <param name="verboseOnly">Whether to only log text when VerboseLogging is on.</param>
        /// <param name="skipTxtFile">Whether to skip appending text to .txt file.</param>
        public static void Log(string text, ConsoleColor color = new ConsoleColor(), bool verboseOnly = false, bool skipTxtFile = false)
        {
            if ((VerboseLogging == true && verboseOnly == true) || verboseOnly == false)
            {
                string logText = $"\n[{DateTime.Now.ToString("MM/dd/yyyy HH:mm tt")}] {text}";

                if (LogPath != "" && !skipTxtFile)
                    File.AppendAllText(LogPath, logText);
                if (LogControl != new RichTextBox())
                {
                    LogControl.SuspendLayout();
                    LogControl.SelectionStart = LogControl.TextLength;
                    LogControl.SelectionLength = 0;
                    LogControl.SelectionColor = FromColor(color);
                    LogControl.AppendText(logText);
                    LogControl.ScrollToCaret();
                    LogControl.ResumeLayout();
                }
                if (color != new ConsoleColor())
                    Console.ForegroundColor = color;
                Console.WriteLine(text);
                Console.ResetColor();
                LogControl.SelectionColor = Color.Silver;
            }
        }

        // Convert ConsoleColor to Color
        private static Color FromColor(ConsoleColor c)
        {
            int cInt = (int)c;

            int brightnessCoefficient = ((cInt & 8) > 0) ? 2 : 1;
            int r = ((cInt & 4) > 0) ? 64 * brightnessCoefficient : 0;
            int g = ((cInt & 2) > 0) ? 64 * brightnessCoefficient : 0;
            int b = ((cInt & 1) > 0) ? 64 * brightnessCoefficient : 0;

            return Color.FromArgb(r, g, b);
        }
    }
}
