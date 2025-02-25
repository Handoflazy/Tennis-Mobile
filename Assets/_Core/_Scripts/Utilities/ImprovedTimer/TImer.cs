using System;
using UnityEngine;

namespace Utilities.ImprovedTimers
{
    public abstract class Timer : IDisposable
    {
       
        public float CurrentTime { get;protected set; }
        public abstract void Tick();
        protected float _initialTime;
        
        public float Progress => Mathf.Clamp(CurrentTime / _initialTime,0,1);
        public Action OnTimerStart = delegate { };
        public Action OnTimerStop = delegate { };
        
        
        public bool IsRunning { get; private set; }

        public Timer(float initialTime)
        {
            _initialTime = initialTime;
        }

        public void Start()
        {
            CurrentTime = _initialTime;
            if (!IsRunning)
            {
                IsRunning = true;
                TimerManager.RegisterTimer(this);
                OnTimerStart.Invoke();
            }
            
        }
        public void Stop()
        {
            if (IsRunning) {
                IsRunning = false;
                TimerManager.DeregisterTimer(this);
                OnTimerStop.Invoke();
            }
        }
        
        public abstract bool IsFinished { get; }

        public void Resume() => IsRunning = true;
        public void Pause() => IsRunning = false;
        
        public virtual void Reset() => CurrentTime = _initialTime;

        public virtual void Reset(float newTime)
        {
            _initialTime = newTime;
            Reset();
        }
        bool _disposed;
        
        ~Timer()
        {
            Dispose(false);
        }
        // call dispose to ensure deregistation of the timer from the timemanager
        // when the consumer is done with timer or begin destroyed
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
            {
                TimerManager.DeregisterTimer(this);
            }
            _disposed = true;
        }
    }
}