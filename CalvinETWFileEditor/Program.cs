
using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using Microsoft.Diagnostics.Tracing.Session;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace TraceEventSamples
{
    /// <summary>
    /// In this trivial example, we show how to use the ETWReloggerTraceEventSource filter out some
    /// events in an ETL file.    It is basically the KernelAndClrFile sample with 'FilterData' 
    /// transformation in between, which only allows a handful of events through based on its own
    /// criteria.  
    /// </summary>
    internal class SimpleFileRelogger
    {
        /// <summary>
        /// Where all the output goes.  
        /// </summary>
        //private static TextWriter Out = AllSamples.Out; // Console.Out

        public static void Main()
        {
            var inputFileName = "PerfViewData.etl";
            var outFileName = "PerfviewData_Output.etl";
            double startMsec = 2956.680;
            double stopMsec = 3163.598;

            // Create some data by listening for 10 seconds
            // DataCollection(inputFileName);

            FilterData(inputFileName, outFileName, startMsec, stopMsec);

            // DataProcessing(outFileName);
        }

        /// <summary>
        /// This routine shows how to use ETWReloggerTraceEventSource take a ETL file (inputFileName)
        /// and filter out events to form another ETL file (outputFileName).  
        /// 
        /// For this example we filter out all events that are not GCAllocationTicks 
        /// </summary>
        private static void FilterData(string inputFileName, string outFileName, double startMsec, double stopMsec)
        {
            // Open the input file and output file.   You will then get a callback on every input event,
            // and if you call 'WriteEvent' you can copy it to output file.   
            // In our example we only copy large object 
            using (var relogger = new ETWReloggerTraceEventSource(inputFileName, outFileName))
            {
                // Here we register callbacks for data we are interested in and further filter by.  

                // log all events in a specific window
                relogger.UnhandledEvents += delegate (TraceEvent data)
                {
                    if (data.TimeStampRelativeMSec > startMsec && stopMsec > data.TimeStampRelativeMSec)
                    {
                        relogger.WriteEvent(data);
                    }
                };

                //source.UnhandledEvents += delegate (TraceEvent data)
                //{
                //    // To avoid 'rundown' events that happen in the beginning and end of the trace filter out things during those times
                //    if (data.TimeStampRelativeMSec < 1000 || 9000 < data.TimeStampRelativeMSec)
                //        return;

                //    Out.WriteLine("GOT UNHANDLED EVENT: " + data.Dump());
                //};

#if false       // Turn on to get debugging on unhandled events.  
                relogger.UnhandledEvents += delegate(TraceEvent data)
                {
                    Console.WriteLine("Unknown Event " + data);
                };
#endif 
                relogger.Process();
            }
        }
    }
}
