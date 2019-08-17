namespace Gu.Wpf.FlipView.Tests.Misc_and_helpers
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    public class FakeTouchDevice : TouchDevice
    {
        public static readonly FakeTouchDevice Default = new FakeTouchDevice();

        private readonly TouchPoint touchPoint;

        public FakeTouchDevice(TouchPoint touchPoint)
            : base(0)
        {
            this.touchPoint = touchPoint;
        }

        private FakeTouchDevice()
            : base(0)
        {
        }

        public override TouchPoint GetTouchPoint(IInputElement relativeTo)
        {
            return this.touchPoint;
        }

        public override TouchPointCollection GetIntermediateTouchPoints(IInputElement relativeTo)
        {
#pragma warning disable GU0090 // Don't throw NotImplementedException.
            throw new NotImplementedException();
#pragma warning restore GU0090 // Don't throw NotImplementedException.
        }
    }
}
