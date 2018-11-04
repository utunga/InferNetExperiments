
using CommandLine;

namespace Model
{
    public class Options
    {

        [Option('a', "nagents", Default=10)]
        public int NAgents { get; set; }

        [Option('p', "npoints", Default = 10)]
        public int NPoints { get; set; }

        [Option('c', "nclaims", Default = 100)]
        public int NClaims { get; set; }

    }
}