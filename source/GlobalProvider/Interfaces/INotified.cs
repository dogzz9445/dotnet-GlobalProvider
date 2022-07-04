using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

#nullable enable
namespace Mini.GlobalProvider
{
    public enum NotifiedEventType
    {
        Notified,
        PropertyChanged
    }

    public class NotifiedEventArgs : EventArgs
    {
        public NotifiedEventArgs()
        {
            NotifiedType = NotifiedEventType.Notified;
        }

        public NotifiedEventArgs(NotifiedEventType? type, string? propertyName)
        {
            NotifiedType = type;
            PropertyName = propertyName;
        }

        public virtual NotifiedEventType? NotifiedType { get; }
        public virtual string? PropertyName { get; }
    }

    public delegate void NotifiedEventHandler(object? sender, NotifiedEventArgs e);

    public interface INotified
    {
        event NotifiedEventHandler Notified;
    }

    public class Notifiable : INotified
    {
        public event NotifiedEventHandler? Notified;

        protected void Notify(object? sender, NotifiedEventArgs eventArgs)
        {
            Notified?.Invoke(this, eventArgs);
        }

        protected void Notify([CallerMemberName] string? propertyName = null)
        {
            Notified?.Invoke(this, new NotifiedEventArgs(NotifiedEventType.PropertyChanged, propertyName));
        }

        protected void Notify()
        {
            Notified?.Invoke(this, new NotifiedEventArgs());
        }

        private void Notify(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                Notified?.Invoke(sender, new NotifiedEventArgs(NotifiedEventType.PropertyChanged, "Add"));
            }
            if (e.OldItems != null)
            {
                Notified?.Invoke(sender, new NotifiedEventArgs(NotifiedEventType.PropertyChanged, "Delete"));
            }
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(storage, value))
            {
                return false;
            }
            storage = value;
            Notify(propertyName);
            return true;
        }

        protected bool SetObservableProperty<T>(ref T? storage, T value, [CallerMemberName] string? propertyName = null) where T : INotified
        {
            if (Equals(storage, value))
            {
                return false;
            }
            if (storage != null)
            {
                storage.Notified -= new NotifiedEventHandler(Notify);
            }
            bool result = SetProperty(ref storage, value);
            if (storage != null)
            {
                storage.Notified += new NotifiedEventHandler(Notify);
            }
            return result;
        }

        protected bool SetCollectionProperty<T>(ref T? storage, T value, [CallerMemberName] string? propertyName = null) where T : INotifyCollectionChanged
        {
            if (storage != null)
            {
                storage.CollectionChanged -= new NotifyCollectionChangedEventHandler((s, e) => Notify());
            }
            bool result = SetProperty(ref storage, value);
            if (storage != null)
            {
                storage.CollectionChanged += new NotifyCollectionChangedEventHandler((s, e) => Notify());
            }
            return result;
        }

        protected bool SetObservablePropertyWithoutNotify<T>(ref T? storage, T value, [CallerMemberName] string? propertyName = null) where T : INotified
        {
            if (Equals(storage, value))
            {
                return false;
            }
            if (storage != null)
            {
                storage.Notified -= new NotifiedEventHandler(Notify);
            }
            storage = value;
            if (storage != null)
            {
                storage.Notified += new NotifiedEventHandler(Notify);
            }
            return true;
        }

        protected bool SetObservablePropertyWithoutNotify(ref dynamic storage, dynamic value, [CallerMemberName] string? propertyName = null) 
        {
            if (Equals(storage, value))
            {
                return false;
            }
            if (storage != null)
            {
                storage.Notified -= new NotifiedEventHandler(Notify);
            }
            storage = value;
            if (storage != null)
            {
                storage.Notified += new NotifiedEventHandler(Notify);
            }
            return true;
        }
    }
}
