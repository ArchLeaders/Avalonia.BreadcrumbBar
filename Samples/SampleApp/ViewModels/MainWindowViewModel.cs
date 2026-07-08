using System.Collections.Generic;
using SampleApp.Models;

namespace SampleApp.ViewModels;

public sealed class MainWindowViewModel
{
    public IEnumerable<Crumb> Items => [
        new("Home", "fa-solid fa-house"),
        new("Projects", "fa-solid fa-cube"),
        new("Documents", "fa-solid fa-file"),
    ];
}