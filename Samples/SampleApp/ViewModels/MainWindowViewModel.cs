using System;
using System.Collections.ObjectModel;
using Avalonia.BreadcrumbBar;
using CommunityToolkit.Mvvm.Input;
using SampleApp.Models;

namespace SampleApp.ViewModels;

public sealed partial class MainWindowViewModel
{
    public ObservableCollection<Crumb> Items { get; } = [
        new("Home", "fa-solid fa-house"),
        new("Projects", "fa-solid fa-cube"),
        new("Documents", "fa-solid fa-file"),
    ];

    [RelayCommand]
    private void Add()
    {
        Items.Add(new Crumb($"Test {Items.Count + 1}", "fa-solid fa-folder"));
    }

    [RelayCommand]
    private void Change()
    {
        Items[0].Title = "someverylongstringoftexttocauseanoverflowonchange";
    }

    [RelayCommand]
    private void NavTo(BreadcrumbBarItem item)
    {
        Console.WriteLine($"Command -> {(item.DataContext is Crumb { Title: { } title } ? title : item)}");
    }
}