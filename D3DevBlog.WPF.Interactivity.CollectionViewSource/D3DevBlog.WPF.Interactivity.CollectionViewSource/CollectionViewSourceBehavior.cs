using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Markup;

namespace D3DevBlog.WPF.Interactivity.CollectionViewSource
{
    [ContentProperty("Filters")]
    public class CollectionViewSourceBehavior : Behavior<ItemsControl>
    {
        #region Properties
        public ICollectionView CollectionView { private set; get; }

        public FilterCollection Filters
        {
            get { return (FilterCollection)GetValue(FiltersProperty); }
        }
        public static DependencyProperty FiltersProperty = DependencyProperty.Register("Filters", typeof(FilterCollection), typeof(CollectionViewSourceBehavior), null);


        public SortingCollection Sortings
        {
            get { return (SortingCollection)GetValue(SortingsProperty); }
        }
        public static DependencyProperty SortingsProperty = DependencyProperty.Register("Sortings", typeof(SortingCollection), typeof(CollectionViewSourceBehavior), null);

        #endregion

        #region ctor
        public CollectionViewSourceBehavior()
        {
            SetValue(CollectionViewSourceBehavior.FiltersProperty, new FilterCollection());
            SetValue(CollectionViewSourceBehavior.SortingsProperty, new SortingCollection());
        }
        #endregion

        #region Behavior
        protected override void OnAttached()
        {
            base.OnAttached();
            if (DesignerProperties.GetIsInDesignMode(AssociatedObject))
                return;

            AssociatedObject.Loaded += OnAssociatedObjectLoaded;
            DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, AssociatedType).AddValueChanged(AssociatedObject, OnItemsSourceChanged);
            CreateCollectionView();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, AssociatedType).RemoveValueChanged(AssociatedObject, OnItemsSourceChanged);
            DestroyCollectionView();
        }
        #endregion

        #region Public

        bool _clear;
        public void ClearAllFilters()
        {
            _clear = true;
            CollectionView.Filter = null;
            foreach (FilterBase filter in Filters)
            {
                filter.Clear();
            }
            _clear = false;
        }

        public void ClearAllSortings()
        {
            _clear = true;
            CollectionView.SortDescriptions.Clear();
            foreach (Sort sort in Sortings)
            {
                sort.Mode = null;
            }
            _clear = false;
        }
        #endregion

        #region Private
        private void OnAssociatedObjectLoaded(object sender, RoutedEventArgs e)
        {
            CreateCollectionView();
            if (AssociatedObject.IsVisible)
                AssociatedObject.Loaded -= OnAssociatedObjectLoaded;
        }

        private void OnItemsSourceChanged(object sender, EventArgs e)
        {
            CreateCollectionView();
        }

        private void CreateCollectionView()
        {
            DestroyCollectionView();
            CollectionView = System.Windows.Data.CollectionViewSource.GetDefaultView(AssociatedObject.ItemsSource);
            if (CollectionView == null)
                return;

            foreach (FilterBase filter in Filters)
            {
                filter.Filtering += OnFiltering;
                OnFiltering(filter);
            }

            foreach (Sort sort in Sortings)
            {
                sort.Sorting += OnSorting;
                OnSorting(sort);
            }
        }

        private void DestroyCollectionView()
        {
            foreach (FilterBase filter in Filters)
            {
                filter.Filtering -= OnFiltering;
            }

            foreach (Sort sort in Sortings)
            {
                sort.Sorting -= OnSorting;
            }

            CollectionView = null;
        }

        private void OnSorting(Sort filter)
        {
            if (_clear)
                return;

            _clear = true;
            foreach (Sort sort in Sortings)
            {
                if (sort == filter)
                    continue;
                sort.Mode = null;
            }
            _clear = false;
            Apply();
        }

        private void OnFiltering(FilterBase filter)
        {
            Apply();
        }

        private void Apply()
        {
            if (_clear)
                return;

            if (Filters.All(f => f.IsClear()))
                CollectionView.Filter = null;
            else
            {
                CollectionView.Filter = (f) =>
                {
                    return Filters.All(fi => fi.Filter(f));
                };
            }

            foreach (Sort sort in Sortings)
            {
                var found = CollectionView.SortDescriptions.FirstOrDefault(s => s.PropertyName == sort.PropertyName);
                if (found != null)
                {
                    CollectionView.SortDescriptions.Remove(found);
                }

                if (sort.Mode != null)
                {
                    CollectionView.SortDescriptions.Add(new SortDescription(sort.PropertyName, sort.Mode.Value));
                    continue;
                }
            }
            CollectionView.Refresh();
        }
        #endregion
    }
}
