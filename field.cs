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


class Field : Button
{
    public int type;
    public bool flagged = false;
    public string displayedContent = "";
    private string content = "Nothing";
    private Field[] neighbors;

    // starts at top Left
    public void setNeighbors(int fieldIndex, int gridRowLength, Field[] fields)
    {

        if (fieldIndex == gridRowLength * gridRowLength - gridRowLength)// bottom left 
        {
            neighbors = new Field[3];
            neighbors[0] = fields[fieldIndex - gridRowLength];
            neighbors[1] = fields[fieldIndex - gridRowLength + 1];
            neighbors[2] = fields[fieldIndex + 1];
        }
        else if (fieldIndex == gridRowLength * gridRowLength - 1)// bottom right
        {
            neighbors = new Field[3];
            neighbors[0] = fields[fieldIndex - gridRowLength - 1];
            neighbors[1] = fields[fieldIndex - gridRowLength];
            neighbors[2] = fields[fieldIndex - 1];
        }
        else if (fieldIndex == 0)// top left 
        {
            neighbors = new Field[3];
            neighbors[0] = fields[fieldIndex + 1];
            neighbors[1] = fields[fieldIndex + gridRowLength + 1];
            neighbors[2] = fields[fieldIndex + gridRowLength];
        }
        else if (fieldIndex == gridRowLength - 1)// top right
        {
            neighbors = new Field[3];
            neighbors[0] = fields[fieldIndex + gridRowLength];
            neighbors[1] = fields[fieldIndex + gridRowLength - 1];
            neighbors[2] = fields[fieldIndex - 1];
        }
        else if (fieldIndex < gridRowLength)// top row
        {
            neighbors = new Field[5];
            neighbors[0] = fields[fieldIndex + 1];
            neighbors[1] = fields[fieldIndex + gridRowLength + 1];
            neighbors[2] = fields[fieldIndex + gridRowLength];
            neighbors[3] = fields[fieldIndex + gridRowLength - 1];
            neighbors[4] = fields[fieldIndex - 1];
        }
        else if (fieldIndex >= gridRowLength * gridRowLength - gridRowLength)// bottom row
        {
            neighbors = new Field[5];
            neighbors[0] = fields[fieldIndex - gridRowLength - 1];
            neighbors[1] = fields[fieldIndex - gridRowLength];
            neighbors[2] = fields[fieldIndex - gridRowLength + 1];
            neighbors[3] = fields[fieldIndex + 1];
            neighbors[4] = fields[fieldIndex - 1];
        }
        else if (fieldIndex % gridRowLength == gridRowLength - 1)// right row
        {
            neighbors = new Field[5];
            neighbors[0] = fields[fieldIndex - gridRowLength - 1];
            neighbors[1] = fields[fieldIndex - gridRowLength];
            neighbors[2] = fields[fieldIndex + gridRowLength];
            neighbors[3] = fields[fieldIndex + gridRowLength - 1];
            neighbors[4] = fields[fieldIndex - 1];
        }
        else if (fieldIndex % gridRowLength == 0)// left row
        {
            neighbors = new Field[5];
            neighbors[0] = fields[fieldIndex - gridRowLength];
            neighbors[1] = fields[fieldIndex - gridRowLength + 1];
            neighbors[2] = fields[fieldIndex + 1];
            neighbors[3] = fields[fieldIndex + gridRowLength + 1];
            neighbors[4] = fields[fieldIndex + gridRowLength];
        }
        else
        {
            neighbors = new Field[8];
            neighbors[0] = fields[fieldIndex - gridRowLength - 1];
            neighbors[1] = fields[fieldIndex - gridRowLength];
            neighbors[2] = fields[fieldIndex - gridRowLength + 1];
            neighbors[3] = fields[fieldIndex + 1];
            neighbors[4] = fields[fieldIndex + gridRowLength + 1];
            neighbors[5] = fields[fieldIndex + gridRowLength];
            neighbors[6] = fields[fieldIndex + gridRowLength - 1];
            neighbors[7] = fields[fieldIndex - 1];
        }
    }

    public void setType(Field[] passedFields)
    {
        int mines = 0;
        if (type != 9)
        {

            foreach (Field neighbor in neighbors)
            {
                if (neighbor.type == 9)
                {
                    mines++;
                }
            }

            type = mines;
            content = type.ToString();
        }
        else
        {
            content = "💣";
        }
    }
    public void setColor()
    {

        this.Background = Brushes.DarkGray;
        switch (type)
        {
            case 0:
                this.Foreground = Brushes.LightGreen;
                break;
            case 1:
                this.Foreground = Brushes.Green;
                break;
            case 2:
                this.Foreground = Brushes.Blue;
                break;
            case 3:
                this.Foreground = Brushes.DarkBlue;
                break;
            case 4:
                this.Foreground = Brushes.LightYellow;
                break;
            case 5:
                this.Foreground = Brushes.Yellow;
                break;
            case 6:
                this.Foreground = Brushes.Orange;
                break;
            case 7:
                this.Foreground = Brushes.Red;
                break;
            default:
                this.Foreground = Brushes.Black;
                break;
        }
    }
    public void neighborCheck(Field passedField)
    {
        if (type == 0 && displayedContent == "")
        {
            if (!flagged)
            {
                displayedContent = content;
                passedField.Content = displayedContent;
                foreach (Field neighbor in neighbors)
                {
                    neighbor.neighborCheck(neighbor);
                }
            }
        }
        displayedContent = content;
        passedField.Content = displayedContent;
    }
    public void uncover_btn(Object sender, RoutedEventArgs e)
    {
        if (flagged == false)
        {
            this.Content = content;
            if (type == 9)
            {
                if (Game.firstClick == true)
                {
                    MainWindow.firstClickMine(this.Name);
                    Game.firstClick = false;
                }
                else
                {
                    MainWindow.boom(false);
                }

            }
            else if (type == 0)
            {
                this.neighborCheck(this);
            }
            displayedContent = content;
            Game.click(sender);
        }
    }
    public void flag_btn(Object sender, RoutedEventArgs e)
    {
        Game.firstClick = false;
        Game.click(sender);
        if (flagged)
        {
            Game.unFlag(this);
        }
        else if (flagged == false && displayedContent == "")
        {
            Game.flag(this);
        }
    }
}
