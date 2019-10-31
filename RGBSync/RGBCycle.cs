using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;

namespace RGBSync
{
    class RGBCycleAgentSettings
    {
        private int _freq;
        public int freq
        {
            get => _freq;
            set
            {
                _freq = value;
                _interval = Convert.ToInt32(Math.Round(1000 / (double)value));
            }
        }
        public double speed { get; set; }
        private int _interval { get; set; }
        public int interval => _interval;
    }

    class RGBCycleAgent
    {
        public CancellationTokenSource tokenSource;
        private async Task<Color> CycleRGB(double t, RGBCycleAgentSettings settings)
            => await Task.Run(() => ColorUtils.HSL2RGB(t * settings.speed % 1, 1, 0.5));
        private async Task SetColorAsync(MasterController controller, RGBCycleAgentSettings settings, TimeSpan elapsed)
        {
            var t = elapsed.TotalSeconds + (elapsed.Milliseconds / 1000);
            var color = await Task.Run(() => CycleRGB(t, settings));
            await Task.Run(() => controller.SetLighting(color));
        }
        private async Task CycleRGBAsync(MasterController controller, RGBCycleAgentSettings settings, CancellationToken token)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (!token.IsCancellationRequested)
            {
                await SetColorAsync(controller, settings, stopwatch.Elapsed);
                await Task.Delay(settings.interval);
            }
        }
        public RGBCycleAgent(MasterController controller)
        {
            tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            var settings = new RGBCycleAgentSettings
            {
                freq = 100,
                speed = 0.1
            };
            Task.Run(() => CycleRGBAsync(controller, settings, token));
        }
    }
}
