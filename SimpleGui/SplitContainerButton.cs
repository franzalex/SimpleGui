using System.Collections.Generic;
using System.Drawing;
using MemoryStream = System.IO.MemoryStream;

namespace System.Windows.Forms
{
    [Flags()]
    internal enum SplitContainerButtons
    {
        /// <summary>No buttons are displayed.</summary>
        None = 0,

        /// <summary>
        /// Displays the button for changing the <see cref="SplitterContainer.Orientation"/> property.
        /// </summary>
        ChangeOrientation = 1,

        /// <summary>
        /// Displays the button for changing the <see cref="SplitterContainer.Panel1Collapsed"/> property.
        /// </summary>
        Panel1 = 2,

        /// <summary>
        /// Displays the button for changing the <see cref="SplitterContainer.Panel2Collapsed"/> property.
        /// </summary>
        Panel2 = 4,

        /// <summary>Displays the buttons for changing the collapsed state of both panels.</summary>
        PanelControls = Panel1 + Panel2,

        All = PanelControls + ChangeOrientation
    }

    internal partial class SplitContainerTools
    {
        private struct SplitContainerButton
        {
            public static readonly Size ButtonSize;
            public static readonly int MinSplitterWidth;
            public Rectangle Rect;
            public SplitContainerButtons Type;
            private static Dictionary<string, Bitmap> ButtonImages;

            static SplitContainerButton()
            {
                var collapseDbl = "iVBORw0KGgoAAAANSUhEUgAAAAsAAAALCAIAAAAmzuBxAAAAAXNSR0IArs4c6QAA" +
                                  "AARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3" +
                                  "YXJlAHBhaW50Lm5ldCA0LjAuNWWFMmUAAABASURBVChTjYxBDgAgCMP4/6cVZFmG" +
                                  "qLEH0xLEvhh3ygbF0WGgoUIvXxN0v7G9KYGGCh3hoPsEtegZoE5g44XZBJoOtkqX" +
                                  "ZsZgAAAAAElFTkSuQmCC";
                var collapseSng = "iVBORw0KGgoAAAANSUhEUgAAAAsAAAALCAIAAAAmzuBxAAAAAXNSR0IArs4c6QAA" +
                                  "AARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3" +
                                  "YXJlAHBhaW50Lm5ldCA0LjAuNWWFMmUAAAA7SURBVChTlctRCgAgCANQ73/pUlzi" +
                                  "0CLf1yZTvqy74QLloKOXFi0iqHw0ueQQmV4del2oWg1aB4sXkQ1MIcU7Q2r30wAA" +
                                  "AABJRU5ErkJggg==";
                var restoreSng = "iVBORw0KGgoAAAANSUhEUgAAAAsAAAALCAIAAAAmzuBxAAAABGdBTUEAALGPC/xhB" +
                                 "QAAAAlwSFlzAAAOwgAADsIBFShKgAAAABh0RVh0U29mdHdhcmUAcGFpbnQubmV0ID" +
                                 "QuMC41ZYUyZQAAAEFJREFUKFOVjUEOACAIw/j/o1HihoJBoz3RbQnyRDvzuaA4KYS" +
                                 "U7AtVjWpgwAg1QGIkxz13MVyCI/4avUsJFzdEOtwmxS9czrOYAAAAAElFTkSuQmCC";
                var chgToHoriz = "iVBORw0KGgoAAAANSUhEUgAAAAsAAAALCAIAAAAmzuBxAAAABGdBTUEAALGPC/xhB" +
                                 "QAAAAlwSFlzAAAOwgAADsIBFShKgAAAABh0RVh0U29mdHdhcmUAcGFpbnQubmV0I" +
                                 "DQuMC41ZYUyZQAAAB1JREFUKFNjIAr8xw3orQLKQgUoKnABuqrABxgYAJK73SMtl" +
                                 "Cy3AAAAAElFTkSuQmCC";
                var chgToVert = "iVBORw0KGgoAAAANSUhEUgAAAAsAAAALCAIAAAAmzuBxAAAABGdBTUEAALGPC/xhB" +
                                "QAAAAlwSFlzAAAOwgAADsIBFShKgAAAABh0RVh0U29mdHdhcmUAcGFpbnQubmV0ID" +
                                "QuMC41ZYUyZQAAADVJREFUKFONykEKACAQw8D9/6eVltyi4twCmS/r7nA4gypnUOU" +
                                "MqpxBlTOocgZVzqBOOF5mNpK73SMczh/JAAAAAElFTkSuQmCC";

                ButtonImages = new Dictionary<string, Bitmap>();
                ButtonImages.Add("Horiz:ChangeOrientation", new Bitmap(new MemoryStream(Convert.FromBase64String(chgToVert))));
                ButtonImages.Add("Vert:ChangeOrientation", new Bitmap(new MemoryStream(Convert.FromBase64String(chgToHoriz))));

                ButtonImages.Add("Horiz:Panel2.Collapse", new Bitmap(new MemoryStream(Convert.FromBase64String(collapseSng))));
                ButtonImages.Add("Horiz:Panel2.Restore", new Bitmap(new MemoryStream(Convert.FromBase64String(restoreSng))));
                ButtonImages.Add("Horiz:Panel2.DoubleCollapse", new Bitmap(new MemoryStream(Convert.FromBase64String(collapseDbl))));

                ButtonImages.Add("Horiz:Panel1.Collapse", (Bitmap)ButtonImages["Horiz:Panel2.Collapse"].Clone());
                ButtonImages.Add("Horiz:Panel1.Restore", (Bitmap)ButtonImages["Horiz:Panel2.Restore"].Clone());
                ButtonImages.Add("Horiz:Panel1.DoubleCollapse", (Bitmap)ButtonImages["Horiz:Panel2.DoubleCollapse"].Clone());

                ButtonImages.Add("Vert:Panel2.Collapse", (Bitmap)ButtonImages["Horiz:Panel2.Collapse"].Clone());
                ButtonImages.Add("Vert:Panel2.Restore", (Bitmap)ButtonImages["Horiz:Panel2.Restore"].Clone());
                ButtonImages.Add("Vert:Panel2.DoubleCollapse", (Bitmap)ButtonImages["Horiz:Panel2.DoubleCollapse"].Clone());

                ButtonImages.Add("Vert:Panel1.Collapse", (Bitmap)ButtonImages["Vert:Panel2.Collapse"].Clone());
                ButtonImages.Add("Vert:Panel1.Restore", (Bitmap)ButtonImages["Vert:Panel2.Restore"].Clone());
                ButtonImages.Add("Vert:Panel1.DoubleCollapse", (Bitmap)ButtonImages["Vert:Panel2.DoubleCollapse"].Clone());

                var rotations = new Dictionary<string, RotateFlipType>()
                                {
                                    {"Horiz:Panel2", RotateFlipType.RotateNoneFlipNone},
                                    {"Horiz:Panel1", RotateFlipType.Rotate180FlipNone},
                                    {"Vert:Panel2", RotateFlipType.Rotate270FlipNone},
                                    {"Vert:Panel1", RotateFlipType.Rotate90FlipNone},
                                    {"Horiz:ChangeOrientation", RotateFlipType.RotateNoneFlipNone},
                                    {"Vert:ChangeOrientation", RotateFlipType.RotateNoneFlipNone}
                                };

                foreach (string imageKey in ButtonImages.Keys)
                {
                    var rotateKey = imageKey.Split('.')[0];
                    ButtonImages[imageKey].RotateFlip(rotations[rotateKey]);
                }

                SplitContainerButton.ButtonSize = ButtonImages["Vert:Panel1.Collapse"].Size;
                // MinSplitterWidth = Max(ButtonWidth, ButtonHeight) + 2px on each side
                SplitContainerButton.MinSplitterWidth = Math.Max(ButtonSize.Width, ButtonSize.Height) + 4;
            }

            /// <summary>Gets a value indicating whether this instance has been instantiated.</summary>
            /// <value><c>true</c> if this instance has been instantiated; otherwise, <c>false</c>.</value>
            internal bool IsInstantiated
            {
                get { return !this.Equals(default(SplitContainerButton)); }
            }

            internal Image GetImage(SplitContainer split)
            {
                var btnKey = split.Orientation == Orientation.Horizontal ? "Horiz:" : "Vert:";
                var pos = 1;

                if (split.Panel1Collapsed)
                    pos = 0;
                else if (split.Panel2Collapsed)
                    pos = 2;

                switch (this.Type)
                {
                    case SplitContainerButtons.Panel1:
                        btnKey += "Panel1." + new[] { "Restore", "Collapse", "DoubleCollapse" }[pos];
                        break;

                    case SplitContainerButtons.Panel2:
                        btnKey += "Panel2." + new[] { "DoubleCollapse", "Collapse", "Restore" }[pos];
                        break;

                    case SplitContainerButtons.ChangeOrientation:
                        btnKey += "ChangeOrientation";
                        break;

                    default:
                        return null;
                }

                return SplitContainerButton.ButtonImages[btnKey];
            }

            internal string GetToolTip(SplitContainer split)
            {
                var pos = 1;
                if (split.Panel1Collapsed)
                    pos = 0;
                else if (split.Panel2Collapsed)
                    pos = 2;

                string panel1, panel2;
                if (split.Orientation == Orientation.Horizontal)
                {
                    panel1 = " top panel";
                    panel2 = " bottom panel";
                }
                else
                {
                    panel1 = " left panel";
                    panel2 = " right panel";
                }

                switch (this.Type)
                {
                    case SplitContainerButtons.ChangeOrientation:
                        return "Change splitter orientation.";

                    case SplitContainerButtons.Panel1:
                        return new[] { "Restore", "Collapse", "Collapse" }[pos] + panel1;

                    case SplitContainerButtons.Panel2:
                        return new[] { "Collapse", "Collapse", "Restore" }[pos] + panel2;

                    default:
                        return "";
                }
            }
        }
    }
}