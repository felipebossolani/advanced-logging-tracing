using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advanced_logging_tracing
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"Creator: Felipe Bossolani - fbossolani[at]gmail.com");
            Console.WriteLine(@"Examples based on: http://returnsmart.blogspot.com/2015/10/mcsd-programming-in-c-part-14-70-483.html");
            Console.WriteLine("Choose a Method: ");
            Console.WriteLine("01- Debug Class");
            Console.WriteLine("02- Trace Source");
            Console.WriteLine("03- Trace Source to Text File");
            Console.WriteLine("04- Windows Event Log");
            Console.WriteLine("05- Read Performance Counters");
            

            int option = 0;
            int.TryParse(Console.ReadLine(), out option);

            switch (option)
            {
                case 1:
                    {
                        DebugExample.HowToUseTheDebugClass();
                        break;
                    }
                case 2:
                    {
                        TraceSourceExample.HowToUseTheTraceSourceClass();
                        break;
                    }
                case 3:
                    {
                        TraceSourceExample.HowToUseTheTraceSourceClassToFile();                        
                        break;
                    }
                case 4:
                    {
                        EventLogExample.HowToWriteToEventLog();
                        break;
                    }
                case 5:
                    {
                        ReadPerformanceCounters.HowToReadPerformanceCounters();
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Invalid option...");
                        break;
                    }
                    
            }
            Console.ReadLine();
        }

        class ReadPerformanceCounters
        {
            public static void HowToReadPerformanceCounters()
            {
                System.Console.WriteLine("Press escape to exit.");
                using (PerformanceCounter pc = new PerformanceCounter("Memory", "Available bytes"))
                {
                    string text = "Available memory: ";
                    System.Console.WriteLine(text);
                    do
                    {
                        System.Console.WriteLine(pc.RawValue);
                        System.Console.SetCursorPosition(text.Length, System.Console.CursorTop);
                    } while (System.Console.ReadKey(true).Key != System.ConsoleKey.Escape);
                }
            }
        }

        public static class EventLogExample
        {
            readonly static string sourceName = "Felipe-Dev";

            public static void HowToWriteToEventLog()
            {
                
                if (!EventLog.SourceExists(sourceName))
                {
                    EventLog.CreateEventSource(sourceName, "MyNewLog");
                    Console.WriteLine("CreatedEventSource");
                    Console.WriteLine("Please restart your application");
                    Console.ReadKey();
                    return;
                }

                EventLog myLog = new EventLog()
                {
                    Source = sourceName
                };
                myLog.WriteEntry("Log event triggered!");
                Console.WriteLine("Finished, see Windows Event Log for details.");
            }
        }

        class DebugExample
        {
            public static void HowToUseTheDebugClass()
            {
                Debug.WriteLine("Here we go!");
                Debug.Indent();
                int i = 1 + 2;
                Debug.Assert(i == 3);
                Debug.WriteLine(i > 0, "i is greater than zero");
            }
        }

        class TraceSourceExample
        {
            public static void HowToUseTheTraceSourceClass()
            {
                TraceSource traceSource = new TraceSource("myTraceSource", SourceLevels.All);

                traceSource.TraceInformation("Tracing app");
                traceSource.TraceEvent(TraceEventType.Critical, 0, "Critical Trace");
                traceSource.TraceData(TraceEventType.Information, 1, new object[] { "a", "b", "c" });
                traceSource.Flush();
                traceSource.Close();
            }

            public static void HowToUseTheTraceSourceClassToFile()
            {
                Stream outputFile = File.Create(@"C:\temp\traceFile.txt");
                TextWriterTraceListener textListener = new TextWriterTraceListener(outputFile);

                TraceSource traceSource = new TraceSource("myTraceSource", SourceLevels.All);
                traceSource.Listeners.Clear();
                traceSource.Listeners.Add(textListener);

                traceSource.TraceInformation("Tracing app");
                traceSource.TraceEvent(TraceEventType.Critical, 0, "Critical Trace");
                traceSource.TraceData(TraceEventType.Information, 1, new object[] { "a", "b", "c" });
                traceSource.Flush();
                traceSource.Close();

                Console.WriteLine(@"See c:\temp\traceFile.txt");
            }
        }
    }
}
