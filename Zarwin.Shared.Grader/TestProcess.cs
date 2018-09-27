using System.Diagnostics;

namespace Zarwin.Shared.Grader
{
    public class TestProcess
    {
        private readonly Process _process;

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
        }

        public void Run()
        {
            _process.Start();
            _process.BeginOutputReadLine();
            _process.BeginErrorReadLine();
            _process.WaitForExit();
        }

        public event DataReceivedEventHandler OutputDataReceived
        {
            add { _process.OutputDataReceived += value; }
            remove { _process.OutputDataReceived -= value; }
        }
    }
}
