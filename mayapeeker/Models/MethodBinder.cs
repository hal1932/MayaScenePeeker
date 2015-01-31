using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace mayapeeker.Models
{
    class MethodBinder
    {
        public MethodBinder() { }


        public MethodBinder(object Instance)
        {
            _instance = Instance;
        }


        public void Bind(string methodName, int parameterCount = 0)
        {
            _methodInfo = _instance.GetType().GetMethods(
                BindingFlags.Instance | BindingFlags.Public)
                .FirstOrDefault(info =>
                    info.Name == methodName &&
                    info.GetParameters().Length == parameterCount);

            if (_methodInfo == null)
            {
                throw new MissingMethodException(string.Format(
                    "not found method that the name is {0} and parameters count is {1}",
                    methodName, parameterCount));
            }
        }


        public void Bind(string methodName, int parameterCount, Type returnType)
        {
            _methodInfo = _instance.GetType().GetMethods(
                BindingFlags.Instance | BindingFlags.Public)
                .FirstOrDefault(info =>
                    info.Name == methodName &&
                    info.GetParameters().Length == parameterCount &&
                    info.ReturnType == returnType);

            if (_methodInfo == null)
            {
                throw new MissingMethodException(string.Format(
                    "not found method that the name is {0} and parameters count is {1} and return type is {2}",
                    methodName, parameterCount, returnType));
            }
        }


        public object Invoke(params object[] parameterArray)
        {
            if (_methodInfo != null)
            {
                return _methodInfo.Invoke(_instance, parameterArray);
            }
            return null;
        }


        private object _instance;
        private MethodInfo _methodInfo;

    }
}
