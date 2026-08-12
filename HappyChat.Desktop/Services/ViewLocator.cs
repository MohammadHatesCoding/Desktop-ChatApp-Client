using Avalonia.Controls;
using Avalonia.Controls.Templates;
using HappyChat.Desktop.ViewModels;
using System;

namespace HappyChat.Desktop.Services;

public sealed class ViewLocator : IDataTemplate
{
    public Control Build(object? data)
    {
        if (data is null)
            return new TextBlock();

        var viewModelType = data.GetType();

        var viewTypeName = viewModelType.FullName!.Replace(".ViewModels.", ".Views.").Replace("ViewModel","View");

        var viewType =Type.GetType(viewTypeName);

        if (viewType is null)
        {
            return new TextBlock
            {
                Text = $"View not found: {viewTypeName}"
            };
        }

        if (Activator.CreateInstance(viewType) is not Control view)
        {
            return new TextBlock
            {
                Text = $"Could not create view: {viewTypeName}"
            };
        }

        return view;
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}