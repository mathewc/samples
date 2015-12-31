using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

namespace ContinuousJobGracefulShutdown
{
    public static class Functions
    {
        [NoAutomaticTrigger]
        public static async Task GracefulShutdown(CancellationToken cancellationToken)
        {
            // Simulate a long running function, passing in the CancellationToken to
            // all async operations we initiate. 
            // We can also monitor CancellationToken.IsCancellationRequested in our code
            // if our code is iterative.
            try
            {
                await Task.Delay(TimeSpan.FromMinutes(10), cancellationToken);
            }
            catch (TaskCanceledException)
            {
                Shutdown().Wait();
            }

            Console.WriteLine("Function completed succesfully.");        
        }

        private static async Task Shutdown()
        {
            // simulate some cleanup work to show that the Continuous job host
            // will wait for the time configured via stopping_wait_time before
            // killing the process. The default value for stopping_wait_time is 5 seconds,
            // so this code will demonstrate that our override in settings.job
            // is functional.
            Console.WriteLine("Function has been cancelled. Performing cleanup ...");
            await Task.Delay(TimeSpan.FromSeconds(30));

            Console.WriteLine("Function was cancelled and has terminated gracefully.");
        }
    }
}
