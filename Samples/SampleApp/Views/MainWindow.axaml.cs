using System;
using Avalonia.BreadcrumbBar.Interactivity;
using Avalonia.Controls;
using SampleApp.Models;

namespace SampleApp.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void BreadcrumbBar_OnItemClicked(object? sender, BreadcrumbBarItemClickedEventArgs e)
    {
        Console.WriteLine($"OnItemClicked -> {e.Item.Content}");
    }
}