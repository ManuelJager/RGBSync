using System;

namespace RGBSync
{
    class Program
    {
        static void Main(string[] args)
        {
            var controller = new MasterController(
                new LogitechController()
                );

            var cycleAgent = new RGBCycleAgent(controller);

            Console.ReadKey();
        }
    }
}

