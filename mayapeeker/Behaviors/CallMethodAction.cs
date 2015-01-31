using mayapeeker.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace mayapeeker.Behaviors
{
    class CallMethodAction : TriggerAction<FrameworkElement>
    {
        // 「引数なし」と「引数が null」を区別するためのオブジェクト
        private static object EmptyParameter = new object();

        #region SourceObject
        public object SourceObject
        {
            get { return (object)GetValue(SourceObjectProperty); }
            set { SetValue(SourceObjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SourceObject.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceObjectProperty =
            DependencyProperty.Register("SourceObject", typeof(object), typeof(CallMethodAction), new PropertyMetadata(null));
        #endregion

        #region MethodName
        public string  MethodName
        {
            get { return (string )GetValue(MethodNameProperty); }
            set { SetValue(MethodNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MethodName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MethodNameProperty =
            DependencyProperty.Register("MethodName", typeof(string ), typeof(CallMethodAction), new PropertyMetadata(null));
        #endregion

        #region Parameter
        public object Parameter
        {
            get { return (object)GetValue(ParameterProperty); }
            set { SetValue(ParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Parameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ParameterProperty =
            DependencyProperty.Register("Parameter", typeof(object), typeof(CallMethodAction), new PropertyMetadata(EmptyParameter));
        #endregion

        #region ParameterArray
        public object[] ParameterArray
        {
            get { return (object[])GetValue(ParameterArrayProperty); }
            set { SetValue(ParameterArrayProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ParameterArray.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ParameterArrayProperty =
            DependencyProperty.Register("ParameterArray", typeof(object[]), typeof(CallMethodAction), new PropertyMetadata(null));
        #endregion
 
        #region ReturnParameter
        public object ReturnParameter
        {
            get { return (object)GetValue(ReturnParameterProperty); }
            set { SetValue(ReturnParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ReturnParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ReturnParameterProperty = 
            DependencyProperty.Register("ReturnParameter", typeof(object), typeof(CallMethodAction), new PropertyMetadata(null));
        #endregion


        protected override void Invoke(object parameter)
        {
            if (SourceObject == null) throw new ArgumentNullException("SourceObject");
            if (MethodName == null) throw new ArgumentNullException("MethodName");

            var binder = new MethodBinder(SourceObject);
            BindMethod(binder);
            InvokeMethod(binder);
        }


        private void BindMethod(MethodBinder binder)
        {
            if (ParameterArray != null)
            {
                binder.Bind(MethodName, ParameterArray.Length);
            }
            else if (Parameter == EmptyParameter)
            {
                binder.Bind(MethodName);
            }
            else
            {
                Debug.Assert(Parameter != null);
                binder.Bind(MethodName, 1);
            }
        }


        private void InvokeMethod(MethodBinder binder)
        {
            if (ParameterArray != null)
            {
                ReturnParameter = binder.Invoke(ParameterArray);
            }
            else if (Parameter == EmptyParameter)
            {
                ReturnParameter = binder.Invoke();
            }
            else
            {
                Debug.Assert(Parameter != null);
                ReturnParameter = binder.Invoke(Parameter);
            }
        }

    }
}
