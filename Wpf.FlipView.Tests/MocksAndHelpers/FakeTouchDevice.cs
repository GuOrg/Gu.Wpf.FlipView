namespace Wpf.FlipView.Tests
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    public class FakeTouchDevice : TouchDevice
    {
        private readonly TouchPoint _touchPoint;
        public static FakeTouchDevice Default = new FakeTouchDevice();
        public FakeTouchDevice() : base(0)
        {
            
        }
        public FakeTouchDevice(TouchPoint touchPoint) : base(0)
        {
            _touchPoint = touchPoint;
        }
        public override TouchPoint GetTouchPoint(IInputElement relativeTo)
        {
            return _touchPoint;
        }
        public override TouchPointCollection GetIntermediateTouchPoints(IInputElement relativeTo)
        {
            throw new NotImplementedException();
        }
    }
}