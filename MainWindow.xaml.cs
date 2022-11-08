using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace dtp11_MUD_1
{
    /// <summary>
    /// MainWindow: GUI interaction base class
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// labyrinth contains the rooms in the game, it is used to find rooms
        /// </summary>
        Room[] lab = new Room[100];
        int currentRoom;
        List<LockKey> allKeys = new List<LockKey>();
        public MainWindow()
        {
            InitializeComponent();

            // Init Rooms here:
            Room R;
            int noDoor = Room.NoDoor;

            R = new Room(0, "Startrummet");
            R.SetStory("Du står i ett rum med rött tegel. Väggarna fladdrar i facklornas sken. Du ser en hög med tyg nere till vänster. ");
            R.SetImage("room-base.png");
            R.SetImage("room-door-forward-closed.png");
            R.SetImage("garbage-left.png");
            R.HasKey(0, 3);
            lab[0] = R;
            lab[0].SetDirections(N: -3, E: noDoor, S: noDoor, W: noDoor);

            R = new Room(1, "Lagerrum väst");
            R.SetStory("Du står i ett rum utan vägar framåt. Du ser en hög med skräp nere till vänster. ");
            R.SetImage("room-base.png");
            R.SetImage("garbage-left.png");
            R.SetImage("room-door-right.png");
            R.HasKey(8, 9);
            lab[1] = R;
            lab[1].SetDirections(N: noDoor, E: 2, S: noDoor, W: noDoor);

            R = new Room(2, "Vaktrum väst");
            R.SetStory("Du står i ett övergivet vaktrum. ");
            R.SetImage("room-base.png");
            R.SetImage("room-door-left.png");
            R.SetImage("room-door-right-closed.png");
            lab[2] = R;
            lab[2].SetDirections(N: noDoor, E: -3, S: noDoor, W: 1);

            R = new Room(3, "Korsvägen");
            R.SetStory("Du står i korsväg. Det går gångar i alla riktningar. ");
            R.SetImage("room-base.png");
            R.SetImage("room-door-left-closed.png");
            R.SetImage("room-door-forward.png");
            R.SetImage("room-door-right-closed.png");
            // R.SetImage("vattar-nobg.png");
            lab[3] = R;
            lab[3].SetDirections(N: 6, E: -4, S: 0, W: -2);

            R = new Room(4, "Vaktrum öst");
            R.SetStory("Du står i ett övergivet vaktrum. ");
            R.SetImage("room-base.png");
            R.SetImage("room-door-left-closed.png");
            R.SetImage("room-door-right.png");
            R.SetImage("garbage-right.png");
            lab[4] = R;
            lab[4].SetDirections(N: noDoor, E: 5, S: noDoor, W: -3);

            R = new Room(5, "Lagerrum öst");
            R.SetStory("Du står i ett tomt rum. ");
            R.SetImage("room-base.png");
            R.SetImage("room-door-left.png");
            R.SetImage("room-door-forward.png");
            lab[5] = R;
            lab[5].SetDirections(N: 7, E: noDoor, S: noDoor, W: 4);

            R = new Room(6, "Bron");
            R.SetStory("Du står vid en bro som går över en stor klyfta som du inte ser botten på. ");
            R.SetImage("bro.png");
            lab[6] = R;
            lab[6].SetDirections(N: 9, E: noDoor, S: 3, W: noDoor);

            R = new Room(7, "Inre rum öst");
            R.SetStory("Du står i ett rum med bråte överallt. ");
            R.SetImage("room-base.png");
            R.SetImage("garbage-left.png");
            R.SetImage("garbage-right.png");
            R.HasKey(2, 3);
            lab[7] = R;
            lab[7].SetDirections(N: noDoor, E: noDoor, S: 5, W: noDoor);

            R = new Room(8, "Trollisrum");
            R.SetStory("Du står i ett rum. ");
            R.SetImage("room-base.png");
            R.SetImage("room-door-right-closed.png");
            R.SetImage("trollis-nobg.png");
            lab[8] = R;
            lab[8].SetDirections(N: noDoor, E: -9, S: noDoor, W: noDoor);

            R = new Room(9, "Svnuftrum");
            R.SetStory("Du står i ett rum-rum. ");
            R.SetImage("room-base.png");
            R.SetImage("room-door-left-closed.png");
            R.SetImage("garbage-right.png");
            R.HasKey(3, 4);
            lab[9] = R;
            lab[9].SetDirections(N: noDoor, E: noDoor, S: 6, W: -8);

            currentRoom = 0;
            DisplayCurrentRoom();
        }

        private void ApplicationKeyPress(object sender, KeyEventArgs e)
        {
            string output = "Key pressed: ";
            output += e.Key.ToString();
            // output += ", ";
            // output += AppDomain.CurrentDomain.BaseDirectory;
            KeyPressDisplay.Text = output;

            if (e.Key == Key.Escape)
            {
                System.Windows.Application.Current.Shutdown();
            }
            else if (e.Key == Key.Up)
            {
                currentRoom = lab[currentRoom].GetNorth();
                DisplayCurrentRoom();
            }
            else if (e.Key == Key.Down)
            {
                currentRoom = lab[currentRoom].GetSouth();
                DisplayCurrentRoom();
            }
            else if (e.Key == Key.Left)
            {
                currentRoom = lab[currentRoom].GetWest();
                DisplayCurrentRoom();
            }
            else if (e.Key == Key.Right)
            {
                currentRoom = lab[currentRoom].GetEast();
                DisplayCurrentRoom();
            }
            
            else if (e.Key == Key.S)
            {
                allKeys = lab[currentRoom].getKeys(allKeys);
                DisplayCurrentRoom();
                string txt = "";
                foreach (LockKey lc in allKeys)
                {
                    txt += $"{lc.adjRooms[0]}/{lc.adjRooms[1]} - ";
                }
                ResponseText.Text = txt;
            }
            else if (e.Key == Key.N)
            {
                lab[currentRoom].Unlock(allKeys);
                DisplayCurrentRoom();
                Room R = lab[currentRoom];
                string txt = "";
                for (int i = Room.North; i <= Room.West; i++)
                {
                    string openTxt;
                    if (R.isOpen[i]) openTxt = "öppen";
                    else if (R.adjacent[i] == Room.NoDoor) openTxt = "finns inte";
                    else openTxt = "stängd";
                    string CD = Room.CardinalDirection(i);
                    txt += $"{CD}: {openTxt} (till rum {R.adjacent[i]}) ";
                }
                ResponseText.Text = txt;
            }
            else if (e.Key == Key.D0)
            {
                currentRoom = 0;
                DisplayCurrentRoom();
            }
        }
        private void DisplayCurrentRoom()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            Room R = lab[currentRoom];
            DrawImages(baseDir, R);
            string text = $"Du befinner dig i {R.roomname}. ";
            text += $"{R.story} ";
            text += CreateDirectionsText(R);
            RoomText.Text = text;

            System.Windows.Controls.TextBlock[] keyAlts = {
                KeyAlt1, KeyAlt2, KeyAlt3, KeyAlt4, KeyAlt5, KeyAlt6, KeyAlt7
            };
            int ix = 0;
            if (R.GetNorth() != currentRoom)
                keyAlts[ix++].Text = "upp      gå norrut";
            if (R.GetEast() != currentRoom)
                keyAlts[ix++].Text = "höger    gå österut";
            if (R.GetSouth() != currentRoom)
                keyAlts[ix++].Text = "ner      gå söderut";
            if (R.GetWest() != currentRoom)
                keyAlts[ix++].Text = "vänster  gå västerut";
            keyAlts[ix++].Text = "s        sök";
            keyAlts[ix++].Text = "n        lås upp";
            keyAlts[ix++].Text = "escape   avsluta";
            while (ix < 7) keyAlts[ix++].Text = "";
        }

        private string CreateDirectionsText(Room R)
        {
            string directions;
            List<string> dir = new List<string>();
            if (R.GetNorth() != currentRoom) dir.Add("norr");
            if (R.GetEast() != currentRoom) dir.Add("öster");
            if (R.GetSouth() != currentRoom) dir.Add("söder");
            if (R.GetWest() != currentRoom) dir.Add("väster");
            if (dir.Count == 0)
            {
                directions = "Du kan inte komma vidare härifrån.";
            }
            else
            {
                directions = $"Härifrån kan du gå åt: {dir[0]}";
                for (int i = 1; i < dir.Count; i++) directions += $", {dir[i]}";
                directions += ". ";
            }
            return directions;
        }

        private void DrawImages(string baseDir, Room R)
        {
            System.Windows.Controls.Image[] imgLayer = {
                RoomImage1, RoomImage2, RoomImage3, RoomImage4, RoomImage5, RoomImage6
            };
            foreach (System.Windows.Controls.Image img in imgLayer) img.Source = null;
            for (int i = 0; i < R.imageFile.Count; i++)
            {
                string bitmapFileName1 = $"{baseDir}/{R.imageFile[i]}";
                if (File.Exists(bitmapFileName1))
                {
                    Uri uri = new Uri(bitmapFileName1, UriKind.RelativeOrAbsolute);
                    imgLayer[i].Source = BitmapFrame.Create(uri);
                }
            }
        }
    }
}
