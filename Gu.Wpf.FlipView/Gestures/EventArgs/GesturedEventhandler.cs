namespace Gu.Wpf.FlipView.Gestures
{
    /// <summary>
    /// Handler for the gesture event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="GestureEventArgs"/>.</param>
#pragma warning disable CA1711 // Identifiers should not have incorrect suffix
#pragma warning disable CA1003 // Use generic event handler instances
    public delegate void GesturedEventHandler(object sender, GesturedEventArgs e);
#pragma warning restore CA1003 // Use generic event handler instances
#pragma warning restore CA1711 // Identifiers should not have incorrect suffix
}
