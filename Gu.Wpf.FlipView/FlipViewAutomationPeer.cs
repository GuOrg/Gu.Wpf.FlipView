namespace Gu.Wpf.FlipView
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Automation.Peers;
    using Gu.Wpf.FlipView.Internals;

    /// <summary>
    /// An <see cref="AutomationPeer"/> for <see cref="FlipView"/>.
    /// </summary>
    public class FlipViewAutomationPeer : SelectorAutomationPeer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FlipViewAutomationPeer"/> class.
        /// </summary>
        /// <param name="owner">The <see cref="FlipView" /> that is associated with this <see cref="FlipViewAutomationPeer" />.</param>
        public FlipViewAutomationPeer(FlipView owner)
            : base(owner)
        {
        }

        /// <inheritdoc />
        protected override ItemAutomationPeer? CreateItemAutomationPeer(object item) => null;

        /// <inheritdoc />
        protected override List<AutomationPeer> GetChildrenCore()
        {
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
            return this.Owner.VisualChildrenRecursive()
                       .Select(CreatePeerForElement)
                       .Where(x => x != null)
                       .ToList();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.

            static AutomationPeer? CreatePeerForElement(DependencyObject o)
            {
                return o switch
                {
                    UIElement uiElement => UIElementAutomationPeer.CreatePeerForElement(uiElement),
                    UIElement3D uiElement3D => UIElement3DAutomationPeer.CreatePeerForElement(uiElement3D),
                    _ => null,
                };
            }
        }

        /// <inheritdoc />
        protected override string GetClassNameCore() => "FlipView";
    }
}
