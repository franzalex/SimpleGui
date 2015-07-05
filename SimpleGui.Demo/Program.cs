namespace SimpleGui.Demo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            SimpleGui.WindowTitle = "Test";
            SimpleGui.ControlsPanel.AddLabel("Welcome");
            SimpleGui.ControlsPanel.AddTextBox("Name", (s) => SimpleGui.Output.Print("The input is " + s));
            SimpleGui.ControlsPanel.AddComboBox("Options", (s) => SimpleGui.Output.Print(s + " has been selected."), "A", "B", "C", "D");
            SimpleGui.ControlsPanel.AddButton("Click me", (s) => SimpleGui.Output.Print("The button has been clicked."));
            SimpleGui.ControlsPanel.AddButton(".", (s) => SimpleGui.CanvasPanel.Visible = !SimpleGui.CanvasPanel.Visible);

            SimpleGui.Start();
        }
    }
}