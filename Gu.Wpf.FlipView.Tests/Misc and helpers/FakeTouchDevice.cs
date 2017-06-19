namespace Gu.Wpf.FlipView.Tests.Misc_and_helpers
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    public class FakeTouchDevice : TouchDevice
    {
        private readonly TouchPoint touchPoint;
        public static FakeTouchDevice Default = new FakeTouchDevice();

        public FakeTouchDevice()
            : base(0)
        {
        }

        public FakeTouchDevice(TouchPoint touchPoint) : base(0)
        {
            this.touchPoint = touchPoint;
        }

        public override TouchPoint GetTouchPoint(IInputElement relativeTo)
        {
            return this.touchPoint;
        }

        public override TouchPointCollection GetIntermediateTouchPoints(IInputElement relativeTo)
        {
            throw new NotImplementedException();
        }
    }
}