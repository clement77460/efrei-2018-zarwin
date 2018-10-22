using System;
using System.Diagnostics;

namespace Zarwin.Shared.Grader
{
    public class TestProcess
    {
        private readonly Process _process;

        public event DataReceivedEventHandler OutputDataReceived;
        public event DataReceivedEventHandler ErrorDataReceived;

        public bool ForwardDataAndError { get; set; }

        public TestProcess(string solutionDirectory, bool noBuild, params string[] otherParameters)
        {
            _process = new Process();
            _process.StartInfo.WorkingDirectory = solutionDirectory;
            _process.StartInfo.FileName = "dotnet";
            _process.StartInfo.Arguments = "test ";
            if (noBuild)
                _process.StartInfo.Arguments += "--no-build ";
            _process.StartInfo.Arguments += string.Join(" ", otherParameters);

            _process.StartInfo.RedirectStandardOutput = true;
            _process.StartInfo.RedirectStandardError = true;

            _process.OutputDataReceived += ProcessDataReceived;
            _process.ErrorDataReceived += ProcessErrorData;
        }

        private void ProcessDataReceived(object sender, DataReceivedEventArgs e)
        {
            OutputDataReceived?.Invoke(sender, e);
            if (ForwardDataAndError && e.Data != null)
                Console.WriteLine(e.Data);
        }

        private void ProcessErrorData(object sender, DataReceivedEventArgs e)
        {
            ErrorDataReceived?.Invoke(sender, e);
            if (ForwardDataAndError && e.Data != null)
                Console.Error.WriteLine(e.Data);
        }

        public void Run()
        {
            _process.Start();
            _process.BeginOutputReadLine();
            _process.BeginErrorReadLine();
            _process.WaitForExit();
        }
    }
}
