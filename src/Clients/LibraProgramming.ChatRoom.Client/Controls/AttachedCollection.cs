using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace LibraProgramming.ChatRoom.Client.Controls
{
    public class AttachedCollection<T> : ObservableCollection<T>, IAttachedObject
        where T : BindableObject, IAttachedObject
    {
        private readonly List<WeakReference> attachedObjects;

        public AttachedCollection()
        {
            attachedObjects = new List<WeakReference>();
        }

        public AttachedCollection(IEnumerable<T> collection)
            : base(collection)
        {
            attachedObjects = new List<WeakReference>();
        }

        public AttachedCollection(IList<T> list)
            : base(list)
        {
            attachedObjects = new List<WeakReference>();
        }

        public void AttachTo(BindableObject bindable)
        {
            if (null == bindable)
            {
                throw new ArgumentNullException(nameof(bindable));
            }

            OnAttachedTo(bindable);
        }

        public void DetachFrom(BindableObject bindable)
        {
            if (null == bindable)
            {
                throw new ArgumentNullException(nameof(bindable));
            }           

            OnDetachingFrom(bindable);
        }

        protected override void ClearItems()
        {
            foreach (var weakbindable in attachedObjects)
            {
                foreach (var item in this)
                {
                    if (weakbindable.Target is BindableObject bindable)
                    {
                        item.DetachFrom(bindable);
                    }
                }
            }

            base.ClearItems();
        }

        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);

            foreach (var weakbindable in attachedObjects)
            {
                if (weakbindable.Target is BindableObject bindable)
                {
                    item.AttachTo(bindable);
                }
            }
        }

        protected virtual void OnAttachedTo(BindableObject bindable)
        {
            lock (attachedObjects)
            {
                attachedObjects.Add(new WeakReference(bindable));
            }

            foreach (var item in this)
            {
                item.AttachTo(bindable);
            }
        }

        protected virtual void OnDetachingFrom(BindableObject bindable)
        {
            foreach (var item in this)
            {
                item.DetachFrom(bindable);
            }

            lock (attachedObjects)
            {
                for (var index = 0; index < attachedObjects.Count; index++)
                {
                    var target = attachedObjects[index].Target;

                    if (target != null && target != bindable)
                    {
                        continue;
                    }

                    attachedObjects.RemoveAt(index);

                    index--;
                }
            }
        }

        protected override void RemoveItem(int index)
        {
            var item = this[index];

            foreach (var weakbindable in attachedObjects)
            {
                if (weakbindable.Target is BindableObject bindable)
                {
                    item.DetachFrom(bindable);
                }
            }

            base.RemoveItem(index);
        }

        protected override void SetItem(int index, T item)
        {
            var old = this[index];

            foreach (var weakbindable in attachedObjects)
            {
                if (weakbindable.Target is BindableObject bindable)
                {
                    old.DetachFrom(bindable);
                }
            }

            base.SetItem(index, item);

            foreach (var weakbindable in attachedObjects)
            {
                if (weakbindable.Target is BindableObject bindable)
                {
                    item.AttachTo(bindable);
                }
            }
        }
    }
}