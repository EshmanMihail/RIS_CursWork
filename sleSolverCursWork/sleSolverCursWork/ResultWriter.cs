using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace sleSolverCursWork
{
    internal class ResultWriter
    {
        public static void WriteResultInFile(string filePath, double[] outer)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (double value in outer)
                {
                    writer.WriteLine(value.ToString("0.0000000"));
                }
            }
        }
    }
}
