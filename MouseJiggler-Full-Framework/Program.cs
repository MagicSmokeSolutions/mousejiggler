#region header

// MouseJiggler - Program.cs
// 
// Created by: Alistair J R Young (avatar) at 2021/01/22 4:12 PM.
// Updates by: Dimitris Panokostas (midwan)

#endregion

#region using

using System;
using System.Threading;
using System.Windows.Forms;

using ArkaneSystems.MouseJiggler.Properties;

#endregion

namespace ArkaneSystems.MouseJiggler
{
  public static class Program
  {
    /// <summary>
    ///     The main entry point for the application.
    /// </summary>
    [STAThread]
    public static int Main(string[] args)
    {
      // Attach to the parent process's console so we can display help, version information, and command-line errors.
      Helpers.AttachConsole();

      // Ensure that we are the only instance of the Mouse Jiggler currently running.
      var instance = new Mutex(false, "single instance: ArkaneSystems.MouseJiggler");

      try
      {
        if (instance.WaitOne(0))

        // Parse arguments and do the appropriate thing.
        {
#if false
          return GetCommandLineParser().Invoke(args);
#endif
          return RootHandler(false, Settings.Default.MinimizeOnStartup, Settings.Default.ZenJiggle, Settings.Default.JigglePeriod);
        }
        else
        {
          Console.WriteLine(@"Mouse Jiggler is already running. Aborting.");

          return 1;
        }
      }
      finally
      {
        instance.Close();

        // Detach from the parent console.
        Helpers.FreeConsole();
      }
    }

    private static int RootHandler(bool jiggle, bool minimized, bool zen, int seconds)
    {
      // Prepare Windows Forms to run the application.
      // Application.SetHighDpiMode(HighDpiMode.SystemAware);
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      // Run the application.
      var mainForm = new MainForm(jiggle,
          minimized,
          zen,
          seconds);

      Application.Run(mainForm);

      return 0;
    }

#if false
    private static void ShowHelp()
    {
      Console.WriteLine("Virtually jiggles the mouse, making the computer seem not idle.");
      Console.WriteLine("");
      Console.WriteLine("Usage:");
      Console.WriteLine("  MouseJiggler-Framework [options]");
      Console.WriteLine("");
      Console.WriteLine("Options:");
      Console.WriteLine("  -j, --jiggle               Start with jiggling enabled. [default: False]");
      Console.WriteLine("  -m, --minimized            Start minimized. [default: False]");
      Console.WriteLine("  -z, --zen                  Start with zen (invisible) jiggling enabled. [default: False]");
      Console.WriteLine("  -s, --seconds <seconds>    Set X number of seconds for the jiggle interval. [default: 60]");
      Console.WriteLine("  --version                  Show version information");
      Console.WriteLine("  -?, -h, --help             Show help and usage information");
    }

    private static RootCommand GetCommandLineParser()
    {
      // Create root command.
      var rootCommand = new RootCommand
      {
        Description = "Virtually jiggles the mouse, making the computer seem not idle.",
        Handler =
              CommandHandler.Create(new Func<bool, bool, bool, int, int>(RootHandler))
      };

      // -j --jiggle
      Option optJiggling = new Option(new string[] { "--jiggle", "-j"}, "Start with jiggling enabled.")
      {
        Argument = new Argument<bool>(() => false)
      };
      rootCommand.AddOption(optJiggling);

      // -m --minimized
      Option optMinimized = new Option( new string[]{ "--minimized", "-m" }, "Start minimized.")
      {
        Argument = new Argument<bool>(() => Settings.Default.MinimizeOnStartup)
      };
      rootCommand.AddOption(optMinimized);

      // -z --zen
      Option optZen = new Option(new string[] { "--zen", "-z" }, "Start with zen (invisible) jiggling enabled.")
      {
        Argument = new Argument<bool>(() => Settings.Default.ZenJiggle)
      };
      rootCommand.AddOption(optZen);

      // -s:60 --seconds:60
      var optPeriod = new Option<int>(new string[] { "--seconds", "-s" }, "Set X number of seconds for the jiggle interval.")
      {
        Argument = new Argument<int>(() => Settings.Default.JigglePeriod)
      };
      optPeriod.AddValidator(p => p.GetValueOrDefault<int>() < 1 ? "Period cannot be shorter than 1 second." : null);
      optPeriod.AddValidator(p =>
          p.GetValueOrDefault<int>() > 10800 ? "Period cannot be longer than 10800 seconds." : null);
      rootCommand.AddOption(optPeriod);

      // Build the command line parser.
      return rootCommand;
    }
#endif
  }
}