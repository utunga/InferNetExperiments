
using CommandLine;

namespace LinqInferTest
{
    public class Options
    {
        [Option('u', "url")]
        public string Url { get; set; }

        [Option('o', "out")]
        public string OutputPath { get; set; }

        [Option('m', "mode")]
        public string Mode { get; set; }

        [Option('a', "nagents", Default=10)]
        public int NAgents { get; set; }

        [Option('p', "npoints", Default = 10)]
        public int NPoints { get; set; }

        [Option('c', "nclaims", Default = 100)]
        public int NClaims { get; set; }

    }
}