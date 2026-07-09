using System;
using System.Collections.Generic;
using Avalonia.BreadcrumbBar;
using CommunityToolkit.Mvvm.Input;
using SampleApp.Models;

namespace SampleApp.ViewModels;

public sealed partial class MainWindowViewModel
{
    public IEnumerable<Crumb> Items => [
        new("Home", "fa-solid fa-house"),
        new("Projects", "fa-solid fa-cube"),
        new("Documents", "fa-solid fa-file"),
    ];

    [RelayCommand]
    private void NavTo(BreadcrumbBarItem item)
    {
        Console.WriteLine($"Command -> {(item.DataContext is Crumb { Title: { } title } ? title : item)}");
    }
}