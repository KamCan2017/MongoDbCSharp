using Developer;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Repository;
using System.Collections.Generic;
using System.Windows;

namespace Client.Developer.ViewModels
{
    public class KnowledgeEditorViewModel : BindableBase
    {
        private DelegateCommand<Window> _saveCommand;
        private KnowledgeModel _knowledge;
        private IKnowledgeRepository _knowledgeRepository;
        private readonly IEventAggregator _eventAggregator;



        public KnowledgeEditorViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _knowledgeRepository = new KnowledgeRepository();
            _saveCommand = new DelegateCommand<Window>(window => Accept(window), CanExecuteSaveCommand);
            Knowledge = new KnowledgeModel();
            Knowledge.PropertyChanged += Knowledge_PropertyChanged;
        }        

        public DelegateCommand <Window> SaveCommand { get { return _saveCommand; } }


        public KnowledgeModel Knowledge
        {
            get { return _knowledge; }
            set
            {
                SetProperty(ref _knowledge, value);
            }
        }

        public List<string> Technologies
        {
            get
            {
                return new List<string>()
                {
                    Technology.APSNet,
                    Technology.NoSQL,
                    Technology.QT,
                    Technology.WPF,
                    Technology.Apps,
                    Technology.UWP,
                    Technology.WinForms,
                    Technology.Xamarin,
                    Technology.ImageProcessing,
                };
            }
        }


        public List<string> Languages
        {
            get
            {
                return new List<string>()
                {
                    ProgrammingLanguage.CPlus,
                    ProgrammingLanguage.CScharp,
                    ProgrammingLanguage.Html,
                    ProgrammingLanguage.Java,
                    ProgrammingLanguage.Python,
                    ProgrammingLanguage.TypScript,
                    ProgrammingLanguage.Catia,
                    ProgrammingLanguage.Xml
                };
            }
        }

        private void Knowledge_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }


        private bool CanExecuteSaveCommand(Window arg)
        {
            return Knowledge != null && Knowledge.IsValid;
        }

        private bool Accept(Window window)
        {
            window.DialogResult = true;
            window.Close();
            _eventAggregator.GetEvent<AddKnowledgePubEvent>().Publish(Knowledge);
            return true;
        }
    }
}
