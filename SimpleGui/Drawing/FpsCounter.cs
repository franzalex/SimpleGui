using System;
using System.Linq;

namespace SimpleGui.Drawing
{
    /// <summary>Class for counting the number of frames per second.</summary>
    public class FpsCounter
    {
        private double[] _fps;
        private int fpsIdx;
        private long last_ms;
        private bool skipNext;

        /// <summary>Initializes a new instance of the <see cref="FpsCounter"/> class.</summary>
        public FpsCounter()
            : this(30)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="FpsCounter"/> class.</summary>
        /// <param name="fpsBufferSize">
        /// The size of the FPS buffer. Higher values reduces jumpiness of the
        /// <see cref="FpsCounter.FPS"/> value.
        /// </param>
        public FpsCounter(int fpsBufferSize)
        {
            _fps = new double[fpsBufferSize];
            fpsIdx = -1;
            this.SkipNextFrame();
        }

        /// <summary>Gets the current frames per second.</summary>
        public double FPS
        {
            get { return _fps.Average(); }
        }

        /// <summary>Gets the size of the FPS averaging buffer.</summary>
        private int FpsBufferSize
        {
            get { return _fps.Length; }
        }

        /// <summary>Marks the end of a frame.</summary>
        /// <returns>The FPS of the frame that was just marked.</returns>
        public double MarkFrame()
        {
            if (skipNext)
                return this.SkipFrame();

            var now_ms = Environment.TickCount;
            // number of elapsed milliseconds now

            fpsIdx = (fpsIdx + 1) % _fps.Length;
            _fps[fpsIdx] = 1 / TimeSpan.FromMilliseconds(now_ms - last_ms).TotalSeconds;

            last_ms = Environment.TickCount;

            return _fps[fpsIdx];
        }

        /// <summmary>Marks the end of a frame that should be skipped in calculating the FPS.</summary>
        public double SkipFrame()
        {
            // set the last frame time to the current time so that the next frame calculates the delta.
            last_ms = Environment.TickCount;
            skipNext = false;
            return this.FPS;
        }

        /// <summmary>Raises the flag indicating that the next frame should be skipped in
        /// calculating the FPS.</summary>
        public void SkipNextFrame()
        {
            skipNext = true;
        }

        /// <summary>Returns a <see cref="System.String"/> that represents this instance.</summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format("{0:#,##0.0} FPS (From last {1} frames)", this.FPS, this.FpsBufferSize);
        }
    }
}