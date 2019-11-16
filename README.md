# ProtoGUI
Simple, quick and good looking GUI windows for Unity, intended for prototyping and debugging. The package includes Inconsolata open source font by Raph Levien.

Warning: It is basically a wrapper for Unity's IMGUI, so it is not very performant and GC friendly! The goal of it however is ease of use and is not recommended for production (although I am using it in (TBICCT)[https://nothke.itch.io/tower])

If you want something like this but more performant, you'll have to wait until Unity's UIElements become available for runtime.

### Installation
Use it as a Unity package, so don't put it in your assets folder, instead either put it in the Packages folder, or some other common folder (to which you can reference from different projects). Then and add it using the Package Manager "+" icon > "Add package from disk..".

### How To Use

All you need to do is just inherit from WindowGUI, and then implement the required methods (ctrl + . in VS or Rider). Set the window title, and you can simply start using your window with GUILayout:

```
using Nothke.ProtoGUI;

public class MyWindow : WindowGUI
{
    public override string WindowLabel => "My cute window!";

    protected override void Window()
    {
        GUILayout.Label("Hello World!");
        GUILayout.Button("Click me!");
    }
}
```

Put the script on any object in the scene and you should see this:

// pic

#### A few more useful methods

There are more methods that you can use for very quick implementation.

A foldout can be used as a categorizer, it shows [+] when contracted and [-] when expanded
```
bool showOptions;

protected override void Window()
{
	Foldout("Options", ref showOptions, "This is the options foldout!");

	if (showOptions)
	{
		option1 = Slider("Option 1", option1, 0, 1, "Option 1 is the best option");
	}
}
```

```
Label("Text", ["Tooltip"]);
```

```
LabelField("Label", ref stringValue)
```

```
FloatField("Label", value)
```

Slider comes in both float and int flavors:
```
Slider("Label", ref value, min, max, ["Tooltip"])
```
A "notched" float slider that will step by stepSize:
```
Slider("Label", ref value, min, max, stepSize, ["Tooltip"])
```

```
Button("Label", ["Tooltip"])
```

```
ToggleButton("Toggle this!", value)
```

#### The Toolbar

Additionally, you can use a toolbar, which can automatically detect all active windows and display them in a toggleable strip. Just add the ToolbarGUI script to any object and it will be rendered on top edge of the screen.

#### Detecting if mouse over the UI

You can use GameWindow.IsMouseOverUI() to detect if the mouse is currently over any of the windows. This is useful for preventing the mouse "clicking through" the UI or disabling keyboard input, for example.

#### Toggle all GUI with a key

Another utility is the GUIToggler, with which you can toggle the entire GUI (default is \`). Just add the GUIToggler script to any GameObject.