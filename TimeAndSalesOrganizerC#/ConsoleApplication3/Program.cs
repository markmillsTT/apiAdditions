using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TTAPI_Init_Worker_UL
{
    class Program
    {
        static void Main(string[] args)
        {

            // Dictates whether TT API will be started on its own thread
            bool startOnSeparateThread = true;

            if (startOnSeparateThread)
            {
                // Start TT API on a separate thread
                TTAPIFunctions tf = new TTAPIFunctions();
                Thread workerThread = new Thread(tf.Start);
                workerThread.Name = "TT API Thread";
                workerThread.Start();

                // Insert other code here that will run on this thread
                BeginInterpOfTimeAndSalesData();
            }
            else
            {
                // Start the TT API on the same thread
                using (TTAPIFunctions tf = new TTAPIFunctions())
                {
                    tf.Start();
                }
            }
        }

        private static void BeginInterpOfTimeAndSalesData()
        {
            
        }
    }
}