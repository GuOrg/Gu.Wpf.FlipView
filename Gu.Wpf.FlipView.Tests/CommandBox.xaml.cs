namespace Gu.Wpf.FlipView.Tests
{
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for CommandBox.xaml
    /// </summary>
    public partial class CommandBox : EventBox
    {
        public CommandBox()
        {
            this.InitializeComponent();
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, (sender, args) => this.Args.Add("Cut")));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, (sender, args) => this.Args.Add("Copy")));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, (sender, args) => this.Args.Add("Paste")));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, (sender, args) => this.Args.Add("Delete")));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Undo, (sender, args) => this.Args.Add("Undo")));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Redo, (sender, args) => this.Args.Add("Redo")));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Find, (sender, args) => this.Args.Add("Find")));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Replace, (sender, args) => this.Args.Add("Replace")));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.SelectAll, (sender, args) => this.Args.Add("SelectAll")));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Help, (sender, args) => this.Args.Add("Help")));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.New, (sender, args) => this.Args.Add("New")));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, (sender, args) => this.Args.Add("Open")));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, (sender, args) => this.Args.Add("Close")));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Save, (sender, args) => this.Args.Add("Save")));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.SaveAs, (sender, args) => this.Args.Add("SaveAs")));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Print, (sender, args) => this.Args.Add("Print")));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.CancelPrint, (sender, args) => this.Args.Add("CancelPrint")));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.PrintPreview, (sender, args) => this.Args.Add("PrintPreview")));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Properties, (sender, args) => this.Args.Add("Properties")));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.ContextMenu, (sender, args) => this.Args.Add("ContextMenu")));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Stop, (sender, args) => this.Args.Add("Stop")));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.CorrectionList, (sender, args) => this.Args.Add("CorrectionList")));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.NotACommand, (sender, args) => this.Args.Add("NotACommand")));
            this.CommandBindings.Add(new CommandBinding(NavigationCommands.BrowseBack, (sender, args) => this.Args.Add("BrowseBack")));
            this.CommandBindings.Add(new CommandBinding(NavigationCommands.BrowseForward, (sender, args) => this.Args.Add("BrowseForward")));
            this.CommandBindings.Add(new CommandBinding(NavigationCommands.BrowseHome, (sender, args) => this.Args.Add("BrowseHome")));
            this.CommandBindings.Add(new CommandBinding(NavigationCommands.BrowseStop, (sender, args) => this.Args.Add("BrowseStop")));
            this.CommandBindings.Add(new CommandBinding(NavigationCommands.Refresh, (sender, args) => this.Args.Add("Refresh")));
            this.CommandBindings.Add(new CommandBinding(NavigationCommands.Favorites, (sender, args) => this.Args.Add("Favorites")));
            this.CommandBindings.Add(new CommandBinding(NavigationCommands.Search, (sender, args) => this.Args.Add("Search")));
            this.CommandBindings.Add(new CommandBinding(NavigationCommands.IncreaseZoom, (sender, args) => this.Args.Add("IncreaseZoom")));
            this.CommandBindings.Add(new CommandBinding(NavigationCommands.DecreaseZoom, (sender, args) => this.Args.Add("DecreaseZoom")));
            this.CommandBindings.Add(new CommandBinding(NavigationCommands.Zoom, (sender, args) => this.Args.Add("Zoom")));
            this.CommandBindings.Add(new CommandBinding(NavigationCommands.NextPage, (sender, args) => this.Args.Add("NextPage")));
            this.CommandBindings.Add(new CommandBinding(NavigationCommands.PreviousPage, (sender, args) => this.Args.Add("PreviousPage")));
            this.CommandBindings.Add(new CommandBinding(NavigationCommands.FirstPage, (sender, args) => this.Args.Add("FirstPage")));
            this.CommandBindings.Add(new CommandBinding(NavigationCommands.LastPage, (sender, args) => this.Args.Add("LastPage")));
            this.CommandBindings.Add(new CommandBinding(NavigationCommands.GoToPage, (sender, args) => this.Args.Add("GoToPage")));
            this.CommandBindings.Add(new CommandBinding(NavigationCommands.NavigateJournal, (sender, args) => this.Args.Add("NavigateJournal")));
            this.CommandBindings.Add(new CommandBinding(MediaCommands.Play, (sender, args) => this.Args.Add("Play")));
            this.CommandBindings.Add(new CommandBinding(MediaCommands.Pause, (sender, args) => this.Args.Add("Pause")));
            this.CommandBindings.Add(new CommandBinding(MediaCommands.Stop, (sender, args) => this.Args.Add("Stop")));
            this.CommandBindings.Add(new CommandBinding(MediaCommands.Record, (sender, args) => this.Args.Add("Record")));
            this.CommandBindings.Add(new CommandBinding(MediaCommands.NextTrack, (sender, args) => this.Args.Add("NextTrack")));
            this.CommandBindings.Add(new CommandBinding(MediaCommands.PreviousTrack, (sender, args) => this.Args.Add("PreviousTrack")));
            this.CommandBindings.Add(new CommandBinding(MediaCommands.FastForward, (sender, args) => this.Args.Add("FastForward")));
            this.CommandBindings.Add(new CommandBinding(MediaCommands.Rewind, (sender, args) => this.Args.Add("Rewind")));
            this.CommandBindings.Add(new CommandBinding(MediaCommands.ChannelUp, (sender, args) => this.Args.Add("ChannelUp")));
            this.CommandBindings.Add(new CommandBinding(MediaCommands.ChannelDown, (sender, args) => this.Args.Add("ChannelDown")));
            this.CommandBindings.Add(new CommandBinding(MediaCommands.TogglePlayPause, (sender, args) => this.Args.Add("TogglePlayPause")));
            this.CommandBindings.Add(new CommandBinding(MediaCommands.Select, (sender, args) => this.Args.Add("Select")));
            this.CommandBindings.Add(new CommandBinding(MediaCommands.IncreaseVolume, (sender, args) => this.Args.Add("IncreaseVolume")));
            this.CommandBindings.Add(new CommandBinding(MediaCommands.DecreaseVolume, (sender, args) => this.Args.Add("DecreaseVolume")));
            this.CommandBindings.Add(new CommandBinding(MediaCommands.MuteVolume, (sender, args) => this.Args.Add("MuteVolume")));
            this.CommandBindings.Add(new CommandBinding(MediaCommands.IncreaseTreble, (sender, args) => this.Args.Add("IncreaseTreble")));
            this.CommandBindings.Add(new CommandBinding(MediaCommands.DecreaseTreble, (sender, args) => this.Args.Add("DecreaseTreble")));
            this.CommandBindings.Add(new CommandBinding(MediaCommands.IncreaseBass, (sender, args) => this.Args.Add("IncreaseBass")));
            this.CommandBindings.Add(new CommandBinding(MediaCommands.DecreaseBass, (sender, args) => this.Args.Add("DecreaseBass")));
            this.CommandBindings.Add(new CommandBinding(MediaCommands.BoostBass, (sender, args) => this.Args.Add("BoostBass")));
            this.CommandBindings.Add(new CommandBinding(MediaCommands.IncreaseMicrophoneVolume, (sender, args) => this.Args.Add("IncreaseMicrophoneVolume")));
            this.CommandBindings.Add(new CommandBinding(MediaCommands.DecreaseMicrophoneVolume, (sender, args) => this.Args.Add("DecreaseMicrophoneVolume")));
            this.CommandBindings.Add(new CommandBinding(MediaCommands.MuteMicrophoneVolume, (sender, args) => this.Args.Add("MuteMicrophoneVolume")));
            this.CommandBindings.Add(new CommandBinding(MediaCommands.ToggleMicrophoneOnOff, (sender, args) => this.Args.Add("ToggleMicrophoneOnOff")));
            this.CommandBindings.Add(new CommandBinding(ComponentCommands.ScrollPageUp, (sender, args) => this.Args.Add("ScrollPageUp")));
            this.CommandBindings.Add(new CommandBinding(ComponentCommands.ScrollPageDown, (sender, args) => this.Args.Add("ScrollPageDown")));
            this.CommandBindings.Add(new CommandBinding(ComponentCommands.ScrollPageLeft, (sender, args) => this.Args.Add("ScrollPageLeft")));
            this.CommandBindings.Add(new CommandBinding(ComponentCommands.ScrollPageRight, (sender, args) => this.Args.Add("ScrollPageRight")));
            this.CommandBindings.Add(new CommandBinding(ComponentCommands.ScrollByLine, (sender, args) => this.Args.Add("ScrollByLine")));
            this.CommandBindings.Add(new CommandBinding(ComponentCommands.MoveLeft, (sender, args) => this.Args.Add("MoveLeft")));
            this.CommandBindings.Add(new CommandBinding(ComponentCommands.MoveRight, (sender, args) => this.Args.Add("MoveRight")));
            this.CommandBindings.Add(new CommandBinding(ComponentCommands.MoveUp, (sender, args) => this.Args.Add("MoveUp")));
            this.CommandBindings.Add(new CommandBinding(ComponentCommands.MoveDown, (sender, args) => this.Args.Add("MoveDown")));
            this.CommandBindings.Add(new CommandBinding(ComponentCommands.MoveToHome, (sender, args) => this.Args.Add("MoveToHome")));
            this.CommandBindings.Add(new CommandBinding(ComponentCommands.MoveToEnd, (sender, args) => this.Args.Add("MoveToEnd")));
            this.CommandBindings.Add(new CommandBinding(ComponentCommands.MoveToPageUp, (sender, args) => this.Args.Add("MoveToPageUp")));
            this.CommandBindings.Add(new CommandBinding(ComponentCommands.MoveToPageDown, (sender, args) => this.Args.Add("MoveToPageDown")));
            this.CommandBindings.Add(new CommandBinding(ComponentCommands.ExtendSelectionUp, (sender, args) => this.Args.Add("ExtendSelectionUp")));
            this.CommandBindings.Add(new CommandBinding(ComponentCommands.ExtendSelectionDown, (sender, args) => this.Args.Add("ExtendSelectionDown")));
            this.CommandBindings.Add(new CommandBinding(ComponentCommands.ExtendSelectionLeft, (sender, args) => this.Args.Add("ExtendSelectionLeft")));
            this.CommandBindings.Add(new CommandBinding(ComponentCommands.ExtendSelectionRight, (sender, args) => this.Args.Add("ExtendSelectionRight")));
            this.CommandBindings.Add(new CommandBinding(ComponentCommands.SelectToHome, (sender, args) => this.Args.Add("SelectToHome")));
            this.CommandBindings.Add(new CommandBinding(ComponentCommands.SelectToEnd, (sender, args) => this.Args.Add("SelectToEnd")));
            this.CommandBindings.Add(new CommandBinding(ComponentCommands.SelectToPageUp, (sender, args) => this.Args.Add("SelectToPageUp")));
            this.CommandBindings.Add(new CommandBinding(ComponentCommands.SelectToPageDown, (sender, args) => this.Args.Add("SelectToPageDown")));
            this.CommandBindings.Add(new CommandBinding(ComponentCommands.MoveFocusUp, (sender, args) => this.Args.Add("MoveFocusUp")));
            this.CommandBindings.Add(new CommandBinding(ComponentCommands.MoveFocusDown, (sender, args) => this.Args.Add("MoveFocusDown")));
            this.CommandBindings.Add(new CommandBinding(ComponentCommands.MoveFocusForward, (sender, args) => this.Args.Add("MoveFocusForward")));
            this.CommandBindings.Add(new CommandBinding(ComponentCommands.MoveFocusBack, (sender, args) => this.Args.Add("MoveFocusBack")));
            this.CommandBindings.Add(new CommandBinding(ComponentCommands.MoveFocusPageUp, (sender, args) => this.Args.Add("MoveFocusPageUp")));
            this.CommandBindings.Add(new CommandBinding(ComponentCommands.MoveFocusPageDown, (sender, args) => this.Args.Add("MoveFocusPageDown")));
        }
    }
}
