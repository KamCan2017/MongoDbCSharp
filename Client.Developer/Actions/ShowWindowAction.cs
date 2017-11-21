using System;
using Prism.Interactivity.InteractionRequest;
using System.Windows.Interactivity;
using System.Windows;
using Client.Developer.Views;

namespace Client.Developer.Actions
{
    public class ShowWindowAction : TriggerAction<FrameworkElement>
    {
        protected override void Invoke(object parameter)
        {
            InteractionRequestedEventArgs args = parameter as InteractionRequestedEventArgs;
            if (args != null)
            {
                Confirmation confirmation = args.Context as Confirmation;
                if (confirmation != null)
                {
                    var window = new KnowledgeEditor();
                    EventHandler closeHandler = null;
                    closeHandler = (sender, e) =>
                    {
                        window.Closed -= closeHandler;
                        args.Callback();
                    };
                    window.Closed += closeHandler;
                    window.ShowDialog();
                }
            }
        }
    }
}
