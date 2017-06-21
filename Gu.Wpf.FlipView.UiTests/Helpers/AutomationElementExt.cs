namespace Gu.Wpf.FlipView.UiTests
{
    using FlaUI.Core.AutomationElements;
    using FlaUI.Core.AutomationElements.Infrastructure;
    using FlaUI.Core.Conditions;
    using FlaUI.Core.Definitions;

    public static class AutomationElementExt
    {
        public static bool WaitUntilResponsive(this AutomationElement automationElement)
        {
            FlaUI.Core.Input.Helpers.WaitUntilInputIsProcessed();
            return FlaUI.Core.Input.Helpers.WaitUntilResponsive(automationElement);
        }

        public static Button FindButton(this AutomationElement parent, string name)
        {
            return parent.FindByNameOrId(name, ControlType.Button)
                         .AsButton();
        }

        public static TextBox FindTextBox(this AutomationElement parent, string name)
        {
            return parent.FindByNameOrId(name, ControlType.Text)
                         .AsTextBox();
        }

        public static ComboBox FindComboBox(this AutomationElement parent, string name)
        {
            return parent.FindByNameOrId(name, ControlType.ComboBox)
                         .AsComboBox();
        }

        public static Slider FindSlider(this AutomationElement parent, string name)
        {
            return parent.FindByNameOrId(name, ControlType.Slider)
                         .AsSlider();
        }

        public static AutomationElement FindByNameOrId(this AutomationElement parent, string name, ControlType controlType)
        {
            return parent.FindFirstDescendant(
                new AndCondition(
                    parent.ConditionFactory.ByControlType(controlType),
                    new OrCondition(
                        parent.ConditionFactory.ByName(name),
                        parent.ConditionFactory.ByAutomationId(name))));
        }

        public static AutomationElement FindByNameOrId(this AutomationElement parent, string name)
        {
            return parent.FindFirstDescendant(
                    new OrCondition(
                        parent.ConditionFactory.ByName(name),
                        parent.ConditionFactory.ByAutomationId(name)));
        }
    }
}
