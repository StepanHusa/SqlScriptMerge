using SqlScriptMerge.GUI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace SqlScriptMerge.GUI.Views;
/// <summary>
/// Interaction logic for MainView.xaml
/// </summary>
public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    private FileModel? draggedItem;

    private void StringList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        ListBox listBox = (ListBox)sender;
        draggedItem = (FileModel)listBox.SelectedItem;
    }

    private void StringList_PreviewMouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            ListBox listBox = (ListBox)sender;
            if (draggedItem!=null)
            {
                DataObject data = new DataObject(DataFormats.StringFormat, draggedItem);
                DragDrop.DoDragDrop(listBox, data, DragDropEffects.Move);
            }
        }
    }

    private void StringList_Drop(object sender, DragEventArgs e)
    {
        ListBox listBox = (ListBox)sender;
        var collection = (ObservableCollection<FileModel>)listBox.ItemsSource;

        if (e.Data.GetDataPresent(DataFormats.StringFormat))
        {
            var droppedItem = e.Data.GetData(DataFormats.StringFormat) as FileModel;
            if(droppedItem == null) { return; }
            int dropIndex = listBox.Items.IndexOf(listBox.SelectedItem);
            if (dropIndex >= 0)
            {
                collection.Remove(droppedItem);
                collection.Insert(dropIndex, droppedItem);
            }
        }
    }

}
