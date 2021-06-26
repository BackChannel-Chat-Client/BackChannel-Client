using BackChannel.Classes;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace BackChannel.ViewModels
{
    public class MemberViewModel : INotifyPropertyChanged
    {
        // UI Notification functions
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // Member collection
        private ObservableCollection<Member> members = new ObservableCollection<Member>();
        public ObservableCollection<Member> Members
        {
            get { return members; }
            set
            {
                members = value;
                this.OnPropertyChanged("Members");
            }
        }

        // Helper functions
        public void AddMember(Member m)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Members.Add(m);
            }));
        }
        public void RemoveMember(Member m)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Members.Remove(m);
            }));
        }
    }
}
