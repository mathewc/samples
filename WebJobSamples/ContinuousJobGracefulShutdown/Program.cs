using System.Reflection;
using Microsoft.Azure.WebJobs;

namespace ContinuousJobGracefulShutdown
{
    class Program
    {
        static void Main(string[] args)
        {
            JobHost host = new JobHost();

            MethodInfo methodInfo = typeof(Functions).GetMethod("GracefulShutdown");
            host.CallAsync(methodInfo);

            host.RunAndBlock();
        }
    }
}
