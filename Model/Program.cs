using MathNet.Numerics.Distributions;
using Microsoft.ML.Probabilistic;
using Microsoft.ML.Probabilistic.Models;
using System;
using System.Collections.Generic;

namespace Model
{
    class Program
    {
        static void Main(string[] args)
        {
            int pointsPerAgent = 10;
            int numAgents = 10;
            double agentVariance = 1;
            double actualAgentBias = 0.9d;
            double actualAgentVariance = 0.5d;

            var agentRange = new Range(numAgents);
            //var agentBiases = Variable.Array<double>(agentRange);
            var agentBias = Variable.BetaFromMeanAndVariance(1, 1);

            // set up model
            var agentDeviation = Variable.GaussianFromMeanAndPrecision(agentBias, agentVariance).Named("agent_sd");
            var agentPoints = new Range(pointsPerAgent);
            var agentSamples = Variable.Array<double>(agentPoints).Named("sample");
            //agentSamples.IsReadOnly = false;
            //agentSamples[agentPoints] = agentDeviation.ForEach(agentPoints);
           
            // setup observed values

            // from MathNet.Distributions
            var agentDist = new Normal(actualAgentBias, actualAgentVariance);
            IList<(int idx, double val)> samples = new List<(int, double)>();
            for (int i=0; i < pointsPerAgent; i++)
            {
                var sample = SampleWithConstraints(agentDist.Sample, x => (x > 0) && (x < 1));
                samples.Add((i, sample));
            }
            foreach(var sample in samples)
            {
                agentSamples[sample.idx].ObservedValue = sample.val;
                Console.Out.WriteLine(sample);
            }
            // run inference engine

            InferenceEngine engine = new InferenceEngine();
            Console.WriteLine("Inferred agent bias: " + engine.Infer(agentBias));
            Console.WriteLine("Actual agent bias: " + actualAgentBias);
            Console.WriteLine("Press any key to close..");
            Console.ReadKey();
        }

        private const int MAX_TRIES = 1000;
        private static T SampleWithConstraints<T>(Func<T> sampleFunc, Func<T, bool> constraint)
        {
            int tries = 0;
            T sample = sampleFunc();
            while (!constraint(sample))
            {
                if (tries++ > MAX_TRIES)
                    throw new ApplicationException("Failed to find a sample within constraints after " + tries + " tries.");

                sample = sampleFunc();
            }
            return sample;
        }
    }
}
