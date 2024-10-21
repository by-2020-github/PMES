using System.ComponentModel;
using System.Windows.Interactivity;

namespace PMES.Manual.Net6.Behavior
{
    /// <summary>
    /// ClassName：  ListBoxScrollToBottomBehavior
    /// Description：列表框滚动条行为
    /// Author：     luc
    /// CreatTime：  2022/12/12 18:11:48  
    /// </summary>
    public class ListBoxScrollToBottomBehavior : Behavior<System.Windows.Controls.ListBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            ((ICollectionView)AssociatedObject.Items).CollectionChanged += ListBoxScrollToBottomBehavior_CollectionChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            ((ICollectionView)AssociatedObject.Items).CollectionChanged -= ListBoxScrollToBottomBehavior_CollectionChanged;
        }

        private void ListBoxScrollToBottomBehavior_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (AssociatedObject.HasItems)
            {
                AssociatedObject.ScrollIntoView(AssociatedObject.Items[AssociatedObject.Items.Count - 1]);
            }
        }
    }

}
