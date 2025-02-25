using System;
using UnityEngine.LowLevel;

namespace Utilities
{
    public abstract class Timer
    {
        protected float _initialTime;
        protected float Time { get; set; }
        public bool IsRunning { get; private set; }

        public float Progress => Time / _initialTime;

        public Action OnTimeStart = delegate { };
        public Action OnTimeStop = delegate { };

        protected Timer(float value)
        {
            _initialTime = value;
            IsRunning = false;
        }

        public void Start()
        {
            Time = _initialTime;
            if (!IsRunning)
            {
                IsRunning = true;
                OnTimeStart.Invoke();
            }
        }

        public void Stop()
        {
            Time = 0;
            if (IsRunning)
            {
                IsRunning = false;
                OnTimeStop.Invoke();
            }
        }

        public void Resume() => IsRunning = true;
        public void Pause() => IsRunning = false;
        public abstract void Tick(float deltaTime);
    }

    public class CooldownTimer : Timer
    {
        public CooldownTimer(float value) : base(value)
        {
        }

        public override void Tick(float deltaTime)
        {
            if (IsRunning && Time > 0)
            {
                Time -= deltaTime;
            }

            if (IsRunning && Time < 0)
            {
                Stop();
            }
        }

        public bool IsFinish() => Time <= 0;
        public void Reset() => Time = 0;
        public float GetTime() => Time;
    }
}