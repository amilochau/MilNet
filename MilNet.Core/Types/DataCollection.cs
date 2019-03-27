using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MilNet.Core.Types
{
    /// <summary>Collection of data</summary>
    /// <typeparam name="TData">Type of data</typeparam>
    public class DataCollection<TData> : Collection<TData>
    {
        /// <summary>Constructor</summary>
        public DataCollection() : base() { }

        /// <summary>Constructor</summary>
        /// <param name="list">List to be wrapped into the collection</param>
        public DataCollection(IList<TData> list) : base(list) { }

        /// <summary>Constructor</summary>
        /// <param name="collection">Collection to be wrapped into the collection</param>
        public DataCollection(IEnumerable<TData> collection) : base()
        {
            AddRangeCollection(collection);
        }

        /// <summary>Constructor</summary>
        /// <param name="collection">Collection to be wrapped into the collection</param>
        public DataCollection(params TData[] collection) : base()
        {
            AddRangeCollection(collection);
        }

        /// <summary>Add elements to the current collection</summary>
        /// <param name="collection">Collection of elements</param>
        /// <exception cref="ArgumentNullException">The collection to add is null</exception>
        public void AddRange(IEnumerable<TData> collection) => AddRangeCollection(collection);
        /// <summary>Add elements to the current collection</summary>
        /// <param name="collection">Collection of elements</param>
        /// <exception cref="ArgumentNullException">The collection to add is null</exception>
        public void AddRange(params TData[] collection) => AddRangeCollection(collection);

        /// <summary>Add elements to the current collection</summary>
        /// <param name="collection">Collection of elements</param>
        /// <exception cref="ArgumentNullException">The collection to add is null</exception>
        private void AddRangeCollection(IEnumerable<TData> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            List<TData> list = this as List<TData>;

            if (list != null)
                list.AddRange(collection);
            else
            {
                foreach (TData item in collection)
                    Add(item);
            }
        }

        /// <summary>Shift the points of the current data collection</summary>
        /// <param name="value">Shift value; if positive, new points will be added first; if negative, first points will be deleted</param>
        public DataCollection<TData> Shift(int value)
        {
            if (value == 0)
                return this;
            
            // Insert new points first if shift > 0
            for (int i = 0; i < value; i++)
            {
                Insert(0, default(TData));
                RemoveAt(Count);
            }

            // Insert new points last if shift < 0
            for (int i = value; i < 0; i++)
            {
                RemoveAt(0);
                Add(default(TData));
            }

            return this;
        }
    }
}
