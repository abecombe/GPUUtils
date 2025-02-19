
namespace Abecombe.GPUUtils
{
    public static class GPUStaticValues
    {
        // for AsyncCompute
        public static bool SimulationUseBuffer1 = true;
        public static bool RenderingUseBuffer1 => !SimulationUseBuffer1;

        public static void SwapSimulationRenderingBuffer()
        {
            SimulationUseBuffer1 = !SimulationUseBuffer1;
        }
    }
}