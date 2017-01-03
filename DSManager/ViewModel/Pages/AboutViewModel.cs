using System.Diagnostics;
using GalaSoft.MvvmLight.Command;

namespace DSManager.ViewModel.Pages {
    public class AboutViewModel : BaseViewModel {
        private RelayCommand _sendEmail;
        private RelayCommand _openGitHubWebPage;
        private RelayCommand _openLinkedInWebPage;

        public RelayCommand SendMail => _sendEmail ?? (_sendEmail = new RelayCommand(() => Process.Start("mailto:marek.kaminski.93@gmail.com?subject=&body=")));
        public RelayCommand OpenGitHubWebPage => _openGitHubWebPage ?? (_openGitHubWebPage = new RelayCommand(() => Process.Start("https://github.com/wornr")));
        public RelayCommand OpenLinkedInWebPage => _openLinkedInWebPage ?? (_openLinkedInWebPage = new RelayCommand(() => Process.Start("https://www.linkedin.com/in/marek-kamiński-a94ab5ba")));
    }
}
