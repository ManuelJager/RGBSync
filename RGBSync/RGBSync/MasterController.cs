using System.Collections.Generic;

namespace RGBSync
{
    class MasterController : IRGBController
    {
        private List<IRGBController> controllers;

        public void add(IRGBController controller) => controllers.Add(controller);

        public void SetLighting(Color color)
        {
            foreach (IRGBController controller in controllers)
                controller.SetLighting(color);
        }

        public MasterController(params IRGBController[] controllers)
        {
            this.controllers = new List<IRGBController>();
            foreach (IRGBController controller in controllers)
                this.controllers.Add(controller);
        }
    }
}
