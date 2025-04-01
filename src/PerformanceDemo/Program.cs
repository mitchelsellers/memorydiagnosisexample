/*
 * NOTE: This code is provided for demonstration purposes only. It purposely contains performance issues to aid in the demonstration of performance profiling.
 */

using System.Text;

namespace PerformanceDemo;

internal class Program
{
    private const int StringIterations = 10000;

    private static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the performance demo");
        ListCommands();
        ClearFiles();

        var command = Console.ReadLine();
        while (command.ToLower() != "x")
        {
            switch (command.ToLower())
            {
                case "list":
                    ListCommands();
                    break;
                case "bad string":
                    BadStringManipulation();
                    break;
                case "good string":
                    GoodStringManipulation();
                    break;
                case "force collection":
                    ForceCollection();
                    break;
                case "good file":
                    GoodFileWritingExample();
                    break;
                case "bad file":
                    BadFileWritingExample();
                    break;
                case "clear file":
                    ClearFiles();
                    break;
                default:
                    Console.WriteLine("Unknown command. Please try again.  Type 'list' to see all available commands.");
                    break;
            }

            Console.WriteLine("Please enter your next command:");
            command = Console.ReadLine();
        }
    }

    /// <summary>
    ///     Displays to the user a listing of commands that are available
    /// </summary>
    private static void ListCommands()
    {
        Console.WriteLine(string.Empty);
        Console.WriteLine("The following commands are available:");
        Console.WriteLine("list = Shows this listing of actions");
        Console.WriteLine("-- Garbage Collection Helpers --");
        Console.WriteLine("force collection = Forces the garbage collector to run");
        Console.WriteLine("-- String Manipulation Examples --");
        Console.WriteLine("bad string = Demonstrates bad string manipulation with 10,000 iterations");
        Console.WriteLine("good string = Demonstrates good string manipulation with 10,000 iterations");
        Console.WriteLine("-- File Writing Examples --");
        Console.WriteLine("bad file = Demonstrates bad file writing with 10,000 iterations");
        Console.WriteLine("good file = Demonstrates good file writing with 10,000 iterations");
        Console.WriteLine(string.Empty);
        Console.WriteLine("Press X to exit");
    }

    #region Garbage Collection

    /// <summary>
    ///     Forces the garbage collector to run, during the demo can be helpful to reset/reduce memory usage for additional
    ///     baseline/example testing
    /// </summary>
    private static void ForceCollection()
    {
        GC.Collect();
    }

    #endregion

    #region Strings

    /// <summary>
    ///     Demonstrates bad string manipulation by using the + operator to concatenate strings, which will result in higher
    ///     allocations, and strings that will hang around longer in memory
    /// </summary>
    /// <remarks>
    ///     Although this is an egregious example, consider the real work implications of a high-traffic site with 10+ string
    ///     concatenation per request.  This can add up quickly and cause performance issues.
    /// </remarks>
    private static void BadStringManipulation()
    {
        Console.WriteLine("Starting Bad String Manipulation");
        var myMessage = string.Empty;
        for (var i = 0; i < StringIterations; i++) myMessage += i.ToString();

        Console.WriteLine("Completed 10,000 string updates");
    }

    /// <summary>
    ///     Demonstrates good string manipulation by using the StringBuilder class to concatenate strings, which will result in
    ///     fewer allocations, and strings that will hang around less in memory
    /// </summary>
    private static void GoodStringManipulation()
    {
        Console.WriteLine("Starting Good String Manipulation");
        var myMessage = new StringBuilder();
        for (var i = 0; i < StringIterations; i++) myMessage.Append(i);
        Console.WriteLine("Completed 10,000 string updates");
    }

    #endregion

    #region Files

    /// <summary>
    ///     Writes 10,000 files to the disk using a bad file writing example, resulting in a lot of file handles being left
    ///     open
    /// </summary>
    /// <remarks>
    ///     In addition to the memory issues with the example, most of these files will also show "In use by another process"
    ///     if they are attempted to be modified.  As such, it is important to note that you may  not be able to run this
    ///     example 2x in a row
    /// </remarks>
    private static void BadFileWritingExample()
    {
        if (!Directory.Exists("badfile")) Directory.CreateDirectory("badfile");

        for (var i = 0; i < 10000; i++) WriteFileBad(i);
    }

    /// <summary>
    ///     Internal implementation of writing a single file
    /// </summary>
    /// <param name="fileNumber"></param>
    private static void WriteFileBad(int fileNumber)
    {
        try
        {
            var fs = new FileStream($"badfile/{fileNumber}-example.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            var writer = new StreamWriter(fs);
            writer.WriteLine("I'm a bad file writer");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    /// <summary>
    ///     Writes 10,000 files to the disk using a good file writing example, resulting in properly managed file handles
    /// </summary>
    private static void GoodFileWritingExample()
    {
        if (!Directory.Exists("goodfile")) Directory.CreateDirectory("goodfile");
        for (var i = 0; i < 10000; i++) WriteFileGood(i);
    }

    private static void WriteFileGood(int fileNumber)
    {
        using var fs = new FileStream($"goodfile/{fileNumber}example.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
        using var writer = new StreamWriter(fs);

        writer.WriteLine("I'm a good file writer!");
    }

    private static void ClearFiles()
    {
        if (Directory.Exists("goodfile")) Directory.Delete("goodfile", true);

        if (Directory.Exists("badfile")) Directory.Delete("badfile", true);
    }

    #endregion
}