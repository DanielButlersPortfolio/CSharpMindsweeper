using System;
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

public partial class MainWindow
{
    private static int mines = 25;
    private static int fields = 100;
    private static Game game;
    private static Grid staticBoard;
    private static TextBlock staticMines;
    private static TextBlock staticFlags;
    public MainWindow()
    {
        InitializeComponent();
        staticBoard = Board;
        staticMines = Mines;
        staticFlags = Flags;
        reset();
    }

    private static void reset()
    {
        game = new Game(mines, fields, staticBoard, staticMines, staticFlags);
        game.startGame();
    }
    public void resetBtn(Object sender, RoutedEventArgs e)
    {
        reset();
    }

    public void easy(Object sender, RoutedEventArgs e)
    {
        mines = fields / 10;
        reset();
    }
    public void normal(Object sender, RoutedEventArgs e)
    {
        mines = fields / 4;
        reset();
    }
    public void hard(Object sender, RoutedEventArgs e)
    {
        mines = fields / 2;
        reset();
    }
    public void fieldSize(Object sender, RoutedEventArgs e)
    {
        fields = fields * 4;
        if (fields > 400)
        {
            fields = 25;
        }
        mines = fields / 2;
        reset();
    }
    public static void boom(bool win)
    {
        if (win)
        {
            MessageBox.Show("YOU WIN!!!");
        }
        else
        {
            MessageBox.Show("BOOM!!!");
        }
        reset();
    }

    public static void firstClickMine(string senderIndex)
    {
        int senderIndexInt = Convert.ToInt32(senderIndex.Remove(0, 1));
        reset();
        Game.fields[senderIndexInt].uncover_btn(new Object(), new RoutedEventArgs());

    }
}
