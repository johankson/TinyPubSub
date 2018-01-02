using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xunit;

namespace TinyPubSub.Fody.Tests
{
    public class EnvironmentsTest
    {
        public EnvironmentsTest()
        {
        }

        [Fact]
        public void StartPeverifyTest()
        {
            var processStartInfo = new ProcessStartInfo("peverify")
            {
               // Arguments = $"\"{assemblyPath}\" /hresult /nologo /ignore={string.Join(",", ignoreCodes)}",
               // WorkingDirectory = workingDirectory,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true
            };

            using (var process = Process.Start(processStartInfo))
            {
                var output = process.StandardOutput.ReadToEnd();
                Console.WriteLine(output);

                Assert.True(true);
            }
        }

        [Fact]
        public void EnumerateFilesTest()
        {
            var programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
          
            foreach(var file in Directory.GetFiles(programFilesPath))
            {
                Console.WriteLine(file);
            }
        }

        [Fact]
        public void FindToolsTest()
        {
            var programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            var windowsSdkDirectory = Path.Combine(programFilesPath, @"Microsoft SDKs\Windows");
            if (!Directory.Exists(windowsSdkDirectory))
            {
                throw new Exception("Could not find SDK directory");
            }

            var peverifyPath = Directory.EnumerateFiles(windowsSdkDirectory, "peverify.exe", SearchOption.AllDirectories)
                .Where(x => !x.ToLowerInvariant().Contains("x64"))
                .OrderByDescending(x =>
                {
                    var info = FileVersionInfo.GetVersionInfo(x);
                    return new Version(info.FileMajorPart, info.FileMinorPart, info.FileBuildPart);
                })
                .FirstOrDefault();
            
            if (peverifyPath == null)
            {
                throw new Exception("Could not find peverify.exe");
            }

        }
    }
}
