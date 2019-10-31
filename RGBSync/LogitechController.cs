using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogiLED;

namespace RGBSync
{
    public class LogitechController : IRGBController
    {
        public LogitechController()
        {
            // Initialize the LED SDK
            bool LedInitialized = LogitechGSDK.LogiLedInitWithName("SetTargetZone Sample C#");

            if (!LedInitialized)
            {
                Console.WriteLine("LogitechGSDK.LogiLedInit() failed.");
                return;
            }

            Console.WriteLine("LED SDK Initialized");

            LogitechGSDK.LogiLedSetTargetDevice(LogitechGSDK.LOGI_DEVICETYPE_ALL);
        }

        private void ColorToPercentagesTuple(Color color, out int red, out int green, out int blue)
        {
            red = Convert.ToInt32(color.red / 2.55);
            green = Convert.ToInt32(color.green / 2.55);
            blue = Convert.ToInt32(color.blue / 2.55);
        }

        public void LogiLedSetLighting(Color color)
        {
            int red, green, blue;
            ColorToPercentagesTuple(color, out red, out green, out blue);
            LogitechGSDK.LogiLedSetLighting(red, green, blue);
        }

        public void LogiLedSetLightingGlobal(Color color)
        {
            int red, green, blue;
            ColorToPercentagesTuple(color, out red, out green, out blue);
            LogitechGSDK.LogiLedSetLighting(red, green, blue);
            LogitechGSDK.LogiLedSetLightingForTargetZone(DeviceType.Mouse, 1, red, green, blue);
        }

        public void SetLighting(Color color)
        {
            LogiLedSetLightingGlobal(color);
        }

        public void LogiLedSetLightingForKeyWithKeyName(keyboardNames keyCode, Color color)
        {
            int red, green, blue;
            ColorToPercentagesTuple(color, out red, out green, out blue);
            LogitechGSDK.LogiLedSetLightingForKeyWithKeyName(keyCode, red, green, blue);
        }

        public void LogiLedSetLightingForKeyWithKeyName(Color color, params keyboardNames[] keyCodes)
        {
            int red, green, blue;
            ColorToPercentagesTuple(color, out red, out green, out blue);
            foreach (var keyCode in keyCodes)
                LogitechGSDK.LogiLedSetLightingForKeyWithKeyName(keyCode, red, green, blue);
        }
    }
}
