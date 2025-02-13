using ReactiveUI;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HogOscChatTester.ViewModels;

public class RoutableViewModelBase : ReactiveObject, IRoutableViewModel, IValidatableViewModel, IActivatableViewModel
{
    public IScreen HostScreen
    {
        get;
        protected set;
    }

    public string? UrlPathSegment
    {
        get;
    } = Guid.NewGuid().ToString().Substring(0, 5);

    public IValidationContext ValidationContext
    {
        get; 
    } = new ValidationContext();

    public ViewModelActivator Activator
    {
        get; 
    } = new ViewModelActivator();
}
