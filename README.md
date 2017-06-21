# Gu.Wpf.FlipView

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![NuGet](https://img.shields.io/nuget/v/Gu.Wpf.FlipView.svg)](https://www.nuget.org/packages/Gu.Wpf.FlipView/)
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

## Samples

### Simple with default animation, fade in & out:

```xaml
<flipView:TransitionControl Content="{Binding SelectedItem, ElementName=ListBox}" 
                            ContentTemplate="{StaticResource SomeDataTemplate}" />
```

### With custom animations

```xaml
<flipView:TransitionControl Content="{Binding SelectedItem, ElementName=ListBox}">
    <flipView:TransitionControl.InAnimation>
        <Storyboard>
            <DoubleAnimation BeginTime="0:0:0"
                                Storyboard.TargetProperty="Opacity"
                                From="1"
                                To="0"
                                Duration="0:0:0.3" />
            <DoubleAnimation BeginTime="0:0:0"
                                Storyboard.TargetProperty="(flipView:Transform.RelativeOffsetX)"
                                From="0"
                                To="1"
                                Duration="0:0:0.3" />
        </Storyboard>
    </flipView:TransitionControl.InAnimation>

    <flipView:TransitionControl.OutAnimation>
        <Storyboard>
            <DoubleAnimation BeginTime="0:0:0"
                                Storyboard.TargetProperty="Opacity"
                                From="0"
                                To="1"
                                Duration="0:0:0.3" />
            <DoubleAnimation BeginTime="0:0:0"
                                Storyboard.TargetProperty="(flipView:Transform.RelativeOffsetX)"
                                From="-1"
                                To="0"
                                Duration="0:0:0.3" />
        </Storyboard>
    </flipView:TransitionControl.OutAnimation>
</flipView:TransitionControl>
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

# Transform

Attached properties for animating transitions.

## RelativeOffsetX
Setting the value to 1 results in `OffsetX` being set to `ActualWidth`. Does not update when size changes as it is only meant to be suwed during transitions.
Animating the value 0 -> 1 means the element animates it's width to the right.

## RelativeOffsetY
Setting the value to 1 results in `OffsetY` being set to `ActualHeight`. Does not update when size changes as it is only meant to be suwed during transitions.
Animating the value 0 -> 1 means the element animates it's height downwards.

## OffsetX
The absolute x value.

## OffsetY
The absolute y value.

## ScaleX
The scale x value.

## ScaleY
The scale y value.

## Sample
Note that the sample below assumes that `TransitionControl.ContentChangedEvent`is raised on the `ContentPresenter` to trigger the animation.

```xaml
<Storyboard x:Key="SlideInAnimation">
    <DoubleAnimation BeginTime="0:0:0"
                        FillBehavior="Stop"
                        Storyboard.TargetProperty="Opacity"
                        From="1"
                        To="0"
                        Duration="0:0:0.3" />
    <DoubleAnimation BeginTime="0:0:0"
                        FillBehavior="Stop"
                        Storyboard.TargetProperty="(a:Transform.RelativeOffsetX)"
                        From="0"
                        To="1"
                        Duration="0:0:0.3" />
    <DoubleAnimation BeginTime="0:0:0"
                     FillBehavior="Stop"
                     Storyboard.TargetProperty="(a:Transform.ScaleY)"
                     From="0"
                     To="1"
                     Duration="0:0:0.3" />
</Storyboard>

<Style x:Key TargetType="{x:Type ContentPresenter}">
    <Setter Property="RenderTransform">
        <Setter.Value>
            <TransformGroup>
                <TranslateTransform X="{Binding Path=(attached:Transform.OffsetX), RelativeSource={RelativeSource AncestorType={x:Type ContentPresenter}}}" 
									Y="{Binding Path=(attached:Transform.OffsetY), RelativeSource={RelativeSource AncestorType={x:Type ContentPresenter}}}" />
                <ScaleTransform ScaleX="{Binding Path=(attached:Transform.ScaleX), RelativeSource={RelativeSource AncestorType={x:Type ContentPresenter}}}" 
								ScaleY="{Binding Path=(attached:Transform.ScaleY), RelativeSource={RelativeSource AncestorType={x:Type ContentPresenter}}}" />
            </TransformGroup>
        </Setter.Value>
    </Setter>
    <Style.Triggers>
        <EventTrigger RoutedEvent="ContentChanged">
            <BeginStoryboard Storyboard="{StaticResource SlideInAnimation}" />
        </EventTrigger>
    </Style.Triggers>
</Style>
```
