using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TradingTechnologies.TTAPI;

namespace TTAPI_Init_Worker_UL
{
    class Program
    {
        static void Main(string[] args)
        {
            string ttUserId = "MARK";
            string ttPassword = "12345678";
            ProductKey currProduct = new ProductKey("CME", "FUTURE", "ES");
            string currProductDate = "Mar16";

            // Dictates whether TT API will be started on its own thread
            bool startOnSeparateThread = true;

            if (startOnSeparateThread)
            {
                // Start TT API on a separate thread
                TTAPIFunctions tf = new TTAPIFunctions(ttUserId, ttPassword);
                tf.productKey = currProduct;
                tf.productDate = currProductDate;
                Thread workerThread = new Thread(tf.Start);
                workerThread.Name = "TT API Thread";
                workerThread.Start();

                // Insert other code here that will run on this thread
                
            }
            else
            {
                // Start the TT API on the same thread
                using (TTAPIFunctions tf = new TTAPIFunctions(ttUserId, ttPassword))
                {
                    tf.Start();
                }
            }
        }
    }
}