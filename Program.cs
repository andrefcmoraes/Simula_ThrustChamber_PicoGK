// =============================================================================
// ENTRY POINT: LEAP 71 / PICOGK LIQUID ROCKET ENGINE THRUST CHAMBER (L* = 0.85)
// Reference STEP: "ThrustChamber.step" ("conjunto_motor_injetor_Lstar_085")
// Run Preview (0.220 mm):  dotnet run -c Release -- 0.220 --exit
// Run Ultra-HD (0.075 mm): dotnet run -c Release -- 0.075 --exit
// =============================================================================

using System;
using System.Globalization;
using System.IO;
using PicoGK;

namespace Leap71.ThrustChamberPicoGK
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Resolution in mm (0.075f = Ultra-HD ~7.65M triangles; use 0.220f for fast ~35MB visualization preview)
            float fVoxelSizeMM = 0.075f;
            if (args.Length > 0 && float.TryParse(args[0], NumberStyles.Float, CultureInfo.InvariantCulture, out float fCustomVoxel))
            {
                fVoxelSizeMM = Math.Clamp(fCustomVoxel, 0.02f, 0.50f);
            }

            string strOutputDir = Path.Combine(AppContext.BaseDirectory, "Output_STL");
            Directory.CreateDirectory(strOutputDir);
            string strLogFile = Path.Combine(strOutputDir, "PicoGK.log");

            Console.WriteLine("==================================================================");
            Console.WriteLine(" LEAP 71 / PICOGK — REGENERATIVE LIQUID ROCKET THRUST CHAMBER");
            Console.WriteLine($" Voxel Size: {fVoxelSizeMM:F3} mm | Output: {strOutputDir}");
            Console.WriteLine("==================================================================");

            bool bEndWithTask = Array.Exists(args, a => a.Equals("--exit", StringComparison.OrdinalIgnoreCase));

            try
            {
                Library.Go(
                    fVoxelSizeMM,
                    ThrustChamberRunner.Run,
                    strLogFile,
                    bEndAppWithTask: bEndWithTask
                );
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[PicoGK Error] {ex.Message}");
                throw;
            }
        }
    }
}
