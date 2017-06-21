# Gu.Wpf.FlipView

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE.md)
[![Build status](https://ci.appveyor.com/api/projects/status/tp8vm8xlvtakfat9/branch/master?svg=true)](https://ci.appveyor.com/project/JohanLarsson/gu-wpf-flipview/branch/master)

A flipview for WPF, handles touch &amp; mouse swipe.

# Table of contents
- [FlipView](#flipview)
  - [IncreaseInAnimation](#increaseinanimation)
  - [IncreaseOutAnimation](#increaseoutanimation)
  - [DecreaseInAnimation](#decreaseinanimation)
  - [DecreaseOutAnimation](#decreaseoutanimation)
  - [CurrentInAnimation](#currentinanimation)
  - [CurrentOutAnimation](#currentoutanimation)
  - [ShowIndex](#showindex)
  - [IndexPlacement](#indexplacement)
  - [IndexItemStyle](#indexitemstyle)
  - [ShowArrows](#showarrows)
  - [ArrowPlacement](#arrowplacement)
  - [ArrowButtonStyle](#arrowbuttonstyle)
  - [Samples](#samples)
- [TransitionControl](#transitioncontrol)
  - [ContentChangedEvent](#contentchangedevent)
  - [OldContent](#oldcontent)
  - [OldContentStyle](#oldcontentstyle)
  - [OutAnimation](#outanimation)
  - [NewContentStyle](#newcontentstyle)
  - [InAnimation](#inanimation)
- [GesturePanel](#gesturepanel)
  - [Gestured](#gestured)
  - [GestureTracker](#gesturetracker)

# FlipView
A selector that transitions when selecteditem changes.
Has bindings to `NavigationCommands.BrowseBack` and `NavigationCommands.BrowseForward`

![animation](https://user-images.githubusercontent.com/1640096/27380318-f546c126-567e-11e7-8cb6-91463b74641f.gif)

## IncreaseInAnimation
The animation to use for animating in new content when selected index increased.

## IncreaseOutAnimation
The animation to use for animating out old content when selected index increased.

## DecreaseInAnimation
The animation to use for animating in new content when selected index decreased.

## DecreaseOutAnimation
The animation to use for animating out old content when selected index decreased.

## CurrentInAnimation
The resulting animation to use for animating in new content.

## CurrentOutAnimation
The resulting animation to use for animating out old content.

## ShowIndex
If the index should be visible

## IndexPlacement
Where the index should be rendered

## IndexItemStyle
A style with `TargetType="ListBoxItem"` for how to render items in the index.

## ShowArrows
Specifies if navigation buttons should be visible.

## ArrowPlacement
Specifies where navigation buttons are rendered.

## ArrowButtonStyle
A style with `TargetType="RepeatButton"` for how to render navigation buttons.

## Samples

Sample slideshow images:

```xaml
<flipView:FlipView SelectedIndex="0">
    <Image Source="http://i.imgur.com/xT3ay.jpg" />
    <Image Source="http://i.stack.imgur.com/lDlr1.jpg" />
</flipView:FlipView>
```

Sample bound itemssource:

```xaml
<flipView:FlipView x:Name="FlipView"
                    ItemsSource="{Binding People}"
                    SelectedIndex="0">
    <flipView:FlipView.ItemTemplate>
        <DataTemplate DataType="{x:Type local:Person}">
            <Border Background="#f1eef6">
                <TextBlock HorizontalAlignment="Center" VerticalAlignment="Center">
                    <TextBlock.Text>
                        <MultiBinding StringFormat="{}{0} {1}">
                            <Binding Path="FirstName" />
                            <Binding Path="LastName" />
                        </MultiBinding>
                    </TextBlock.Text>
                </TextBlock>
            </Border>
        </DataTemplate>
    </flipView:FlipView.ItemTemplate>
</flipView:FlipView>
```

# TransitionControl
A contentcontrol that animates content changes. Used in the `ControlTemplate` for `FlipView`.
The default animation is fade new content in & old content out.
When a transition starts the ContentChangedEvent is raised for `PART_OldContent` and `PART_NewContent` if they are found in the template.

## ContentChangedEvent
Notifies when content changes. When a transition starts the ContentChangedEvent is raised for `PART_OldContent` and `PART_NewContent` if they are found in the template.

## OldContent
This property holds the old content until the transition animation finishes.

## OldContentStyle 
The style for the old content. TargetType="ContentPresenter"

## OutAnimation
The storyboard used for animating out old content.

## NewContentStyle 
The style for the new and current content. TargetType="ContentPresenter"

## InAnimation
The storyboard used for animating in new content.

Sample
```xaml
<flipView:TransitionControl Content="{Binding SelectedItem, ElementName=ListBox}" 
                            ContentTemplate="{StaticResource SomeDataTemplate}" />
```

# GesturePanel
A contentcontrol that detects gestures such as swipes. Used in the `ContentTemplate` for `FlipView`

## Gestured
A routed event that notifies when a gesture was detected. 

## GestureTracker
Plug in a gesture tracker. 
The default value is `new TouchGestureTracker()`

Included in the library is
- TouchGestureTracker
- MouseGestureTracker

The MouseGestureTracker can be useful for testing things if no touch device is available.

Sample:
```xaml
<flipView:GesturePanel Background="Lavender">
    <!-- content goes here -->
</flipView:GesturePanel>
```

Sample with custom tracker:
```xaml
<flipView:GesturePanel.GestureTracker>
    <flipView:MouseGestureTracker>
        <flipView:MouseGestureTracker.Interpreter>
            <flipView:DefaultGestureInterpreter MinSwipeLength="15" 
                                                MinSwipeVelocity="1" 
                                                MaxDeviationFromHorizontal="30" />
        </flipView:MouseGestureTracker.Interpreter>
    </flipView:MouseGestureTracker>
     <!-- content goes here -->
</flipView:GesturePanel.GestureTracker>
```

