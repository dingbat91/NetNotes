using System;
using Microsoft.VisualBasic;
using Terminal.Gui;

//Init Terminal UI
Application.Init();
//quick alias for top level UI
var top = Application.Top;

//Primary window
var mainWindow = new Window("NetNote")
{
    X = 0,
    Y = 1,

    Width = Dim.Fill(),
    Height = Dim.Fill()
};
//add primary window to element
top.Add(mainWindow);

var mainMenu = new MenuBar(new MenuBarItem[]{
new MenuBarItem("_File",new MenuItem[]{
    new MenuItem("_New","Creates new note", null),
    new MenuItem("_Quit","Quits the Application",() => {if (Quit ()) top.Running = false;})
})


});
top.Add(mainMenu);



// Quit prompt
static bool Quit()
{
    var n = MessageBox.Query(50, 7, "Quit NetNote", "Are you sure you want to quit this app?", "Yes", "No");
    return n == 0;
}
//Execute GUI
Application.Run();

