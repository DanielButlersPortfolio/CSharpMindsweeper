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

class Game
{
    public static bool firstClick = true;
    private int totalMines;
    private int totalFields;
    private int rowLength;
    public static int availableFlags;
    public static Field[] fields;
    private static Field[] mineFields;
    private Grid board;
    private TextBlock mineIndicator;
    private static TextBlock flagIndicator;

    public Game(int passedTotalMines, int passedTotalFields, Grid passedBoard, TextBlock passedMineIndicator, TextBlock PassedFlagIndicator)
    {
        totalMines = passedTotalMines;
        totalFields = passedTotalFields;
        fields = new Field[passedTotalFields];
        rowLength = (int)Math.Sqrt(totalFields);
        mineIndicator = passedMineIndicator;
        board = passedBoard;
        flagIndicator = PassedFlagIndicator;
    }
    public bool checkWin()
    {
        foreach (Field field in mineFields)
        {
            if (!field.flagged)
            {
                return false;
            }
        }
        return true;
    }

    public static void click(Object sender)
    {
        bool flagged = false;
        bool uncovered = false;

        // check if all mines are flagged
        foreach (Field field in mineFields)
        {
            if (!field.flagged)
            {
                flagged = false;
                break;
            }
            else
            {
                flagged = true;
            }
        }

        // check if all other field are uncovered
        foreach (Field field in fields)
        {
            if (field.type == 9)
            {
            }
            else if (field.displayedContent != "" && field.displayedContent != "🏴")
            {
                uncovered = true;
            }
            else
            {
                uncovered = false;
                break;
            }
        }

        if (flagged || uncovered)
        {
            MainWindow.boom(true);
        }
    }

    public static void flag(Field sender)
    {
        if (availableFlags <= 0)
        {
            return;
        }
        sender.Foreground = Brushes.Black;
        sender.displayedContent = "🏴";
        sender.Content = sender.displayedContent;
        sender.flagged = true;

        availableFlags--;
        flagIndicator.Text = "Available Flags:\n" + Convert.ToString(availableFlags);
    }
    public static void unFlag(Field sender)
    {
        sender.displayedContent = "";
        sender.flagged = false;
        sender.Content = sender.displayedContent;
        sender.setColor();

        availableFlags++;
        flagIndicator.Text = "Available Flags:\n" + Convert.ToString(availableFlags);
    }

    public void startGame()
    {
        Game.firstClick = true;
        int index = 0;
        int btnContentIndex = 0;
        int availableMines = totalMines;
        Random ran = new Random();
        mineIndicator.Text = Convert.ToString("Total Mines:\n" + totalMines);
        availableFlags = totalMines;

        foreach (Field field in fields)// make fields in code
        {
            fields[index] = new Field();
            index++;
        }
        index = 0;

        while (availableMines > 0)// set field type
        {
            int ranInt = ran.Next(0, totalFields);
            if (fields[ranInt].type != 9)
            {
                fields[ranInt].type = 9;
                availableMines--;
            }
        }

        foreach (Field field in fields)//set field neighbors
        {
            field.setNeighbors(index, rowLength, fields);
            field.setType(fields);
            index++;
        }

        // clear wpf
        board.Children.Clear();
        board.RowDefinitions.Clear();
        board.ColumnDefinitions.Clear();
        for (int i = 0; i < rowLength; i++)// add rows and columns to wpf
        {
            board.RowDefinitions.Add(new RowDefinition());
            board.ColumnDefinitions.Add(new ColumnDefinition());
        }

        foreach (Field field in fields)// add fields to wpf
        {
            field.Content = fields[btnContentIndex].displayedContent;
            int column = 0;
            int row = 0;

            if (btnContentIndex < rowLength)
            {
                row = 0;
                column = btnContentIndex;
            }
            else
            {
                column = btnContentIndex % rowLength;
                row = (btnContentIndex - column) / rowLength;
            }

            Grid.SetRow(field, row);
            Grid.SetColumn(field, column);

            field.MouseRightButtonDown += field.flag_btn;
            field.Click += field.uncover_btn;
            field.Margin = new Thickness(0, 0, 0, 0);
            field.Name = "f" + Convert.ToString(btnContentIndex);
            field.SetValue(TextBlock.FontWeightProperty, FontWeights.Bold);
            field.FontSize = 25;

            board.Children.Add(field);
            btnContentIndex++;
        }

        index = 0;
        mineFields = new Field[totalMines];

        foreach (Field field in fields)// determine mine fields and set color
        {
            field.setColor();
            if (field.type == 9)
            {
                mineFields[index] = field;
                index++;
            }
        }

        // set Flag Indicator
        flagIndicator.Text = "Available Flags:\n" + Convert.ToString(availableFlags);
    }
}
