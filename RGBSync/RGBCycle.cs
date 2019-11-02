using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;

namespace RGBSync
{
    class RGBCycleAgentSettings
    {
        public enum ColorCycleFunction
        {
            HSL2RGB,
            SINWAVE
        }
        public ColorCycleFunction ColorMode { get; set; }

        private int freq;
        public int Freq
        {
            get => freq;
            set
            {
                freq = value;
                interval = Convert.ToInt32(Math.Round(1000 / (double)value));
            }
        }
        public double Speed { get; set; }
        private int interval;
        public int Interval => interval;

        public Func<Color> cycleFunction;
    }

    class RGBCycleAgent
    {
        public CancellationTokenSource tokenSource;
        private async Task<Color> CycleRGB_SINWAVEAsync(double t, RGBCycleAgentSettings settings)
        {
            byte red   = await Task.Run(() => (byte)Math.Round(255 * Math.Sin((Math.PI  * t * settings.Speed)                    % Math.PI)));
            byte green = await Task.Run(() => (byte)Math.Round(255 * Math.Sin(((Math.PI * t * settings.Speed) + Math.PI / 3)     % Math.PI)));
            byte blue  = await Task.Run(() => (byte)Math.Round(255 * Math.Sin(((Math.PI * t * settings.Speed) + Math.PI / 3 * 2) % Math.PI)));

            return new Color(red, green, blue);
        }
        private async Task<Color> CycleRGB_HSL2RGBAsync(double t, RGBCycleAgentSettings settings)
            => await Task.Run(() => ColorUtils.HSL2RGB(t * settings.Speed % 1));

        private async Task<Color> GetColorByModeAsync(double t, RGBCycleAgentSettings settings)
        {
            switch (settings.ColorMode)
            {
                case RGBCycleAgentSettings.ColorCycleFunction.HSL2RGB:
                    return await CycleRGB_HSL2RGBAsync(t, settings);
                case RGBCycleAgentSettings.ColorCycleFunction.SINWAVE:
                    return await CycleRGB_SINWAVEAsync(t, settings);
                default:
                    return new Color();
            }
        }
        private async Task SetColorAsync(MasterController controller, RGBCycleAgentSettings settings, TimeSpan elapsed)
        {
            var t = elapsed.TotalSeconds + (elapsed.Milliseconds / 1000);
            var color = await GetColorByModeAsync(t, settings);
            await Task.Run(() => controller.SetLighting(color));
        }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       
        private async Task CycleRGBAsync(MasterController controller, RGBCycleAgentSettings settings, CancellationToken token)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (!token.IsCancellationRequested)
            {
                await SetColorAsync(controller, settings, stopwatch.Elapsed);
                await Task.Delay(settings.Interval);
            }
        }
        public RGBCycleAgent(MasterController controller)
        {
            tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            var settings = new RGBCycleAgentSettings
            {
                Freq = 100,
                Speed = 0.1,
                ColorMode = RGBCycleAgentSettings.ColorCycleFunction.SINWAVE
            };
            Task.Run(() => CycleRGBAsync(controller, settings, token));
        }
    }
}
