namespace Wpf.FlipView.Tests
{
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for CommandBox.xaml
    /// </summary>
    public partial class CommandBox : EventBox
    {
        public CommandBox()
        {
            InitializeComponent();
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, (sender, args) => Args.Add("Cut")));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, (sender, args) => Args.Add("Copy")));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, (sender, args) => Args.Add("Paste")));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, (sender, args) => Args.Add("Delete")));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Undo, (sender, args) => Args.Add("Undo")));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Redo, (sender, args) => Args.Add("Redo")));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Find, (sender, args) => Args.Add("Find")));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Replace, (sender, args) => Args.Add("Replace")));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.SelectAll, (sender, args) => Args.Add("SelectAll")));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Help, (sender, args) => Args.Add("Help")));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.New, (sender, args) => Args.Add("New")));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, (sender, args) => Args.Add("Open")));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, (sender, args) => Args.Add("Close")));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Save, (sender, args) => Args.Add("Save")));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.SaveAs, (sender, args) => Args.Add("SaveAs")));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Print, (sender, args) => Args.Add("Print")));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.CancelPrint, (sender, args) => Args.Add("CancelPrint")));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.PrintPreview, (sender, args) => Args.Add("PrintPreview")));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Properties, (sender, args) => Args.Add("Properties")));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.ContextMenu, (sender, args) => Args.Add("ContextMenu")));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Stop, (sender, args) => Args.Add("Stop")));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.CorrectionList, (sender, args) => Args.Add("CorrectionList")));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.NotACommand, (sender, args) => Args.Add("NotACommand")));
            CommandBindings.Add(new CommandBinding(NavigationCommands.BrowseBack, (sender, args) => Args.Add("BrowseBack")));
            CommandBindings.Add(new CommandBinding(NavigationCommands.BrowseForward, (sender, args) => Args.Add("BrowseForward")));
            CommandBindings.Add(new CommandBinding(NavigationCommands.BrowseHome, (sender, args) => Args.Add("BrowseHome")));
            CommandBindings.Add(new CommandBinding(NavigationCommands.BrowseStop, (sender, args) => Args.Add("BrowseStop")));
            CommandBindings.Add(new CommandBinding(NavigationCommands.Refresh, (sender, args) => Args.Add("Refresh")));
            CommandBindings.Add(new CommandBinding(NavigationCommands.Favorites, (sender, args) => Args.Add("Favorites")));
            CommandBindings.Add(new CommandBinding(NavigationCommands.Search, (sender, args) => Args.Add("Search")));
            CommandBindings.Add(new CommandBinding(NavigationCommands.IncreaseZoom, (sender, args) => Args.Add("IncreaseZoom")));
            CommandBindings.Add(new CommandBinding(NavigationCommands.DecreaseZoom, (sender, args) => Args.Add("DecreaseZoom")));
            CommandBindings.Add(new CommandBinding(NavigationCommands.Zoom, (sender, args) => Args.Add("Zoom")));
            CommandBindings.Add(new CommandBinding(NavigationCommands.NextPage, (sender, args) => Args.Add("NextPage")));
            CommandBindings.Add(new CommandBinding(NavigationCommands.PreviousPage, (sender, args) => Args.Add("PreviousPage")));
            CommandBindings.Add(new CommandBinding(NavigationCommands.FirstPage, (sender, args) => Args.Add("FirstPage")));
            CommandBindings.Add(new CommandBinding(NavigationCommands.LastPage, (sender, args) => Args.Add("LastPage")));
            CommandBindings.Add(new CommandBinding(NavigationCommands.GoToPage, (sender, args) => Args.Add("GoToPage")));
            CommandBindings.Add(new CommandBinding(NavigationCommands.NavigateJournal, (sender, args) => Args.Add("NavigateJournal")));
            CommandBindings.Add(new CommandBinding(MediaCommands.Play, (sender, args) => Args.Add("Play")));
            CommandBindings.Add(new CommandBinding(MediaCommands.Pause, (sender, args) => Args.Add("Pause")));
            CommandBindings.Add(new CommandBinding(MediaCommands.Stop, (sender, args) => Args.Add("Stop")));
            CommandBindings.Add(new CommandBinding(MediaCommands.Record, (sender, args) => Args.Add("Record")));
            CommandBindings.Add(new CommandBinding(MediaCommands.NextTrack, (sender, args) => Args.Add("NextTrack")));
            CommandBindings.Add(new CommandBinding(MediaCommands.PreviousTrack, (sender, args) => Args.Add("PreviousTrack")));
            CommandBindings.Add(new CommandBinding(MediaCommands.FastForward, (sender, args) => Args.Add("FastForward")));
            CommandBindings.Add(new CommandBinding(MediaCommands.Rewind, (sender, args) => Args.Add("Rewind")));
            CommandBindings.Add(new CommandBinding(MediaCommands.ChannelUp, (sender, args) => Args.Add("ChannelUp")));
            CommandBindings.Add(new CommandBinding(MediaCommands.ChannelDown, (sender, args) => Args.Add("ChannelDown")));
            CommandBindings.Add(new CommandBinding(MediaCommands.TogglePlayPause, (sender, args) => Args.Add("TogglePlayPause")));
            CommandBindings.Add(new CommandBinding(MediaCommands.Select, (sender, args) => Args.Add("Select")));
            CommandBindings.Add(new CommandBinding(MediaCommands.IncreaseVolume, (sender, args) => Args.Add("IncreaseVolume")));
            CommandBindings.Add(new CommandBinding(MediaCommands.DecreaseVolume, (sender, args) => Args.Add("DecreaseVolume")));
            CommandBindings.Add(new CommandBinding(MediaCommands.MuteVolume, (sender, args) => Args.Add("MuteVolume")));
            CommandBindings.Add(new CommandBinding(MediaCommands.IncreaseTreble, (sender, args) => Args.Add("IncreaseTreble")));
            CommandBindings.Add(new CommandBinding(MediaCommands.DecreaseTreble, (sender, args) => Args.Add("DecreaseTreble")));
            CommandBindings.Add(new CommandBinding(MediaCommands.IncreaseBass, (sender, args) => Args.Add("IncreaseBass")));
            CommandBindings.Add(new CommandBinding(MediaCommands.DecreaseBass, (sender, args) => Args.Add("DecreaseBass")));
            CommandBindings.Add(new CommandBinding(MediaCommands.BoostBass, (sender, args) => Args.Add("BoostBass")));
            CommandBindings.Add(new CommandBinding(MediaCommands.IncreaseMicrophoneVolume, (sender, args) => Args.Add("IncreaseMicrophoneVolume")));
            CommandBindings.Add(new CommandBinding(MediaCommands.DecreaseMicrophoneVolume, (sender, args) => Args.Add("DecreaseMicrophoneVolume")));
            CommandBindings.Add(new CommandBinding(MediaCommands.MuteMicrophoneVolume, (sender, args) => Args.Add("MuteMicrophoneVolume")));
            CommandBindings.Add(new CommandBinding(MediaCommands.ToggleMicrophoneOnOff, (sender, args) => Args.Add("ToggleMicrophoneOnOff")));
            CommandBindings.Add(new CommandBinding(ComponentCommands.ScrollPageUp, (sender, args) => Args.Add("ScrollPageUp")));
            CommandBindings.Add(new CommandBinding(ComponentCommands.ScrollPageDown, (sender, args) => Args.Add("ScrollPageDown")));
            CommandBindings.Add(new CommandBinding(ComponentCommands.ScrollPageLeft, (sender, args) => Args.Add("ScrollPageLeft")));
            CommandBindings.Add(new CommandBinding(ComponentCommands.ScrollPageRight, (sender, args) => Args.Add("ScrollPageRight")));
            CommandBindings.Add(new CommandBinding(ComponentCommands.ScrollByLine, (sender, args) => Args.Add("ScrollByLine")));
            CommandBindings.Add(new CommandBinding(ComponentCommands.MoveLeft, (sender, args) => Args.Add("MoveLeft")));
            CommandBindings.Add(new CommandBinding(ComponentCommands.MoveRight, (sender, args) => Args.Add("MoveRight")));
            CommandBindings.Add(new CommandBinding(ComponentCommands.MoveUp, (sender, args) => Args.Add("MoveUp")));
            CommandBindings.Add(new CommandBinding(ComponentCommands.MoveDown, (sender, args) => Args.Add("MoveDown")));
            CommandBindings.Add(new CommandBinding(ComponentCommands.MoveToHome, (sender, args) => Args.Add("MoveToHome")));
            CommandBindings.Add(new CommandBinding(ComponentCommands.MoveToEnd, (sender, args) => Args.Add("MoveToEnd")));
            CommandBindings.Add(new CommandBinding(ComponentCommands.MoveToPageUp, (sender, args) => Args.Add("MoveToPageUp")));
            CommandBindings.Add(new CommandBinding(ComponentCommands.MoveToPageDown, (sender, args) => Args.Add("MoveToPageDown")));
            CommandBindings.Add(new CommandBinding(ComponentCommands.ExtendSelectionUp, (sender, args) => Args.Add("ExtendSelectionUp")));
            CommandBindings.Add(new CommandBinding(ComponentCommands.ExtendSelectionDown, (sender, args) => Args.Add("ExtendSelectionDown")));
            CommandBindings.Add(new CommandBinding(ComponentCommands.ExtendSelectionLeft, (sender, args) => Args.Add("ExtendSelectionLeft")));
            CommandBindings.Add(new CommandBinding(ComponentCommands.ExtendSelectionRight, (sender, args) => Args.Add("ExtendSelectionRight")));
            CommandBindings.Add(new CommandBinding(ComponentCommands.SelectToHome, (sender, args) => Args.Add("SelectToHome")));
            CommandBindings.Add(new CommandBinding(ComponentCommands.SelectToEnd, (sender, args) => Args.Add("SelectToEnd")));
            CommandBindings.Add(new CommandBinding(ComponentCommands.SelectToPageUp, (sender, args) => Args.Add("SelectToPageUp")));
            CommandBindings.Add(new CommandBinding(ComponentCommands.SelectToPageDown, (sender, args) => Args.Add("SelectToPageDown")));
            CommandBindings.Add(new CommandBinding(ComponentCommands.MoveFocusUp, (sender, args) => Args.Add("MoveFocusUp")));
            CommandBindings.Add(new CommandBinding(ComponentCommands.MoveFocusDown, (sender, args) => Args.Add("MoveFocusDown")));
            CommandBindings.Add(new CommandBinding(ComponentCommands.MoveFocusForward, (sender, args) => Args.Add("MoveFocusForward")));
            CommandBindings.Add(new CommandBinding(ComponentCommands.MoveFocusBack, (sender, args) => Args.Add("MoveFocusBack")));
            CommandBindings.Add(new CommandBinding(ComponentCommands.MoveFocusPageUp, (sender, args) => Args.Add("MoveFocusPageUp")));
            CommandBindings.Add(new CommandBinding(ComponentCommands.MoveFocusPageDown, (sender, args) => Args.Add("MoveFocusPageDown")));


        }
    }
}
