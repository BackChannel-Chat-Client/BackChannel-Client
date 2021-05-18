using BackChannel.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BackChannel.ViewModels
{
    public class MemberViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
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
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

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
