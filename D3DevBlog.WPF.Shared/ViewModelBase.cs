using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace D3DevBlog.WPF.Shared
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public ViewModelBase()
        {
            RegisterCommands();
        }

        protected virtual void RegisterCommands()
        {

        }

        public virtual void NotifyOfPropertyChange<TProperty>(Expression<Func<TProperty>> property)
        {
            var lambda = (LambdaExpression)property;

            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)lambda.Body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else
                memberExpression = (MemberExpression)lambda.Body;

            NotifyPropertyChanged(memberExpression.Member.Name);
        }

        public void SetValue<P>(ref P field, P value, [CallerMemberName]string propertyName = null)
        {
            if (propertyName == null)
                throw new Exception("Property name not set");
            PropertyInfo pi = GetType().GetRuntimeProperty(propertyName);
            object oldValue = pi.GetValue(this);
            field = value;
            NotifyPropertyChanged(pi, value, oldValue);
        }

        protected void NotifyPropertyChanged(PropertyInfo propertyInfo, object newValue, object oldValue)
        {
            if (propertyInfo == null) return;
            NotifyPropertyChanged(propertyInfo.Name);
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
